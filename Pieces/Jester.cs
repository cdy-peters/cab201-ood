namespace Advance
{
    internal static class Jester
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int destPos = pos;
            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    destPos = pos + offsetY * Board.Size + offsetX;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null ||
                        (Piece.IsFriendlyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.Jester) ||
                        (Piece.IsEnemyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.General))
                    {
                        Moves.AddValidMove(board, square, destPos);
                    }
                }
            }
        }
    }
}