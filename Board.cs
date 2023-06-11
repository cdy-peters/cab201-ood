using System.Text.RegularExpressions;

namespace Advance
{
    /// <summary>
    /// Class representing the board.
    /// </summary>
    internal class Board
    {
        /// <summary>
        /// The size of the board.
        /// </summary>
        internal const int Size = 9;

        /// <summary>
        /// The current player color.
        /// </summary>
        internal PieceColor Player;

        /// <summary>
        /// The current score of the game.
        /// </summary>
        internal int Score = 0;

        /// <summary>
        /// The last move made on the board.
        /// </summary>
        internal MovingPiece LastMove;

        /// <summary>
        /// An array representing the squares on the board.
        /// </summary>
        internal Square[] Squares;

        /// <summary>
        /// An array indicating the squares threatened by white pieces.
        /// </summary>
        internal bool[] ThreatenedByWhite = new bool[Size * Size];

        /// <summary>
        /// An array indicating the squares threatened by black pieces.
        /// </summary>
        internal bool[] ThreatenedByBlack = new bool[Size * Size];

        /// <summary>
        /// Indicates whether the white player is in check.
        /// </summary>
        internal bool WhiteCheck = false;

        /// <summary>
        /// Indicates whether the black player is in check.
        /// </summary>
        internal bool BlackCheck = false;

        /// <summary>
        /// Creates a new Board.
        /// </summary>
        private Board()
        {
            Squares = new Square[Size * Size];
            Player = Game.PlayerColor;
            LastMove = new MovingPiece();
            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        /// <summary>
        /// Creates a Board from a string.
        /// </summary>
        /// <param name="boardStr">The string to create the Board from.</param>
        internal Board(string boardStr) : this()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                char c = boardStr[i];

                if (c == '.')
                    continue;
                else if (c == '#')
                {
                    Squares[i].Piece = new Piece(PieceType.Wall);
                    continue;
                }

                PieceColor pieceColor = Char.IsUpper(c) ? PieceColor.White : PieceColor.Black;
                PieceType pieceType = Char.ToLower(c) switch
                {
                    'z' => PieceType.Zombie,
                    'b' => PieceType.Builder,
                    'm' => PieceType.Miner,
                    'j' => PieceType.Jester,
                    's' => PieceType.Sentinel,
                    'c' => PieceType.Catapult,
                    'd' => PieceType.Dragon,
                    'g' => PieceType.General,
                    _ => throw new ArgumentException("An invalid piece was found in the board string.")
                };
                Squares[i].Piece = new Piece(pieceType, pieceColor);
            }
        }

        /// <summary>
        /// Creates a Board from an array of Squares.
        /// </summary>
        /// <param name="squares">The array of Squares to create the Board from.</param>
        private Board(Square[] squares) : this()
        {
            for (int i = 0; i < Size * Size; i++)
                if (squares[i].Piece != null)
                    Squares[i] = new Square(squares[i].Piece);
        }

        /// <summary>
        /// Creates a copy of the board. This is useful when you want to change the state of the board without affecting the original.
        /// </summary>
        /// <returns>A copy of the board.</returns>
        internal Board CopyBoard()
        {
            Board newBoard = new Board(Squares);

            newBoard.Player = Player;

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];

            return newBoard;
        }

        /// <summary>
        /// Moves a piece and updates the board.
        /// </summary>
        /// <param name="board">The board to update.</param>
        /// <param name="srcPos">The position of the piece to move.</param>
        /// <param name="moveDest">The destination to move the piece</param>
        internal static void MovePiece(Board board, int srcPos, MoveDest moveDest)
        {
            int destPos = moveDest.Pos;
            Piece srcPiece = board.Squares[srcPos].Piece;
            Piece destPiece = board.Squares[destPos].Piece;

            board.LastMove = new MovingPiece(srcPiece.PieceColor, srcPiece.PieceType, srcPos, moveDest);

            board.Player = board.Player == PieceColor.White ? PieceColor.Black : PieceColor.White;

            // Builder 
            if (srcPiece.PieceType == PieceType.Builder && moveDest.IsWall)
            {
                board.Squares[destPos].Piece = new Piece(PieceType.Wall);
                return;
            }

            // Catapult move/shot
            if (srcPiece.PieceType == PieceType.Catapult)
            {
                // Check if the catapult is moving
                int diff = Math.Abs(srcPos - destPos);
                if (diff == 1 || diff == Size)
                {
                    board.Squares[srcPos].Piece = null!;
                    board.Squares[destPos].Piece = srcPiece;
                    return;
                }

                board.Squares[destPos].Piece = null!;
                return;
            }

            // Jester abilities
            if (srcPiece.PieceType == PieceType.Jester && (destPiece != null))
            {
                /// Swap friendly pieces
                if (destPiece.PieceColor == srcPiece.PieceColor)
                {
                    board.Squares[srcPos].Piece = destPiece;
                    board.Squares[destPos].Piece = srcPiece;
                    return;
                }

                /// Convert enemy piece
                if (destPiece.PieceColor != PieceColor.None)
                {
                    board.Squares[destPos].Piece.PieceColor = srcPiece.PieceColor;
                    return;
                }
            }

            // Generic move
            board.Squares[srcPos].Piece = null!;
            board.Squares[destPos].Piece = srcPiece;
        }

        /// <summary>
        /// Converts the Board to a string.
        /// </summary>
        /// <returns>A string representation of the Board.</returns>
        public override string ToString()
        {
            string boardStr = "";

            foreach (Square square in Squares)
            {
                if (square.Piece == null)
                {
                    boardStr += ".";
                    continue;
                }

                PieceType type = square.Piece.PieceType;
                PieceColor color = square.Piece.PieceColor;

                if (type == PieceType.Wall)
                    boardStr += "#";
                else
                {
                    char c = type.ToString()[0];
                    if (color == PieceColor.Black)
                        c = Char.ToLower(c);

                    boardStr += c;
                }
            }

            return Regex.Replace(boardStr, $".{{{Size}}}", "$0\n");
        }
    }
}