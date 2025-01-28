using TestTask.Domain;
using TestTask.Domain.Cards;
using TestTask.Infrastructure.Abstraction.Services;

namespace TestTask.Infrastructure.Services;

public class ActionProviderService : IActionProviderService
{
    private static readonly Dictionary<(CardType, CardStatus), CardActions> ActionMap = new()
    {
        {
            (CardType.Prepaid, CardStatus.Ordered),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9 |
            CardActions.Action10 | CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Prepaid, CardStatus.Inactive),
            CardActions.Action2 | CardActions.Action3 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action11 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Prepaid, CardStatus.Active),
            CardActions.Action1 | CardActions.Action3 | CardActions.Action4 | CardActions.Action5 |
            CardActions.Action8 | CardActions.Action9 | CardActions.Action10 | CardActions.Action11 |
            CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Prepaid, CardStatus.Restricted),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action12 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Prepaid, CardStatus.Blocked),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9
        },
        { (CardType.Prepaid, CardStatus.Expired), CardActions.Action3 | CardActions.Action4 | CardActions.Action9 },
        { (CardType.Prepaid, CardStatus.Closed), CardActions.Action3 | CardActions.Action4 | CardActions.Action9 },

        {
            (CardType.Debit, CardStatus.Ordered),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9 |
            CardActions.Action10 | CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Debit, CardStatus.Inactive),
            CardActions.Action2 | CardActions.Action3 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action11 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Debit, CardStatus.Active),
            CardActions.Action1 | CardActions.Action3 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action11 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Debit, CardStatus.Restricted),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action12 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Debit, CardStatus.Blocked),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9
        },
        { (CardType.Debit, CardStatus.Expired), CardActions.Action3 | CardActions.Action4 | CardActions.Action9 },
        { (CardType.Debit, CardStatus.Closed), CardActions.Action3 | CardActions.Action4 | CardActions.Action9 },

        {
            (CardType.Credit, CardStatus.Ordered),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action5 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Credit, CardStatus.Inactive),
            CardActions.Action2 | CardActions.Action3 | CardActions.Action4 | CardActions.Action5 |
            CardActions.Action8 | CardActions.Action9 | CardActions.Action10 | CardActions.Action11 |
            CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Credit, CardStatus.Active),
            CardActions.Action1 | CardActions.Action3 | CardActions.Action4 | CardActions.Action5 |
            CardActions.Action8 | CardActions.Action9 | CardActions.Action10 | CardActions.Action11 |
            CardActions.Action12 | CardActions.Action13
        },
        {
            (CardType.Credit, CardStatus.Restricted),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action4 | CardActions.Action8 |
            CardActions.Action9 | CardActions.Action10 | CardActions.Action12 | CardActions.Action12 |
            CardActions.Action13
        },
        {
            (CardType.Credit, CardStatus.Blocked),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action5 | CardActions.Action8 | CardActions.Action9
        },
        {
            (CardType.Credit, CardStatus.Expired),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action5 | CardActions.Action9
        },
        {
            (CardType.Credit, CardStatus.Closed),
            CardActions.Action3 | CardActions.Action4 | CardActions.Action5 | CardActions.Action9
        },
    };

    public async Task<CardActions> GetAllowedActions(CardDetails cardDetails)
    {
        foreach (var ((cardType, cardStatus), actions) in ActionMap)
        {
            var resultActions = actions;
            if (cardDetails.CardType == cardType && cardDetails.CardStatus == cardStatus)
            {
                if (cardStatus == CardStatus.Blocked && cardDetails.IsPinSet)
                {
                    resultActions |= CardActions.Action6;
                    resultActions |= CardActions.Action7;
                }

                if ((cardStatus == CardStatus.Ordered || cardStatus == CardStatus.Inactive ||
                     cardStatus == CardStatus.Active) && cardDetails.IsPinSet)
                {
                    resultActions |= CardActions.Action6;
                }

                if ((cardStatus == CardStatus.Ordered || cardStatus == CardStatus.Inactive ||
                     cardStatus == CardStatus.Active) && !cardDetails.IsPinSet)
                {
                    resultActions |= CardActions.Action7;
                }

                return resultActions;
            }
        }

        return CardActions.None;
    }
}