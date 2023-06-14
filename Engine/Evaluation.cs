namespace Engine
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
        /// <returns>The score of the piece</returns>
        private static int PieceEvaluation(Piece piece)
        {
            int totalScore = 0;

            totalScore += piece.PieceMaterialValue;
            totalScore += piece.DefenseValue;
            totalScore -= piece.AttackValue;

            if (piece.DefenseValue < piece.AttackValue)
                totalScore -= ((piece.AttackValue - piece.DefenseValue) * 10);

            totalScore += piece.ValidMoves.Count;

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
                board.Score -= 70;
            if (board.BlackCheck)
                board.Score += 70;

            /// Calculate the score of each piece on the board.
            for (int i = 0; i < 64; i++)
            {
                Piece piece = board.Squares[i].Piece;
                if (piece == null)
                    continue;

                if (piece.PieceColor == PieceColor.White)
                    board.Score += PieceEvaluation(piece);
                else
                    board.Score -= PieceEvaluation(piece);
            }
        }
    }
}