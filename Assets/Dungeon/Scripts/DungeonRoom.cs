using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Corner
{
    BottomLeft,
    BottomRight,
    TopRight,
    TopLeft
}

public struct Bounds
{
    private Vector2Int originPosition, scale, scaleDir;
    private Corner originCorner;

    public Bounds(Corner originCorner, Vector2Int originPosition, Vector2Int scale)
    {
        this.originCorner = originCorner;
        this.scaleDir = (originCorner == Corner.BottomLeft) ? new Vector2Int(1, 1) :
                        (originCorner == Corner.BottomRight) ? new Vector2Int(-1, 1) :
                        (originCorner == Corner.TopRight) ? new Vector2Int(-1, -1) :
                        (originCorner == Corner.TopLeft) ? new Vector2Int(1, -1) :
                        Vector2Int.zero;
        this.originPosition = originPosition;
        this.scale = scale;
    }

    public void DrawBounds()
    {
        Color color = Color.red;
        Debug.DrawLine((Vector2)GetCorner(Corner.BottomLeft), (Vector2)GetCorner(Corner.BottomRight), color);
        Debug.DrawLine((Vector2)GetCorner(Corner.BottomRight), (Vector2)GetCorner(Corner.TopRight), color);
        Debug.DrawLine((Vector2)GetCorner(Corner.TopRight), (Vector2)GetCorner(Corner.TopLeft), color);
        Debug.DrawLine((Vector2)GetCorner(Corner.TopLeft), (Vector2)GetCorner(Corner.BottomLeft), color);
    }

    public Vector2Int GetCorner(Corner corner)
    {
        if (originCorner == Corner.BottomLeft)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    return originPosition;
                case Corner.BottomRight:
                    return originPosition + (scale * Vector2Int.right);
                case Corner.TopRight:
                    return originPosition + scale;
                case Corner.TopLeft:
                    return originPosition + (scale * Vector2Int.up);
            }
        }
        if (originCorner == Corner.BottomRight)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    return originPosition + (scale * Vector2Int.left);
                case Corner.BottomRight:
                    return originPosition;
                case Corner.TopRight:
                    return originPosition + (scale * Vector2Int.up);
                case Corner.TopLeft:
                    return originPosition + (scale * scaleDir);
            }
        }
        if (originCorner == Corner.TopRight)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    return originPosition + (scale * scaleDir);
                case Corner.BottomRight:
                    return originPosition + (scale * Vector2Int.down);
                case Corner.TopRight:
                    return originPosition;
                case Corner.TopLeft:
                    return originPosition + (scale * Vector2Int.left);
            }
        }
        if (originCorner == Corner.TopLeft)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    return originPosition + (scale * Vector2Int.down);
                case Corner.BottomRight:
                    return originPosition + (scale * scaleDir);
                case Corner.TopRight:
                    return originPosition + (scale * Vector2Int.right);
                case Corner.TopLeft:
                    return originPosition;
            }
        }
        Debug.LogError(corner.ToString() + " corner could not be found for bound with originCorner " + originCorner.ToString());
        return Vector2Int.zero;
    }

    public Vector2 RandomPoint()
    {
        float x = originPosition.x + Random.Range(0, (float)scale.x * scaleDir.x);
        float y = originPosition.y + Random.Range(0, (float)scale.y * scaleDir.y);
        return new Vector2(x, y);// + ((Vector2)scaleDir * 0.5f);
    }

    public Vector2Int RandomPointInt(bool includeEdges = true)
    {
        Vector2 randomPoint = RandomPoint();
        if (includeEdges == false)
        {
            randomPoint.x = Mathf.MoveTowards(randomPoint.x, GetCenter().x, 1.0f);
            randomPoint.y = Mathf.MoveTowards(randomPoint.y, GetCenter().y, 1.0f);
        }
        return new Vector2Int
        {
            x = Mathf.RoundToInt(randomPoint.x),
            y = Mathf.RoundToInt(randomPoint.y)
        };
    }

    public Vector2 GetCenter()
    {
        return originPosition + ((Vector2)scale * 0.5f * scaleDir);
    }

    public bool Intersects(Vector2 point)
    {
        Vector2Int min = GetCorner(Corner.BottomLeft);
        Vector2Int max = GetCorner(Corner.TopRight);
        bool isRight = point.x >= min.x;
        bool isLeft = point.x <= max.x;
        bool isAbove = point.y >= min.y;
        bool isBelow = point.y <= max.y;
        return isRight && isLeft && isAbove && isBelow;
    }

    public bool Inside(Vector2 point)
    {
        Vector2Int min = GetCorner(Corner.BottomLeft);
        Vector2Int max = GetCorner(Corner.TopRight);
        bool isRight = point.x > min.x;
        bool isLeft = point.x < max.x;
        bool isAbove = point.y > min.y;
        bool isBelow = point.y < max.y;
        return isRight && isLeft && isAbove && isBelow;
    }

}

public class DungeonRoom
{
    public Bounds[] RoomBounds { get { return roomBounds; } }
    public Vector2 Center { get { return center; } }

