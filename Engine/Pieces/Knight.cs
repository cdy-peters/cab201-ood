namespace Engine
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
            int row = pos / 8;
            int col = pos % 8;

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
                    int destRow = destPos / 8;
                    int destCol = destPos % 8;
                    if (destRow != row + offsetY || destCol != col + offsetX)
                        continue;

                    Moves.AddMove(board, square, destPos);
                }
        }
    }
}