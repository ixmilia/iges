// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSolidOfRevolution : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SolidOfRevolution; } }

        public IgesEntity Curve { get; set; }
        public double RevolutionAmount { get; set; }
        public IgesPoint PointOnAxis { get; set; }
        public IgesVector AxisDirection { get; set; }

        public bool IsClosedToAxis
        {
            get { return FormNumber == 0; }
            set { FormNumber = value ? 0 : 1; }
        }

        public bool IsClosedToSelf
        {
            get { return FormNumber == 1; }
            set { FormNumber = value ? 1 : 0; }
        }

        public IgesSolidOfRevolution()
        {
            RevolutionAmount = 1.0;
            PointOnAxis = IgesPoint.Origin;
            AxisDirection = IgesVector.ZAxis;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => Curve = e);
            RevolutionAmount = Double(parameters, 1);
            PointOnAxis.X = Double(parameters, 2);
            PointOnAxis.Y = Double(parameters, 3);
            PointOnAxis.Z = Double(parameters, 4);
            AxisDirection.X = Double(parameters, 5);
            AxisDirection.Y = Double(parameters, 6);
            AxisDirection.Z = Double(parameters, 7);
            return 8;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Curve;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Curve));
            parameters.Add(RevolutionAmount);
            parameters.Add(PointOnAxis?.X ?? 0.0);
            parameters.Add(PointOnAxis?.Y ?? 0.0);
            parameters.Add(PointOnAxis?.Z ?? 0.0);
            parameters.Add(AxisDirection?.X ?? 0.0);
            parameters.Add(AxisDirection?.Y ?? 0.0);
            parameters.Add(AxisDirection?.Z ?? 1.0);
        }
    }
}
