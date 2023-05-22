namespace Advance
{
    internal static class Dragon
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            // Up
            int destPos = pos;
            while (destPos >= Board.Size)
            {
                destPos -= Board.Size;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - Board.Size)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size)
            {
                destPos += Board.Size;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos + Board.Size)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Left
            destPos = pos;
            while (destPos % Board.Size != 0)
            {
                destPos -= 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Right
            destPos = pos;
            while (destPos % Board.Size != Board.Size - 1)
            {
                destPos += 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos + 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Up Left
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != 0)
            {
                destPos -= Board.Size + 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - Board.Size - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Up Right
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos -= Board.Size - 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos - Board.Size + 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down Left
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != 0)
            {
                destPos += Board.Size - 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos + Board.Size - 1)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down Right
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos += Board.Size + 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    if (destPos != pos + Board.Size + 1 && destSquare.Piece.PieceType != PieceType.Wall)
                        Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }
        }
    }
}