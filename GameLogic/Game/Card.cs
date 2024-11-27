namespace PokerGame.Game;

public class Card
{
    public Rank Rank { get; }
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
