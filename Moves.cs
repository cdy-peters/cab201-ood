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
                }
            }

            General.GetValidMoves(board, whiteGeneralPos);
            General.GetValidMoves(board, blackGeneralPos);
        }

        internal static int GetDestPos(int pos, int offsetX, int offsetY)
        {
            int destPos = pos + offsetY * Board.Size + offsetX;

            if (IsOutOfBounds(destPos, offsetX, offsetY))
                return -1;
            return destPos;
        }

        internal static bool IsOutOfBounds(int pos, int? offsetX = null, int? offsetY = null)
        {
            if (pos < 0 || pos >= Board.Size * Board.Size)
                return true;

            if (offsetX != null)
            {
                if (offsetX == 1 && pos % Board.Size <= 0)
                    return true;
                if (offsetX == -1 && pos % Board.Size >= Board.Size - 1)
                    return true;
            }

            if (offsetY != null)
            {
                if (offsetY == 1 && pos / Board.Size <= 0)
                    return true;
                if (offsetY == -1 && pos / Board.Size >= Board.Size - 1)
                    return true;
            }

            return false;
        }

        internal static void AddValidMove(Board board, Square square, int destPos)
        {

            // Check if destination piece is protected by a sentinel
            if (IsProtected(board, square, destPos))
                return;

            // Set destination square as threatened
            SetThreat(board, square, destPos);

            // Check if the general is in check
            IsGeneralInCheck(board, destPos);

            // Add move
            square.Piece.ValidMoves.Add(new ValidMove(destPos, false));
            if (square.Piece.PieceType == PieceType.Builder)
                square.Piece.ValidMoves.Add(new ValidMove(destPos, true));
        }

        internal static void SetThreat(Board board, Square square, int destPos)
        {
            if (square.Piece.PieceColor == PieceColor.White)
                board.ThreatenedByWhite[destPos] = true;
            else
                board.ThreatenedByBlack[destPos] = true;
        }

        internal static bool IsProtected(Board board, Square square, int destPos)
        {
            if (square.Piece.PieceType == PieceType.Jester)
                return false;

            int[] offsets = { -1, 0, 1 };

            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;

                    int tempPos = GetDestPos(destPos, offsetX, offsetY);
                    if (tempPos == -1)
                        continue;

                    Square tempSquare = board.Squares[tempPos];
                    if (tempSquare.Piece == null)
                        continue;

                    if (tempSquare.Piece.PieceColor != square.Piece.PieceColor && tempSquare.Piece.PieceType == PieceType.Sentinel)
                        return true;
                }

            return false;
        }

        internal static void IsGeneralInCheck(Board board, int destPos)
        {
            Square destSquare = board.Squares[destPos];
            if (destSquare.Piece == null || destSquare.Piece.PieceType != PieceType.General)
                return;

            if (destSquare.Piece.PieceColor == PieceColor.White)
                board.WhiteCheck = true;
            else
                board.BlackCheck = true;
        }
    }
}