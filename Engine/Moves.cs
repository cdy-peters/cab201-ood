namespace Engine
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
            for (int i = 0; i < 64; i++)
            {
                Square square = board.Squares[i];
                if (square.Piece == null)
                    continue;

                square.Piece.ValidMoves = new List<int>();

                switch (square.Piece.PieceType)
                {
                    case PieceType.Pawn:
                        Pawn.GetMoves(board, square, i);
                        break;
                    case PieceType.Bishop:
                        Bishop.GetMoves(board, square, i);
                        break;
                    case PieceType.Knight:
                        Knight.GetMoves(board, square, i);
                        break;
                    case PieceType.Rook:
                        Rook.GetMoves(board, square, i);
                        break;
                    case PieceType.Queen:
                        Queen.GetMoves(board, square, i);
                        break;
                    case PieceType.King:
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
            King.GetMoves(board, whiteGeneralPos);
            King.GetMoves(board, blackGeneralPos);
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
            int destPos = pos + offsetY * 8 + offsetX;

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
            if (pos < 0 || pos >= 64)
                return true;

            if (offsetX != null)
            {
                if (offsetX == 1 && pos % 8 <= 0)
                    return true;
                if (offsetX == -1 && pos % 8 >= 8 - 1)
                    return true;
            }

            if (offsetY != null)
            {
                if (offsetY == 1 && pos / 8 <= 0)
                    return true;
                if (offsetY == -1 && pos / 8 >= 8 - 1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Validates and adds a move to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the sentinel is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        internal static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            if (destSquare.Piece != null)
            {
                // Add attack/defense values
                if (square.Piece.PieceColor == destSquare.Piece.PieceColor)
                {
                    square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
                    return;
                }
                else
                    square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

                // Set check flag
                if (destSquare.Piece.PieceType == PieceType.King)
                {
                    if (destSquare.Piece.PieceColor == PieceColor.White)
                        board.WhiteCheck = true;
                    else
                        board.BlackCheck = true;
                }
            }

            /// Sets the square of the destination position to threatened.
            if (square.Piece.PieceType != PieceType.Pawn)
            {
                if (square.Piece.PieceColor == PieceColor.White)
                    board.ThreatenedByWhite[destPos] = true;
                else
                    board.ThreatenedByBlack[destPos] = true;
            }

            square.Piece.ValidMoves.Add(destPos);
        }
    }
}