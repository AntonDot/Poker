namespace PokerGame.Game;

public class Game
{
    private LinkedList<Player> players;
    private Table table;
    private Player currentPlayer;

    public Game(Player[] players)
    {
        this.players = new LinkedList<Player>(players);
        table = new Table();
        currentPlayer = this.players.First();
    }

    public async Task StartGameAsync()
    {
        // TODO
    }

    private Player GetNextPlayer()
    {
        var nextNode = players.Find(currentPlayer).Next;
        if (nextNode == null)
            return players.First();
        return nextNode.Value;
    }
}
