namespace PokerGame.Game;

public static class CombinationsEvaluator
{
    public static bool IsStraightFlush(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var oneSuitGroups = cards.GroupBy(c => c.Suit).Where(g => g.Count() >= 5);
        var suitableGroups = new List<List<Card>>();
        var found = false;
        foreach (var group in oneSuitGroups)
        {
            if (IsStraight(cards, out var straightCards))
            {
                suitableGroups.Add(straightCards);
                found = true;
            }
        }

        if (found)
        {
            resultCards = suitableGroups.OrderByDescending(group => group[0].Rank).First();
            return true;
        }
        return false;
    }

    public static bool IsQuads(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = new List<Card>();

        var quadGroups = cards
            .GroupBy(c => c.Rank)
            .Where(g => g.Count() == 4)
            .ToList();

        if (quadGroups.Count != 0)
        {
            var strongestQuad = quadGroups.OrderByDescending(g => g.Key).First();
            resultCards.AddRange(strongestQuad);

            var remainingCards = cards
                .Except(strongestQuad)
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.Add(remainingCards.First());
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();

            return true;
        }
        return false;
    }

    public static bool IsFullHouse(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = new List<Card>();
        var groups = cards.GroupBy(c => c.Rank);
        var triplets = groups.Where(g => g.Count() == 3);
        var pairs = groups.Where(g => g.Count() == 2);
        if (triplets.Any() && pairs.Any())
        {
            resultCards = triplets.First().Key > pairs.First().Key
                ? triplets
                .OrderByDescending(g => g.Key)
                .First()
                .Concat(pairs
                .OrderByDescending(g => g.Key)
                .First())
                .ToList()
                : pairs
                .OrderByDescending(g => g.Key)
                .First()
                .Concat(triplets
                .OrderByDescending(g => g.Key)
                .First())
                .ToList();
            return true;
        }

        if (triplets.Count() == 2)
        {
            var strongestTriplet = triplets.OrderByDescending(c => c.Key).First();
            resultCards.AddRange(strongestTriplet);

            var remainingTriplets = triplets
                .SelectMany(g => g)
                .Except(strongestTriplet)
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.AddRange(remainingTriplets.Take(2));
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();
            return true;
        }
        return false;
    }

    public static bool IsFlush(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var groups = cards.GroupBy(c => c.Suit);
        var suitableGroups = new List<List<Card>>();
        var found = false;
        foreach (var group in groups)
        {
            if (group.Count() >= 5)
            {
                suitableGroups.Add(group
                    .OrderByDescending(c => c.Rank)
                    .Take(5)
                    .ToList());
                found = true;
            }
        }

        if (found)
        {
            resultCards = suitableGroups.OrderBy(group => group[0].Rank).First();
            return true;
        }
        return false;
    }

    public static bool IsStraight(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var sortedRanks = cards
            .OrderByDescending(card => card.Rank)
            .Select(c => c.Rank)
            .ToArray();
        for (var i = 0; i <= sortedRanks.Length - 5; i++)
        {
            var j = i + 1;
            while (j < sortedRanks.Length && sortedRanks[j] == sortedRanks[i] + (i - j))
                j++;
            if (j - i >= 5)
            {
                resultCards = cards
                    .Where(c => sortedRanks.Skip(i).Take(5).Contains(c.Rank))
                    .OrderByDescending(c => c.Rank)
                    .ToList();
                return true;
            }
        }

        // spesical case for "A 2 3 4 5"
        if (sortedRanks.Contains(Rank.Ace)
            && sortedRanks.Contains(Rank.Two)
            && sortedRanks.Contains(Rank.Three)
            && sortedRanks.Contains(Rank.Four)
            && sortedRanks.Contains(Rank.Five))
        {
            resultCards = cards
                .OrderByDescending(c => c.Rank)
                .Take(5)
                .ToList();
            return true;
        }
        return false;
    }

    public static bool IsSet(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = new List<Card>();
        var triplets = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 3);
        
        if (triplets.Count() == 1)
        {
            resultCards.AddRange(triplets.First());

            var remainingCards = cards
                .Except(triplets.First())
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.AddRange(remainingCards.Take(2));
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();
            return true;
        }
        return false;
    }

    public static bool IsTwoPairs(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = cards;
        var pairs = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).ToArray();
        if (pairs.Length >= 2)
        {
            resultCards = pairs
                .SelectMany(p => p)
                .Concat(cards.Except(pairs.SelectMany(p => p)))
                .OrderByDescending(c => c.Rank)
                .Take(5)
                .ToList();
            return true;
        }
        return false;
    }

    public static bool IsOnePair(List<Card> cards, out List<Card> resultCards)
    {
        resultCards = new List<Card>();
        var pairs = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).ToArray();
        if (pairs.Length == 1)
        {
            resultCards.AddRange(pairs[0]);
            var remainingCards = cards
                .Except(pairs[0])
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.AddRange(remainingCards.Take(3));
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();
            return true;
        }
        return false;
    }
}
