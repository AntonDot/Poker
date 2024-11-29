namespace PokerGame.Game;

/// <summary>
/// A class that represents poker table.
/// </summary>
public class Table
{
    private Stack<Card> deck = new();
    private List<Card> shownCards = new();
    private int bank = 0;

    /// <summary>
    /// Array of <see cref="Card"/> that are currently on the table. 
    /// </summary>
    public IReadOnlyList<Card> ShownCards => shownCards;

    /// <summary>
    /// Current amount of chips in the bank.
    /// </summary>
    public int Bank => bank;

    /// <summary>
    /// Returns <see cref="Player"/> that has the highest layout. 
    /// If there are several such players, returns all of them.
    /// </summary>
    /// <param name="players">All players in the game</param>
    /// <returns>Array of <see cref="Player"/>.</returns>
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

    /// <summary>
    /// Takes first <paramref name="n"/> cards from the top of deck and remove them.
    /// By default takes 1 card.
    /// </summary>
    /// <param name="n">The number of cards</param>
    /// <returns>Array of <see cref="Card"/>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="n"/> was non-positive 
    /// or there are not so many avaluable cards in the deck.</exception>
    public Card[] GetDeckTopCards(int n = 1)
    {
        if (deck.Count < n || n < 1)
            throw new ArgumentException($"Can't take {n} cards from the deck.");
        var cards = new Card[n];
        for (var i = 0; i < n; i++)
            cards[i] = deck.Pop();
        return cards;
    }

    /// <summary>
    /// Draws <paramref name="n"/> cards to the table by removing them from the deck.
    /// By default draws 1 card.
    /// </summary>
    /// <param name="n">The number of cards</param>
    /// <exception cref="ArgumentException">If <paramref name="n"/> was non-positive 
    /// or there are not so many avaluable cards in the deck.</exception>
    public void DrawTopCardOnTable(int n = 1)
    {
        if (deck.Count < n || n < 1)
            throw new ArgumentException($"Can't take {n} cards from the deck.");
        for (var i = 0; i < n; i++)
            shownCards.Add(deck.Pop());
    }

    /// <summary>
    /// Clears old deck, creates a new one and shuffles it.
    /// </summary>
    public void RecreateDeck()
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

    /// <summary>
    /// Adds chips to the table's bank.
    /// </summary>
    /// <param name="chips">The amount of chips</param>
    /// <exception cref="ArgumentException">If <paramref name="chips"/> are non-positive.</exception>
    public void AddChipsToBank(int chips)
    {
        if (chips <= 0)
            throw new ArgumentException($"Can't add {chips} chips to table bank.");
        bank += chips;
    }

    /// <summary>
    /// Sets the table's bank to zero.
    /// </summary>
    public void ResetBank()
    {
        bank = 0;
    }
}
