// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesClosedAreaType
    {
        ReferencedEntity = 0,
        Circular = 1,
        Rectangular = 2,
        Donut = 3,
        Canoe = 4
    }

    public class IgesFlash : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Flash; } }

        private IgesEntity _referenceEntity;

        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double SizeParameter1 { get; set; }
        public double SizeParameter2 { get; set; }
        public double RotationAngle { get; set; }

        public IgesEntity ReferenceEntity
        {
            get { return _referenceEntity; }
            set
            {
                _referenceEntity = value;
                AreaType = IgesClosedAreaType.ReferencedEntity;
            }
        }

        public IgesClosedAreaType AreaType
        {
            get { return (IgesClosedAreaType)FormNumber; }
            set
            {
                FormNumber = (int)value;
                if (AreaType != IgesClosedAreaType.ReferencedEntity)
                {
                    _referenceEntity = null;
                }
            }
        }

        public IgesFlash()
            : base()
        {
            Hierarchy = IgesHierarchy.GlobalTopDown;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            XOffset = Double(parameters, 0);
            YOffset = Double(parameters, 1);
            SizeParameter1 = Double(parameters, 2);
            SizeParameter2 = Double(parameters, 3);
            RotationAngle = Double(parameters, 4);
            SubEntityIndices.Add(Integer(parameters, 5));
            return 6;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(XOffset);
            parameters.Add(YOffset);
            parameters.Add(SizeParameter1);
            parameters.Add(SizeParameter2);
            parameters.Add(RotationAngle);
            parameters.Add(SubEntityIndices[0]);
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(ReferenceEntity);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(Hierarchy == IgesHierarchy.GlobalTopDown);
            _referenceEntity = SubEntities[0];
        }
    }
}
