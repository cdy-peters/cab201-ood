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
        internal int PieceScore;
        internal List<int> ValidMoves;

        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
            PieceScore = piece.PieceScore;
            ValidMoves = piece.ValidMoves;
        }

        internal Piece(PieceColor pieceColor, PieceType pieceType)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            PieceScore = pieceType switch
            {
                PieceType.Wall => 0,
                PieceType.Zombie => 1,
                PieceType.Builder => 2,
                PieceType.Jester => 3,
                PieceType.Miner => 4,
                PieceType.Sentinel => 5,
                PieceType.Catapult => 6,
                PieceType.Dragon => 7,
                PieceType.General => 10000,
                _ => 0
            };
            ValidMoves = new List<int>();
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
