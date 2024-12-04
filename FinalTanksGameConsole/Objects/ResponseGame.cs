namespace FinalTanksGameConsole.Objects;
public class ResponseGame {
    public List<ResponseTank> tanks {  get; set; }
    public List<ResponseBullet> bullets { get; set; }
}

public class ResponseTank {
    public ResponseTank() { }
    public ResponseTank(Tank tank) {
        id = tank.Id;
        x = tank.Coords.x;
        y = tank.Coords.y;
        lowRotation = tank.LowerRotation.RotationAngle;
        highRotation = tank.UpperRotation.RotationAngle;
        velocity = tank.Velocity;
        health = tank.Health;
    }
    public int id { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public int lowRotation { get; set; }
    public int highRotation { get; set; }
    public double velocity { get; set; }
    public int health { get; set; } 
    public int score { get; set; }
}

public class ResponseBullet {
    public double x { get; set; }
    public double y { get; set; }
    public int rotation { get; set; }
}

