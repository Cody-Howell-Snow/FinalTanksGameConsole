using FinalTanksGameConsole;
using FinalTanksGameConsole.Objects;
using System.Net.Http.Json;

Console.WriteLine("Insert the link to connect to the game to.");

string link = Console.ReadLine();

Console.WriteLine("Input the number of the game you want to join.");

int gameId = int.Parse(Console.ReadLine());

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(link);

Console.WriteLine("Please choose an ID not currently on the screen to connect as. You aren't authenticated, so you will be given a negative number.");
int playerId;
bool validId;
do {
    string value = Console.ReadLine();
    validId = int.TryParse(value, out playerId);
} while (!validId);

if (playerId > 0) {
    playerId *= -1;
}


// Initialize game
var response = await client.GetAsync($"/api/join/{gameId}/{playerId}");
ResponseNewGame gameInit = await response.Content.ReadFromJsonAsync<ResponseNewGame>();

ClientTankGame game = new ClientTankGame(gameInit);
game.client = client;
game.gameId = gameId;
game.UpdateFromServer(new Tank(gameInit.playerTank!, true)); // Adjust this bool to remove drag from your tank (you must manually slow down)
Console.WriteLine("Completed game initialization.");

Thread.Sleep(2000);

game.StartGame();

Console.Clear();
Console.WriteLine("Use WASD to move, J and K to rotate the arm, and K to fire.");

while (true) {
    Console.WriteLine(game.playerTank);
    //Console.WriteLine("Tanks: "); // Eventually, have an output of the location of some tanks and some bullets. Maybe the closest ones? 

    ConsoleKey key = Console.ReadKey(true).Key;
    TankInputs action;
    switch (key) {
        case ConsoleKey.W: action = TankInputs.Forward; break;
        case ConsoleKey.S: action = TankInputs.Backward; break;
        case ConsoleKey.A: action = TankInputs.Right; break;
        case ConsoleKey.D: action = TankInputs.Left; break;
        case ConsoleKey.J: action = TankInputs.RotateArmRight; break;
        case ConsoleKey.L: action = TankInputs.RotateArmLeft; break;
        case ConsoleKey.K: action = TankInputs.Fire; break;
        default: Console.WriteLine("Not a valid input."); continue;
    }

    game.InsertInput(action);
}



