namespace Advance
{
    internal struct Square
    {
        internal Piece Piece;

        internal Square(Piece piece)
        {
            Piece = new Piece(piece);
        }
    }
}