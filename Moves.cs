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
                        GetValidMovesZombie(board, square, i);
                        break;
                    case PieceType.Builder:
                        GetValidMovesBuilder(board, square, i);
                        break;
                    case PieceType.Jester:
                        GetValidMovesJester(board, square, i);
                        break;
                    case PieceType.Miner:
                        GetValidMovesMiner(board, square, i);
                        break;
                    case PieceType.Sentinel:
                        GetValidMovesSentinel(board, square, i);
                        break;
                    case PieceType.Catapult:
                        GetValidMovesCatapult(board, square, i);
                        break;
                    case PieceType.Dragon:
                        GetValidMovesDragon(board, square, i);
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

            GetValidMovesGeneral(board, whiteGeneralPos);
            GetValidMovesGeneral(board, blackGeneralPos);
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

            // Check if the move puts the enemy general in check
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

        private static void GetValidMovesZombie(Board board, Square square, int pos)
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
                            AddValidMove(board, square, destPos);

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
                                    AddValidMove(board, square, destPos);
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
                            AddValidMove(board, square, destPos);

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
                                    AddValidMove(board, square, destPos);
                                board.ThreatenedByBlack[destPos] = true;
                            }
                        }
                    }
                }
            }
        }

        private static void GetValidMovesBuilder(Board board, Square square, int pos)
        {
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
                        AddValidMove(board, square, destPos);
                }
            }
        }

        private static void GetValidMovesJester(Board board, Square square, int pos)
        {
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

                    if (destSquare.Piece == null ||
                        (Piece.IsFriendlyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.Jester) ||
                        (Piece.IsEnemyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.General))
                    {
                        AddValidMove(board, square, destPos);
                    }
                }
            }
        }

        private static void GetValidMovesMiner(Board board, Square square, int pos)
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        private static void GetValidMovesSentinel(Board board, Square square, int pos)
        {
            int[] offsets = { -2, -1, 1, 2 };

            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    int destPos = pos + offsetY * Board.Size + offsetX;
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;
                    if (destPos < 0 || destPos >= Board.Size * Board.Size)
                        continue;
                    if (offsetX == 1 && destPos % Board.Size <= 0)
                        continue;
                    if (offsetX == -1 && destPos % Board.Size >= Board.Size - 1)
                        continue;
                    if (offsetY == 1 && destPos / Board.Size <= 0)
                        continue;
                    if (offsetY == -1 && destPos / Board.Size >= Board.Size - 1)
                        continue;
                    Square destSquare = board.Squares[destPos];

                    if (destSquare.Piece == null || Piece.IsEnemyPiece(square, destSquare))
                        AddValidMove(board, square, destPos);
                }
            }
        }

        private static void GetValidMovesCatapult(Board board, Square square, int pos)
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
                        AddValidMove(board, square, destPos);
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
                        AddValidMove(board, square, destPos);
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
                        AddValidMove(board, square, destPos);
                }
            }
        }

        private static void GetValidMovesDragon(Board board, Square square, int pos)
        {
            // Up
            int destPos = pos;
            while (destPos >= Board.Size)
            {
                destPos -= Board.Size;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    AddValidMove(board, square, destPos);
                }
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - Board.Size)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos + Board.Size)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
            }

            // Left
            destPos = pos;
            while (destPos % Board.Size != 0)
            {
                destPos -= 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    AddValidMove(board, square, destPos);
                }
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - 1)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
            }

            // Right
            destPos = pos;
            while (destPos % Board.Size != Board.Size - 1)
            {
                destPos += 1;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos + 1)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos - Board.Size - 1)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall)
                {
                    if (destPos != pos - Board.Size + 1)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (Piece.IsEnemyPiece(square, destSquare))
                {
                    if (destPos != pos + Board.Size - 1)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
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
                {
                    AddValidMove(board, square, destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    if (destPos != pos + Board.Size + 1 && destSquare.Piece.PieceType != PieceType.Wall)
                        AddValidMove(board, square, destPos);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        private static void GetValidMovesGeneral(Board board, int pos)
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
                        if (IsGeneralInCheck(board, square, destPos))
                            continue;

                        AddValidMove(board, square, destPos);
                    }
                }
            }
        }

        private static bool IsGeneralInCheck(Board board, Square square, int destPos)
        {
            if (square.Piece.PieceColor == PieceColor.White)
                return board.ThreatenedByBlack[destPos];
            else
                return board.ThreatenedByWhite[destPos];
        }
    }
}