namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the builder piece.
    /// </summary>
    internal static class Builder
    {
        /// <summary>
        /// Gets the moves for the builder piece. Builders can move, capture or build on any adjacent square.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the builder is on.</param>
        /// <param name="pos">The position of the builder.</param>
        internal static void GetMoves(Board board, Square square, int pos)
        {
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
        /// <param name="square">The square that the builder is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Check if destination piece is protected by a sentinel
            Piece? sentinel = Moves.IsProtected(board, square, destPos);
            if (sentinel != null && sentinel.PieceColor == square.Piece.PieceColor) // Protected by friendly sentinel
                Moves.SetThreat(board, square, destPos);

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                square.Piece.ValidMoves.Add(new MoveDest(destPos, true));
                return;
            }

            // Capture only if not protected by a sentinel
            if (sentinel == null && Piece.IsEnemyPiece(square, destSquare))
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }
    }
}