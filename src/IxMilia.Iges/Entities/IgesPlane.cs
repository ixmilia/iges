using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesPlaneBounding
    {
        BoundedNegative = -1,
        Unbounded = 0,
        BoundedPositive = 1
    }

    public class IgesPlane: IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Plane; } }

        // properties
        public double PlaneCoefficientA { get; set; }
        public double PlaneCoefficientB { get; set; }
        public double PlaneCoefficientC { get; set; }
        public double PlaneCoefficientD { get; set; }
        public IgesPoint DisplaySymbolLocation { get; set; }
        public double DisplaySymbolSize { get; set; }

        public IgesEntity ClosedCurveBoundingEntity { get; set; }

        // custom properties
        public IgesPlaneBounding Bounding
        {
            get
            {
                return (IgesPlaneBounding)FormNumber;
            }
            set
            {
                FormNumber = (int)value;
            }
        }

        public IgesPlane()
            : base()
        {
            DisplaySymbolLocation = IgesPoint.Origin;
        }

        public bool IsPointOnPlane(IgesPoint point)
        {
            return (PlaneCoefficientA * point.X) + (PlaneCoefficientB * point.Y) + (PlaneCoefficientC * point.Z) == PlaneCoefficientD;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.PlaneCoefficientA = Double(parameters, index++);
            this.PlaneCoefficientB = Double(parameters, index++);
            this.PlaneCoefficientC = Double(parameters, index++);
            this.PlaneCoefficientD = Double(parameters, index++);

            var closedCurvePointer = Integer(parameters, index++);
            Debug.Assert((FormNumber == 0 && closedCurvePointer == 0) || (FormNumber != 0 && closedCurvePointer != 0), "Form 0 should have no pointer, form (+/-)1 should");
            if (closedCurvePointer != 0)
            {
                binder.BindEntity(closedCurvePointer, e => ClosedCurveBoundingEntity = e);
            }

            this.DisplaySymbolLocation = Point3(parameters, ref index);
            this.DisplaySymbolSize = Double(parameters, index++);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return ClosedCurveBoundingEntity;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(PlaneCoefficientA);
            parameters.Add(PlaneCoefficientB);
            parameters.Add(PlaneCoefficientC);
            parameters.Add(PlaneCoefficientD);
            parameters.Add(binder.GetEntityId(ClosedCurveBoundingEntity));
            parameters.Add(DisplaySymbolLocation.X);
            parameters.Add(DisplaySymbolLocation.Y);
            parameters.Add(DisplaySymbolLocation.Z);
            parameters.Add(DisplaySymbolSize);
        }
    }
}
