// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public abstract class IgesViewBase : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.View; } }

        // properties
        public int ViewNumber { get; set; }
        public double ScaleFactor { get; set; }

        protected IgesViewBase(int viewNumber, double scaleFactor)
            : base()
        {
            this.EntityUseFlag = IgesEntityUseFlag.Annotation;
            this.ViewNumber = viewNumber;
            this.ScaleFactor = scaleFactor;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.ViewNumber = Integer(parameters, 0);
            this.ScaleFactor = Double(parameters, 1);
            return 2;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(ViewNumber);
            parameters.Add(ScaleFactor);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Annotation);
        }
    }
}
