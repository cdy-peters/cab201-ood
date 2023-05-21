namespace Advance
{
    // ! Currently using PieceColor from Piece.cs
    // public enum PlayerColor
    // {
    //     White,
    //     Black
    // }

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

            MoveContent bestMove = Search.IterativeSearch(Board, 7);
            Console.WriteLine($"Moving {bestMove.MovingPiece.SrcPos} to {bestMove.MovingPiece.DestPos.DestPos}");
            if (bestMove.MovingPiece.SrcPos == 0 && bestMove.MovingPiece.DestPos.DestPos == 0)
                throw new Exception("No valid moves found");

            Board.MovePiece(Board, bestMove.MovingPiece.SrcPos, bestMove.MovingPiece.DestPos); // TODO: Make an overload that takes a MoveContent)

            FileIO.SaveFile(destFile, Board.ToString());
        }

        public override string ToString()
        {
            return $"Game:\n{Board}";
        }
    }
}