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
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down
            destPos = pos;
            while (row < Board.Size - 1)
            {
                destPos += Board.Size;
                if (destPos >= Board.Size * Board.Size)
                    break;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Left
            destPos = pos;
            while (col > 0)
            {
                destPos -= 1;
                if (destPos % Board.Size >= Board.Size - 1 || destPos < 0)
                    break;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Right
            destPos = pos;
            while (col < Board.Size - 1)
            {
                destPos += 1;
                if (destPos % Board.Size <= 0)
                    break;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                    Moves.AddValidMove(board, square, destPos);
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddValidMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }
        }
    }
}