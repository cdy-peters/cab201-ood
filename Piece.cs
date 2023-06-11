namespace Advance
{
    /// <summary>
    /// Enumeration of the possible colors of a piece.
    /// </summary>
    internal enum PieceColor
    {
        None,
        White,
        Black
    }

    /// <summary>
    /// Enumeration of the possible types of pieces.
    /// </summary>
    internal enum PieceType
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

    /// <summary>
    /// Structure representing the destination of a move.
    /// </summary>
    internal struct MoveDest
    {
        internal int Pos;
        internal bool IsWall;

        /// <summary>
        /// Creates a new move destination, instantiated with an invalid move.
        /// </summary>
        public MoveDest()
        {
            Pos = -1;
            IsWall = false;
        }

        /// <summary>
        /// Creates a new move destination.
        /// </summary>
        internal MoveDest(int destPos, bool isWall = false)
        {
            Pos = destPos;
            IsWall = isWall;
        }
    }

    /// <summary>
    /// Structure representing a moving piece.
    /// </summary>
    internal struct MovingPiece
    {
        internal PieceColor PieceColor;
        internal PieceType ?PieceType;
        internal int SrcPos;
        internal MoveDest Dest;

        /// <summary>
        /// Creates a new moving piece, instantiated with an invalid move.
        /// </summary>
        public MovingPiece()
        {
            PieceColor = PieceColor.None;
            PieceType = null;
            SrcPos = -1;
            Dest = new MoveDest();
        }

        /// <summary>
        /// Creates a new moving piece.
        /// </summary>
        /// <param name="pieceColor">The color of the piece to move</param>
        /// <param name="pieceType">The type of piece to move</param>
        /// <param name="srcPos">The position of the piece to move</param>
        /// <param name="dest">The destination of the piece to move</param>
        internal MovingPiece(PieceColor pieceColor, PieceType pieceType, int srcPos, MoveDest dest)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            SrcPos = srcPos;
            Dest = dest;
        }

        /// <summary>
        /// Creates a copy of a moving piece.
        /// </summary>
        /// <param name="MovingPiece">The moving piece to copy</param>
        internal MovingPiece(MovingPiece MovingPiece)
        {
            PieceColor = MovingPiece.PieceColor;
            PieceType = MovingPiece.PieceType;
            SrcPos = MovingPiece.SrcPos;
            Dest = MovingPiece.Dest;
        }
    }

    /// <summary>
    /// Class representing a piece.
    /// </summary>
    internal class Piece
    {
        internal PieceColor PieceColor;
        internal PieceType PieceType;
        internal int PieceMaterialValue;
        internal int PieceActionValue;
        internal int AttackValue;
        internal int DefenseValue;
        internal List<MoveDest> ValidMoves;

        /// <summary>
        /// Creates a new piece.
        /// </summary>
        /// <param name="pieceType">The type of piece to create</param>
        /// <param name="pieceColor">The color of the piece to create</param>
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

        /// <summary>
        /// Creates a copy of a piece.
        /// </summary>
        /// <param name="piece">The piece to copy</param>
        internal Piece(Piece piece)
        {
            PieceColor = piece.PieceColor;
            PieceType = piece.PieceType;
            PieceMaterialValue = piece.PieceMaterialValue;
            PieceActionValue = piece.PieceActionValue;
            ValidMoves = piece.ValidMoves;
        }

        /// <summary>
        /// Checks if the destination square is the same as the moving piece.
        /// </summary>
        /// <param name="square">The square of the moving piece</param>
        /// <param name="destSquare">The destination square of the moving piece</param>
        /// <returns>True if the destination square is the same as the moving piece, false otherwise.</returns>
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

        /// <summary>
        /// Checks if the destination square is an enemy piece.
        /// </summary>
        /// <param name="square">The square of the moving piece</param>
        /// <param name="destSquare">The destination square of the moving piece</param>
        /// <returns>True if the destination square is an enemy piece, false otherwise.</returns>
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
    }
}
