namespace Chess
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
        /// <param name="deep">Whether to evaluate the piece deeply</param>
        /// <returns>The score of the piece</returns>
        private static int PieceEvaluation(Piece piece, bool deep)
        {
            int totalScore = 0;

            totalScore += piece.PieceMaterialValue;
            if (deep)
            {
                totalScore += piece.DefenseValue;
                totalScore -= piece.AttackValue;
            }

            return totalScore;
        }

        /// <summary>
        /// Evaluates the score of a board as a zero sum result.
        /// </summary>
        /// <param name="board">The board to evaluate</param>
        /// <param name="deep">Whether to evaluate the board deeply</param>
        internal static void BoardEvaluation(Board board, bool deep)
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
                Piece piece = board.Squares[i].Piece;
                if (piece == null)
                    continue;

                if (piece.PieceColor == PieceColor.White)
                    board.Score += PieceEvaluation(piece, deep);
                else
                    board.Score -= PieceEvaluation(piece, deep);
            }
        }
    }
}