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

            // MoveContent bestMove = Search.ShallowSearch(Board, 3);
            MovingPiece bestMove = Search.ShallowSearch(Board, 3);
            if (bestMove.SrcPos == 0 && bestMove.DestPos.DestPos == 0)
                throw new Exception("No valid moves found");

            Board.MovePiece(Board, bestMove.SrcPos, bestMove.DestPos); // TODO: Make an overload that takes a MoveContent)

            FileIO.SaveFile(destFile, Board.ToString());
        }

        public override string ToString()
        {
            return $"Moving {Board!.LastMove.SrcPos} to {Board.LastMove.DestPos.DestPos}\n{Board}";
        }
    }
}