using System.Text.RegularExpressions;

namespace Advance
{
    internal class Board
    {
        internal const int Size = 9;

        internal PieceColor Player;
        internal int Score = 0;
        internal MovingPiece LastMove;
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
                Squares[i] = new Square();

            Player = Game.PlayerColor;

            LastMove = new MovingPiece();

            ThreatenedByWhite = new bool[Size * Size];
            ThreatenedByBlack = new bool[Size * Size];
        }

        internal Board(string boardStr) : this()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                char c = boardStr[i];
                PieceColor pieceColor;
                PieceType pieceType;

                if (c == '.')
                    continue;
                else if (c == '#')
                {
                    Squares[i].Piece = new Piece(PieceType.Wall);
                    continue;
                }
                else
                {
                    pieceColor = Char.IsUpper(c) ? PieceColor.White : PieceColor.Black;
                    pieceType = Char.ToLower(c) switch
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
                }

                Squares[i].Piece = new Piece(pieceType, pieceColor);
            }
        }

        internal Board(Board board)
        {
            Squares = new Square[Size * Size];
            for (int i = 0; i < Size * Size; i++)
            {
                ThreatenedByWhite[i] = board.ThreatenedByWhite[i];
                ThreatenedByBlack[i] = board.ThreatenedByBlack[i];

                if (board.Squares[i].Piece == null || board.Squares[i].Piece.PieceType == PieceType.Wall)
                    Squares[i] = new Square(board.Squares[i].Piece);
            }

            Player = board.Player;
            Score = board.Score;
            LastMove = board.LastMove;

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
                if (squares[i].Piece != null)
                    Squares[i] = new Square(squares[i].Piece);

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

        internal static void MovePiece(Board board, int srcPos, MoveDest moveDest)
        {
            int destPos = moveDest.Pos;
            Piece srcPiece = board.Squares[srcPos].Piece;
            Piece destPiece = board.Squares[destPos].Piece;

            board.LastMove = new MovingPiece(srcPiece.PieceColor, srcPiece.PieceType, srcPos, moveDest);

            board.Player = board.Player == PieceColor.White ? PieceColor.Black : PieceColor.White;

            // Builder move (wall)
            if (srcPiece.PieceType == PieceType.Builder && moveDest.IsWall)
            {
                board.Squares[destPos].Piece = new Piece(PieceType.Wall);
                return;
            }

            // Catapult move
            if (srcPiece.PieceType == PieceType.Catapult)
            {
                // Check for move or shot
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
                if (destPiece.PieceColor == srcPiece.PieceColor)
                {
                    // Swap pieces
                    board.Squares[srcPos].Piece = destPiece;
                    board.Squares[destPos].Piece = srcPiece;
                    return;
                }
                else if (destPiece.PieceColor != PieceColor.None)
                {
                    // Change color of destination piece
                    board.Squares[destPos].Piece.PieceColor = srcPiece.PieceColor;
                    return;
                }
            }

            // Generic move
            board.Squares[srcPos].Piece = null!;
            board.Squares[destPos].Piece = srcPiece;
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

            return Regex.Replace(boardStr, $".{{{Size}}}", "$0\n");
        }
    }
}