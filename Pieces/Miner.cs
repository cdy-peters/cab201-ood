namespace Advance
{
    internal static class Miner
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int row = pos / Board.Size;
            int col = pos % Board.Size;

            // Up
            int destPos = pos;
            while (row > 0)
            {
                destPos -= Board.Size;
                if (destPos < 0)
                    break;

                AddMove(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down
            destPos = pos;
            while (row < Board.Size - 1)
            {
                destPos += Board.Size;
                if (destPos >= Board.Size * Board.Size)
                    break;

                AddMove(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Left
            destPos = pos;
            while (col > 0)
            {
                destPos--;
                if (destPos % Board.Size >= Board.Size - 1 || destPos < 0)
                    break;

                AddMove(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Right
            destPos = pos;
            while (col < Board.Size - 1)
            {
                destPos++;
                if (destPos % Board.Size <= 0)
                    break;

                AddMove(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
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

            // Capture
            if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            }
        }
    }
}