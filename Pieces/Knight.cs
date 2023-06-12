namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the sentinel piece.
    /// </summary>
    internal static class Knight
    {
        /// <summary>
        /// Gets the moves for the sentinel piece. Sentinels can move and capture in an L shape (2x1 squares) and can jump over other pieces.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the sentinel is on.</param>
        /// <param name="pos">The position of the sentinel.</param>
        internal static void GetMoves(Board board, Square square, int pos)
        {
            int row = pos / Board.Size;
            int col = pos % Board.Size;

            int[] offsets = { -2, -1, 1, 2 };
            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;

                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    // Check if destination square is in the correct row and column
                    int destRow = destPos / Board.Size;
                    int destCol = destPos % Board.Size;
                    if (destRow != row + offsetY || destCol != col + offsetX)
                        continue;

                    AddMove(board, square, destPos);
                }
        }

        /// <summary>
        /// Validates and adds a move to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the sentinel is on.</param>
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

            // If destination piece is general, set check
            Moves.IsGeneralInCheck(board, destPos);

            square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
        }
    }
}