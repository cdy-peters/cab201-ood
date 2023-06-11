namespace Advance
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
        /// Creates a new Game with the source file and makes the best move, outputting the resulting board to the destination file.
        /// </summary>
        /// <param name="player">The player color.</param>
        /// <param name="srcFile">The source file.</param>
        /// <param name="destFile">The destination file.</param>
        internal Game(string player, string srcFile, string destFile)
        {
            PlayerColor = player == "white" ? PieceColor.White : PieceColor.Black;
            string boardStr = FileIO.LoadFile(srcFile);

            Board = new Board(boardStr);
            Moves.GetValidMoves(Board);

            MovingPiece bestMove = Search.ShallowSearchRoot(Board, 3);

            Board.MovePiece(Board, bestMove.SrcPos, bestMove.Dest);

            FileIO.SaveFile(destFile, Board.ToString());
        }

        /// <returns>A string representation of the move made and the resulting board.</returns>
        public override string ToString()
        {
            return $"Moving {Board!.LastMove.SrcPos} to {Board.LastMove.Dest.Pos}\n{Board}";
        }
    }
}