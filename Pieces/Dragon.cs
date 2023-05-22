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

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos - Board.Size)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Down
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size)
            {
                destPos += Board.Size;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos + Board.Size)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Left
            destPos = pos;
            while (destPos % Board.Size != 0)
            {
                destPos--;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Right
            destPos = pos;
            while (destPos % Board.Size != Board.Size - 1)
            {
                destPos++;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos + 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Up Left
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != 0)
            {
                destPos -= Board.Size + 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos - Board.Size - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Up Right
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos -= Board.Size - 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos - Board.Size + 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Down Left
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != 0)
            {
                destPos += Board.Size - 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos + Board.Size - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }

            // Down Right
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos += Board.Size + 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else
                {
                    if (Piece.IsEnemyPiece(square, destSquare) && destPos != pos + Board.Size + 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
            }
        }
    }
}