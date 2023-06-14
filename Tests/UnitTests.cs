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
                Assert.Fail($"Case {i}: The best move {bm} was not found for the board,\n{fen}");
            }
        }
    }
}
