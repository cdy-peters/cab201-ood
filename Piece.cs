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

    public struct ValidMove
    {
        internal int DestPos;
        internal bool IsWall;

        internal ValidMove(int destPos, bool isWall = false)
        {
            DestPos = destPos;
            IsWall = isWall;
        }
    }

    internal class Piece
    {
        internal PieceColor PieceColor;
        internal PieceType PieceType;
        internal int PieceValue;
        internal List<ValidMove> ValidMoves;

        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
            PieceValue = piece.PieceValue;
            ValidMoves = piece.ValidMoves;
        }

        internal Piece(PieceType pieceType, PieceColor pieceColor = PieceColor.None)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            PieceValue = pieceType switch
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
            ValidMoves = new List<ValidMove>();
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
