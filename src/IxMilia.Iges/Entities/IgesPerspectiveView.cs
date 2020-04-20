using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    [Flags]
    public enum IgesDepthClipping
    {
        None = 0x00,
        BackClipping = 0x01,
        FrontClipping = 0x02,
        FrontAndBackClipping = BackClipping | FrontClipping
    }

    public class IgesPerspectiveView : IgesViewBase
    {
        public IgesPerspectiveView()
            : this(0, 0.0, IgesVector.Zero, IgesPoint.Origin, IgesPoint.Origin, IgesVector.Zero, 0.0, 0.0, 0.0, 0.0, 0.0, IgesDepthClipping.None, 0.0, 0.0)
        {
        }

        public IgesPerspectiveView(
            int viewNumber,
            double scaleFactor,
            IgesVector viewPlaneNormal,
            IgesPoint referencePoint,
            IgesPoint centerOfProjection,
            IgesVector upVector,
            double viewPlaneDistance,
            double leftClippingCoordinate,
            double rightClippingCoordinate,
            double bottomClippingCoordinate,
            double topClippingCoordinate,
            IgesDepthClipping depthClipping,
            double backClippingCoordinate,
            double frontClippingCoordinate)
            : base(viewNumber, scaleFactor)
        {
            this.FormNumber = 1;
            this.ViewPlaneNormal = viewPlaneNormal;
            this.ViewReferencePoint = referencePoint;
            this.CenterOfProjection = centerOfProjection;
            this.ViewUpVector = upVector;
            this.ViewPlaneDistance = viewPlaneDistance;
            this.ClippingWindowLeftCoordinate = leftClippingCoordinate;
            this.ClippingWindowRightCoordinate = rightClippingCoordinate;
            this.ClippingWindowBottomCoordinate = bottomClippingCoordinate;
            this.ClippingWindowTopCoordinate = topClippingCoordinate;
            this.DepthClipping = depthClipping;
            this.ClippingWindowBackCoordinate = backClippingCoordinate;
            this.ClippingWindowFrontCoordinate = frontClippingCoordinate;
        }

        public IgesVector ViewPlaneNormal { get; set; }
        public IgesPoint ViewReferencePoint { get; set; }
        public IgesPoint CenterOfProjection { get; set; }
        public IgesVector ViewUpVector { get; set; }
        public double ViewPlaneDistance { get; set; }
        public double ClippingWindowLeftCoordinate { get; set; }
        public double ClippingWindowRightCoordinate { get; set; }
        public double ClippingWindowBottomCoordinate { get; set; }
        public double ClippingWindowTopCoordinate { get; set; }
        public IgesDepthClipping DepthClipping { get; set; }
        public double ClippingWindowBackCoordinate { get; set; }
        public double ClippingWindowFrontCoordinate { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var nextIndex = base.ReadParameters(parameters, binder);
            this.ViewPlaneNormal = VectorOrDefault(parameters, ref nextIndex, IgesVector.Zero);
            this.ViewReferencePoint = Point3(parameters, ref nextIndex);
            this.CenterOfProjection = Point3(parameters, ref nextIndex);
            this.ViewUpVector = VectorOrDefault(parameters, ref nextIndex, IgesVector.Zero);
            this.ViewPlaneDistance = Double(parameters, nextIndex++);
            this.ClippingWindowLeftCoordinate = Double(parameters, nextIndex++);
            this.ClippingWindowRightCoordinate = Double(parameters, nextIndex++);
            this.ClippingWindowBottomCoordinate = Double(parameters, nextIndex++);
            this.ClippingWindowTopCoordinate = Double(parameters, nextIndex++);
            this.DepthClipping = (IgesDepthClipping)Integer(parameters, nextIndex++);
            this.ClippingWindowBackCoordinate = Double(parameters, nextIndex++);
            this.ClippingWindowFrontCoordinate = Double(parameters, nextIndex++);
            return nextIndex;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            base.WriteParameters(parameters, binder);
            parameters.Add(ViewPlaneNormal.X);
            parameters.Add(ViewPlaneNormal.Y);
            parameters.Add(ViewPlaneNormal.Z);
            parameters.Add(ViewReferencePoint.X);
            parameters.Add(ViewReferencePoint.Y);
            parameters.Add(ViewReferencePoint.Z);
            parameters.Add(CenterOfProjection.X);
            parameters.Add(CenterOfProjection.Y);
            parameters.Add(CenterOfProjection.Z);
            parameters.Add(ViewUpVector.X);
            parameters.Add(ViewUpVector.Y);
            parameters.Add(ViewUpVector.Z);
            parameters.Add(ViewPlaneDistance);
            parameters.Add(ClippingWindowLeftCoordinate);
            parameters.Add(ClippingWindowRightCoordinate);
            parameters.Add(ClippingWindowBottomCoordinate);
            parameters.Add(ClippingWindowTopCoordinate);
            parameters.Add((int)DepthClipping);
            parameters.Add(ClippingWindowBackCoordinate);
            parameters.Add(ClippingWindowFrontCoordinate);
        }
    }
}
