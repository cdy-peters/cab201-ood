namespace Advance;


public enum PieceColor
{
    None,
    White,
    Black
}

public enum PieceType
{
    None,
    Wall,
    Zombie,
    Builder,
    Miner,
    Jester,
    Sentinel,
    Catapult,
    Dragon,
    General
}

public class Piece
{
    public PieceColor PieceColor;
    public PieceType PieceType;

    public Piece(Piece piece)
    {
        PieceColor = piece.PieceColor;
        PieceType = piece.PieceType;
    }

    public Piece(PieceColor pieceColor, PieceType pieceType)
    {
        PieceColor = pieceColor;
        PieceType = pieceType;
    }
}