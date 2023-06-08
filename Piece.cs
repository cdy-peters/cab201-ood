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

    public struct MoveDest
    {
        internal int Pos;
        internal bool IsWall;

        internal MoveDest(int destPos, bool isWall = false)
        {
            Pos = destPos;
            IsWall = isWall;
        }
    }

    public struct MovingPiece
    {
        public PieceColor PieceColor;
        public PieceType PieceType;
        public int SrcPos;
        public MoveDest Dest;

        public MovingPiece(PieceColor pieceColor, PieceType pieceType, int srcPos, MoveDest dest)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            SrcPos = srcPos;
            Dest = dest;
        }

        public MovingPiece(MovingPiece MovingPiece)
        {
            PieceColor = MovingPiece.PieceColor;
            PieceType = MovingPiece.PieceType;
            SrcPos = MovingPiece.SrcPos;
            Dest = MovingPiece.Dest;
        }
    }

    internal class Piece
    {
        internal PieceColor PieceColor;
        internal PieceType PieceType;
        internal int PieceMaterialValue;
        internal int PieceActionValue;
        internal int AttackValue;
        internal int DefenseValue;
        internal List<MoveDest> ValidMoves;

        internal static bool IsFriendlyPiece(Square square, Square destSquare)
        {
            if (destSquare.Piece == null)
                return false;

            if (destSquare.Piece.PieceColor == PieceColor.None)
                return false;

            if (square.Piece.PieceColor == destSquare.Piece.PieceColor)
                return true;

            return false;
        }

        internal static bool IsEnemyPiece(Square square, Square destSquare)
        {
            if (destSquare.Piece == null)
                return false;

            if (destSquare.Piece.PieceColor == PieceColor.None)
                return false;

            if (square.Piece.PieceColor != destSquare.Piece.PieceColor)
                return true;
                
            return false;
        }

        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
            PieceMaterialValue = piece.PieceMaterialValue;
            PieceActionValue = piece.PieceActionValue;
            ValidMoves = piece.ValidMoves;
        }

        internal Piece(PieceType pieceType, PieceColor pieceColor = PieceColor.None)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            PieceMaterialValue = pieceType switch
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
            PieceActionValue = pieceType switch
            {
                PieceType.Wall => 0,
                PieceType.Zombie => 8,
                PieceType.Builder => 7,
                PieceType.Jester => 6,
                PieceType.Miner => 5,
                PieceType.Sentinel => 4,
                PieceType.Catapult => 3,
                PieceType.Dragon => 2,
                PieceType.General => 1,
                _ => 0
            };
            ValidMoves = new List<MoveDest>();
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
