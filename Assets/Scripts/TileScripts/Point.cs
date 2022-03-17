using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point //노드로 변경
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    

    public static Point operator+(Point p1, Point p2)
    {
        return new Point(p1.x + p2.x, p1.y + p2.y);
    }

    public static Point operator-(Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.y - p2.y);
    }

    public static bool operator==(Point p1, Point p2)
    {
        return ((p1.x == p2.x) && (p1.y == p2.y));
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return ((p1.x != p2.x) && (p1.y != p2.y));
    }

    public override string ToString()
    {
        return string.Format("({0} , {1})", x, y);
    }
}
