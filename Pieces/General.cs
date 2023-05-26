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
                    if (offsetX == 0 && offsetY == 0)
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

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Check if the destination square is threatened
            if (square.Piece.PieceColor == PieceColor.White)
            {
                if (board.ThreatenedByBlack[destPos])
                    return;
            }
            else
            {
                if (board.ThreatenedByWhite[destPos])
                    return;
            }

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                return;
            }

            // Add capture
            if (Piece.IsEnemyPiece(square, destSquare))
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }
    }
}