using Engine;

namespace UnitTests;

[TestClass]
public class FunctionalityTests
{
    [TestMethod]
    public void StartingBoardIO()
    {
        string fenIn = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = new Board(fenIn);

        string fenOut = board.ToFEN();
        Assert.AreEqual(fenIn, fenOut);
    }

    [TestMethod]
    public void EnPassantTest()
    {
        string fen = "8/6bb/8/8/R1pP2k1/4P3/P7/K7 b - d3";

        Board board = new Board(fen);
        Moves.GetValidMoves(board);

        if (board.IsValidMove(34, 43))
        {
            Board.MovePiece(board, 34, 43);
            if (board.Squares[35].Piece != null)
                Assert.Fail("Pawn was not captued");
            return;
        }
        Assert.Fail("En passant move does not exist");
    }

    [TestMethod]
    public void KaufmanTests()
    {
        string[] lines = File.ReadAllLines(@"kaufman.txt");
        List<string> failException = new List<string>();
        List<string> failBestMoveFound = new List<string>();
        List<string> failBestMoveNotFound = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string testStr = line.Split(";")[0];

            string[] separator = { "bm", "am" };
            string[] parts = testStr.Split(separator, StringSplitOptions.TrimEntries);

            string fen = parts[0];
            string bm = parts[1];

            Board board = new Board(fen);
            Moves.GetValidMoves(board);
            MovingPiece bestMove = Search.AlphaBetaRoot(board, 3);

            string an = ToAN(board, bestMove);
            try
            {
                Assert.AreEqual(an, bm);
            }
            catch
            {
                MovingPieceAN mpAN;
                try
                {
                mpAN = FromAN(bm);
                }
                catch (Exception e)
                {
                    failException.Add($"Case {i + 1} threw an exception: {e.Message} {e.StackTrace}");
                    continue;
                }

                bool valid = board.IsValidMoveAN(mpAN);

                if (valid)
                    failBestMoveFound.Add($"Case {i + 1}: Expected '{bm}', received '{an}'");
                else
                    failBestMoveNotFound.Add($"Case {i + 1}: Expected best move '{bm}' was not found");
            }
        }


        int totalFailed = failException.Count + failBestMoveFound.Count + failBestMoveNotFound.Count;
        string totalMsg = $"{totalFailed}/{lines.Length} tests failed\n\n";

        string exceptionMsg = "";
        if (failException.Count > 0)
            exceptionMsg = string.Join("\n", failException) + "\n\n";

        string notFoundMsg = "";
        if (failBestMoveNotFound.Count > 0)
            notFoundMsg = string.Join("\n", failBestMoveNotFound) + "\n\n";

        string foundMsg = "";
        if (failBestMoveFound.Count > 0)
            foundMsg = string.Join("\n", failBestMoveFound) + "\n";

        Assert.Fail(totalMsg + exceptionMsg + notFoundMsg + foundMsg);
    }

    private static PieceType GetPiece(char piece)
    {
        switch (piece)
        {
            case 'K':
                return PieceType.King;
            case 'Q':
                return PieceType.Queen;
            case 'R':
                return PieceType.Rook;
            case 'B':
                return PieceType.Bishop;
            case 'N':
                return PieceType.Knight;
            case 'P':
                return PieceType.Pawn;
            default:
                throw new ArgumentException($"Invalid piece: {piece}");
        }
    }

    private static MovingPieceAN FromAN(string AN)
    {
        // string str = "";
        MovingPieceAN mpAN = new MovingPieceAN();

        int rIdx = AN.Length - 1;

        // Check flag
        if (AN[rIdx] == '+')
        {
            mpAN.Check = true;
            rIdx--;

            if (AN[rIdx] == ' ')
                rIdx--;
        }

        // Destination
        string dest = AN.Substring(rIdx - 1, 2);
        mpAN.DestPos = Board.FromAN(dest);
        rIdx -= 2;

        // Moving piece
        if (rIdx < 0)
        {
            mpAN.PieceType = PieceType.Pawn;
            return mpAN;
        }
        if (rIdx == 0)
        {
            mpAN.PieceType = GetPiece(AN[rIdx]);
            return mpAN;
        }

        // Capture
        if (AN[rIdx] == 'x')
        {
            mpAN.Capture = true;
            rIdx--;
        }

        // Rank
        if (Char.IsDigit(AN[rIdx]))
        {
            mpAN.Rank = AN[rIdx];
            rIdx--;

            if (rIdx < 0)
            {
                mpAN.PieceType = PieceType.Pawn;
                return mpAN;
            }
        }

        // File
        if (Char.IsLower(AN[rIdx]))
        {
            mpAN.File = AN[rIdx];
            rIdx--;

            if (rIdx < 0)
            {
                mpAN.PieceType = PieceType.Pawn;
                return mpAN;
            }
        }

        mpAN.PieceType = GetPiece(AN[rIdx]);
        return mpAN;
    }

    private static string ToAN(Board board, MovingPiece bestMove)
    {
        string AN = "";

        int src = bestMove.SrcPos;
        int dest = bestMove.DestPos;

        Piece srcPiece = board.Squares[src].Piece;
        Piece destPiece = board.Squares[dest].Piece;

        // Piece
        if (srcPiece.PieceType != PieceType.Pawn)
        {
            char piece;
            if (srcPiece.PieceType == PieceType.Knight)
                piece = 'N';
            else
                piece = srcPiece.PieceType.ToString()[0];

            if (srcPiece.PieceColor == PieceColor.Black)
                Char.ToLower(piece);
            AN += piece;
        }

        // Capture
        if (destPiece != null)
        {
            // Pawn capture
            if (srcPiece.PieceType == PieceType.Pawn)
            {
                int pFile = src % 8;
                char pFileChar = (char)('a' + pFile);
                AN += pFileChar.ToString();
            }
            AN += 'x';
        }

        // Destination
        int file = dest % 8;
        int rank = 8 - (dest / 8) - 1;

        char fileChar = (char)('a' + file);
        char rankChar = (char)('1' + rank);

        string destAN = fileChar.ToString() + rankChar.ToString();
        AN += destAN;

        // Check
        if (srcPiece.PieceColor == PieceColor.White)
        {
            if (board.BlackCheck)
                AN += " +";
        }
        else
        {
            if (board.WhiteCheck)
                AN += " +";
        }

        return AN;
    }
}
