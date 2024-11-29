namespace PokerGame.Game;

/// <summary>
/// A class that represents a card.
/// </summary>
public class Card
{
    /// <summary>
    /// Rank of the card.
    /// </summary>
    public Rank Rank { get; }

    /// <summary>
    /// Suit of the card.
    /// </summary>
    public Suit Suit { get; }

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is Card other)) return false;
        return Equals(other);
    }

    private bool Equals(Card other)
    {
        return Rank == other.Rank && Suit == other.Suit;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)Rank * 397) ^ (int)Suit;
        }
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
