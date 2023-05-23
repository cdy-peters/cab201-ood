namespace Advance
{
    internal static class Dragon
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int destPos;

            // Up
            destPos = pos;
            while (destPos >= Board.Size)
            {
                destPos -= Board.Size;

                if (destPos == pos - Board.Size)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size)
            {
                destPos += Board.Size;

                if (destPos == pos + Board.Size)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Left
            destPos = pos;
            while (destPos % Board.Size != 0)
            {
                destPos--;

                if (destPos == pos - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Right
            destPos = pos;
            while (destPos % Board.Size != Board.Size - 1)
            {
                destPos++;

                if (destPos == pos + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Up Left
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != 0)
            {
                destPos -= Board.Size + 1;

                if (destPos == pos - Board.Size - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Up Right
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos -= Board.Size - 1;

                if (destPos == pos - Board.Size + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down Left
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != 0)
            {
                destPos += Board.Size - 1;

                if (destPos == pos + Board.Size - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down Right
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos += Board.Size + 1;

                if (destPos == pos + Board.Size + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }
        }

        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceValue;

            if (destSquare.Piece == null)
            {
                // Add move
                square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            }
        }

        private static void AddCapture(Board board, Square square, int destPos)
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