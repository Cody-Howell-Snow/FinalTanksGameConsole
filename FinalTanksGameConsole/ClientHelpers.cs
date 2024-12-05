using FinalTanksGameConsole.Objects;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Json;

namespace FinalTanksGameConsole; 
public class ClientHelpers {
    public static void SendBullet(HttpClient client, int gameId, int playerId) {
        client.PutAsync($"/api/bullet/{gameId}/{playerId}", null); // Does not need to be awaited
    }

    public static async Task<ResponseGame> GetServerUpdates(HttpClient client, int gameId) {
        var response = await client.GetAsync($"/api/game/{gameId}");
        ResponseGame? serverGame;
        try {
            serverGame = await response.Content.ReadFromJsonAsync<ResponseGame>();
        } catch (Exception ex) {
            serverGame = null;
        }
        return serverGame;
    }

    public static async Task UpdateServer(HttpClient client, ClientTankGame game) {
        if (game.playerTank is not null && game.playerTank.Health > 0) {
            RequestTank sentTank = new RequestTank(game.playerTank);
            var send = await client.PostAsJsonAsync($"/api/tank/{game.gameId}", sentTank);
        }
    }
}
