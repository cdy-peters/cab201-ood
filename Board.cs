namespace Advance;

public class Board
{
    public const int Size = 9;

    public Square[] Squares = new Square[Size * Size];

    public Board(string text)
    {
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