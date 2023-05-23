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

                    AddMove(board, square, destPos);
                }
        }

        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Check if destination piece is protected by a sentinel
            if (Moves.IsProtected(board, square, destPos))
                return;

            // Set destination square as threatened
            Moves.SetThreat(board, square, destPos);

            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceValue; //? Should protected pieces be added
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceValue;

            if (destSquare.Piece == null || Piece.IsEnemyPiece(square, destSquare))
            {
                // Check if the general is in check
                Moves.IsGeneralInCheck(board, destPos);

                // Add move
                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            }
        }
    }
}