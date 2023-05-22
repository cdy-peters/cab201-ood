namespace Advance
{
    public struct PieceMoving
    {
        public PieceColor PieceColor;
        public PieceType PieceType;
        public int SrcPos;
        public ValidMove DestPos;

        public PieceMoving(PieceColor pieceColor, PieceType pieceType, int srcPos, ValidMove destPos)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            SrcPos = srcPos;
            DestPos = destPos;
        }

        public PieceMoving(PieceMoving pieceMoving)
        {
            PieceColor = pieceMoving.PieceColor;
            PieceType = pieceMoving.PieceType;
            SrcPos = pieceMoving.SrcPos;
            DestPos = pieceMoving.DestPos;
        }
    }

    // ? Is this necessary
    public class MoveContent
    {
        public PieceMoving MovingPiece;

        public MoveContent()
        {
            MovingPiece = new PieceMoving();
        }

        public MoveContent(MoveContent moveContent)
        {
            MovingPiece = new PieceMoving(moveContent.MovingPiece);
        }
    }
}