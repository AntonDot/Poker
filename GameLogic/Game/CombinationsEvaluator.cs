namespace PokerGame.Game;

/// <summary>
/// A class that represents an evaluator of poker combinations.
/// </summary>
public static class CombinationsEvaluator
{
    /// <summary>
    /// Checks if <paramref name="cards"/> can made up straight flush layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up straight flush layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsStraightFlush(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

        var oneSuitGroups = cards.GroupBy(c => c.Suit).Where(g => g.Count() >= 5);
        var suitableGroups = new List<List<Card>>();
        var found = false;
        foreach (var group in oneSuitGroups)
        {
            if (IsStraight(cards, out var straightCards) && straightCards != null)
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

        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up quads layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up quads layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsQuads(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

        var quadGroups = cards
            .GroupBy(c => c.Rank)
            .Where(g => g.Count() == 4)
            .ToList();

        if (quadGroups.Count != 0)
        {
            resultCards = new List<Card>();
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
        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up full house layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up full house layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsFullHouse(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

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
            resultCards = new List<Card>();
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
        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up flush layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up flush layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsFlush(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

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
        resultCards = null;
        return false;
    }


    /// <summary>
    /// Checks if <paramref name="cards"/> can made up straight layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up straight layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsStraight(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

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

        // special case for "A 2 3 4 5"
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
        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up set layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up set layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsSet(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

        var triplets = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 3);
        
        if (triplets.Count() == 1)
        {
            resultCards = new List<Card>();
            resultCards.AddRange(triplets.First());

            var remainingCards = cards
                .Except(triplets.First())
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.AddRange(remainingCards.Take(2));
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();
            return true;
        }
        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up two pairs layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up two pairs layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsTwoPairs(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

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
        resultCards = null;
        return false;
    }

    /// <summary>
    /// Checks if <paramref name="cards"/> can made up one pair layout.
    /// </summary>
    /// <param name="cards">Input cards</param>
    /// <param name="resultCards">Resulting cards</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Card"/> that mades up one pair layout or <c>null</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsOnePair(Card[] cards, out List<Card>? resultCards)
    {
        if (cards == null || cards.Length == 0)
            throw new ArgumentNullException(nameof(cards));

        var pairs = cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).ToArray();
        if (pairs.Length == 1)
        {
            resultCards = new List<Card>();
            resultCards.AddRange(pairs[0]);
            var remainingCards = cards
                .Except(pairs[0])
                .OrderByDescending(c => c.Rank)
                .ToList();
            resultCards.AddRange(remainingCards.Take(3));
            resultCards = resultCards.OrderByDescending(c => c.Rank).ToList();
            return true;
        }
        resultCards = null;
        return false;
    }
}
