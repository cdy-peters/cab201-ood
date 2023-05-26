namespace Advance
{
    internal static class Sentinel
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int[] offsets = { -2, -1, 1, 2 };
            int row = pos / Board.Size;
            int col = pos % Board.Size;

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

        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Check if destination piece is protected by a sentinel
            if (Moves.IsProtected(board, square, destPos))
                return;

            // Set destination square as threatened
            Moves.SetThreat(board, square, destPos);

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
                return;
            }

            // Add capture
            if (Piece.IsEnemyPiece(square, destSquare))
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            }
        }
    }
}