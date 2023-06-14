namespace Engine
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the general piece.
    /// </summary>
    internal static class King
    {
        /// <summary>
        /// Gets the moves for the general piece. Generals can move and capture any adjacent square that would not put them in check.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the general is on.</param>
        /// <param name="pos">The position of the general.</param>
        internal static void GetMoves(Board board, int pos)
        {
            if (pos < 0 || pos >= 64)
                return;

            Square square = board.Squares[pos];

            int[] offsets = { -1, 0, 1 };
            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    if (offsetX == 0 && offsetY == 0)
                        continue;

                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    AddMove(board, square, destPos);
                }

            Castle(board);
        }

        /// <summary>
        /// Validates and adds a move to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the general is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
            {
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
                return;
            }
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Check if the destination square is threatened
            if (square.Piece.PieceColor == PieceColor.White)
            {
                if (board.ThreatenedByBlack[destPos])
                    return;
            }
            else
            {
                if (board.ThreatenedByWhite[destPos])
                    return;
            }

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(destPos);
                return;
            }

            // If destination piece is general, set check
            Moves.IsGeneralInCheck(board, destPos);

            square.Piece.ValidMoves.Add(destPos);
        }

        private static void Castle(Board board)
        {
            if (board.Player == PieceColor.White)
            {
                Piece king = board.Squares[60].Piece;
                if (king == null)
                    return;

                if (board.WhiteCastleKingSide)
                {
                    if (board.Squares[61].Piece == null && board.Squares[62].Piece == null)
                        if (!board.ThreatenedByBlack[61] && !board.ThreatenedByBlack[62])
                            king.ValidMoves.Add(62);
                }

                if (board.WhiteCastleQueenSide)
                {
                    if (board.Squares[61].Piece == null && board.Squares[62].Piece == null && board.Squares[63].Piece == null)
                        if (!board.ThreatenedByBlack[58] && !board.ThreatenedByBlack[59])
                            king.ValidMoves.Add(58);
                }
            }
            else
            {
                Piece king = board.Squares[4].Piece;
                if (king == null)
                    return;

                if (board.BlackCastleKingSide)
                {
                    if (board.Squares[5].Piece == null && board.Squares[6].Piece == null)
                        if (!board.ThreatenedByWhite[5] && !board.ThreatenedByWhite[6])
                            king.ValidMoves.Add(6);
                }

                if (board.BlackCastleQueenSide)
                {
                    if (board.Squares[1].Piece == null && board.Squares[2].Piece == null && board.Squares[3].Piece == null)
                        if (!board.ThreatenedByWhite[2] && !board.ThreatenedByWhite[3])
                            king.ValidMoves.Add(2);
                }
            }
        }
    }
}