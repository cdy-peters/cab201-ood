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

            MoveContent bestMove = Search.IterativeSearch(Board, 1);
            if (bestMove.MovingPiece.SrcPos == 0 && bestMove.MovingPiece.DestPos.DestPos == 0)
                throw new Exception("No valid moves found");

            Board.MovePiece(Board, bestMove.MovingPiece.SrcPos, bestMove.MovingPiece.DestPos); // TODO: Make an overload that takes a MoveContent)

            FileIO.SaveFile(destFile, Board.ToString());
        }

        public override string ToString()
        {
            return $"Moving {Board!.LastMove.MovingPiece.SrcPos} to {Board.LastMove.MovingPiece.DestPos.DestPos}\n{Board}";
        }
    }
}