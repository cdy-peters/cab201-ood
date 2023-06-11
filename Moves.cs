namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for each piece.
    /// </summary>
    internal static class Moves
    {
        /// <summary>
        /// Gets the valid moves for each piece on the board.
        /// </summary>
        /// <param name="board">The board to examine for valid moves and place</param>
        internal static void GetValidMoves(Board board)
        {
            int whiteGeneralPos = -1;
            int blackGeneralPos = -1;

            /// Iterate through each square on the board and getting the valid moves for each piece
            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];
                if (square.Piece == null || square.Piece.PieceType == PieceType.Wall)
                    continue;

                square.Piece.ValidMoves = new List<MoveDest>();

                switch (square.Piece.PieceType)
                {
                    case PieceType.Zombie:
                        Zombie.GetMoves(board, square, i);
                        break;
                    case PieceType.Builder:
                        Builder.GetMoves(board, square, i);
                        break;
                    case PieceType.Jester:
                        Jester.GetMoves(board, square, i);
                        break;
                    case PieceType.Miner:
                        Miner.GetMoves(board, square, i);
                        break;
                    case PieceType.Sentinel:
                        Sentinel.GetMoves(board, square, i);
                        break;
                    case PieceType.Catapult:
                        Catapult.GetMoves(board, square, i);
                        break;
                    case PieceType.Dragon:
                        Dragon.GetMoves(board, square, i);
                        break;
                    case PieceType.General:
                        /// Set the position of the generals on the board.
                        if (square.Piece.PieceColor == PieceColor.White)
                            whiteGeneralPos = i;
                        else
                            blackGeneralPos = i;
                        break;
                    default:
                        break;
                }
            }

            /// Get the valid moves for the generals
            General.GetMoves(board, whiteGeneralPos);
            General.GetMoves(board, blackGeneralPos);
        }

        /// <summary>
        /// Gets the destination position to move to.
        /// </summary>
        /// <param name="pos">The position to move to.</param>
        /// <param name="offsetX">The offset from the left to the right of the position.</param>
        /// <param name="offsetY">The offset from the top to the position.</param>
        /// <returns>The destPos to move to or - 1 if out of bounds.</returns>
        internal static int GetDestPos(int pos, int offsetX, int offsetY)
        {
            int destPos = pos + offsetY * Board.Size + offsetX;

            if (IsOutOfBounds(destPos, offsetX, offsetY))
                return -1;

            return destPos;
        }

        /// <summary>
        /// Checks if a position is out of bounds. This is used to prevent a move that may be off the board
        /// </summary>
        /// <param name="pos">The position to check.</param>
        /// <param name="offsetX">The offset from the left to the right of the position.</param>
        /// <param name="offsetY">The offset from the top to the position.</param>
        /// <returns>True if the position is out of bounds false otherwise.</returns>
        internal static bool IsOutOfBounds(int pos, int? offsetX = null, int? offsetY = null)
        {
            /// Check if the initial position is off the board.
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

        /// <summary>
        /// Sets threat on a square.
        /// </summary>
        /// <param name="board">The board to set the threatened squares on</param>
        /// <param name="square">The square of the moving piece.</param>
        /// <param name="destPos">The destination position of the moving piece.</param>
        internal static void SetThreat(Board board, Square square, int destPos)
        {
            /// Does not threaten a square if the piece is a Jester.
            if (square.Piece.PieceType == PieceType.Jester)
                return;

            /// Sets the square of the destination position to threatened.
            if (square.Piece.PieceColor == PieceColor.White)
                board.ThreatenedByWhite[destPos] = true;
            else
                board.ThreatenedByBlack[destPos] = true;
        }

        /// <summary>
        /// Checks if a square is protected by a sentinel piece.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square of the moving piece</param>
        /// <param name="destPos">The destination position of the moving piece</param>
        internal static Piece? IsProtected(Board board, Square square, int destPos)
        {
            /// Jesters cannot be protected.
            if (square.Piece.PieceType == PieceType.Jester)
                return null;

            /// Check the cardinal directions of the destination position for a sentinel.
            int[] offsets = { -1, 0, 1 };
            foreach (int offsetY in offsets)
            {
                foreach (int offsetX in offsets)
                {
                    /// Continue if the offset is diagonal.
                    if (Math.Abs(offsetX) == Math.Abs(offsetY))
                        continue;

                    int tempPos = GetDestPos(destPos, offsetX, offsetY);
                    if (tempPos == -1)
                        continue;

                    Square tempSquare = board.Squares[tempPos];
                    if (tempSquare.Piece == null)
                        continue;

                    if (tempSquare.Piece.PieceType == PieceType.Sentinel && tempSquare.Piece.PieceColor != square.Piece.PieceColor)
                        return tempSquare.Piece;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the piece at the destination is a general piece and sets the check flag.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="destPos">The destination position to check.</param>
        internal static void IsGeneralInCheck(Board board, int destPos)
        {
            Square destSquare = board.Squares[destPos];
            if (destSquare.Piece.PieceType != PieceType.General)
                return;

            if (destSquare.Piece.PieceColor == PieceColor.White)
                board.WhiteCheck = true;
            else
                board.BlackCheck = true;
        }
    }
}