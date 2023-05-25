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
        internal int PieceActionValue;
        internal int AttackValue;
        internal int DefenseValue;
        internal List<ValidMove> ValidMoves;

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
            PieceValue = piece.PieceValue;
            PieceActionValue = piece.PieceActionValue;
            ValidMoves = piece.ValidMoves;
        }

        internal Piece(PieceType pieceType, PieceColor pieceColor = PieceColor.None)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            PieceValue = pieceType switch
            {
                PieceType.Wall => 0,
                PieceType.Zombie => 100,
                PieceType.Builder => 200,
                PieceType.Jester => 300,
                PieceType.Miner => 400,
                PieceType.Sentinel => 500,
                PieceType.Catapult => 600,
                PieceType.Dragon => 700,
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
            ValidMoves = new List<ValidMove>();
        }

        public new string ToString()
        {
            return $"Piece: {PieceColor} {PieceType}";
        }
    }
}
