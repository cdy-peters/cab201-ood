namespace Advance
{
    public enum PieceColor
    {
        None,
        White,
        Black
    }

    public enum PieceType
    {
        None,
        Wall,
        Zombie,
        Builder,
        Jester,
        Miner,
        Sentinel,
        Catapult,
        Dragon,
        General
    }

    internal class Piece
    {
        internal PieceColor PieceColor;
        internal PieceType PieceType;
        internal List<int> ValidMoves;

        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
            ValidMoves = piece.ValidMoves;
        }

        internal Piece(PieceColor pieceColor, PieceType pieceType)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            ValidMoves = new List<int>();
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
