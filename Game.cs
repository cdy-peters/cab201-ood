namespace Advance
{
    public class Game
    {
        internal static PieceColor PlayerColor { get; private set; }
        internal Board? Board { get; }

        public Game(string player, string srcFile, string destFile)
        {
            PlayerColor = player == "white" ? PieceColor.White : PieceColor.Black;
            string boardStr = FileIO.LoadFile(srcFile);

            Board = new Board(boardStr);

            Moves.GetValidMoves(Board);

            MovingPiece bestMove = Search.ShallowSearchRoot(Board, 3);
            if (bestMove.SrcPos == 0 && bestMove.Dest.Pos == 0)
                throw new Exception("No valid moves found");

            Board.MovePiece(Board, bestMove.SrcPos, bestMove.Dest);

            FileIO.SaveFile(destFile, Board.ToString());
        }

        public override string ToString()
        {
            return $"Moving {Board!.LastMove.SrcPos} to {Board.LastMove.Dest.Pos}\n{Board}";
        }
    }
}