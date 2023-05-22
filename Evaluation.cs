namespace Advance
{
    internal static class Evaluation
    {
        private static int PieceEvaluation(Square square, int pos)
        {
            int totalScore = 0;

            totalScore += square.Piece.PieceValue;
            // TODO: Consider opponent pieces that are threatening this piece
            // TODO: Consider pieces that are protected by this piece
            // ? Consider if a pawn is in a poor position

            return totalScore;
        }

        internal static void BoardEvaluation(Board board)
        {
            board.Score = 0;

            if (board.WhiteCheck)
                board.Score += 1000;
            if (board.BlackCheck)
                board.Score -= 1000;

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