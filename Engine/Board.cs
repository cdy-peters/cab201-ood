using System.Text.RegularExpressions;

namespace Engine
{
    /// <summary>
    /// Class representing the board.
    /// </summary>
    internal class Board
    {
        internal const int Size = 8;

        internal PieceColor Player;
        internal int Score = 0;
        internal MovingPiece LastMove;
        internal Square[] Squares;

        internal bool[] ThreatenedByWhite = new bool[Size * Size];
        internal bool[] ThreatenedByBlack = new bool[Size * Size];

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
            Squares = new Square[Size * Size];
            Player = Game.PlayerColor;
            LastMove = new MovingPiece();
            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        /// <summary>
        /// Creates a Board from a string.
        /// </summary>
        /// <param name="fen">The FEN string to create the Board from.</param>
        internal Board(string fen) : this() // ! Currently assumes a valid FEN string
        {
            // Set pieces
            int idx = 0;
            foreach (char c in fen)
            {
                if (idx >= Size * Size)
                    break;

                if (c == '/')
                    continue;

                if (Char.IsDigit(c))
                {
                    idx += (int)Char.GetNumericValue(c);
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
                Squares[idx++].Piece = new Piece(pieceType, pieceColor);
            }

            // Set color
            if (fen.Contains(" w "))
                Player = PieceColor.White;
            else if (fen.Contains(" b "))
                Player = PieceColor.Black;
            else
                throw new ArgumentException("An invalid player was found in the board string.");

            // TODO: Set castling
            // TODO: Set en passant
            // TODO: Set halfmove clock
            // TODO: Set fullmove number
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

            // Generic move
            board.Squares[srcPos].Piece = null!;
            board.Squares[destPos].Piece = srcPiece;
        }

        /// <summary>
        /// Converts the Board to a FEN string.
        /// </summary>
        /// <param name="board">The Board to convert.</param>
        internal static string ToFEN(Board board)
        {
            string fen = "";

            // Get pieces
            int emptySquares = 0;
            for (int i = 0; i < Size * Size; i++)
            {
                if (i % Size == 0 && i != 0)
                {
                    if (emptySquares > 0)
                    {
                        fen += emptySquares;
                        emptySquares = 0;
                    }
                    fen += "/";
                }

                if (board.Squares[i].Piece == null)
                {
                    emptySquares++;
                    continue;
                }

                if (emptySquares > 0)
                {
                    fen += emptySquares;
                    emptySquares = 0;
                }

                PieceType type = board.Squares[i].Piece.PieceType;
                PieceColor color = board.Squares[i].Piece.PieceColor;
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
            fen += board.Player == PieceColor.White ? "w" : "b";

            // Get Castling
            fen += " ";
            if (board.WhiteCastleKingSide)
                fen += "K";
            if (board.WhiteCastleQueenSide)
                fen += "Q";
            if (board.BlackCastleKingSide)
                fen += "k";
            if (board.BlackCastleQueenSide)
                fen += "q";
            if (!board.WhiteCastleKingSide && !board.WhiteCastleQueenSide && !board.BlackCastleKingSide && !board.BlackCastleQueenSide)
                fen += "-";

            // Get En Passant
            fen += " ";
            if (board.EnPassant == -1)
                fen += "-";
            else
                fen += board.EnPassant; // TODO: Convert to algebraic notation

            // Get Halfmove Clock
            fen += " ";
            fen += board.HalfMoveClock;

            // Get Fullmove Number
            fen += " ";
            fen += board.FullMoves;
            

            return fen;
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

            return Regex.Replace(boardStr, $".{{{Size}}}", "$0\n");
        }
    }
}