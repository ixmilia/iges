using System;

namespace IxMilia.Iges
{
    public struct IgesPoint : IEquatable<IgesPoint>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public IgesPoint(double x, double y, double z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static implicit operator IgesVector(IgesPoint point)
        {
            return new IgesVector(point.X, point.Y, point.Z);
        }

        public static bool operator ==(IgesPoint p1, IgesPoint p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(IgesPoint p1, IgesPoint p2)
        {
            return !(p1 == p2);
        }

        public static IgesPoint operator +(IgesPoint p1, IgesVector p2)
        {
            return new IgesPoint(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static IgesVector operator -(IgesPoint p1, IgesVector p2)
        {
            return new IgesVector(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static IgesPoint operator *(IgesPoint p, double scalar)
        {
            return new IgesPoint(p.X * scalar, p.Y * scalar, p.Z * scalar);
        }

        public static IgesPoint operator /(IgesPoint p, double scalar)
        {
            return new IgesPoint(p.X / scalar, p.Y / scalar, p.Z / scalar);
        }

        public bool Equals(IgesPoint other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is IgesPoint && this == (IgesPoint)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }

        public static IgesPoint Origin
        {
            get { return new IgesPoint(0, 0, 0); }
        }
    }
}
