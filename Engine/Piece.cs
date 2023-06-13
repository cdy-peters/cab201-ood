namespace Engine
{
    /// <summary>
    /// Enumeration of the possible colors of a piece.
    /// </summary>
    internal enum PieceColor
    {
        White,
        Black
    }

    /// <summary>
    /// Enumeration of the possible types of pieces.
    /// </summary>
    internal enum PieceType
    {
        Pawn,
        Bishop,
        Knight,
        Rook,
        Queen,
        King
    }

    /// <summary>
    /// Structure representing the destination of a move.
    /// </summary>
    internal struct MoveDest
    {
        /// <summary>
        /// The position of the move destination.
        /// </summary>
        internal int Pos;

        /// <summary>
        /// Indicates whether the move destination is a wall.
        /// </summary>
        internal bool IsWall;

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
        /// <summary>
        /// The color of the piece.
        /// </summary>
        internal PieceColor PieceColor;

        /// <summary>
        /// The type of the piece.
        /// </summary>
        internal PieceType? PieceType;

        /// <summary>
        /// The source position of the move.
        /// </summary>
        internal int SrcPos;

        /// <summary>
        /// The destination of the move.
        /// </summary>
        internal MoveDest Dest;

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
        /// <summary>
        /// The color of the piece.
        /// </summary>
        internal PieceColor PieceColor;

        /// <summary>
        /// The type of the piece.
        /// </summary>
        internal PieceType PieceType;

        /// <summary>
        /// The material value of the piece.
        /// </summary>
        internal int PieceMaterialValue;

        /// <summary>
        /// The action value of the piece.
        /// </summary>
        internal int PieceActionValue;

        /// <summary>
        /// The attack value of the piece.
        /// </summary>
        internal int AttackValue;

        /// <summary>
        /// The defense value of the piece.
        /// </summary>
        internal int DefenseValue;

        /// <summary>
        /// The list of valid moves for the piece.
        /// </summary>
        internal List<MoveDest> ValidMoves;

        /// <summary>
        /// Creates a new piece.
        /// </summary>
        /// <param name="pieceType">The type of piece to create</param>
        /// <param name="pieceColor">The color of the piece to create</param>
        internal Piece(PieceType pieceType, PieceColor pieceColor)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            PieceMaterialValue = pieceType switch
            {
                PieceType.Pawn => 1,
                PieceType.Bishop => 2,
                PieceType.Knight => 3,
                PieceType.Rook => 4,
                PieceType.Queen => 5,
                PieceType.King => 10000,
                _ => 0
            };
            PieceActionValue = pieceType switch
            {
                PieceType.Pawn => 6,
                PieceType.Bishop => 5,
                PieceType.Knight => 4,
                PieceType.Rook => 3,
                PieceType.Queen => 2,
                PieceType.King => 1,
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

            if (square.Piece.PieceColor != destSquare.Piece.PieceColor)
                return true;

            return false;
        }
    }
}
