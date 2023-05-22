namespace Advance
{
    internal static class Zombie
    {
        internal static void GetValidMoves(Board board, Square square, int pos)
        {
            int row = pos / Board.Size;
            int col = pos % Board.Size;

            if (square.Piece.PieceColor == PieceColor.White)
            {
                // Move
                if (row > 0)
                {
                    int destPos = pos;
                    int[] offsets = { -1, 0, 1 };

                    foreach (int offset in offsets)
                    {
                        destPos = pos - Board.Size + offset;
                        if (offset == 1 && destPos % Board.Size <= 0)
                            continue;
                        if (offset == -1 && destPos % Board.Size >= Board.Size - 1)
                            continue;
                        if (destPos < 0)
                            continue;
                        Square destSquare = board.Squares[destPos];

                        if (destSquare.Piece == null || destSquare.Piece.PieceColor == PieceColor.Black)
                        {
                            Moves.AddValidMove(board, square, destPos);

                            // Kill
                            if (destSquare.Piece == null && row > 1)
                            {
                                destPos = pos - Board.Size * 2 + offset * 2;
                                if (destPos < 0)
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
            else
            {
                // Move
                if (row < Board.Size - 1)
                {
                    int destPos = pos;
                    int[] offsets = { -1, 0, 1 };

                    foreach (int offset in offsets)
                    {
                        destPos = pos + Board.Size + offset;
                        if (offset == 1 && destPos % Board.Size <= 0)
                            continue;
                        if (offset == -1 && destPos % Board.Size >= Board.Size - 1)
                            continue;
                        Square destSquare = board.Squares[destPos];

                        if (destSquare.Piece == null || destSquare.Piece.PieceColor == PieceColor.White)
                        {
                            Moves.AddValidMove(board, square, destPos);

                            // Kill
                            if (destSquare.Piece == null && row < Board.Size - 2)
                            {
                                destPos = pos + Board.Size * 2 + offset * 2;
                                if (destPos < 0 || destPos >= Board.Size * Board.Size)
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
}