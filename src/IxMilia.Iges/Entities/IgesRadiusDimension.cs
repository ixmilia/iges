// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRadiusDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RadiusDimension; } }

        public IgesPoint ArcCenter { get; set; }

        public bool HasTwoLeaders { get { return SecondLeader != null; } }

        public IgesRadiusDimension()
        {
            ArcCenter = IgesPoint.Origin;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            ArcCenter.X = Double(parameters, index++);
            ArcCenter.Y = Double(parameters, index++);
            if (FormNumber == 1)
            {
                binder.BindEntity(Integer(parameters, index++), leader => SecondLeader = leader as IgesLeader);
            }

            return index;
        }

        internal override void OnBeforeWrite()
        {
            FormNumber = HasTwoLeaders ? 1 : 0;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(ArcCenter?.X ?? 0.0);
            parameters.Add(ArcCenter?.Y ?? 0.0);
            if (HasTwoLeaders)
            {
                parameters.Add(binder.GetEntityId(SecondLeader));
            }
        }
    }
}
