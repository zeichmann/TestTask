using FluentAssertions;
using TestTask.Domain;
using TestTask.Domain.Cards;
using TestTask.Infrastructure.Abstraction.Services;
using TestTask.Infrastructure.Services;
using Xunit;

namespace TestTask.Tests
{
    public class ActionProviderServiceTests
    {
        private readonly IActionProviderService _actionProviderService;

        public ActionProviderServiceTests()
        {
            _actionProviderService = new ActionProviderService();
        }

        /// <summary>
        /// Sprawdza, czy zwraca None, jeśli para (CardType, CardStatus) nie istnieje w ActionMap.
        /// </summary>
        [Fact]
        public async Task GetAllowedActions_ShouldReturnNone_WhenNotFoundInDictionary()
        {
            // Arrange
            // Dobieramy np. CardType = 999 (poza zakresem) albo inny typ, który nie istnieje w mapie
            var cardDetails = new CardDetails(
                CardNumber: "1234 5678",
                CardType: (CardType)999,
                CardStatus: CardStatus.Blocked,
                IsPinSet: false);

            // Act
            var result = await _actionProviderService.GetAllowedActions(cardDetails);

            // Assert
            result.Should().Be(CardActions.None);
        }

        /// <summary>
        /// Przykładowy test dla Prepaid Blocked, gdzie IsPinSet=false -> NIE dodaje Action6/7.
        /// </summary>
        [Fact]
        public async Task GetAllowedActions_PrepaidBlocked_PinNotSet_ShouldNotAddAction6OrAction7()
        {
            // Arrange
            var cardDetails = new CardDetails(
                "1111 2222",
                CardType.Prepaid,
                CardStatus.Blocked,
                false);
            
            var expectedBaseActions = CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9;

            // Act
            var result = await _actionProviderService.GetAllowedActions(cardDetails);

            // Assert
            result.Should().Be(expectedBaseActions);
        }

        /// <summary>
        /// Przykładowy test dla Prepaid Blocked z ustawionym PINem -> powinniśmy dostać Action6 i Action7 dodatkowo.
        /// </summary>
        [Fact]
        public async Task GetAllowedActions_PrepaidBlocked_PinSet_ShouldAddAction6AndAction7()
        {
            // Arrange
            var cardDetails = new CardDetails(
                "1111 2222",
                CardType.Prepaid,
                CardStatus.Blocked,
                true);
            
            var expectedBaseActions = CardActions.Action3 | CardActions.Action4 | CardActions.Action8 | CardActions.Action9;
            
            var expectedActions = expectedBaseActions | CardActions.Action6 | CardActions.Action7;

            // Act
            var result = await _actionProviderService.GetAllowedActions(cardDetails);

            // Assert
            result.Should().Be(expectedActions);
        }

        /// <summary>
        /// Przykład testu parametryzowanego (Theory) sprawdzającego,
        /// czy dla (Ordered/Inactive/Active) z ustawionym PIN, dodaje się Action6, a dla bez PIN -> Action7.
        /// </summary>
        [Xunit.Theory]
        [InlineData(CardStatus.Ordered,  true,  CardActions.Action6)] // pin set -> Action6
        [InlineData(CardStatus.Ordered,  false, CardActions.Action7)] // pin not set -> Action7
        [InlineData(CardStatus.Inactive, true,  CardActions.Action6)]
        [InlineData(CardStatus.Inactive, false, CardActions.Action7)]
        [InlineData(CardStatus.Active,   true,  CardActions.Action6)]
        [InlineData(CardStatus.Active,   false, CardActions.Action7)]
        public async Task GetAllowedActions_PrepaidOrderedInactiveActive_AddsAction6Or7(CardStatus status, bool isPinSet, CardActions expectedExtra)
        {
            // Arrange
            var cardDetails = new CardDetails(
                "9999 0000",
                CardType.Prepaid,
                status,
                isPinSet);
            
            var expectedBaseActions = status switch
            {
                CardStatus.Ordered => CardActions.Action3 | CardActions.Action4 | CardActions.Action8 |
                                      CardActions.Action9 | CardActions.Action10 | CardActions.Action12 | CardActions.Action13,

                CardStatus.Inactive => CardActions.Action2 | CardActions.Action3 | CardActions.Action4 | CardActions.Action8 |
                                       CardActions.Action9 | CardActions.Action10 | CardActions.Action11 | CardActions.Action12 |
                                       CardActions.Action13,

                CardStatus.Active => CardActions.Action1 | CardActions.Action3 | CardActions.Action4 | CardActions.Action5 |
                                     CardActions.Action8 | CardActions.Action9 | CardActions.Action10 | CardActions.Action11 |
                                     CardActions.Action12 | CardActions.Action13,
                
                _ => CardActions.None
            };

            // Dodajemy do bazowych akcji oczekiwaną "extra" akcję
            var expectedActions = expectedBaseActions | expectedExtra;

            // Act
            var result = await _actionProviderService.GetAllowedActions(cardDetails);

            // Assert
            result.Should().Be(expectedActions);
        }

        /// <summary>
        /// Przykład testu parametryzowanego (Theory) dla typu Credit i statusu Blocked.
        /// Z uwagi na powtarzanie w ActionMap, można weryfikować logikę pinSet / pinNotSet.
        /// </summary>
        [Xunit.Theory]
        [InlineData(true)]   // pin set
        [InlineData(false)]  // pin not set
        public async Task GetAllowedActions_CreditBlocked_DependsOnPin(bool isPinSet)
        {
            // Arrange
            var cardDetails = new CardDetails(
                "0000 1111",
                CardType.Credit,
                CardStatus.Blocked,
                isPinSet);

            // Z mapy (Credit, Blocked) => Action3|Action4|Action5|Action8|Action9
            var expectedBaseActions = CardActions.Action3 | CardActions.Action4 | CardActions.Action5 | CardActions.Action8 | CardActions.Action9;

             /*Logika w kodzie:
             if Blocked && IsPinSet => dodaj Action6 i Action7*/
            var expectedActions = isPinSet
                ? expectedBaseActions | CardActions.Action6 | CardActions.Action7
                : expectedBaseActions;

            // Act
            var result = await _actionProviderService.GetAllowedActions(cardDetails);

            // Assert
            result.Should().Be(expectedActions);
        }
    }
}
