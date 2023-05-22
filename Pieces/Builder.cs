namespace Advance
{
    internal static class Builder
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
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