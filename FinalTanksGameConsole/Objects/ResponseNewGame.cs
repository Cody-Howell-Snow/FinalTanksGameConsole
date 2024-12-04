namespace FinalTanksGameConsole.Objects;

public class ResponseNewGame {
    public ResponseNewGame() { }
    public int tankSpeed {  get; set; }
    public int bulletSpeed { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public List<ResponseObstacle> obstacles { get; set; }
    public ResponseTank? playerTank { get; set; }
    public bool isJoinable { get; set; }
}