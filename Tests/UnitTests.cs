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
}
