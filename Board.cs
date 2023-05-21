namespace Advance
{
    internal class Board
    {
        internal const int Size = 9;

        internal Square[] Squares = new Square[Size * Size];
        internal PieceColor Player;
        internal int Score = 0;
        internal MoveContent LastMove;

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

        internal Board(string text) : this()
        {
            Player = Game.PlayerColor;

            for (int i = 0; i < Size * Size; i++)
            {
                char c = text[i];
                PieceColor pieceColor;
                PieceType pieceType;

                if (c == '.')
                {
                    pieceColor = PieceColor.None;
                    pieceType = PieceType.None;
                }
                else if (c == '#')
                {
                    pieceColor = PieceColor.None;
                    pieceType = PieceType.Wall;
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
                        _ => PieceType.None
                    };
                }

                Squares[i].Piece = new Piece(pieceColor, pieceType);
            }
        }

        internal Board(Board board)
        {
            Squares = new Square[Size * Size];

            for (int i = 0; i < Size * Size; i++)
            {
                if (board.Squares[i].Piece.PieceType == PieceType.None || board.Squares[i].Piece.PieceType == PieceType.Wall)
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
                if (squares[i].Piece == null || squares[i].Piece.PieceType == PieceType.None)
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
                    board.Squares[destPos].Piece = new Piece(PieceColor.None, PieceType.Wall);
                    return board.LastMove;
                }
            }

            // Catapult move
            if (srcPiece.PieceType == PieceType.Catapult)
            {
                // check if destpos is a square away
                if (Math.Abs(srcPos - destPos) == 1 || Math.Abs(srcPos - destPos) == Size)
                {
                    board.Squares[srcPos].Piece = new Piece(PieceColor.None, PieceType.None);
                    board.Squares[destPos].Piece = srcPiece;
                    return board.LastMove;
                }

                board.Squares[destPos].Piece = new Piece(PieceColor.None, PieceType.None);
                return board.LastMove;
            }

            if (srcPiece.PieceType != PieceType.Jester || destPiece == null)
            {
                board.Squares[srcPos].Piece = new Piece(PieceColor.None, PieceType.None);
                board.Squares[destPos].Piece = srcPiece;
                return board.LastMove;
            }

            // Jester abilities
            if (srcPiece.PieceType == PieceType.Jester && (destPiece.PieceType != PieceType.None))
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
            string boardString = "";
            for (int i = 0; i < Size * Size; i++)
            {
                PieceType type = Squares[i].Piece.PieceType;
                PieceColor color = Squares[i].Piece.PieceColor;

                if (type == PieceType.None)
                    boardString += ".";
                else if (type == PieceType.Wall)
                    boardString += "#";
                else
                    if (color == PieceColor.White)
                    boardString += type.ToString()[0];
                else
                    boardString += Char.ToLower(type.ToString()[0]);

                if (i % Size == Size - 1)
                    boardString += "\n";
            }
            return boardString;
        }
    }
}