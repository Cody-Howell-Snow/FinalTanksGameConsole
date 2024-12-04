namespace FinalTanksGameConsole.Objects;
public class ResponseObstacle {
    public ResponseObstacle() { }
    public ResponseObstacle(GameObstacle obstacle) {
        centerX = obstacle.Center.x;
        centerY = obstacle.Center.y;
        boxRadius = obstacle.BoxRadius;
    }
    public int centerX { get; set; }
    public int centerY { get; set; }
    public int boxRadius { get; set; }
}
