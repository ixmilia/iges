using System;

namespace IxMilia.Iges
{
    public struct IgesVector : IEquatable<IgesVector>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public IgesVector(double x, double y, double z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public bool IsZeroVector
        {
            get { return this.X == 0.0 && this.Y == 0.0 && this.Z == 0.0; }
        }

        public IgesVector Normalize()
        {
            return this / this.Length;
        }

        public IgesVector Cross(IgesVector v)
        {
            return new IgesVector(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);
        }

        public double Dot(IgesVector v)
        {
            return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
        }

        public static implicit operator IgesPoint(IgesVector vector)
        {
            return new IgesPoint(vector.X, vector.Y, vector.Z);
        }

        public static IgesVector operator -(IgesVector vector)
        {
            return new IgesVector(-vector.X, -vector.Y, -vector.Z);
        }

        public static IgesVector operator +(IgesVector p1, IgesVector p2)
        {
            return new IgesVector(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static IgesVector operator -(IgesVector p1, IgesVector p2)
        {
            return new IgesVector(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static IgesVector operator *(IgesVector vector, double operand)
        {
            return new IgesVector(vector.X * operand, vector.Y * operand, vector.Z * operand);
        }

        public static IgesVector operator /(IgesVector vector, double operand)
        {
            return new IgesVector(vector.X / operand, vector.Y / operand, vector.Z / operand);
        }

        public static bool operator ==(IgesVector p1, IgesVector p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(IgesVector p1, IgesVector p2)
        {
            return !(p1 == p2);
        }

        public bool Equals(IgesVector other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is IgesVector && this == (IgesVector)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public bool IsParallelTo(IgesVector other)
        {
            return this.Cross(other).IsZeroVector;
        }

        public static IgesVector XAxis
        {
            get { return new IgesVector(1, 0, 0); }
        }

        public static IgesVector YAxis
        {
            get { return new IgesVector(0, 1, 0); }
        }

        public static IgesVector ZAxis
        {
            get { return new IgesVector(0, 0, 1); }
        }

        public static IgesVector Zero
        {
            get { return new IgesVector(0, 0, 0); }
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }

        public static IgesVector RightVectorFromNormal(IgesVector normal)
        {
            if (normal == IgesVector.XAxis)
                return IgesVector.ZAxis;
            var right = IgesVector.XAxis;
            var up = normal.Cross(right);
            return up.Cross(normal).Normalize();
        }

        public static IgesVector NormalFromRightVector(IgesVector right)
        {
            // these two functions are identical, but the separate name makes them easier to understand
            return RightVectorFromNormal(right);
        }
    }
}
