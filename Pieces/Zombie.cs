namespace Advance
{
    internal static class Zombie
    {
        private static int[] offsets = { -1, 0, 1 };

        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            if (square.Piece.PieceColor == PieceColor.White)
            {
                if (pos / Board.Size > 0)
                    WhiteZombie(board, square, pos);
            }
            else
            {
                if (pos / Board.Size < Board.Size - 1)
                    BlackZombie(board, square, pos);
            }
        }

        private static void WhiteZombie(Board board, Square square, int pos)
        {
            int row = pos / Board.Size;
            int col = pos % Board.Size;
            int destPos = pos;

            foreach (int offsetX in offsets)
            {
                // Move
                destPos = Moves.GetDestPos(pos, offsetX, -1);
                if (destPos == -1)
                    continue;

                AddMove(board, square, destPos);

                // Kill
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null && pos / Board.Size > 1)
                {
                    destPos = Moves.GetDestPos(pos, offsetX * 2, -2);
                    if (destPos == -1)
                        continue;

                    // Check if destination square is in the correct row and column
                    int destRow = destPos / Board.Size;
                    int destCol = destPos % Board.Size;
                    if (destRow != row - 2 || destCol != col + offsetX * 2)
                        continue;

                    AddCapture(board, square, destPos);
                }
            }
        }

        private static void BlackZombie(Board board, Square square, int pos)
        {
            int row = pos / Board.Size;
            int col = pos % Board.Size;
            int destPos = pos;

            foreach (int offsetX in offsets)
            {
                // Move
                destPos = Moves.GetDestPos(pos, offsetX, 1);
                if (destPos == -1)
                    continue;

                AddMove(board, square, destPos);

                // Kill
                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece == null && pos / Board.Size < Board.Size - 2)
                {
                    destPos = Moves.GetDestPos(pos, offsetX * 2, 2);
                    if (destPos == -1)
                        continue;

                    // Check if destination square is in the correct row and column
                    int destRow = destPos / Board.Size;
                    int destCol = destPos % Board.Size;
                    if (destRow != row + 2 || destCol != col + offsetX * 2)
                        continue;

                    AddCapture(board, square, destPos);
                }
            }
        }

        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Check if destination piece is protected by a sentinel
            Piece? sentinel = Moves.IsProtected(board, square, destPos);
            if (sentinel != null && sentinel.PieceColor == square.Piece.PieceColor) // Protected by friendly sentinel
                Moves.SetThreat(board, square, destPos);

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                return;
            }

            // Capture only if not protected by a sentinel
            if (sentinel == null && Piece.IsEnemyPiece(square, destSquare))
            {
                // Check if the general is in check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }

        private static void AddCapture(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];
            if (destSquare.Piece == null)
                return;

            // Check if destination piece is protected by a sentinel
            Piece? sentinel = Moves.IsProtected(board, square, destPos);
            if (sentinel != null && sentinel.PieceColor == square.Piece.PieceColor) // Protected by friendly sentinel
                Moves.SetThreat(board, square, destPos);

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Capture only if not protected by a sentinel
            if (sentinel == null && Piece.IsEnemyPiece(square, destSquare))
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                // Add move
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }
    }
}