namespace FinalTanksGameConsole.Objects;

public class Bullet {
    private int playerId;
    private int numOfBounces;
    private int msElapsed;
    private (double x, double y) coords;
    private Rotation rotation;

    public int PlayerId { get { return playerId; } }
    public (double x, double y) Coords { get { return coords; } }
    public Rotation Rotation { get { return rotation; } }
    public int NumOfBounces { get { return numOfBounces; } }
    public int Damage {
        get {
            return 1 + numOfBounces + (int)Math.Floor(msElapsed / 3000.0);
        }
    }
    public int Score {
        get {
            return (1 + numOfBounces) * (150 + msElapsed);
        }
    }
    private bool AffectedByDrag => false;

    public Bullet(double x, double y, int r) {
        playerId = 0;
        coords = (x, y);
        rotation = new Rotation(r);
    }

    public Bullet(int playerId, double x, double y, int r) {
        this.playerId = playerId;
        coords = (x, y);
        rotation = new Rotation(r);
    }

    public Bullet(ResponseBullet bullet) {
        coords.x = bullet.x;
        coords.y = bullet.y;
        rotation = new Rotation(bullet.rotation);
    }

    public void GameTick(int ms, double bulletSpeed, List<GameObstacle> obstacles) {
        AdjustPosition(ms, bulletSpeed);
        CalculateCollision(obstacles);
        msElapsed += ms;
    }

    /// <summary>
    /// Calculates and sets the new position of the bullet.
    /// </summary>
    public void AdjustPosition(int ms, double bulletSpeed) {
        double xChange = rotation.X_Coord * bulletSpeed * (ms / 1000.0);
        double yChange = rotation.Y_Coord * bulletSpeed * (ms / 1000.0);

        coords = (Math.Round(coords.x + xChange, 3), Math.Round(coords.y + yChange, 3));
    }

    public void CalculateCollision(List<GameObstacle> obstacles) {
        foreach (GameObstacle obstacle in obstacles) {
            ReturnCoordinate? value = obstacle.GetUncollidedCoordinates(coords.x, coords.y, 1);
            if (value != null) {
                numOfBounces++;
                if (value.X == coords.x) {
                    coords.y += value.Y - coords.y;
                    rotation.FlipX();
                } else {
                    coords.x += value.X - coords.x;
                    rotation.FlipY();
                }
                return; // Only check the first.
            }
        }
    }

    internal void SetValuesTo(ResponseBullet responseBullet) {
        coords.x = responseBullet.x;
        coords.y = responseBullet.y;
        Rotation.RotationAngle = responseBullet.rotation;
    }
}
