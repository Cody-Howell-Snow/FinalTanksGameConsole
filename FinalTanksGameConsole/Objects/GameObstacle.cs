namespace FinalTanksGameConsole.Objects;
public class GameObstacle {
    private (int x, int y) center;
    private int boxRadius; // Box radius implies that the corners are not rounded
                           // and they are orthogonal with the coordinate grid (not rotated). 

    public (int x, int y) Center { get { return center; } }
    public int BoxRadius { get { return boxRadius; } }

    public GameObstacle(int x, int y, int boxRadius) {
        center = (x, y);
        this.boxRadius = boxRadius;
    }

    public GameObstacle(ResponseObstacle obstacle) {
        center = (obstacle.centerX, obstacle.centerY);
        boxRadius = obstacle.boxRadius;
    }

    /// <summary>
    /// Takes in a 2D point and radius of the object, and returns whether the object 
    /// or the box radius of the object are within the boundary of this obstacle.
    /// </summary>
    /// <param name="x">X-coordinate of the incoming object</param>
    /// <param name="y">Y-coordinate of the incoming object</param>
    /// <param name="radiusOfObject">Box radius of the incoming object</param>
    /// <returns>Boolean if object is on or within boundary</returns>
    public bool IsWithinBoundary(double x, double y, int radiusOfObject) {
        int lowerBound = center.x - boxRadius;
        int upperBound = center.x + boxRadius;
        // Validate X axis
        if ((x + radiusOfObject) < lowerBound || (x - radiusOfObject) > upperBound) {
            return false;
        }

        lowerBound = center.y - boxRadius;
        upperBound = center.y + boxRadius;

        // If inside, validate Y axis
        if ((y + radiusOfObject) < lowerBound || (y - radiusOfObject) > upperBound) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets coordinates based off the center of the incoming object. It's calculated by 
    /// extending lines out from the corners of the obstacle. It prioritizes top and 
    /// bottom pushes. 
    /// </summary>
    /// <returns>A null or a Return Coordinate to help adjust the object.</returns>
    public ReturnCoordinate? GetUncollidedCoordinates(double x, double y, int radiusOfObject) {
        if (!IsWithinBoundary(x, y, radiusOfObject)) { return null; }

        double xDist = x - center.x;
        double yDist = y - center.y;

        if (Math.Abs(xDist) > Math.Abs(yDist)) {
            if (xDist < 0) {
                return new ReturnCoordinate(center.x - boxRadius - radiusOfObject, y);
            }
            else {
                return new ReturnCoordinate(center.x + boxRadius + radiusOfObject, y);
            }
        }
        else {
            if (yDist < 0) {
                return new ReturnCoordinate(x, center.y - boxRadius - radiusOfObject);
            }
            else {
                return new ReturnCoordinate(x, center.y + boxRadius + radiusOfObject);
            }
        }
    }
}
