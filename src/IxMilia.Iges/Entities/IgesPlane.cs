// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        protected override int ReadParameters(List<string> parameters)
        {
            this.PlaneCoefficientA = Double(parameters, 0);
            this.PlaneCoefficientB = Double(parameters, 1);
            this.PlaneCoefficientC = Double(parameters, 2);
            this.PlaneCoefficientD = Double(parameters, 3);

            var closedCurvePointer = Integer(parameters, 4);
            closedCurvePointer = Integer(parameters, 4);
            Debug.Assert((FormNumber == 0 && closedCurvePointer == 0) || (FormNumber != 0 && closedCurvePointer != 0), "Form 0 should have no pointer, form (+/-)1 should");
            if (closedCurvePointer != 0)
            {
                SubEntityIndices.Add(closedCurvePointer);
            }

            this.DisplaySymbolLocation.X = Double(parameters, 5);
            this.DisplaySymbolLocation.Y = Double(parameters, 6);
            this.DisplaySymbolLocation.Z = Double(parameters, 7);
            this.DisplaySymbolSize = Double(parameters, 8);

            return 9;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            this.ClosedCurveBoundingEntity = SubEntities.Count > 0 ? SubEntities[0] : null;
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(ClosedCurveBoundingEntity);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(PlaneCoefficientA);
            parameters.Add(PlaneCoefficientB);
            parameters.Add(PlaneCoefficientC);
            parameters.Add(PlaneCoefficientD);
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(DisplaySymbolLocation.X);
            parameters.Add(DisplaySymbolLocation.Y);
            parameters.Add(DisplaySymbolLocation.Z);
            parameters.Add(DisplaySymbolSize);
        }
    }
}
