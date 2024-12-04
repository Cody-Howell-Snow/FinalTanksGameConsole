namespace FinalTanksGameConsole.Objects;

/// <summary>
/// A dinky class to return values from the GameObstacle methods. I tried a few other methods, but 
/// I think this will work best. 
/// </summary>
public class ReturnCoordinate {
    private double x;
    private double y;

    public double X { get { return x; } }
    public double Y { get { return y; } }

    public ReturnCoordinate(double x, double y) {
        this.x = x;
        this.y = y;
    }
}
