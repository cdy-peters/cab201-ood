namespace Advance
{
    internal static class General
    {
        internal static void GetValidMoves(Board board, int pos)
        {
            if (pos < 0 || pos >= Board.Size * Board.Size)
                return;
            Square square = board.Squares[pos];

            int destPos = pos;
            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    destPos = pos + offsetY * Board.Size + offsetX;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || Piece.IsEnemyPiece(square, destSquare))
                    {
                        // Check if new position is in check
                        if (square.Piece.PieceColor == PieceColor.White)
                            board.ThreatenedByWhite[destPos] = true;
                        else
                            board.ThreatenedByBlack[destPos] = true;

                        Moves.AddValidMove(board, square, destPos);
                    }
                }
            }
        }
    }
}