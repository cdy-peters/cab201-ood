namespace Advance
{
    internal class Evaluation
    {
        private int PieceEvaluation(Square square, int pos)
        {
            int totalScore = 0;

            totalScore += square.Piece.PieceScore;
            // TODO: Consider pieces that are threatened by the opponent
            // TODO: Consider opponent pieces that are threatening this piece
            // TODO: Consider pieces that are protected by this piece
            // ? Consider if a pawn is in a poor position

            return totalScore;
        }

        internal int BoardEvaluation(Board board)
        {
            int totalScore = 0;

            if (board.WhiteInCheck)
            {
                totalScore += 1000;
            }
            else if (board.BlackInCheck)
            {
                totalScore -= 1000;
            }

            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];
                if (square.Piece.PieceType == PieceType.None || square.Piece.PieceType == PieceType.Wall)
                    continue;

                if (square.Piece.PieceColor == PieceColor.White)
                {
                    totalScore += PieceEvaluation(square, i);
                }
                else
                {
                    totalScore -= PieceEvaluation(square, i);
                }
            }

            return totalScore;
        }
    }
}