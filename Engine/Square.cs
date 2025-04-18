namespace Engine
{
    /// <summary>
    /// Structure representing a square on the board.
    /// </summary>
    internal struct Square
    {
        internal Piece Piece;

        /// <summary>
        /// Creates a new square with the given piece.
        /// </summary>
        /// <param name="piece">The piece to place on the square.</param>
        internal Square(Piece piece)
        {
            Piece = new Piece(piece);
        }
    }
}