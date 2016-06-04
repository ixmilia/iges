// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSelectedComponent : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SelectedComponent; } }

        public IgesBooleanTree BooleanTree { get; set; }
        public IgesPoint SelectionPoint { get; set; }

        public IgesSelectedComponent()
            : this(null, IgesPoint.Origin)
        {
        }

        public IgesSelectedComponent(IgesBooleanTree booleanTree, IgesPoint selectionPoint)
        {
            EntityUseFlag = IgesEntityUseFlag.Other;
            BooleanTree = booleanTree;
            SelectionPoint = selectionPoint;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => BooleanTree = e as IgesBooleanTree);
            SelectionPoint.X = Double(parameters, 1);
            SelectionPoint.Y = Double(parameters, 2);
            SelectionPoint.Z = Double(parameters, 3);
            return 4;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return BooleanTree;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(BooleanTree));
            parameters.Add(SelectionPoint?.X ?? 0.0);
            parameters.Add(SelectionPoint?.Y ?? 0.0);
            parameters.Add(SelectionPoint?.Z ?? 0.0);
        }
    }
}
