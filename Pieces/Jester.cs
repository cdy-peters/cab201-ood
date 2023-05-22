namespace Advance
{
    internal static class Jester
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