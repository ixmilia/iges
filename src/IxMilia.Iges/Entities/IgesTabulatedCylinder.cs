// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesTabulatedCylinder : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TabulatedCylinder; } }

        public IgesEntity Directrix { get; set; }
        public IgesPoint GeneratrixTerminatePoint { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => Directrix = e);
            var x = Double(parameters, 1);
            var y = Double(parameters, 2);
            var z = Double(parameters, 3);
            GeneratrixTerminatePoint = new IgesPoint(x, y, z);
            return 4;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Directrix;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Directrix));
            parameters.Add(GeneratrixTerminatePoint?.X ?? 0.0);
            parameters.Add(GeneratrixTerminatePoint?.Y ?? 0.0);
            parameters.Add(GeneratrixTerminatePoint?.Z ?? 0.0);
        }
    }
}
