namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the dragon piece.
    /// </summary>
    internal static class Dragon
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

            // Up
            destPos = pos;
            while (destPos >= Board.Size)
            {
                destPos -= Board.Size;

                if (destPos == pos - Board.Size)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size)
            {
                destPos += Board.Size;

                if (destPos == pos + Board.Size)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Left
            destPos = pos;
            while (destPos % Board.Size != 0)
            {
                destPos--;

                if (destPos == pos - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Right
            destPos = pos;
            while (destPos % Board.Size != Board.Size - 1)
            {
                destPos++;

                if (destPos == pos + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Up Left
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != 0)
            {
                destPos -= Board.Size + 1;

                if (destPos == pos - Board.Size - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Up Right
            destPos = pos;
            while (destPos >= Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos -= Board.Size - 1;

                if (destPos == pos - Board.Size + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down Left
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != 0)
            {
                destPos += Board.Size - 1;

                if (destPos == pos + Board.Size - 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }

            // Down Right
            destPos = pos;
            while (destPos < Board.Size * Board.Size - Board.Size && destPos % Board.Size != Board.Size - 1)
            {
                destPos += Board.Size + 1;

                if (destPos == pos + Board.Size + 1)
                    AddMove(board, square, destPos);
                else
                    AddCapture(board, square, destPos);

                Square destSquare = board.Squares[destPos];
                if (destSquare.Piece != null)
                    break;
            }
        }

        /// <summary>
        /// Validates and adds a move (not a capture) to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the dragon is on.</param>
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
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
        }

        /// <summary>
        /// Validates and adds a capture to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the dragon is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddCapture(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Check if destination piece is protected by a sentinel
            Piece? sentinel = Moves.IsProtected(board, square, destPos);
            if (sentinel != null && sentinel.PieceColor == square.Piece.PieceColor) // Protected by friendly sentinel
                Moves.SetThreat(board, square, destPos);

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

            // Capture only if not protected by a sentinel
            if (sentinel == null && Piece.IsEnemyPiece(square, destSquare))
            {
                // If destination piece is general, set check
                Moves.IsGeneralInCheck(board, destPos);

                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }
    }
}