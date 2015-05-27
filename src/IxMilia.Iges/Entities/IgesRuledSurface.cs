// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesRuledSurfaceDirection
    {
        FirstToFirst_LastToLast = 0,
        FirstToLast_LastToFirst = 1
    }

    public class IgesRuledSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RuledSurface; } }

        public IgesEntity FirstCurve { get; set; }
        public IgesEntity SecondCurve { get; set; }
        public IgesRuledSurfaceDirection Direction { get; set; }
        public bool IsDevelopable { get; set; }
        public bool CurvesProvideParameterization
        {
            get { return FormNumber == 0; }
            set { FormNumber = value ? 0 : 1; }
        }

        public IgesRuledSurface()
            : base()
        {
        }

        public IgesRuledSurface(IgesEntity firstCurve, IgesEntity secondCurve)
            : this()
        {
            FirstCurve = firstCurve;
            SecondCurve = secondCurve;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            SubEntityIndices.Add(Integer(parameters, 0));
            SubEntityIndices.Add(Integer(parameters, 1));
            Direction = (IgesRuledSurfaceDirection)Integer(parameters, 2);
            IsDevelopable = Boolean(parameters, 3);
            return 4;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(SubEntityIndices[1]);
            parameters.Add((int)Direction);
            parameters.Add(IsDevelopable ? 1 : 0);
        }

        internal override void OnBeforeWrite()
        {
            base.OnBeforeWrite();
            SubEntities.Clear();
            SubEntities.Add(FirstCurve);
            SubEntities.Add(SecondCurve);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            Debug.Assert(FormNumber == 0 || FormNumber == 1);
            FirstCurve = SubEntities[0];
            SecondCurve = SubEntities[1];
        }
    }
}
