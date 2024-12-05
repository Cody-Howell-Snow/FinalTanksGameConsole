namespace FinalTanksGameConsole.Objects;
public partial class Tank {
    private int id;
    private int msElapsed;
    private (double x, double y) coords;
    private double velocity;
    private Rotation lowerRotation;
    private Rotation upperRotation;
    private bool drag;
    private int lowerRotationSpeed = 120;
    private int upperRotationSpeed = 160;
    private Bullet? shotBullet = null;
    private int lastBulletTime = 0;
    private int health = 10;
    private int score = 0;

    public int Id { get { return id; } }
    public (double x, double y) Coords { get { return coords; } }
    public double Velocity { get { return velocity; } } // Is a component of movement based off of lowerRotation
    public Rotation LowerRotation { get { return lowerRotation; } }
    public Rotation UpperRotation { get { return upperRotation; } }
    public Bullet? ShotBullet { get { return shotBullet; } set { shotBullet = null; } } // Set simply resets it, no matter the value. 
    public int Health { get { return health; } set { health = value; } }
    public int Score { get { return score; } set { score = value; } }
    private int LastBulletTime {
        get { return lastBulletTime; }
        set {
            lastBulletTime = value;
            if (lastBulletTime > 0) {
                lastBulletTime = 0;
            }
        }
    }

    // Constructors are in a separate partial class

    public override string ToString() {
        return $"X: {coords.x}, Y: {coords.y}, Movement Angle: {lowerRotation.RotationAngle}, Arm Angle: {upperRotation.RotationAngle}";
    }

    /// <summary>
    /// Computes a full game tick for this tank; movement, collisions, and firing. Bullets created 
    /// by this method are a public property to be picked up later, then set and removed. 
    /// </summary>
    public void GameTick(int ms, double tankSpeed, HashSet<TankInputs>? inputs, List<GameObstacle> obstacles) {
        LastBulletTime += ms;
        Score += (int)Math.Round(ms / 50.0); // Value can be adjusted to taste
        if (inputs is not null) {
            foreach (var input in inputs) {
                AcceptInput(ms, input);
            }
        }
        AdjustPosition(ms, tankSpeed);
        CalculateCollision(obstacles);
        if (drag) {
            velocity /= 1.1;
        }
        msElapsed += ms;
    }

    public void AdjustPosition(int ms, double tankSpeed) {
        double xChange = lowerRotation.X_Coord * tankSpeed * (ms / 1000.0) * velocity;
        double yChange = lowerRotation.Y_Coord * tankSpeed * (ms / 1000.0) * velocity;

        coords = (Math.Round(coords.x + xChange, 3), Math.Round(coords.y + yChange, 3));
    }

    public void AcceptInput(int ms, TankInputs input) {
        switch (input) {
            case TankInputs.Left: AdjustLowerRotation((int)(ms / 1000.0 * lowerRotationSpeed)); break;
            case TankInputs.Right: AdjustLowerRotation((int)(ms / 1000.0 * -lowerRotationSpeed)); break;
            case TankInputs.RotateArmLeft: AdjustUpperRotation((int)(ms / 1000.0 * upperRotationSpeed)); break;
            case TankInputs.RotateArmRight: AdjustUpperRotation((int)(ms / 1000.0 * -upperRotationSpeed)); break;
            case TankInputs.Forward: AddVelocity(); break; // The following don't take into account ms time
            case TankInputs.Backward: MinusVelocity(); break;
            case TankInputs.Fire: FireBullet(); break;
        }
    }

    public void CalculateCollision(List<GameObstacle> obstacles) {
        foreach (GameObstacle obstacle in obstacles) {
            ReturnCoordinate? value = obstacle.GetUncollidedCoordinates(coords.x, coords.y, 20);
            if (value != null) {
                if (value.X == coords.x) {
                    coords.y += value.Y - coords.y;
                } else {
                    coords.x += value.X - coords.x;
                }
            }
        }
    }

    public List<Bullet> CalculateBullets(List<Bullet> bullets) {
        List<Bullet> collidedBullets = new List<Bullet>();
        if (health > 0) {
            for (int i = 0; i < bullets.Count; i++) {
                double distance =
                    Math.Pow(bullets[i].Coords.x - coords.x, 2) +
                    Math.Pow(bullets[i].Coords.y - coords.y, 2);
                if (distance < 676) {
                    health -= bullets[i].Damage;
                    collidedBullets.Add(bullets[i]);
                }
            }
        }
        return collidedBullets;
    }

    public List<int> CalculateBulletKeys(Dictionary<int, Bullet> bullets) {
        List<int> collidedBullets = new List<int>();
        if (health > 0) {
            foreach (KeyValuePair<int, Bullet> bullet in bullets) {
                double distance =
                    Math.Pow(bullet.Value.Coords.x - coords.x, 2) +
                    Math.Pow(bullet.Value.Coords.y - coords.y, 2);
                if (distance < 676) {
                    health -= bullet.Value.Damage;
                    collidedBullets.Add(bullet.Key);
                }
            }
        }
        return collidedBullets;
    }

    private void AddVelocity() {
        velocity = Math.Tanh(velocity + 0.1);
    }

    private void MinusVelocity() {
        velocity = Math.Tanh(velocity - 0.1);
    }

    // This should be an API call on Clients, and should only be implemented in the server client. A post request for the user 
    // and their most updated Tank should be made and the new bullet returned. 
    public void FireBullet() {
        // Bullet timer is 1 time per second
        if (lastBulletTime >= 0 && health > 0) {
            double newX = upperRotation.X_Coord * 30 + coords.x;
            double newY = upperRotation.Y_Coord * 30 + coords.y;
            shotBullet = new Bullet(id, newX, newY, upperRotation.RotationAngle);
            lastBulletTime = -1000;
        }
    }

    public void AdjustLowerRotation(int x) {
        lowerRotation.AdjustBy(x);
        //upperRotation.AdjustBy(x);    // I think this feature is nice, but having double-rotation actually makes the tank quite unweildy. 
        // You can uncomment this line to test it, but I don't love how it's implemented.
    }

    public void AdjustUpperRotation(int x) {
        upperRotation.AdjustBy(x);
    }

    public void SetUpperRotation(int x) {
        upperRotation.RotationAngle = x;
    }
    public void SetLowerRotation(int x) {
        lowerRotation.RotationAngle = x;
    }

    public void SetValuesTo(RequestTank tank) {
        coords.x = tank.x;
        coords.y = tank.y;
        lowerRotation.RotationAngle = tank.lowerRotation;
        upperRotation.RotationAngle = tank.upperRotation;
        velocity = tank.velocity;
    }

    public void SetValuesTo(ResponseTank tank) {
        coords.x = tank.x;
        coords.y = tank.y;
        lowerRotation.RotationAngle = tank.lowRotation;
        upperRotation.RotationAngle = tank.highRotation;
        velocity = tank.velocity;
        health = tank.health;
        score = tank.score;
    }

    public void UpdatePlayerTank(ResponseTank tank) {
        health = tank.health;
        score = tank.score;
    }

    public void SetUpperAngleToCoordinate(int x, int y) {
        if (x == coords.x && y == coords.y) { return; }
        double dx = x - coords.x;
        double dy = y - coords.y;

        upperRotation.RotationRadian = Math.Atan2(dy, dx);
    }
}
