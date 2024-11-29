namespace PokerGame.Game;

public enum Combination
{
    HighCard,
    OnePair,
    TwoPairs,
    Set,
    Straight,
    Flush,
    FullHouse,
    Quads,
    StraightFlush
}

public class Layout
{
    private Combination combination;
    private List<Card> cards;

    public Layout(Combination combination, List<Card> cards)
    {
        this.combination = combination;
        this.cards = cards;
    }

    public Combination Combination => combination;
    public List<Card> Cards => cards;

    public static Layout GetHighestLayout(Card[] cards)
    {
        if (CombinationsEvaluator.IsStraightFlush(cards, out var straightFlushCards))
            return new Layout(Combination.StraightFlush, straightFlushCards);

        if (CombinationsEvaluator.IsQuads(cards, out var quadsCards))
            return new Layout(Combination.Quads, quadsCards);

        if (CombinationsEvaluator.IsFullHouse(cards, out var fullHouseCards))
            return new Layout(Combination.FullHouse, fullHouseCards);

        if (CombinationsEvaluator.IsFlush(cards, out var flushCards))
            return new Layout(Combination.Flush, flushCards);

        if (CombinationsEvaluator.IsStraight(cards, out var straightCards))
            return new Layout(Combination.Straight, straightCards);

        if (CombinationsEvaluator.IsSet(cards, out var setCards))
            return new Layout(Combination.Set, setCards);

        if (CombinationsEvaluator.IsTwoPairs(cards, out var twoPairsCards))
            return new Layout(Combination.TwoPairs, twoPairsCards);

        if (CombinationsEvaluator.IsOnePair(cards, out var onePairCards))
            return new Layout(Combination.OnePair, onePairCards);

        return new Layout(Combination.HighCard, cards.OrderByDescending(c => c.Rank).ToList());
    }

    public static bool operator <(Layout left, Layout right)
    {
        if (left.Combination < right.Combination) 
            return true;
        if (left.Combination > right.Combination)
            return false;
        for (var i = 0; i < left.Cards.Count; i++)
            if (left.Cards[i].Rank > right.Cards[i].Rank)
                return false;
        return false;
    }

    public static bool operator >(Layout left,  Layout right)
    {
        if (left.Combination > right.Combination)
            return true;
        if (left.Combination < right.Combination)
            return false;
        for (var i = 0; i < left.Cards.Count; i++)
            if (left.Cards[i].Rank < right.Cards[i].Rank)
                return false;
        return false;
    }

    public static bool operator ==(Layout left, Layout right)
    {
        if (left.Combination < right.Combination || left.Combination > right.Combination)
            return false;
        for (var i = 0; i < left.Cards.Count; i++)
            if (left.Cards[i].Rank != right.Cards[i].Rank)
                return false;
        return true;
    }

    public static bool operator !=(Layout left, Layout right)
    {
        if (left.Combination < right.Combination || left.Combination > right.Combination)
            return true;
        for (var i = 0; i < left.Cards.Count; i++)
            if (left.Cards[i].Rank == right.Cards[i].Rank)
                return false;
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is Layout other)) return false;
        return Equals(other);
    }

    private bool Equals(Layout other)
    {
        return Combination == other.Combination
            && Cards.AsEnumerable().Equals(other.Cards.AsEnumerable());
    }

    public override string ToString()
    {
        return $"{combination}: {string.Join(", ", cards.Select(card => card.ToString()))}";
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 0;
            cards.ForEach(card => hash += card.GetHashCode());
            return hash ^ combination.GetHashCode();
        }
    }
}
