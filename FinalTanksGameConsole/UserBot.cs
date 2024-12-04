using FinalTanksGameConsole.Objects;

namespace FinalTanksGameConsole;

public static class UserBot
{
    /// <summary>
    /// This is where you can program your logic. At the start of each tick, this 
    /// method will get called. You can read the game state, find all obstacles and 
    /// enemies, and add inputs to your game. Then, it will calculate the tick, and then
    /// it will call it again. 
    /// </summary>
    public static void GameTick(ClientTankGame game)
    {
        List<Tank> tanks = game.Tanks;
        List<Bullet> bullets = game.Bullets;
        List<GameObstacle> obstacles = game.Obstacles;
        Tank? you = game.playerTank;

        if (you == null) { return; }

        // This is a class created to help with pointing and finding other tanks. This is the
        // same class for the movement rotation and gun rotation of your tank. Explore the 
        // methods on it!
        Rotation r = new Rotation();




        // A basic HoldForward bot.
        //game.InsertInput(TankInputs.Forward);


        // Otherwise, run through 


        // You *can* directly update the state of your player. It's a bit of a hack. A 
        // ResponseTank is the updates you send back to the server, so you can assign them values directly.
        // However, if you do it to gain a significant advantage, you will be automatically destroyed for cheating. So don't go overboard!

        //ResponseTank t = new ResponseTank(you);
        // Here, set properties of t to what you want your new tank to be. Then, assign your tank those values.
        //you.SetValuesTo(t);

    }

    private static double calculateDistance(Tank tank1, Tank tank2)
    {
        return Math.Sqrt(Math.Pow(tank1.Coords.x - tank2.Coords.x, 2) + Math.Pow(tank1.Coords.y - tank2.Coords.y, 2));
    }
}