using MediatR;

namespace TestTask.Application.CardActions.Queries;

public record CardActionsQuery(string UserId, string CardNumber) : IRequest<List<string>>;