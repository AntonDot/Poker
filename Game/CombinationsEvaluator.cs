namespace PokerGame.Game;

public static class CombinationsEvaluator
{
    public static bool IsStraightFlush(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Suit);
        foreach (var group in groups)
        {
            if (group.Count() >= 5)
            {
                var orderedGroup = group.OrderBy(c => c.Rank).ToArray();
                for (var i = 0; i <= orderedGroup.Length - 5; i++)
                {
                    var j = i + 1;
                    while (j < orderedGroup.Length && orderedGroup[j].Rank == orderedGroup[i].Rank + (j - i))
                        j++;
                    if (j - i == 5)
                    {
                        resultCards = orderedGroup.Skip(i).Take(5).ToList();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool IsQuads(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Rank);
        foreach (var group in groups)
        {
            if (group.Count() == 4)
            {
                resultCards = group.ToList();
                return true;
            }
        }
        return false;
    }

    public static bool IsFullHouse(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Rank);
        var triplets = groups.Where(g => g.Count() == 3);
        var pairs = groups.Where(g => g.Count() == 2);
        if (triplets.Any() && pairs.Any())
        {
            resultCards = triplets.First().Concat(pairs.First()).ToList();
            return true;
        }
        return false;
    }

    public static bool IsFlush(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Suit);
        foreach (var group in groups)
        {
            if (group.Count() >= 5)
            {
                resultCards = group.OrderByDescending(c => c.Rank).Take(5).ToList();
                return true;
            }
        }
        return false;
    }

    public static bool IsStraight(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var ranks = cards.Select(c => c.Rank).Distinct().OrderBy(r => r).ToArray();
        for (var i = 0; i <= ranks.Length - 5; i++)
        {
            var j = i + 1;
            while (j < ranks.Length && ranks[j] == ranks[i] + (j - i))
                j++;
            if (j - i == 5)
            {
                resultCards = cards.Where(c => ranks.Skip(i).Take(5).Contains(c.Rank)).OrderByDescending(c => c.Rank).ToList();
                return true;
            }
        }
        return false;
    }

    public static bool IsSet(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Rank);
        foreach (var group in groups)
        {
            if (group.Count() == 3)
            {
                resultCards = group.ToList();
                return true;
            }
        }
        return false;
    }

    public static bool IsTwoPairs(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var pairs = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).ToArray();
        if (pairs.Length == 2)
        {
            resultCards = pairs.SelectMany(p => p).Concat(cards.Except(pairs.SelectMany(p => p))).OrderByDescending(c => c.Rank).Take(5).ToList();
            return true;
        }
        return false;
    }

    public static bool IsOnePair(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var pairs = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).ToArray();
        if (pairs.Length == 1)
        {
            resultCards = pairs[0].Concat(cards.Except(pairs[0])).OrderByDescending(c => c.Rank).Take(5).ToList();
            return true;
        }
        return false;
    }
}
