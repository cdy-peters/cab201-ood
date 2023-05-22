namespace Advance
{
    internal static class Moves
    {
        internal static void GetValidMoves(Board board)
        {
            int whiteGeneralPos = -1;
            int blackGeneralPos = -1;

            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];

                if (square.Piece == null || square.Piece.PieceType == PieceType.Wall)
                    continue;

                square.Piece.ValidMoves = new List<ValidMove>();

                switch (square.Piece.PieceType)
                {
                    case PieceType.Zombie:
                        Zombie.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Builder:
                        Builder.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Jester:
                        Jester.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Miner:
                        Miner.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Sentinel:
                        Sentinel.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Catapult:
                        Catapult.GetValidMoves(board, square, i);
                        break;
                    case PieceType.Dragon:
                        Dragon.GetValidMoves(board, square, i);
                        break;
                    case PieceType.General:
                        if (square.Piece.PieceColor == PieceColor.White)
                            whiteGeneralPos = i;
                        else
                            blackGeneralPos = i;
                        break;

                        // ? Move methods into their own classes
                }
            }

            General.GetValidMoves(board, whiteGeneralPos);
            General.GetValidMoves(board, blackGeneralPos);
        }

        internal static void AddValidMove(Board board, Square square, int destPos)
        {
            if (square.Piece.PieceType != PieceType.Dragon)
            {
                if (square.Piece.PieceColor == PieceColor.White)
                    board.ThreatenedByWhite[destPos] = true;
                else
                    board.ThreatenedByBlack[destPos] = true;
            }

            // Check if destination piece is protected by a sentinel
            if (IsProtected(board, square, destPos))
                return;

            square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            if (square.Piece.PieceType == PieceType.Builder)
                square.Piece.ValidMoves.Add(new ValidMove(destPos, true));

            // Check if the general is in check
            Square destSquare = board.Squares[destPos];

            if (destSquare.Piece == null)
                return;

            if (destSquare.Piece.PieceType == PieceType.General)
            {
                if (destSquare.Piece.PieceColor == PieceColor.White)
                    board.WhiteCheck = true;
                else
                    board.BlackCheck = true;
            }
        }

        private static bool IsProtected(Board board, Square square, int destPos)
        {
            if (square.Piece.PieceType == PieceType.Jester)
                return false;

            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;
                    int tempPos = destPos + offsetY * Board.Size + offsetX;
                    if (tempPos < 0 || tempPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && tempPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && tempPos % Board.Size >= Board.Size - 1)
                        continue;
                    Square tempSquare = board.Squares[tempPos];

                    if (tempSquare.Piece == null)
                        continue;

                    if (tempSquare.Piece.PieceColor != square.Piece.PieceColor && tempSquare.Piece.PieceType == PieceType.Sentinel)
                        return true;
                }
            }

            return false;
        }
    }
}