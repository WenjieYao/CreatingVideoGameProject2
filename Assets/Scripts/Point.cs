using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator == (Point p1, Point p2)
        {
            if ((object)p1 == null)
                return (object)p2 == null;

            return p1.Equals(p2);
        }

        public static bool operator != (Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var p2 = (Point)obj;
            return (X == p2.X && Y == p2.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
}
