// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.Offset.X = Double(parameters, 0);
            this.Offset.Y = Double(parameters, 1);
            this.Offset.Z = Double(parameters, 2);
            binder.BindEntity(Integer(parameters, 3), e => DisplacementCoordinateSystem = e as IgesTransformationMatrix);
            return 4;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return DisplacementCoordinateSystem;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.Offset.X);
            parameters.Add(this.Offset.Y);
            parameters.Add(this.Offset.Z);
            parameters.Add(binder.GetEntityId(DisplacementCoordinateSystem));
        }
    }
}
