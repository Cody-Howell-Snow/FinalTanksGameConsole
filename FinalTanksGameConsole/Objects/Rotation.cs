namespace FinalTanksGameConsole.Objects;
public class Rotation {
    private int rotationAngle;
    public int RotationAngle {
        get { return rotationAngle; }
        set {
            int incomingValue = value;
            while (incomingValue < 0) {
                incomingValue += 360;
            }
            rotationAngle = incomingValue % 360;
        }
    }

    public double RotationRadian {
        get {
            return Math.Round(rotationAngle * Math.PI / 180, 3);
        } set {
            RotationAngle = (int) Math.Round(value / Math.PI * 180);
        }
    }

    public double X_Coord {
        get {
            double radians = (Math.PI / 180) * rotationAngle;
            return Math.Round(Math.Cos(radians), 2);
        }
    }

    public double Y_Coord {
        get {
            double radians = (Math.PI / 180) * rotationAngle;
            return Math.Round(Math.Sin(radians), 2);
        }
    }

    public Rotation() {
        rotationAngle = 0;
    }

    public Rotation(int angle) {
        RotationAngle = angle; // Purposefully calls Property logic
    }

    public void AdjustBy(int angleDifference) {
        RotationAngle += angleDifference;
    }

    /// <summary>
    /// Takes in a pair of coordinates from origin (so convert them to that beforehand) 
    /// and assigns the class's rotation to that value. 
    /// </summary>
    public void AssignToCoordinates(double x, double y) {
        RotationRadian = Math.Atan2(y, x);
    }

    public void FlipX() {
        // Depending on quadrant, flip to a different quadrant
        if (rotationAngle < 90) {
            RotationAngle = 360 - rotationAngle;
        }
        else if (rotationAngle < 180) {
            RotationAngle = 180 + (180 - rotationAngle);
        }
        else if (rotationAngle < 270) {
            RotationAngle = 180 - (rotationAngle - 180);
        }
        else {
            RotationAngle = 360 - rotationAngle;
        }
    }

    public void FlipY() {
        // Depending on quadrant, flip to a different quadrant
        if (rotationAngle < 90) {
            RotationAngle = 90 + (90 - rotationAngle);
        }
        else if (rotationAngle < 180) {
            RotationAngle = 180 - rotationAngle;
        }
        else if (rotationAngle < 270) {
            RotationAngle = 270 + (270 - rotationAngle);
        }
        else {
            RotationAngle = 270 - (rotationAngle - 270);
        }
    }
}
