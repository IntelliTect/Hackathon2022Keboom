using Keboom.Server.Hubs;

namespace Keboom.Server.Tests.Hubs;

public class GameStoreTests
{
    [Fact]
    public void JoinGame_IsPublic_CanJoinAnyOtherPublicGame()
    {
        var player1 = "player1";
        var player2 = "player2";

        var request1 = new JoinGameRequest
        {
            IsPublic = true,
            PlayerId = player1,
            GameName = "something-random"
        };

        var request2 = new JoinGameRequest
        {
            IsPublic = true,
            PlayerId = player2
        };

        var gameStore = new GameStore();

        var player1Game = gameStore.JoinGame(request1);

        var player2Game = gameStore.JoinGame(request2);

        Assert.Equal(player1Game.Id, player2Game.Id);
        Assert.Contains(player2Game.Players, x => x.Id == player1);
        Assert.Contains(player2Game.Players, x => x.Id == player2);

    }
}
