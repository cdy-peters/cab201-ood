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
                    if (offsetX == 0 && offsetY == 0)
                        continue;
                        
                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    Square destSquare = board.Squares[destPos];

                    AddMove(board, square, destPos);
                }
        }

        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Set destination square as threatened
            Moves.SetThreat(board, square, destPos);

            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceValue;

            if (destSquare.Piece == null ||
                (Piece.IsFriendlyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.Jester) ||
                (Piece.IsEnemyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.General))
            {
                // Add move
                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            }
        }
    }
}