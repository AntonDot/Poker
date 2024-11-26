namespace PokerGame.Game;

internal class Program
{
    static void Main()
    {
        // TODO
        var players = new[]
        {
            new Player("Player1"),
            new Player("Player2"),
            new Player("Player3")
        };
        var game = new Game(players);
        game.StartGameAsync().Wait();
    }
}
