namespace PokerGame.Game;

public class Table
{
    private Stack<Card> deck = new();
    private List<Card> shownCards = new();
    private int bank = 0;

    public IReadOnlyList<Card> ShownCards => shownCards;
    public int Bank => bank;

    public Card GetDeckTopCard()
    {
        return deck.Pop();
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
