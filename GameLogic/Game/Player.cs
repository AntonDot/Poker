namespace PokerGame.Game;

/// <summary>
/// A class that represents poker player.
/// </summary>
public class Player
{
    private Card[] cards = new Card[2];
    private int chips = 0;
    private string name;
    private bool isPlaying = false;

    /// <summary>
    /// Current cards of the player.
    /// </summary>
    public IReadOnlyList<Card> Cards => cards.AsReadOnly();

    /// <summary>
    /// Current chips of the player.
    /// </summary>
    public int Chips => chips;

    /// <summary>
    /// Name of the player.
    /// </summary>
    public string Name => name;

    /// <summary>
    /// If the player is playing now.
    /// </summary>
    public bool IsPlaying => isPlaying;

    public Player(string name)
    {
        this.name = name;
    }

    public async Task MakeMove()
    {
        // TODO
        await Task.Delay(1000);
    }

    /// <summary>
    /// Returns current layout of the player.
    /// </summary>
    /// <param name="tableCards">All cards from the table</param>
    /// <returns>The highest possible <see cref="Layout"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Layout GetLayout(Card[] tableCards)
    {
        if (tableCards == null || tableCards.Length == 0)
            throw new ArgumentNullException(nameof(tableCards), "Table cards array was null or zero length.");
        var allCards = cards.Concat(tableCards).ToArray();
        return Layout.GetHighestLayout(allCards);
    }

    /// <summary>
    /// Sets the cards of the player. Input array must be length of 2.
    /// </summary>
    /// <param name="cards">The cards array</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void SetCards(Card[] cards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards), "Cards array was null or zero length.");
        if (cards.Length != 2)
            throw new ArgumentException("Cards array must be length of 2.", nameof(cards));
        this.cards = cards;
    }

    /// <summary>
    /// Resets cards of the player.
    /// </summary>
    public void ResetCards()
    {
        cards = new Card[2];
    }

    /// <summary>
    /// Adds <paramref name="count"/> chips to the player.
    /// </summary>
    /// <param name="count">Count of chips</param>
    /// <exception cref="ArgumentException"></exception>
    public void AddChips(int count)
    {
        if (count <= 0)
            throw new ArgumentException("Count of chips must be positive", nameof(count));
        chips += count;
    }

    /// <summary>
    /// Removes <paramref name="count"/> chips from the player.
    /// </summary>
    /// <param name="count">Count of chips</param>
    /// <exception cref="ArgumentException"></exception>
    public void RemoveChips(int count)
    {
        if (count <= 0)
            throw new ArgumentException("Count of chips must be positive", nameof(count));
        if (chips >= count)
            chips -= count;
        else
            chips = 0;
    }

    /// <summary>
    /// Resets chips of the player.
    /// </summary>
    public void ResetChips()
    {
        chips = 0;
    }

    /// <summary>
    /// Sets current playing state of the player.
    /// </summary>
    /// <param name="state">State of playing</param>
    public void SetPlayingState(bool state)
    {
        isPlaying = state;
    }

    public override string ToString()
    {
        return $"Player \"{name}\"";
    }
}
