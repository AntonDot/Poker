namespace PokerGame.Game;

public class Player
{
    private List<Card> cards = new();
    private int chips = 0;
    private string name;
    private bool isPlaying = false;

    public IReadOnlyList<Card> Cards => cards.AsReadOnly();
    public int Chips { get { return chips; } }
    public string Name { get { return name; } } 
    public bool IsPlaying { get { return isPlaying; } }

    public Player(string name)
    {
        this.name = name;
    }

    public async Task MakeMove()
    {
        // TODO
        await Task.Delay(1000);
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void ResetCards()
    {
        cards.Clear();
    }

    public void AddChips(int count)
    {
        if (count >= 0)
            chips += count;
        else
            throw new ArgumentException("Count of chips must be positive", nameof(count));
    }

    public void RemoveChips(int count)
    {
        if (count < 0)
            throw new ArgumentException("Count of chips must be positive", nameof(count));
        if (chips >= count)
            chips -= count;
        else
            chips = 0;
    }

    public void ResetChips()
    {
        chips = 0;
    }

    public void SetPlayingState(bool state)
    {
        isPlaying = state;
    }
}
