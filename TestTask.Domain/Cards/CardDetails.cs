namespace TestTask.Domain.Cards;

public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);