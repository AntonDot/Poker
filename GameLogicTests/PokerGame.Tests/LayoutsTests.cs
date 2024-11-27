using NUnit.Framework;
using PokerGame.Game;

namespace PokerGame.Tests;

[TestFixture]
public class PokerLogicTests
{
    [TestCase(Combination.HighCard,
        new[] { "JD", "TH", "4D", "3H", "2C" },
        new[] { "2C", "4D", "3H", "TH", "JD" })]
    [TestCase(Combination.HighCard,
        new[] { "5H", "4S" },
        new[] { "5H", "4S" })]

    [TestCase(Combination.OnePair,
        new[] { "3D", "2C", "2D" },
        new[] { "2C", "2D", "3D" })]
    [TestCase(Combination.OnePair,
        new[] { "AD", "5H", "5D", "4S" },
        new[] { "5H", "4S", "5D", "AD" })]
    [TestCase(Combination.OnePair,
        new[] { "AD", "QH", "JS" ,"5H", "5D" },
        new[] { "5H", "4S", "5D", "AD", "JS", "QH", "6D" })]

    [TestCase(Combination.TwoPairs,
        new[] { "3D", "3H", "2C", "2D" },
        new[] { "2C", "2D", "3D", "3H" })]
    [TestCase(Combination.TwoPairs,
        new[] { "AS", "AH", "QH", "7H", "7D" },
        new[] { "7H", "AS", "7D", "AH", "QH" })]
    [TestCase(Combination.TwoPairs,
        new[] { "AS", "AH", "QH", "QD", "7H" },
        new[] { "7H", "AS", "7D", "AH", "QH", "QD", "6D" })]

    [TestCase(Combination.Set,
        new[] { "2C", "2D", "2S" },
        new[] { "2C", "2D", "2S" })]
    [TestCase(Combination.Set, 
        new[] { "AC", "AD", "AS", "3D", "2H" },
        new[] { "AC", "AD", "2H", "AS", "3D" })]
    [TestCase(Combination.Set,
        new[] { "AS", "QC", "QD", "QS", "8D" },
        new[] { "QC", "QD", "2H", "QS", "3D", "AS", "8D" })]

    [TestCase(Combination.Straight,
        new[] { "6H", "5H", "4D", "3D", "2C" },
        new[] { "2C", "3D", "4D", "5H", "6H" })]
    [TestCase(Combination.Straight,
        new[] { "AS", "5H", "4S", "3D", "2H" },
        new[] { "5H", "4S", "AS", "3D", "2H" })]
    [TestCase(Combination.Straight,
        new[] { "7S", "6H", "5H", "4D", "3D" },
        new[] { "2C", "3D", "7S", "4D", "5H", "6H", "QH" })]

    [TestCase(Combination.Flush,
        new[] { "AC", "JC", "7C", "5C", "2C" },
        new[] { "2C", "5C", "JC", "AC", "7C" })]
    [TestCase(Combination.Flush,
        new[] { "AH", "QH", "TH", "4H", "2H" },
        new[] { "QH", "AH", "2H", "TH", "4H" })]
    [TestCase(Combination.Flush,
        new[] { "AC", "QC", "JC", "8C", "7C" },
        new[] { "2C", "5C", "JC", "AC", "7C", "8C", "QC" })]

    [TestCase(Combination.FullHouse,
        new[] { "JC", "JD", "5C", "5D", "5H" },
        new[] { "5C", "5D", "JC", "5H", "JD" })]
    [TestCase(Combination.FullHouse,
        new[] { "QH", "QS", "QC", "4D", "4H" },
        new[] { "QH", "4D", "QS", "4H", "QC" })]
    [TestCase(Combination.FullHouse,
        new[] { "QH", "QS", "QC", "4D", "4H" },
        new[] { "QH", "4D", "QS", "4H", "QC", "4S", "5D" })]

    [TestCase(Combination.Quads,
        new[] { "6D", "2C", "2D", "2S", "2H" },
        new[] { "2C", "2D", "2S", "2H", "6D" })]
    [TestCase(Combination.Quads,
        new[] { "JC", "JD", "JS", "JH", "7H" },
        new[] { "JC", "JD", "7H", "JS", "JH" })]
    [TestCase(Combination.Quads,
        new[] { "QD", "JC", "JD", "JS", "JH" },
        new[] { "JC", "JD", "7H", "JS", "JH", "TS", "QD" })]

    [TestCase(Combination.StraightFlush,
        new[] { "6C", "5C", "4C", "3C", "2C" },
        new[] { "2C", "3C", "4C", "5C", "6C" })]
    [TestCase(Combination.StraightFlush,
        new[] { "AH", "5H", "4H", "3H", "2H" },
        new[] { "5H", "4H", "AH", "3H", "2H" })]
    [TestCase(Combination.StraightFlush,
        new[] { "7H", "6H", "5H", "4H", "3H" },
        new[] { "5H", "4H", "AH", "3H", "6H", "7H", "2H" })]
    public void LayoutTests(Combination expectedComb, string[] expectedCards, string[] cards)
    {
        // checks correct higher layout recognition
        var parsedCards = cards.Select(card => ParseCard(card)).ToList();
        var parsedExpectedCards = expectedCards
            .Select(card => ParseCard(card))
            .ToList();

        var answer = Layout.GetHighestLayout(parsedCards);
        Assert.That(answer.Combination, Is.EqualTo(expectedComb), "Combinations are not the same.");
        Assert.That(answer.Cards, Is.EqualTo(parsedExpectedCards), "Cards weren't in correct order or there are different arrays.");
    }

    private static Card ParseCard(string cardStr)
    {
        if (string.IsNullOrEmpty(cardStr))
            throw new ArgumentException("Invalid card format");

        Rank rank;
        switch (cardStr[0])
        {
            case 'A': rank = Rank.Ace; break;
            case 'K': rank = Rank.King; break;
            case 'Q': rank = Rank.Queen; break;
            case 'J': rank = Rank.Jack; break;
            case 'T': rank = Rank.Ten; break;
            default:
                var value = cardStr[0] - '0' - 2;
                if (value >= 0 && value <= 7)
                    rank = (Rank)value;
                else
                    throw new ArgumentException($"Unknown rank '{cardStr[0]}'");
                break;
        }

        Suit suit;
        switch (cardStr[1])
        {
            case 'S': suit = Suit.Spades; break;
            case 'H': suit = Suit.Hearts; break;
            case 'D': suit = Suit.Diamonds; break;
            case 'C': suit = Suit.Clubs; break;
            default:
                throw new ArgumentException($"Unknown suit '{cardStr[1]}'");
        }

        return new Card(rank, suit);
    }
}
