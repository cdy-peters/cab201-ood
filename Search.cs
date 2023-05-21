namespace Advance
{
    internal static class Search
    {
        private struct Position
        {
            internal int SrcPos;
            internal int DestPos;
            internal int Score;
            // internal string Move;

            // public new string ToString()
            // {
            //     return Move;
            // }
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

        internal struct ResultBoards
        {
            internal List<Board> Positions;
        }

        internal static MoveContent IterativeSearch(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            MoveContent bestMove = new MoveContent();

            ResultBoards succ = GetSortValidMoves(board);

            foreach (Board pos in succ.Positions)
            {
                int value = -AlphaBeta(pos, depth - 1, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;
            }

            int currentBoard = 0;
            alpha = -100000000;
            succ.Positions.Sort(Sort);
            depth--;
            int plyDepthReached = ModifyDepth(depth, succ.Positions.Count);

            foreach (Board pos in succ.Positions)
            {
                currentBoard++;
                int value = -AlphaBeta(pos, plyDepthReached, -beta, -alpha);

                if (value >= 10000)
                    return pos.LastMove;

                pos.Score = value;

                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMove = pos.LastMove;
                }
            }
            plyDepthReached++;
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

            succ.Positions.Sort(Sort);
            return succ;
        }

        private static int AlphaBeta(Board board, int depth, int alpha, int beta)
        {
            if (depth == 0)
            {
                Evaluation.BoardEvaluation(board);
                return SideToMoveScore(board.Score, board.Player);
            }

            List<Position> positions = EvaluateMoves(board);

            if (board.WhiteCheck || board.BlackCheck || positions.Count == 0)
            {
                if (SearchForMate(board.Player, board, ref board.BlackMate, ref board.WhiteMate, ref board.StaleMate))
                {
                    if (board.WhiteMate)
                    {
                        if (board.Player == PieceColor.White)
                            return -10000 - depth;
                        return 10000 + depth;
                    }
                    if (board.BlackMate)
                    {
                        if (board.Player == PieceColor.Black)
                            return -10000 - depth;
                        return 10000 + depth;
                    }

                    return 0;
                }
            }

            positions.Sort(Sort);

            foreach (Position move in positions)
            {
                Board newBoard = board.CopyBoard();
                Board.MovePiece(newBoard, move.SrcPos, new ValidMove(move.DestPos));
                Moves.GetValidMoves(newBoard);

                if (newBoard.WhiteCheck)
                {
                    if (board.Player == PieceColor.White)
                        continue;
                }
                if (newBoard.BlackCheck)
                {
                    if (board.Player == PieceColor.Black)
                        continue;
                }

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

                    if (destPiece == null)
                        continue;

                    if (destPiece != null)
                    {
                        move.Score += destPiece.PieceValue;
                        if (piece.PieceValue < destPiece.PieceValue)
                            move.Score += destPiece.PieceValue - piece.PieceValue;
                    }
                    positions.Add(move);
                }
            }

            return positions;
        }

        internal static bool SearchForMate(PieceColor player, Board board, ref bool blackMate, ref bool whiteMate, ref bool staleMate)
        {
            bool foundNonCheckWhite = false;
            bool foundNonCheckBlack = false;

            for (int i = 0; i < Board.Size * Board.Size; i++)
            {
                Square square = board.Squares[i];

                if (square.Piece == null || square.Piece.PieceType == PieceType.Wall)
                    continue;

                if (square.Piece.PieceColor != player)
                    continue;

                foreach (ValidMove validMove in square.Piece.ValidMoves)
                {
                    Board newBoard = board.CopyBoard();
                    Board.MovePiece(newBoard, i, validMove);
                    Moves.GetValidMoves(newBoard);

                    if (newBoard.WhiteCheck == false)
                        foundNonCheckWhite = true;
                    else if (player == PieceColor.White)
                        continue;

                    if (newBoard.BlackCheck == false)
                        foundNonCheckBlack = true;
                    else if (player == PieceColor.Black)
                        continue;
                }
            }

            if (foundNonCheckWhite == false)
            {
                if (board.WhiteCheck)
                {
                    whiteMate = true;
                    return true;
                }
                if (!board.BlackMate && player != PieceColor.Black)
                {
                    staleMate = true;
                    return true;
                }
            }

            if (foundNonCheckBlack == false)
            {
                if (board.BlackCheck)
                {
                    blackMate = true;
                    return true;
                }
                if (!board.WhiteMate && player != PieceColor.White)
                {
                    staleMate = true;
                    return true;
                }
            }

            return false;
        }

        private static int ModifyDepth(int depth, int possibleMoves)
        {
            if (possibleMoves <= 20)
            {
                if (possibleMoves <= 10)
                    depth += 1;
                depth += 1;
            }

            return depth;
        }
    }
}