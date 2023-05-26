namespace Advance
{
    internal static class Search
    {
        private struct Position
        {
            internal int SrcPos;
            internal int DestPos;
            internal int Score;
        }

        internal struct ResultBoards
        {
            internal List<Board> Positions;
        }

        private static int Sort(Position s2, Position s1)
        {
            return (s1.Score).CompareTo(s2.Score);
        }

        private static int Sort(Board s2, Board s1)
        {
            return (s1.Score).CompareTo(s2.Score);
        }

        private static int SideToMoveScore(int score, PieceColor color)
        {
            if (color == PieceColor.Black)
                return -score;
            return score;
        }

        internal static MoveContent ShallowSearch(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            List<MoveContent> bestMoves = new List<MoveContent>(30);
            ResultBoards succ = GetSortValidMoves(board);
            succ.Positions.Sort(Sort);

            foreach (Board pos in succ.Positions)
            {
                int value = -AlphaBeta(pos, 1, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;
            }

            alpha = -100000000;

            foreach (Board pos in succ.Positions)
            {
                int value = -AlphaBeta(pos, 0, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;

                pos.Score = value;

                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMoves.Clear();
                    bestMoves.Add(pos.LastMove);
                }
                else if (value == alpha)
                    bestMoves.Add(pos.LastMove);
            }

            if (bestMoves.Count == 1)
                return bestMoves[0];
            else
            {
                List<MoveContent> bestMaterialMoves = new List<MoveContent>(30);

                for (int i = 0; i < bestMoves.Count; i++)
                {
                    ValidMove dest = bestMoves[i].MovingPiece.DestPos;
                    Square destSquare = board.Squares[dest.DestPos];
                    if (destSquare.Piece != null)
                        bestMaterialMoves.Add(bestMoves[i]);
                }

                if (bestMaterialMoves.Count == 1)
                    return bestMaterialMoves[0];

                // Deeper search
                return DeepSearch(board, depth);
            }
        }

        internal static MoveContent DeepSearch(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            MoveContent bestMove = new MoveContent();
            ResultBoards succ = GetSortValidMoves(board);
            succ.Positions.Sort(Sort);

            foreach (Board pos in succ.Positions)
            {
                int value = -AlphaBeta(pos, 1, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;
            }

            alpha = -100000000;
            depth--;

            foreach (Board pos in succ.Positions)
            {
                int value = -AlphaBeta(pos, depth, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;

                pos.Score = value;

                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMove = pos.LastMove;
                }
            }

            return bestMove;
        }

        private static ResultBoards GetSortValidMoves(Board board)
        {
            ResultBoards succ = new ResultBoards
            {
                Positions = new List<Board>(30)
            };

            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];

                if (square.Piece == null || square.Piece.PieceType == PieceType.Wall)
                    continue;

                if (square.Piece.PieceColor != board.Player)
                    continue;

                foreach (ValidMove validMove in square.Piece.ValidMoves)
                {
                    Board newBoard = board.CopyBoard();
                    Board.MovePiece(newBoard, i, validMove);
                    Moves.GetValidMoves(newBoard);

                    if (newBoard.WhiteCheck && board.Player == PieceColor.White)
                        continue;
                    if (newBoard.BlackCheck && board.Player == PieceColor.Black)
                        continue;

                    Evaluation.BoardEvaluation(newBoard);
                    newBoard.Score = SideToMoveScore(newBoard.Score, newBoard.Player);
                    succ.Positions.Add(newBoard);
                }
            }

            return succ;
        }

        private static int AlphaBeta(Board board, int depth, int alpha, int beta)
        {
            if (depth == 0)
            {
                if (board.WhiteCheck || board.BlackCheck)
                    return AlphaBeta(board, 1, alpha, beta);

                Evaluation.BoardEvaluation(board);
                return SideToMoveScore(board.Score, board.Player);
            }

            List<Position> positions = EvaluateMoves(board);
            positions.Sort(Sort);

            foreach (Position move in positions)
            {
                Board newBoard = board.CopyBoard();
                Board.MovePiece(newBoard, move.SrcPos, new ValidMove(move.DestPos));
                Moves.GetValidMoves(newBoard);

                if (newBoard.WhiteCheck && board.Player == PieceColor.White)
                    continue;
                if (newBoard.BlackCheck && board.Player == PieceColor.Black)
                    continue;

                int value = -AlphaBeta(newBoard, depth - 1, -beta, -alpha);

                if (value >= beta)
                    return beta; // beta-cutoff
                if (value > alpha)
                    alpha = value;
            }

            return alpha;
        }

        private static List<Position> EvaluateMoves(Board board)
        {
            List<Position> positions = new List<Position>();

            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Piece piece = board.Squares[i].Piece;

                if (piece == null || piece.PieceType == PieceType.Wall)
                    continue;

                if (piece.PieceColor != board.Player)
                    continue;

                foreach (ValidMove validMove in piece.ValidMoves)
                {
                    Position move = new Position();
                    move.SrcPos = i;
                    move.DestPos = validMove.DestPos;

                    Piece destPiece = board.Squares[move.DestPos].Piece;
                    // if (destPiece.PieceType == PieceType.Wall)
                    //     continue;

                    // ? Should walls go beyond this? what about miners moving to a wall?
                    if (destPiece != null)
                    {
                        move.Score += destPiece.PieceValue;
                        if (piece.PieceValue < destPiece.PieceValue)
                            move.Score += destPiece.PieceValue - piece.PieceValue;
                    }
                    move.Score += piece.PieceActionValue;

                    positions.Add(move);
                }
            }

            return positions;
        }
    }
}