using MediatR;
using TestTask.Application.CardActions.Queries;
using TestTask.Infrastructure.Abstraction.Services;
using TestTask.Infrastructure.Extenders;

namespace TestTask.Application.CardActions.QueryHandlers;

public class CardActionsHandler(ICardService cardService, IActionProviderService actionProviderService)
    : IRequestHandler<CardActionsQuery, List<string>>
{
    public async Task<List<string>> Handle(CardActionsQuery request, CancellationToken cancellationToken)
    {
        var details = await cardService.GetCardDetails(request.UserId, request.CardNumber);
        if (details == null)
            return [];
        
        var result = await actionProviderService.GetAllowedActions(details);

        return result.ToList().Select(x => x.ToString()).ToList();
    }
}