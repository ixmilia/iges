// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesDiameterDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.DiameterDimension; } }

        public IgesPoint ArcCenter { get; set; } = IgesPoint.Origin;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(index++, generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(index++, leader => FirstLeader = leader as IgesLeader);
            binder.BindEntity(index++, leader => SecondLeader = leader as IgesLeader);
            ArcCenter = new IgesPoint(
                Double(parameters, index++),
                Double(parameters, index++),
                0.0);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(binder.GetEntityId(SecondLeader));
            parameters.Add(ArcCenter?.X ?? 0.0);
            parameters.Add(ArcCenter?.Y ?? 0.0);
        }
    }
}
