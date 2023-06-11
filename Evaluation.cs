namespace Advance
{
    /// <summary>
    /// Class containing the logic for evaluating the score of a board.
    /// </summary>
    internal static class Evaluation
    {
        /// <summary>
        /// Evaluates the score of a piece.
        /// </summary>
        /// <param name="square">The square that the piece is on</param>
        /// <param name="pos">The position of the piece in the game.</param>
        /// <returns>The score of the piece</returns>
        private static int PieceEvaluation(Square square, int pos)
        {
            int totalScore = 0;

            totalScore += square.Piece.PieceMaterialValue;

            return totalScore;
        }

        /// <summary>
        /// Evaluates the score of a board as a zero sum result.
        /// </summary>
        /// <param name="board">The board to evaluate</param>
        internal static void BoardEvaluation(Board board)
        {
            board.Score = 0;

            /// Penalty for being in check.
            if (board.WhiteCheck)
                board.Score -= 100;
            if (board.BlackCheck)
                board.Score += 100;

            /// Calculate the score of each piece on the board.
            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];
                if (square.Piece == null || square.Piece.PieceType == PieceType.Wall)
                    continue;

                if (square.Piece.PieceColor == PieceColor.White)
                    board.Score += PieceEvaluation(square, i);
                else
                    board.Score -= PieceEvaluation(square, i);
            }
        }
    }
}