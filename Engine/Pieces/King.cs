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
            if (pos < 0 || pos >= Board.Size * Board.Size)
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
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                return;
            }

            // If destination piece is general, set check
            Moves.IsGeneralInCheck(board, destPos);
            
            square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
        }
    }
}