// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesNode : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Node; } }

        public IgesPoint Offset { get; set; }

        public IgesTransformationMatrix DisplacementCoordinateSystem { get; set; }

        public uint NodeNumber
        {
            get { return EntitySubscript; }
            set { EntitySubscript = value; }
        }

        public IgesNode()
            : this(IgesPoint.Origin)
        {
        }

        public IgesNode(IgesPoint offset)
            : base()
        {
            EntityUseFlag = IgesEntityUseFlag.LogicalOrPositional;
            FormNumber = 0;
            Offset = offset;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.Offset.X = Double(parameters, 0);
            this.Offset.Y = Double(parameters, 1);
            this.Offset.Z = Double(parameters, 2);
            SubEntityIndices.Add(Integer(parameters, 3));
            return 4;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.Offset.X);
            parameters.Add(this.Offset.Y);
            parameters.Add(this.Offset.Z);
            parameters.Add(SubEntityIndices.Single());
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(DisplacementCoordinateSystem);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            DisplacementCoordinateSystem = SubEntities.Single() as IgesTransformationMatrix;
        }
    }
}
