namespace FinalTanksGameConsole.Objects;
public partial class Tank {

    public Tank(double x, double y) {
        id = msElapsed = 0;
        coords = (x, y);
        velocity = 0;
        lowerRotation = new Rotation();
        upperRotation = new Rotation();
        drag = true;
    }

    public Tank(double x, double y, int lowerAngle) {
        id = msElapsed = 0;
        coords = (x, y);
        velocity = 0;
        lowerRotation = new Rotation(lowerAngle);
        upperRotation = new Rotation();
        drag = true;
    }

    public Tank(double x, double y, int lowerAngle, int upperAngle) {
        id = msElapsed = 0;
        coords = (x, y);
        velocity = 0;
        lowerRotation = new Rotation(lowerAngle);
        upperRotation = new Rotation(upperAngle);
        drag = true;
    }

    public Tank(double x, double y, int lowerAngle, double velocity) {
        id = msElapsed = 0;
        coords = (x, y);
        this.velocity = velocity;
        lowerRotation = new Rotation(lowerAngle);
        upperRotation = new Rotation();
        drag = true;
    }

    // Preferred game generation constructor
    public Tank(double x, double y, int id, int centerX, int centerY, int health, bool drag) {
        this.id = id;
        msElapsed = 0;
        coords = (x, y);
        velocity = 0;
        lowerRotation = new Rotation(90);
        upperRotation = new Rotation();
        SetUpperAngleToCoordinate(centerX, centerY);
        this.drag = drag;
        this.health = health;
    }

    public Tank(ResponseTank tank, bool inDrag) {
        id = tank.id;
        msElapsed = 0;
        coords = (tank.x, tank.y);
        velocity = 0;
        lowerRotation = new Rotation(tank.lowRotation);
        upperRotation = new Rotation(tank.highRotation);
        drag = inDrag;
        health = 10;
    }
}
