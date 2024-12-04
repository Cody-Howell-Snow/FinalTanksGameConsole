namespace FinalTanksGameConsole.Objects;
public class RequestTank {
    public RequestTank() {}
    public RequestTank(Tank tank) {
        id = tank.Id;
        x = tank.Coords.x;
        y = tank.Coords.y;
        lowerRotation = tank.LowerRotation.RotationAngle;
        upperRotation = tank.UpperRotation.RotationAngle;
        velocity = tank.Velocity;
    }
    public int id { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public int lowerRotation { get; set; }
    public int upperRotation { get; set; }
    public double velocity { get; set; }
}
