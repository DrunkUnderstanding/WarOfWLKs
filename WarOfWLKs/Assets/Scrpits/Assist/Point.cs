using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point ——（x,y)
/// </summary>
[System.Serializable]
public struct Point
{

    public int X { get; set; }
    public int Y { get; set; }

    /// <summary>
    /// 设置Point的值
    /// </summary>
    /// <param name="x">起始x</param>
    /// <param name="y">起始y</param>
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }

    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);
    }
}