namespace Engine
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the zombie piece.
    /// </summary>
    internal static class Pawn
    {
        private static int[] offsets = { -1, 1 };

        /// <summary>
        /// Gets the moves for the zombie piece. Zombies can move and capture any of the three squares in front of them, if those squares are empty, zombies can capture the square beyond the empty square in the same direction.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the zombie is on.</param>
        /// <param name="pos">The position of the zombie.</param>
        internal static void GetMoves(Board board, Square square, int pos)
        {
            if (square.Piece.PieceColor == PieceColor.White)
            {
                if (pos / 8 > 0)
                {
                    WhiteZombie(board, square, pos);
                }
            }
            else
            {
                if (pos / 8 < 8 - 1)
                {
                    BlackZombie(board, square, pos);
                }
            }
        }

        /// <summary>
        /// Gets the moves for the white zombie piece. Zombies can move and capture any of the three squares in front of them, if those squares are empty, zombies can capture the square beyond the empty square in the same direction.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the zombie is on.</param>
        /// <param name="pos">The position of the zombie.</param>
        private static void WhiteZombie(Board board, Square square, int pos)
        {
            int row = pos / 8;
            int col = pos % 8;
            int destPos = pos;

            // 1 square move
            destPos = Moves.GetDestPos(pos, 0, -1);
            if (destPos != -1)
            {
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);

                    // 2 square move
                    if (row == 8 - 2)
                    {
                        destPos = Moves.GetDestPos(pos, 0, -2);
                        if (destPos != -1)
                        {
                            destSquare = board.Squares[destPos];
                            if (destSquare.Piece == null)
                                Moves.AddMove(board, square, destPos);
                        }
                    }
                }
            }

            // Capture
            foreach (int offsetX in offsets)
            {
                destPos = Moves.GetDestPos(pos, offsetX, -1);
                if (destPos == -1)
                    continue;

                // Add en passant move
                if (destPos == board.EnPassant)
                {
                    Moves.AddMove(board, square, destPos);
                    return;
                }

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null && destSquare.Piece.PieceColor == PieceColor.Black)
                    AddCapture(board, square, destPos);
            }
        }

        /// <summary>
        /// Gets the moves for the black zombie piece. Zombies can move and capture any of the three squares in front of them, if those squares are empty, zombies can capture the square beyond the empty square in the same direction.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the zombie is on.</param>
        /// <param name="pos">The position of the zombie.</param>
        private static void BlackZombie(Board board, Square square, int pos)
        {
            int row = pos / 8;
            int col = pos % 8;
            int destPos = pos;

            // 1 square move
            destPos = Moves.GetDestPos(pos, 0, 1);
            if (destPos != -1)
            {
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);

                    // 2 square move
                    if (row == 1)
                    {
                        destPos = Moves.GetDestPos(pos, 0, 2);
                        if (destPos != -1)
                        {
                            destSquare = board.Squares[destPos];
                            if (destSquare.Piece == null)
                                Moves.AddMove(board, square, destPos);
                        }
                    }

                }
            }

            // Capture
            foreach (int offsetX in offsets)
            {
                destPos = Moves.GetDestPos(pos, offsetX, 1);
                if (destPos == -1)
                    continue;

                // Add en passant move
                if (destPos == board.EnPassant)
                {
                    Moves.AddMove(board, square, destPos);
                    return;
                }

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null && destSquare.Piece.PieceColor == PieceColor.White)
                    AddCapture(board, square, destPos);
            }
        }

        /// <summary>
        /// Validates and adds a capture to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the zombie is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddCapture(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];
            if (destSquare.Piece == null)
                return;

            // Add attack/defense values
            if (square.Piece.PieceColor == destSquare.Piece.PieceColor)
            {
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
                return;
            }
            else
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Set check flag
            if (destSquare.Piece.PieceType == PieceType.King)
            {
                if (destSquare.Piece.PieceColor == PieceColor.White)
                    board.WhiteCheck = true;
                else
                    board.BlackCheck = true;
            }

            // Add move
            square.Piece.ValidMoves.Add(destPos);
        }
    }
}