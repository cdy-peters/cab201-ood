namespace Advance
{
    internal static class Sentinel
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int[] offsets = { -2, -1, 1, 2 };

            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;

                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || Piece.IsEnemyPiece(square, destSquare))
                        Moves.AddValidMove(board, square, destPos);
                }
        }
    }
}