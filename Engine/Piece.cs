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

    internal struct MovingPieceAN
    {
        internal PieceType PieceType;
        internal char? File;
        internal int? Rank;
        internal int DestPos;
        internal bool Capture;
        internal bool Check;

        internal MovingPieceAN(PieceType pieceType, char? file, int? rank, int destPos, bool capture, bool check)
        {
            PieceType = pieceType;
            File = file;
            Rank = rank;
            DestPos = destPos;
            Capture = capture;
            Check = check;
        }
    }

    /// <summary>
    /// Structure representing a moving piece.
    /// </summary>
    internal struct MovingPiece
    {
        internal PieceColor PieceColor;
        internal PieceType? PieceType;

        internal int SrcPos;
        internal int DestPos;

        /// <summary>
        /// Creates a new moving piece.
        /// </summary>
        /// <param name="pieceColor">The color of the piece to move</param>
        /// <param name="pieceType">The type of piece to move</param>
        /// <param name="srcPos">The position of the piece to move</param>
        /// <param name="dest">The destination of the piece to move</param>
        internal MovingPiece(PieceColor pieceColor, PieceType pieceType, int srcPos, int destPos)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            SrcPos = srcPos;
            DestPos = destPos;
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
            DestPos = MovingPiece.DestPos;
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

        internal List<int> ValidMoves;

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
                PieceType.Pawn => 100,
                PieceType.Bishop => 325,
                PieceType.Knight => 320,
                PieceType.Rook => 500,
                PieceType.Queen => 975,
                PieceType.King => 32767,
                _ => 0
            };
            PieceActionValue = pieceType switch
            {
                PieceType.Pawn => 6,
                PieceType.Bishop => 3,
                PieceType.Knight => 3,
                PieceType.Rook => 2,
                PieceType.Queen => 1,
                PieceType.King => 1,
                _ => 0
            };
            ValidMoves = new List<int>();
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
    }
}
