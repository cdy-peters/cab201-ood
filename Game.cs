namespace Chess
{
    /// <summary>
    /// Class representing the game.
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// The color of the player making a move.
        /// </summary>
        internal static PieceColor PlayerColor;

        /// <summary>
        /// The game board.
        /// </summary>
        internal Board Board;

        /// <summary>
        /// Creates a new Game and makes the best move.
        /// </summary>
        internal Game()
        {
            Board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Moves.GetValidMoves(Board);

            MovingPiece bestMove = Search.AlphaBetaRoot(Board, 3);

            Board.MovePiece(Board, bestMove.SrcPos, bestMove.Dest);
        }

        /// <returns>A string representation of the move made and the resulting board.</returns>
        public override string ToString()
        {
            return $"Moving {Board!.LastMove.SrcPos} to {Board.LastMove.Dest.Pos}\n{Board}";
        }
    }
}