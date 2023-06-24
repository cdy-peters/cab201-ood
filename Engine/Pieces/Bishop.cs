namespace Engine
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the dragon piece.
    /// </summary>
    internal static class Bishop
    {
        /// <summary>
        /// Gets the moves for the dragon piece. Dragons can move in any direction any number of squares without jumping over other pieces, but cannot capture pieces in the adjacent squares.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the dragon is on.</param>
        /// <param name="pos">The position of the dragon.</param>
        internal static void GetMoves(Board board, Square square, int pos)
        {
            int destPos;

            // Up Left
            destPos = pos;
            while (destPos >= 8 && destPos % 8 != 0)
            {
                destPos -= 8 + 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Up Right
            destPos = pos;
            while (destPos >= 8 && destPos % 8 != 8 - 1)
            {
                destPos -= 8 - 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down Left
            destPos = pos;
            while (destPos < 64 - 8 && destPos % 8 != 0)
            {
                destPos += 8 - 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Down Right
            destPos = pos;
            while (destPos < 64 - 8 && destPos % 8 != 8 - 1)
            {
                destPos += 8 + 1;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    Moves.AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    Moves.AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }
        }
    }
}