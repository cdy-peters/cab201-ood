namespace Advance
{
    internal static class Catapult
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            // ? Possible refactor?

            // Moving
            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;
                    int destPos = pos + offsetY * Board.Size + offsetX;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        Moves.AddValidMove(board, square, destPos);
                }
            }

            // Shooting
            // Cardinal directions
            offsets = new int[] { -3, 0, 3 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;
                    int destPos = pos + offsetY * Board.Size + offsetX;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null)
                        continue;

                    if (Piece.IsEnemyPiece(square, destSquare))
                        Moves.AddValidMove(board, square, destPos);
                }
            }

            // Diagonal directions
            offsets = new int[] { -2, 2 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    int destPos = pos + offsetY * Board.Size + offsetX;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null)
                        continue;

                    if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                        Moves.AddValidMove(board, square, destPos);
                }
            }
        }
    }
}