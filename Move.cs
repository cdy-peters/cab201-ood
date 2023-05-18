namespace Advance
{
    internal class Moves
    {
        internal void GetValidMoves(Board board)
        {
            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];

                if (square.Piece.PieceType == PieceType.None || square.Piece.PieceType == PieceType.Wall)
                    continue;

                square.Piece.ValidMoves = new List<int>();

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
                        GetValidMovesGeneral(board, square, i);
                        break;

                    // TODO: Check if any of the added moves are protected by the sentinel
                    // ? Maybe make a method for adding the moves and then check if they are protected
                    // ? Move methods into their own classes
                }
            }
        }

        private void GetValidMovesZombie(Board board, Square square, int pos)
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
                        Square destSquare = board.Squares[destPos];

                        if (destSquare.Piece.PieceType == PieceType.None)
                        {
                            square.Piece.ValidMoves.Add(destPos);

                            // Kill
                            if (row > 1)
                            {
                                destPos = pos - Board.Size * 2 + offset * 2;
                                destSquare = board.Squares[destPos];

                                if (destSquare.Piece.PieceColor == PieceColor.Black)
                                {
                                    square.Piece.ValidMoves.Add(destPos);
                                }
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

                        if (destSquare.Piece.PieceType == PieceType.None)
                        {
                            square.Piece.ValidMoves.Add(destPos);

                            // Kill
                            if (row < Board.Size - 2)
                            {
                                destPos = pos + Board.Size * 2 + offset * 2;
                                destSquare = board.Squares[destPos];

                                if (destSquare.Piece.PieceColor == PieceColor.White)
                                {
                                    square.Piece.ValidMoves.Add(destPos);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetValidMovesBuilder(Board board, Square square, int pos)
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
                }
            }
        }

        private void GetValidMovesJester(Board board, Square square, int pos)
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

                    if (destSquare.Piece.PieceType == PieceType.None ||
                        (destSquare.Piece.PieceColor == square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Jester) ||
                        (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall && destSquare.Piece.PieceType != PieceType.General))
                    {
                        square.Piece.ValidMoves.Add(destPos);
                    }
                }
            }
        }

        private void GetValidMovesMiner(Board board, Square square, int pos)
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
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
                if (destPos % Board.Size >= Board.Size - 1)
                    break;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        private void GetValidMovesSentinel(Board board, Square square, int pos)
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
                }
            }
        }

        private void GetValidMovesCatapult(Board board, Square square, int pos)
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
                }
            }
        }

        private void GetValidMovesDragon(Board board, Square square, int pos)
        {
            // Up
            int destPos = pos;
            while (destPos >= Board.Size)
            {
                destPos -= Board.Size;
                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos - Board.Size)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos + Board.Size)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos - 1)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos + 1)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos - Board.Size - 1)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos - Board.Size + 1)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos + Board.Size - 1)
                        square.Piece.ValidMoves.Add(destPos);
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

                if (destSquare.Piece.PieceType == PieceType.None)
                {
                    square.Piece.ValidMoves.Add(destPos);
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.None)
                {
                    if (destPos != pos + Board.Size + 1)
                        square.Piece.ValidMoves.Add(destPos);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        private void GetValidMovesGeneral(Board board, Square square, int pos)
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

                    if (destSquare.Piece.PieceType == PieceType.None || (destSquare.Piece.PieceColor != square.Piece.PieceColor && destSquare.Piece.PieceType != PieceType.Wall))
                        square.Piece.ValidMoves.Add(destPos);
                }
            }
        }
    }
}