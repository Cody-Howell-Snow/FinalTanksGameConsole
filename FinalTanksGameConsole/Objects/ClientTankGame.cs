
using FinalTanksGameConsole;

namespace FinalTanksGameConsole.Objects;
public class ClientTankGame {
    private int id = -1;
    private List<Tank> tanks = new List<Tank>();
    private List<Bullet> bullets = new List<Bullet>();
    private List<GameObstacle> obstacles;
    private (int x, int y) size;
    private bool isStarted = false;
    private HashSet<TankInputs> inputs = new HashSet<TankInputs>();
    private int fps = 20;
    private DateTime lastUpdateTime = DateTime.Now;

    // Game state
    private int tankSpeed = 120;
    private int bulletSpeed = 150;

    public volatile Tank? playerTank;
    public List<Tank> Tanks { get { return tanks; } }
    public List<Bullet> Bullets { get { return bullets; } }
    public List<GameObstacle> Obstacles { get { return obstacles; } }
    public (int x, int y) Size { get { return size; } }
    public bool IsStarted { get { return isStarted; } }

    public HashSet<TankInputs> Inputs { get { return inputs; } }
    public HttpClient client { get; set; }
    public int gameId { get; set; }

    public ClientTankGame() {
    }

    public ClientTankGame(ResponseNewGame newGame) {
        tankSpeed = newGame.tankSpeed;
        bulletSpeed = newGame.bulletSpeed;
        obstacles = new List<GameObstacle>();
        foreach (ResponseObstacle obstacle in newGame.obstacles) {
            obstacles.Add(new GameObstacle(obstacle));
        }
        size = (newGame.SizeX, newGame.SizeY);
    }

    public async Task StartGame() {
        if (!isStarted) {
            isStarted = true;
            while (isStarted) {
                await Task.Run(GameTick);
            }
        }
    }

    public void StopGame() {
        isStarted = false;
    }

    public void UpdateFromServer(Tank tank) {
        id = tank.Id;
        playerTank = tank;
    }

    private async Task GameTick() {
        UserBot.GameTick(this); // This call plays the game from the bot point of view. 

        int ms = (DateTime.Now - lastUpdateTime).Milliseconds;
        lastUpdateTime = DateTime.Now;
        for (int i = 0; i < bullets.Count; i++) {
            bullets[i].GameTick(ms, bulletSpeed, obstacles);
            if (bullets[i].NumOfBounces >= 3) {
                lock (Bullets) {
                    bullets.RemoveAt(i);
                }
                i--;
            }
        }
        lock (Tanks) {
            foreach (Tank tank in tanks) {
                if (tank.Health <= 0) { continue; }
                tank.GameTick(ms, tankSpeed, null, obstacles);
                List<Bullet> collidedBullets = tank.CalculateBullets(bullets);
                if (collidedBullets.Count > 0) {
                    lock (Bullets) {
                        for (int i = 0; i < collidedBullets.Count; i++) {
                            bullets.Remove(collidedBullets[i]);
                        }
                    }
                }
            }
        }

        if (playerTank != null && playerTank.Health > 0) {
            lock (Inputs) { // Lock on public properties to share with the outside world
                playerTank.GameTick(ms, tankSpeed, inputs, obstacles);
                inputs.Clear();
            }

            if (playerTank.ShotBullet != null) {
                ClientHelpers.SendBullet(client, gameId, id);
                playerTank.ShotBullet = null;
            }
        }


        ClientHelpers.UpdateServer(client, this);
        ResponseGame? gameUpdate = await ClientHelpers.GetServerUpdates(client, gameId);
        if (gameUpdate != null) {
            UpdateGameState(gameUpdate);
        }
        Thread.Sleep(1000 / fps);
    }

    public void UpdateGameState(ResponseGame incomingGame) {
        List<ResponseTank> inTanks = incomingGame.tanks;
        int currentTankIndex = 0;
        for (int i = 0; i < inTanks.Count; i++) {
            if (inTanks[i].id == id) {
                playerTank!.UpdatePlayerTank(inTanks[i]);
                continue;
            }

            lock (Tanks) {
                if (tanks.Count > currentTankIndex) {
                    tanks[currentTankIndex].SetValuesTo(inTanks[i]);
                    currentTankIndex++;
                } else {
                    tanks.Add(new Tank(inTanks[i], false));
                }
            }
        }

        List<ResponseBullet> inBullets = incomingGame.bullets;
        lock (Bullets) {
            bullets.Clear();
            for (int i = 0; i < inBullets.Count; i++) {
                bullets.Add(new Bullet(inBullets[i]));
            }
        }
    }

    public void InsertInput(TankInputs input) {
        inputs.Add(input);
    }

    public void ClearInputs() {
        inputs.Clear();
    }
}
