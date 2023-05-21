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

        // public PieceMoving(PieceType pieceType)
        // {
        //     PieceType = pieceType;
        //     PieceColor = PieceColor.White;
        //     SrcPos = 0;
        //     DestPos = 0;
        // }
    }

    public struct PieceTaken
    {
        public PieceColor PieceColor;
        public PieceType PieceType;
        public int Pos;

        public PieceTaken(PieceColor pieceColor, PieceType pieceType, int pos)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            Pos = pos;
        }

        public PieceTaken(PieceTaken pieceTaken)
        {
            PieceColor = pieceTaken.PieceColor;
            PieceType = pieceTaken.PieceType;
            Pos = pieceTaken.Pos;
        }

        // public PieceTaken(PieceType pieceType)
        // {
        //     PieceType = pieceType;
        //     PieceColor = PieceColor.White;
        //     Pos = 0;
        // }
    }

    public class MoveContent
    {
        public PieceMoving MovingPiece;
        public PieceTaken TakenPiece;

        public MoveContent()
        {
            MovingPiece = new PieceMoving();
            TakenPiece = new PieceTaken();
        }

        public MoveContent(MoveContent moveContent)
        {
            MovingPiece = new PieceMoving(moveContent.MovingPiece);
            TakenPiece = new PieceTaken(moveContent.TakenPiece);
        }
    }
}