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

	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		if (this.GetType() != obj.GetType()) return false;
		Point point = (Point)obj;

		if (this.X == point.X && this.Y == point.Y)
		{
			return true;
		}

		return false;


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