namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the zombie piece.
    /// </summary>
    internal static class Pawn
    {
        /// <summary>
        /// The offsets for the zombie piece.
        /// </summary>
        private static int[] offsets = { -1, 0, 1 };

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
                if (pos / Board.Size > 0)
                    WhiteZombie(board, square, pos);
            }
            else
            {
                if (pos / Board.Size < Board.Size - 1)
                    BlackZombie(board, square, pos);
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
            int row = pos / Board.Size;
            int col = pos % Board.Size;
            int destPos = pos;

            foreach (int offsetX in offsets)
            {
                // Move
                destPos = Moves.GetDestPos(pos, offsetX, -1);
                if (destPos == -1)
                    continue;

                AddMove(board, square, destPos);

                // Kill
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null && pos / Board.Size > 1)
                {
                    destPos = Moves.GetDestPos(pos, offsetX * 2, -2);
                    if (destPos == -1)
                        continue;

                    // Check if destination square is in the correct row and column
                    int destRow = destPos / Board.Size;
                    int destCol = destPos % Board.Size;
                    if (destRow != row - 2 || destCol != col + offsetX * 2)
                        continue;

                    AddCapture(board, square, destPos);
                }
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
            int row = pos / Board.Size;
            int col = pos % Board.Size;
            int destPos = pos;

            foreach (int offsetX in offsets)
            {
                // Move
                destPos = Moves.GetDestPos(pos, offsetX, 1);
                if (destPos == -1)
                    continue;

                AddMove(board, square, destPos);

                // Kill
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null && pos / Board.Size < Board.Size - 2)
                {
                    destPos = Moves.GetDestPos(pos, offsetX * 2, 2);
                    if (destPos == -1)
                        continue;

                    // Check if destination square is in the correct row and column
                    int destRow = destPos / Board.Size;
                    int destCol = destPos % Board.Size;
                    if (destRow != row + 2 || destCol != col + offsetX * 2)
                        continue;

                    AddCapture(board, square, destPos);
                }
            }
        }

        /// <summary>
        /// Validates and adds a move (not a capture) to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the zombie is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                return;
            }

            // Check if the general is in check
            Moves.IsGeneralInCheck(board, destPos);

            square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
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
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // If destination piece is general, set check
            Moves.IsGeneralInCheck(board, destPos);

            // Add move
            square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
        }
    }
}