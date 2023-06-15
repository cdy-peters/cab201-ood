using System.Text.RegularExpressions;

namespace Engine
{
    /// <summary>
    /// Class representing the board.
    /// </summary>
    internal class Board
    {
        internal PieceColor Player;
        internal int Score = 0;
        internal MovingPiece LastMove;
        internal Square[] Squares;

        internal bool[] ThreatenedByWhite = new bool[64];
        internal bool[] ThreatenedByBlack = new bool[64];

        internal bool WhiteCheck = false;
        internal bool BlackCheck = false;

        internal bool WhiteCastleKingSide = true;
        internal bool WhiteCastleQueenSide = true;
        internal bool BlackCastleKingSide = true;
        internal bool BlackCastleQueenSide = true;

        internal int EnPassant = -1;

        internal int HalfMoveClock = 0;
        internal int FullMoves = 1;

        /// <summary>
        /// Creates a new Board.
        /// </summary>
        private Board()
        {
            Squares = new Square[64];
            Player = Game.PlayerColor;
            LastMove = new MovingPiece();
            ThreatenedByWhite = new bool[64];
            ThreatenedByBlack = new bool[64];
        }

        /// <summary>
        /// Creates a Board from a string.
        /// </summary>
        /// <param name="fen">The FEN string to create the Board from.</param>
        internal Board(string fen) : this() // ! Currently assumes a valid FEN string
        {
            int charIdx = 0;

            // Set pieces
            int squareIdx = 0;
            while (fen[charIdx] != ' ')
            {
                char c = fen[charIdx];

                if (c == '/')
                {
                    charIdx++;
                    continue;
                }

                if (Char.IsDigit(c))
                {
                    squareIdx += (int)Char.GetNumericValue(c);
                    charIdx++;
                    continue;
                }

                PieceColor pieceColor = Char.IsUpper(c) ? PieceColor.White : PieceColor.Black;
                PieceType pieceType = Char.ToLower(c) switch
                {
                    'p' => PieceType.Pawn,
                    'b' => PieceType.Bishop,
                    'n' => PieceType.Knight,
                    'r' => PieceType.Rook,
                    'q' => PieceType.Queen,
                    'k' => PieceType.King,
                    _ => throw new ArgumentException("An invalid piece was found in the board string.")
                };
                Squares[squareIdx++].Piece = new Piece(pieceType, pieceColor);
                charIdx++;
            }
            charIdx++;

            // Set color
            Player = fen[charIdx] switch
            {
                'w' => PieceColor.White,
                'b' => PieceColor.Black,
                _ => throw new ArgumentException("An invalid player was found in the board string.")
            };

            // Set castling
            charIdx += 2;
            while (fen[charIdx] != ' ')
            {
                char c = fen[charIdx];

                switch (c)
                {
                    case 'K':
                        WhiteCastleKingSide = true;
                        break;
                    case 'Q':
                        WhiteCastleQueenSide = true;
                        break;
                    case 'k':
                        BlackCastleKingSide = true;
                        break;
                    case 'q':
                        BlackCastleQueenSide = true;
                        break;
                    case '-':
                        break;
                    default:
                        throw new ArgumentException("An invalid castling was found in the board string.");
                }

                charIdx++;
            }

            // Set en passant
            charIdx++;
            if (fen[charIdx] != '-')
                EnPassant = FromAN(fen.Substring(charIdx++, 2));
            else
                EnPassant = -1;

            // Set halfmove clock
            charIdx += 2;
            try
            {
                HalfMoveClock = int.Parse(fen.Substring(charIdx, 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                HalfMoveClock = 0;
                FullMoves = 1;
                return;
            }

            // Set fullmove number
            charIdx += 2;
            FullMoves = int.Parse(fen.Substring(charIdx, 1));
        }

        /// <summary>
        /// Creates a Board from an array of Squares.
        /// </summary>
        /// <param name="squares">The array of Squares to create the Board from.</param>
        private Board(Square[] squares) : this()
        {
            for (int i = 0; i < 64; i++)
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

            ThreatenedByWhite = new bool[64];
            ThreatenedByBlack = new bool[64];

            return newBoard;
        }

        /// <summary>
        /// Moves a piece and updates the board.
        /// </summary>
        /// <param name="board">The board to update.</param>
        /// <param name="srcPos">The position of the piece to move.</param>
        /// <param name="moveDest">The destination to move the piece</param>
        internal static void MovePiece(Board board, int srcPos, int destPos)
        {
            Piece srcPiece = board.Squares[srcPos].Piece;
            Piece destPiece = board.Squares[destPos].Piece;

            board.LastMove = new MovingPiece(srcPiece.PieceColor, srcPiece.PieceType, srcPos, destPos);

            board.Player = board.Player == PieceColor.White ? PieceColor.Black : PieceColor.White;

            // Generic move
            board.Squares[srcPos].Piece = null!;
            board.Squares[destPos].Piece = srcPiece;
        }

        internal bool IsValidMoveAN(string move)
        {
            int srcPos = FromAN(move.Substring(0, 2));
            int destPos = FromAN(move.Substring(2, 2));

            Piece srcPiece = Squares[srcPos].Piece;
            foreach (int moveDest in srcPiece.ValidMoves)
                if (moveDest == destPos)
                    return true;

            return false;
        }

        /// <summary>
        /// Converts the Board to a FEN string.
        /// </summary>
        /// <param name="board">The Board to convert.</param>
        internal string ToFEN()
        {
            string fen = "";

            // Get pieces
            int emptySquares = 0;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    if (emptySquares > 0)
                    {
                        fen += emptySquares;
                        emptySquares = 0;
                    }
                    fen += "/";
                }

                if (this.Squares[i].Piece == null)
                {
                    emptySquares++;
                    continue;
                }

                if (emptySquares > 0)
                {
                    fen += emptySquares;
                    emptySquares = 0;
                }

                PieceType type = this.Squares[i].Piece.PieceType;
                PieceColor color = this.Squares[i].Piece.PieceColor;
                char c;

                if (type == PieceType.Knight)
                    c = 'N';
                else
                    c = type.ToString()[0];

                if (color == PieceColor.Black)
                    c = Char.ToLower(c);

                fen += c;
            }

            if (emptySquares > 0)
                fen += emptySquares;

            // Get color
            fen += " ";
            fen += this.Player == PieceColor.White ? "w" : "b";

            // Get Castling
            fen += " ";
            if (this.WhiteCastleKingSide)
                fen += "K";
            if (this.WhiteCastleQueenSide)
                fen += "Q";
            if (this.BlackCastleKingSide)
                fen += "k";
            if (this.BlackCastleQueenSide)
                fen += "q";
            if (!this.WhiteCastleKingSide && !this.WhiteCastleQueenSide && !this.BlackCastleKingSide && !this.BlackCastleQueenSide)
                fen += "-";

            // Get En Passant
            fen += " ";
            if (this.EnPassant == -1)
                fen += "-";
            else
                fen += ToAN(this.EnPassant);

            // Get Halfmove Clock
            fen += " ";
            fen += this.HalfMoveClock;

            // Get Fullmove Number
            fen += " ";
            fen += this.FullMoves;

            return fen;
        }

        internal static int FromAN(string an)
        {
            int file = an[0] - 'a';
            int rank = 8 - (an[1] - '1') - 1;

            return rank * 8 + file;
        }

        internal static string ToAN(int pos)
        {
            int file = pos % 8;
            int rank = 8 - (pos / 8) - 1;

            char fileChar = (char)('a' + file);
            char rankChar = (char)('1' + rank);

            return fileChar.ToString() + rankChar.ToString();
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
                char c;

                if (type == PieceType.Knight)
                    c = 'N';
                else
                    c = type.ToString()[0];

                if (color == PieceColor.Black)
                    c = Char.ToLower(c);

                boardStr += c;

            }

            return Regex.Replace(boardStr, $".{{{8}}}", "$0\n");
        }
    }
}