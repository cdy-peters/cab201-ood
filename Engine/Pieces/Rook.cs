namespace Engine
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the miner piece.
    /// </summary>
    internal static class Rook
    {
        /// <summary>
        /// Gets the moves for the miner piece. Miners can move and capture (including walls) any number of squares in the cardinal directions without jumping over other pieces.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the miner is on.</param>
        /// <param name="pos">The position of the miner.</param>
        internal static void GetMoves(Board board, Square square, int pos)
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
                    AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
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
                    AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Left
            destPos = pos;
            while (col > 0)
            {
                destPos--;
                if (destPos % Board.Size >= Board.Size - 1 || destPos < 0)
                    break;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }

            // Right
            destPos = pos;
            while (col < Board.Size - 1)
            {
                destPos++;
                if (destPos % Board.Size <= 0)
                    break;

                Square destSquare = board.Squares[destPos];

                if (destSquare.Piece == null)
                {
                    AddMove(board, square, destPos);
                    continue;
                }
                else if (destSquare.Piece.PieceColor != square.Piece.PieceColor)
                {
                    AddMove(board, square, destPos);
                    break;
                }
                else
                    break;
            }
        }

        /// <summary>
        /// Validates and adds a move to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the miner is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Add attack/defense values
            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            // Add move
            if (destSquare.Piece == null)
            {
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
                return;
            }

            // If destination piece is general, set check
            Moves.IsGeneralInCheck(board, destPos);

            square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
        }
    }
}