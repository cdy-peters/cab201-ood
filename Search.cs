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

        private static int SideToMove(int score, PieceColor color)
        {
            return color == PieceColor.Black ? -score : score;
        }

        internal static MovingPiece ShallowSearchRoot(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            List<MovingPiece> bestMoves = new List<MovingPiece>(30);
            ResultBoards resBoards = PlayValidMoves(board);
            resBoards.Positions.Sort(Sort);

            foreach (Board resBoard in resBoards.Positions)
            {
                int value = -AlphaBeta(resBoard, 1, -beta, -alpha);

                if (value >= 10000)
                    return resBoard.LastMove;
            }

            alpha = -100000000;

            foreach (Board resBoard in resBoards.Positions)
            {
                int value = -AlphaBeta(resBoard, 0, -beta, -alpha);

                if (value >= 10000)
                    return resBoard.LastMove;

                resBoard.Score = value;

                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMoves.Clear();
                    bestMoves.Add(resBoard.LastMove);
                }
                else if (value == alpha)
                    bestMoves.Add(resBoard.LastMove);
            }

            if (bestMoves.Count == 1)
                return bestMoves[0];
            else
            {
                List<MovingPiece> bestMaterialMoves = new List<MovingPiece>(30);

                for (int i = 0; i < bestMoves.Count; i++)
                {
                    MoveDest dest = bestMoves[i].Dest;
                    Square destSquare = board.Squares[dest.Pos];
                    if (destSquare.Piece != null)
                        bestMaterialMoves.Add(bestMoves[i]);
                }

                if (bestMaterialMoves.Count == 1)
                    return bestMaterialMoves[0];

                // Deeper search
                return DeepSearchRoot(board, depth);
            }
        }

        internal static MovingPiece DeepSearchRoot(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            MovingPiece bestMove = new MovingPiece();
            ResultBoards resBoards = PlayValidMoves(board);
            resBoards.Positions.Sort(Sort);

            foreach (Board resBoard in resBoards.Positions)
            {
                int value = -AlphaBeta(resBoard, 1, -beta, -alpha);

                if (value >= 10000)
                    return resBoard.LastMove;
            }

            alpha = -100000000;
            depth--;

            foreach (Board resBoard in resBoards.Positions)
            {
                int value = -AlphaBeta(resBoard, depth, -beta, -alpha);

                if (value >= 10000)
                    return resBoard.LastMove;

                resBoard.Score = value;

                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMove = resBoard.LastMove;
                }
            }

            return bestMove;
        }

        private static ResultBoards PlayValidMoves(Board board)
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

                foreach (MoveDest moveDest in square.Piece.ValidMoves)
                {
                    Board newBoard = board.CopyBoard();
                    Board.MovePiece(newBoard, i, moveDest);
                    Moves.GetValidMoves(newBoard);

                    if (newBoard.WhiteCheck && board.Player == PieceColor.White)
                        continue;
                    if (newBoard.BlackCheck && board.Player == PieceColor.Black)
                        continue;

                    Evaluation.BoardEvaluation(newBoard);
                    newBoard.Score = SideToMove(newBoard.Score, newBoard.Player);
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
                return SideToMove(board.Score, board.Player);
            }

            List<Position> positions = EvaluateMoves(board);
            positions.Sort(Sort);

            foreach (Position move in positions)
            {
                Board newBoard = board.CopyBoard();
                Board.MovePiece(newBoard, move.SrcPos, new MoveDest(move.DestPos));
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

                foreach (MoveDest moveDest in piece.ValidMoves)
                {
                    Position move = new Position();
                    move.SrcPos = i;
                    move.DestPos = moveDest.Pos;

                    Piece destPiece = board.Squares[move.DestPos].Piece;

                    if (destPiece != null && destPiece.PieceType != PieceType.Wall)
                    {
                        move.Score += destPiece.PieceMaterialValue;
                        if (piece.PieceMaterialValue < destPiece.PieceMaterialValue)
                            move.Score += destPiece.PieceMaterialValue - piece.PieceMaterialValue;
                    }
                    move.Score += piece.PieceActionValue;

                    positions.Add(move);
                }
            }

            return positions;
        }
    }
}