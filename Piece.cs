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
        Miner,
        Jester,
        Sentinel,
        Catapult,
        Dragon,
        General
    }

    internal class Piece
    {
        internal PieceColor PieceColor;
        internal PieceType PieceType;

        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
        }

        internal Piece(PieceColor pieceColor, PieceType pieceType)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
