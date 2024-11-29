namespace PokerGame.Game;

public class Table
{
    private Stack<Card> deck = new();
    private List<Card> shownCards = new();
    private int bank = 0;

    public IReadOnlyList<Card> ShownCards => shownCards;
    public int Bank => bank;

    public Player[] GetPlayersWithHighestLayout(Player[] players)
    {
        var maxLayoutPlayers = new List<Player>();
        maxLayoutPlayers.Add(players[0]);

        foreach (var player in players)
        {
            if (player.IsPlaying)
            {
                var maxLayout = maxLayoutPlayers[0].GetLayout(shownCards.ToArray());
                var playerLayout = player.GetLayout(shownCards.ToArray());
                if (maxLayout < playerLayout)
                {
                    maxLayoutPlayers.Clear();
                    maxLayoutPlayers.Add(player);
                }
                if (maxLayout == playerLayout)
                    maxLayoutPlayers.Add(player);
            }
        }

        return maxLayoutPlayers.ToArray();
    }

    public Card[] GetDeckTopCards(int n = 1)
    {
        var cards = new Card[n];
        for (var i = 0; i < n; i++)
            cards[i] = deck.Pop();
        return cards;
    }

    public void DrawTopCardOnTable()
    {
        shownCards.Add(deck.Pop());
    }

    public void ShuffleNewDeck()
    {
        deck.Clear();
        var newDeck = new Card[52];

        var random = new Random();
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 13; j++)
            {
                newDeck[i + j] = new Card((Rank)j, (Suit)i);
            }
        }
        random.Shuffle(newDeck);

        for (var i = 0; i < newDeck.Length; i++)
            deck.Push(newDeck[i]);
    }

    public void SetBank(int bank)
    { 
        this.bank = bank; 
    }

    public void ResetBank()
    {
        bank = 0;
    }
}
