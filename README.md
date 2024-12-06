### CODING YOUR BOT
 
**You are allowed to modify the code** inside UserBot.cs (located at `..\FinalTanksGame\FinalTanksGame.Shared\GameClasses\UserBot.cs` in the server app, or [here](https://github.com/Cody-Howell-Snow/FinalTanksGameConsole/blob/master/FinalTanksGameConsole/UserBot.cs) in the client app).
 
**You can move your tank** by calling the `InsertInput` method on the game object in the `UserBot.cs` file and passing a `TankInput` enum value as a parameter. There are more instructions about how to code a bot inside the `UserBot.cs` file as well.
 
**Have fun--but not too much fun!** The game has anti-cheating functionality to prevent you from gaining too much of an advantage with your coding. Doing things like trying to move out of bounds in the game to avoid being hit or increasing your tank speed too much will cause your tank bot to be booted from the game for cheating.
 
### HOW TO PLAY

**Using the client app via localhost:**
Navigate to the Games page and select a game to join.
Open your console and follow the prompts.
 - First will be the link to your website; from the other project, this should be http://localhost:5080
 - Second, put in the Game ID. This should be in the address bar
 - Third, input a valid ID. It will convert it to a negative value to not conflict with other web-based players, and is limited by the C# Int size (2^32).

Now you should be able to press keys in your console app and you should move on the web visualization. 
 
**Using the client app via a DevTunnel:**
Navigate to the Games page and select a game to join.
If you are not logged in, you'll need to choose a player ID.
Select the game board by clicking on it.
 
Use `W` and `S` to move forward and backwards.
`A` and `D` rotate counterclockwise and clockwise.
`J` and `L` rotate the tank gun; and `K` allows you to fire 1 bullet per second (but only after the game starts).
 
Your goal is to eliminate other players' tanks and have your tank be the longest standing! Points are earned based on the
amount of time you stay alive in the game. Each bullet you are hit by (including your own!) decreases your health, and
bullets do more damage with each time they bounce--but they only last up to 3 bounces.
 
 
