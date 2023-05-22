namespace Advance
{
    internal static class Zombie
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            if (square.Piece.PieceColor == PieceColor.White)
                WhiteZombie(board, square, pos);
            else
                BlackZombie(board, square, pos);
        }

        private static void WhiteZombie(Board board, Square square, int pos)
        {

            // Move
            if (pos / Board.Size > 0)
            {
                int destPos = pos;
                int[] offsets = { -1, 0, 1 };

                foreach (int offset in offsets)
                {
                    destPos = pos - Board.Size + offset;
                    if (Moves.IsOutOfBounds(destPos, offset))
                        continue;

                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || destSquare.Piece.PieceColor == PieceColor.Black)
                    {
                        Moves.AddValidMove(board, square, destPos);

                        // Kill
                        if (destSquare.Piece == null && pos / Board.Size > 1)
                        {
                            destPos = pos - Board.Size * 2 + offset * 2;
                            if (Moves.IsOutOfBounds(destPos))
                                continue;

                            destSquare = board.Squares[destPos];
                            if (destSquare.Piece == null)
                                continue;

                            if (destSquare.Piece.PieceColor == PieceColor.Black)
                                Moves.AddValidMove(board, square, destPos);
                            board.ThreatenedByWhite[destPos] = true;
                        }
                    }
                }
            }
        }

        private static void BlackZombie(Board board, Square square, int pos)
        {

            // Move
            if (pos / Board.Size < Board.Size - 1)
            {
                int destPos = pos;
                int[] offsets = { -1, 0, 1 };

                foreach (int offset in offsets)
                {
                    destPos = pos + Board.Size + offset;
                    if (Moves.IsOutOfBounds(destPos, offset))
                        continue;

                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || destSquare.Piece.PieceColor == PieceColor.White)
                    {
                        Moves.AddValidMove(board, square, destPos);

                        // Kill
                        if (destSquare.Piece == null && pos / Board.Size < Board.Size - 2)
                        {
                            destPos = pos + Board.Size * 2 + offset * 2;
                            if (Moves.IsOutOfBounds(destPos))
                                continue;

                            destSquare = board.Squares[destPos];
                            if (destSquare.Piece == null)
                                continue;

                            if (destSquare.Piece.PieceColor == PieceColor.White)
                                Moves.AddValidMove(board, square, destPos);
                            board.ThreatenedByBlack[destPos] = true;
                        }
                    }
                }
            }
        }
    }
}