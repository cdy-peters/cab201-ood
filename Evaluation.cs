namespace Advance
{
    internal static class Evaluation
    {
        private static int PieceEvaluation(Square square, int pos)
        {
            int totalScore = 0;

            totalScore += square.Piece.PieceValue;
            totalScore -= square.Piece.AttackValue;
            totalScore += square.Piece.DefenseValue;

            // ? Consider if a pawn is in a poor position

            return totalScore;
        }

        internal static void BoardEvaluation(Board board)
        {
            board.Score = 0;

            if (board.WhiteCheck)
                board.Score -= 250;
            if (board.BlackCheck)
                board.Score += 250;

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