    private Bounds[] roomBounds;
    private Vector2 center;

    public DungeonRoom(int complexity)
    {
        roomBounds = new Bounds[complexity + 1];
        // Build a room out of multiple different bounds, based on the complexity value
        switch (complexity)
        {
            case 0:
                roomBounds[0] = new Bounds(Corner.BottomLeft, Vector2Int.zero, RandomVec2Int(3, 6));
                break;
            case 1:
                roomBounds[0] = new Bounds(Corner.BottomLeft, Vector2Int.zero, RandomVec2Int(2, 4));
                roomBounds[1] = new Bounds(RandomCorner(), roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                break;
            case 2:
                roomBounds[0] = new Bounds(Corner.BottomLeft, Vector2Int.zero, RandomVec2Int(2, 4));
                Corner firstCorner = RandomCorner();
                roomBounds[1] = new Bounds(firstCorner, roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                Corner secondCorner = RandomCorner(firstCorner);
                roomBounds[2] = new Bounds(secondCorner, roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                break;
            case 3:
                roomBounds[0] = new Bounds(Corner.BottomLeft, Vector2Int.zero, RandomVec2Int(2, 4));
                firstCorner = RandomCorner();
                roomBounds[1] = new Bounds(firstCorner, roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                secondCorner = RandomCorner(firstCorner);
                roomBounds[2] = new Bounds(secondCorner, roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                Corner thirdCorner = RandomCorner(firstCorner, secondCorner);
                roomBounds[3] = new Bounds(thirdCorner, roomBounds[0].RandomPointInt(false), RandomVec2Int(3, 5));
                break;
            default:
                Debug.LogWarning("Invalid complexity value has been passed to DungeonRoom");
                return;
        }

        center = Vector2.zero;
        for (int i = 0; i < roomBounds.Length; ++i)
        {
            center += roomBounds[i].GetCenter();
        }
        center = center / roomBounds.Length;
    }

    private Corner RandomCorner()
    {
        return (Corner)Random.Range(0, 4);
    }
    
    private Corner RandomCorner(params Corner[] avoid)
    {
        List<Corner> valid = new List<Corner>();
        // Iterate through all possible corners
        foreach (Corner corner in System.Enum.GetValues(typeof(Corner)))
        {
            bool isValid = true;
            // Iterate through all corners to avoid, if a match is found, flag the corner to not be added
            for (int i = 0; i < avoid.Length; ++i)
            {
                if (avoid[i] == corner)
                {
                    isValid = false;
                }
            }
            // If no match was found, add the corner to the list of valid corners
            if (isValid)
            {
                valid.Add(corner);
            }
        }
        if (valid.Count == 0)
        {
            Debug.LogError("Cannot generate random corner");
            return 0;
        }
        return valid[Random.Range(0, valid.Count)];
    }

    private Vector2Int RandomVec2Int(int min, int max)
    {
        return new Vector2Int(Random.Range(min, max), Random.Range(min, max));
    }

    public Vector2Int Min()
    {
        // Get all bottom left bound corners
        int[] x = new int[roomBounds.Length];
        int[] y = new int[roomBounds.Length];
        for (int i = 0; i < roomBounds.Length; ++i)
        {
            Vector2Int bl = roomBounds[i].GetCorner(Corner.BottomLeft);
            x[i] = bl.x;
            y[i] = bl.y;
        }
        // return the minimum values from each array
        return new Vector2Int(Mathf.Min(x), Mathf.Min(y));
    }

    public Vector2Int Max()
    {
        // Get all bottom left bound corners
        int[] x = new int[roomBounds.Length];
        int[] y = new int[roomBounds.Length];
        for (int i = 0; i < roomBounds.Length; ++i)
        {
            Vector2Int tr = roomBounds[i].GetCorner(Corner.TopRight);
            x[i] = tr.x;
            y[i] = tr.y;
        }
        // return the maximum values from each array
        return new Vector2Int(Mathf.Max(x), Mathf.Max(y));
    }

    public bool IntersectsRoom(Vector2 point)
    {
        foreach (Bounds bound in roomBounds)
        {
            if (bound.Intersects(point))
            {
                return true;
            }
        }
        return false;
    }

    public bool InsideRoom(Vector2 point)
    {
        foreach (Bounds bound in roomBounds)
        {
            if (bound.Inside(point))
            {
                return true;
            }
        }
        return false;
    }

    public void DrawRoom()
    {
        foreach (Bounds bound in roomBounds)
        {
            bound.DrawBounds();
            // Mark random point inside bound
            //DebugPosition(bound.RandomPointInt(false));
        }
    }

    public void DebugPosition(Vector2 point)
    {
        float pointScale = 0.1f;
        float duration = 1.0f;
        Color pointColor = Color.green;
        Debug.DrawLine(point + (Vector2.left * pointScale), point + (Vector2.right * pointScale), pointColor, duration);
        Debug.DrawLine(point + (Vector2.down * pointScale), point + (Vector2.up * pointScale), pointColor, duration);
    }
}
