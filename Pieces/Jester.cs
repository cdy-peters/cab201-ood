namespace Advance
{
    /// <summary>
    /// Class containing the logic for getting valid moves for the jester piece.
    /// </summary>
    internal static class Jester
    {
        /// <summary>
        /// Gets the moves for the jester piece. Jesters can move to any adjacent square and can swap with friendly pieces. Jesters cannot capture pieces but can convert enemy pieces to friendly pieces.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the jester is on.</param>
        /// <param name="pos">The position of the jester.</param>
        internal static void GetMoves(Board board, Square square, int pos)
        {
            int[] offsets = { -1, 0, 1 };
            foreach (int offsetY in offsets)
                foreach (int offsetX in offsets)
                {
                    if (offsetX == 0 && offsetY == 0)
                        continue;

                    int destPos = Moves.GetDestPos(pos, offsetX, offsetY);
                    if (destPos == -1)
                        continue;

                    Square destSquare = board.Squares[destPos];

                    AddMove(board, square, destPos);
                }
        }

        /// <summary>
        /// Validates and adds a move to the list of valid moves.
        /// </summary>
        /// <param name="board">The board to examine.</param>
        /// <param name="square">The square that the jester is on.</param>
        /// <param name="destPos">The position of the destination square.</param>
        private static void AddMove(Board board, Square square, int destPos)
        {
            Square destSquare = board.Squares[destPos];

            // Set destination square as threatened
            Moves.SetThreat(board, square, destPos);

            if (Piece.IsFriendlyPiece(square, destSquare))
                square.Piece.DefenseValue += destSquare.Piece.PieceActionValue;
            else if (Piece.IsEnemyPiece(square, destSquare))
                square.Piece.AttackValue += destSquare.Piece.PieceActionValue;

            if (destSquare.Piece == null ||
                (Piece.IsFriendlyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.Jester) ||
                (Piece.IsEnemyPiece(square, destSquare) && destSquare.Piece.PieceType != PieceType.General))
            {
                // Add move
                square.Piece.ValidMoves.Add(new MoveDest(destPos, false));
            }
        }
    }
}