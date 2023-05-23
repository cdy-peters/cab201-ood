namespace Advance
{
    internal static class General
    {
        internal static void GetValidMoves(Board board, int pos)
        {
            if (pos < 0 || pos >= Board.Size * Board.Size)
                return;
            Square square = board.Squares[pos];

            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    // TODO: Is new general position in check?

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
                square.Piece.DefenseValue += destSquare.Piece.PieceValue;
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