// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Iges
{
    public class IgesPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public IgesPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static IgesPoint Origin
        {
            get
            {
                return new IgesPoint(0.0, 0.0, 0.0);
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }

        public static bool operator ==(IgesPoint a, IgesPoint b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (((object)a) == null || ((object)b) == null)
                return false;
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(IgesPoint a, IgesPoint b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() & Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is IgesPoint)
                return this == (IgesPoint)obj;
            return false;
        }
    }

    public class IgesVector : IgesPoint
    {
        public IgesVector(double x, double y, double z)
            : base(x, y, z)
        {
        }

        public static IgesVector Zero
        {
            get { return new IgesVector(0.0, 0.0, 0.0); }
        }

        public static IgesVector ZAxis
        {
            get { return new IgesVector(0.0, 0.0, 1.0); }
        }
    }
}
