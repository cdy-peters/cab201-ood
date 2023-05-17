namespace Advance;

public struct Square
{
    public Piece Piece;

    public Square(Piece piece)
    {
        Piece = new Piece(piece);
    }
}