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
    public void ValidMovesBenchmark()
    {
        string[] lines = File.ReadAllLines(@"benchmark.txt");
        List<string> failures = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] parts = line.Split(" bm ");
            string fen = parts[0];
            string bm = parts[1];

            Board board = new Board(fen);
            Moves.GetValidMoves(board);

            try
            {
                Assert.IsTrue(board.IsValidMoveAN(bm));
            }
            catch
            {
                failures.Add($"Case {i + 1}: The best move {bm} was not found for the board,\n{fen}");
            }
        }

        if (failures.Count > 0)
            Assert.Fail($"{failures.Count}/{lines.Length} tests failed\n" + string.Join("\n\n", failures));
    }

    [TestMethod]
    public void ValidMovesBenchmark2()
    {
        string[] lines = File.ReadAllLines(@"kaufman.txt");
        List<string> failures = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            //Console.WriteLine(i);
            string line = lines[i];
            string[] parts = line.Split(";");

            string fen = parts[0].Trim();

            string bm = parts[1];
            bm = bm.Replace("\0", "");
            bm = bm.Replace("bm", "");
            bm = bm.Replace("am", "");
            bm = bm.Trim();

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
                failures.Add($"Case {i + 1}: Expected '{bm}', received '{an}'");
            }
        }

        if (failures.Count > 0)
            Assert.Fail($"{failures.Count}/{lines.Length} tests failed\n" + string.Join("\n", failures));
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
