using TestTask.Domain.Cards;

namespace TestTask.Infrastructure.Abstraction.Services;

public interface ICardService
{
    public Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
} 