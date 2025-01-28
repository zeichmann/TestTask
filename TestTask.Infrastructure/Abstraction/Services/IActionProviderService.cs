using TestTask.Domain;
using TestTask.Domain.Cards;

namespace TestTask.Infrastructure.Abstraction.Services;

public interface IActionProviderService
{
    public Task<CardActions> GetAllowedActions(CardDetails cardDetails);
}