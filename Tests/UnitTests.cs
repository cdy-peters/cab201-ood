using Engine;

namespace UnitTests;

[TestClass]
public class FunctionalityTests
{
    [TestMethod]
    public void InitialBoardIO()
    {
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = new Board(fen);
        Assert.AreEqual(Board.ToFEN(board), fen);
    }
}
