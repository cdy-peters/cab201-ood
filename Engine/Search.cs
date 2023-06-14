namespace Engine
{
    /// <summary>
    /// Class containing the logic for searching the board for the best move.
    /// </summary>
    internal static class Search
    {
        /// <summary>
        /// Structure representing a move and it's score.
        /// </summary>
        private struct Position
        {
            internal int SrcPos;
            internal int DestPos;

            internal int Score;
        }

        /// <summary>
        /// Structure representing a list of boards.
        /// </summary>
        private struct ResultBoards
        {
            internal List<Board> Boards;
        }

        /// <summary>
        /// Comparison function for sorting the moves by the score.
        /// </summary>
        private static int Sort(Position s2, Position s1)
        {
            return (s1.Score).CompareTo(s2.Score);
        }

        /// <summary>
        /// Comparison function for sorting the boards by the score.
        /// </summary>
        private static int Sort(Board s2, Board s1)
        {
            return (s1.Score).CompareTo(s2.Score);
        }

        /// <summary>
        /// Returns the score relative to the side moving. This is necessary for a zero sum evaluation.
        /// </summary>
        /// <param name="score">The score of the piece</param>
        /// <param name="color">The color of the piece</param>
        /// <returns>The score relative to the side moving. Positive for a white piece, negative for a black piece.</returns>
        private static int SideToMove(int score, PieceColor color)
        {
            return color == PieceColor.Black ? -score : score;
        }

        /// <summary>
        /// The root for a deep search of the board. Each of the equally best moves is played and the resulting board is evaluated.
        /// </summary>
        /// <param name="board">The board to search.</param>
        /// <param name="bestMoves">The best moves to play.</param>
        /// <param name="depth">The depth of the search.</param>
        /// <returns>The best move or null if there are no valid moves.</returns>
        internal static MovingPiece AlphaBetaRoot(Board board, int depth)
        {
            int alpha = -100000000;
            const int beta = 100000000;

            MovingPiece bestMove = new MovingPiece();
            ResultBoards resBoards = PlayValidMoves(board); /// Get a list of boards, each with a best move played.
            resBoards.Boards.Sort(Sort);

            alpha = -100000000;
            depth--;

            foreach (Board resBoard in resBoards.Boards)
            {
                int value = -AlphaBeta(resBoard, depth, -beta, -alpha);
                if (value >= 32767) /// If the resulting board is a checkmate.
                    return resBoard.LastMove;

                resBoard.Score = value;

                /// If the move is better than the previous move.
                if (value > alpha || alpha == -100000000)
                {
                    alpha = value;
                    bestMove = resBoard.LastMove;
                }
            }

            return bestMove;
        }

        /// <summary>
        /// Plays all the valid moves on the board and returns a list of boards with the valid moves played.
        /// </summary>
        /// <param name="board">The board to play the valid moves on.</param>
        /// <returns>A list of boards each with a valid move played.</returns>
        private static ResultBoards PlayValidMoves(Board board)
        {
            ResultBoards resBoards = new ResultBoards
            {
                Boards = new List<Board>(30)
            };

            /// Iterate through each piece of the moving player.
            for (int i = 0; i < 64; i++)
            {
                Square square = board.Squares[i];
                if (square.Piece == null)
                    continue;
                if (square.Piece.PieceColor != board.Player)
                    continue;

                foreach (int destPos in square.Piece.ValidMoves)
                {
                    /// Create a new board and make a move.
                    Board newBoard = board.CopyBoard();
                    Board.MovePiece(newBoard, i, destPos);
                    Moves.GetValidMoves(newBoard);

                    /// Check if a move puts the moving player in check.
                    if (newBoard.WhiteCheck && board.Player == PieceColor.White)
                        continue;
                    if (newBoard.BlackCheck && board.Player == PieceColor.Black)
                        continue;

                    /// Evaluate the board.
                    Evaluation.BoardEvaluation(newBoard);
                    newBoard.Score = SideToMove(newBoard.Score, newBoard.Player);
                    resBoards.Boards.Add(newBoard);
                }
            }

            return resBoards;
        }

        /// <summary>
        /// The alpha-beta pruning algorithm. This is a recursive algorithm that searches the board for the best move by iterating through the tree of moves.
        /// </summary>
        /// <param name="board">The board to search.</param>
        /// <param name="depth">The depth of the search.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <param name="beta">The beta value.</param>
        /// <returns>The score of the board.</returns>
        private static int AlphaBeta(Board board, int depth, int alpha, int beta)
        {
            if (depth == 0)
                return Quiesce(board, alpha, beta);

            // Evaluate the moves on the board, sorting them by the score.
            List<Position> positions = EvaluateMoves(board);
            positions.Sort(Sort);

            /// Iterate through each move.
            foreach (Position move in positions)
            {
                /// Create a new board and make a move.
                Board newBoard = board.CopyBoard();
                Board.MovePiece(newBoard, move.SrcPos, move.DestPos);
                Moves.GetValidMoves(newBoard);

                /// Check if a move puts the moving player in check.
                if (newBoard.WhiteCheck && board.Player == PieceColor.White)
                    continue;
                if (newBoard.BlackCheck && board.Player == PieceColor.Black)
                    continue;

                /// Recursively call the algorithm to have the next player make a move.
                int value = -AlphaBeta(newBoard, depth - 1, -beta, -alpha);

                /// If the move is worse than the previous move, don't consider it.
                if (value >= beta)
                    return beta; // beta-cutoff

                /// If the move is better than the previous move, update the alpha value.
                if (value > alpha)
                    alpha = value;
            }

            return alpha;
        }

        /// <summary>
        /// Evaluates the moves on the board and returns a list of moves sorted by the score.
        /// </summary>
        /// <param name="board">The board to evaluate.</param>
        /// <returns>A list of moves sorted by the score.</returns>
        private static List<Position> EvaluateMoves(Board board)
        {
            List<Position> positions = new List<Position>();

            /// Iterate through each piece of the moving player.
            for (int i = 0; i < 64; i++)
            {
                Piece piece = board.Squares[i].Piece;
                if (piece == null)
                    continue;
                if (piece.PieceColor != board.Player)
                    continue;

                /// Iterate through each valid move of the piece.
                foreach (int destPos in piece.ValidMoves)
                {
                    Position move = new Position();
                    move.SrcPos = i;
                    move.DestPos = destPos;

                    Piece destPiece = board.Squares[move.DestPos].Piece;
                    if (destPiece != null)
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

        /// <summary>
        /// Performs a quiescence search on the board to capture captures and check for winning moves if necessary.
        /// Otherwise, evaluates the board position directly.
        /// </summary>
        /// <param name="board">The current board state.</param>
        /// <param name="alpha">The alpha value for alpha-beta pruning.</param>
        /// <param name="beta">The beta value for alpha-beta pruning.</param>
        /// <returns>The score of the board after the quiescence search or direct evaluation.</returns>
        private static int Quiesce(Board board, int alpha, int beta)
        {
            /// If the last move was a possible winning move, continue the search until a 'quiet' position is found.
            if (board.WhiteCheck || board.BlackCheck)
                return AlphaBeta(board, 1, alpha, beta);

            /// Return the score of the board.
            Evaluation.BoardEvaluation(board);
            return SideToMove(board.Score, board.Player);
        }
    }
}