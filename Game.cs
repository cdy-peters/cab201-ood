namespace Advance
{
    internal class Game
    {
        internal static PieceColor PlayerColor;
        internal Board Board;

        internal Game(string player, string srcFile, string destFile)
        {
            PlayerColor = player == "white" ? PieceColor.White : PieceColor.Black;
            string boardStr = FileIO.LoadFile(srcFile);

            Board = new Board(boardStr);
            Moves.GetValidMoves(Board);

            MovingPiece bestMove = Search.ShallowSearchRoot(Board, 3);
            if (bestMove.SrcPos == -1 && bestMove.Dest.Pos == -1)
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