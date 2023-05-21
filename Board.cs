using System.Text.RegularExpressions;

namespace Advance
{
    internal class Board
    {
        internal const int Size = 9;

        internal PieceColor Player;
        internal int Score = 0;
        internal MoveContent LastMove;
        internal Square[] Squares;

        internal bool[] ThreatenedByWhite = new bool[Size * Size];
        internal bool[] ThreatenedByBlack = new bool[Size * Size];

        internal bool WhiteCheck = false;
        internal bool BlackCheck = false;
        internal bool WhiteMate = false;
        internal bool BlackMate = false;
        internal bool StaleMate = false;

        internal Board()
        {
            Squares = new Square[Size * Size];

            for (int i = 0; i < Size * Size; i++)
            {
                Squares[i] = new Square();
            }

            LastMove = new MoveContent();

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        internal Board(string boardStr) : this()
        {
            Player = Game.PlayerColor;

            for (int i = 0; i < Size * Size; i++)
            {
                char c = boardStr[i];
                PieceColor pieceColor;
                PieceType pieceType;

                if (c == '.')
                {
                    continue;
                }
                else if (c == '#')
                {
                    Squares[i].Piece = new Piece(PieceType.Wall);
                    continue;
                }
                else
                {
                    pieceColor = Char.IsUpper(c) ? PieceColor.White : PieceColor.Black;

                    c = Char.ToLower(c);

                    pieceType = c switch
                    {
                        'z' => PieceType.Zombie,
                        'b' => PieceType.Builder,
                        'm' => PieceType.Miner,
                        'j' => PieceType.Jester,
                        's' => PieceType.Sentinel,
                        'c' => PieceType.Catapult,
                        'd' => PieceType.Dragon,
                        'g' => PieceType.General,
                        _ => throw new ArgumentException("An invalid character was found in the board string.")
                    };
                }

                Squares[i].Piece = new Piece(pieceType, pieceColor);
            }
        }

        internal Board(Board board)
        {
            Squares = new Square[Size * Size];

            for (int i = 0; i < Size * Size; i++)
            {
                if (board.Squares[i].Piece == null || board.Squares[i].Piece.PieceType == PieceType.Wall)
                    Squares[i] = new Square(board.Squares[i].Piece);
            }

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];

            for (int i = 0; i < Size * Size; i++)
            {
                ThreatenedByWhite[i] = board.ThreatenedByWhite[i];
                ThreatenedByBlack[i] = board.ThreatenedByBlack[i];
            }

            Player = board.Player;
            Score = board.Score;
            LastMove = new MoveContent(board.LastMove);

            BlackCheck = board.BlackCheck;
            WhiteCheck = board.WhiteCheck;
            BlackMate = board.BlackMate;
            WhiteMate = board.WhiteMate;
            StaleMate = board.StaleMate;
        }

        private Board(Square[] squares)
        {
            Squares = new Square[Size * Size];

            for (int i = 0; i < Size * Size; i++)
            {
                if (squares[i].Piece == null)
                    continue;
                Squares[i] = new Square(squares[i].Piece);
            }

            LastMove = new MoveContent();

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        internal Board(int score) : this()
        {
            Score = score;

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        internal Board CopyBoard()
        {
            Board newBoard = new Board(Squares);

            newBoard.Player = Player;

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];

            return newBoard;
        }

        internal static MoveContent MovePiece(Board board, int srcPos, ValidMove validMove)
        {
            int destPos = validMove.DestPos;

            // TODO: Refactor this method
            Piece srcPiece = board.Squares[srcPos].Piece;
            Piece destPiece = board.Squares[destPos].Piece;

            board.LastMove = new MoveContent();
            board.LastMove.MovingPiece = new PieceMoving(srcPiece.PieceColor, srcPiece.PieceType, srcPos, validMove);

            // Builder move
            if (srcPiece.PieceType == PieceType.Builder)
            {
                if (validMove.IsWall)
                {
                    board.Squares[destPos].Piece = new Piece(PieceType.Wall);
                    return board.LastMove;
                }
            }

            // Catapult move
            if (srcPiece.PieceType == PieceType.Catapult)
            {
                // check if destpos is a square away
                if (Math.Abs(srcPos - destPos) == 1 || Math.Abs(srcPos - destPos) == Size)
                {
                    board.Squares[srcPos].Piece = null!;
                    board.Squares[destPos].Piece = srcPiece;
                    return board.LastMove;
                }

                board.Squares[destPos].Piece = null!;
                return board.LastMove;
            }

            if (srcPiece.PieceType != PieceType.Jester || destPiece == null)
            {
                board.Squares[srcPos].Piece = null!;
                board.Squares[destPos].Piece = srcPiece;
                return board.LastMove;
            }

            // Jester abilities
            if (srcPiece.PieceType == PieceType.Jester && (destPiece != null))
            {
                if (destPiece.PieceColor == srcPiece.PieceColor)
                {
                    // Swap pieces
                    board.Squares[srcPos].Piece = destPiece;
                    board.Squares[destPos].Piece = srcPiece;
                }
                else if (destPiece.PieceColor != PieceColor.None)
                {
                    // Change color of destination piece
                    board.Squares[destPos].Piece.PieceColor = srcPiece.PieceColor;
                }
            }

            return board.LastMove;
        }

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

            return Regex.Replace(boardStr, $".{{{Size}}}", "$0\n"); ;
        }
    }
}