// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesView: IgesViewBase
    {
        public IgesView()
            : this(0, 0.0, null, null, null, null, null, null)
        {
        }

        public IgesView(int viewNumber, double scaleFactor, IgesPlane left, IgesPlane top, IgesPlane right, IgesPlane bottom, IgesPlane back, IgesPlane front)
            : base(viewNumber, scaleFactor)
        {
            this.FormNumber = 0;
            ViewVolumeLeft = left;
            ViewVolumeTop = top;
            ViewVolumeRight = right;
            ViewVolumeBottom = bottom;
            ViewVolumeBack = back;
            ViewVolumeFront = front;
        }

        public IgesPlane ViewVolumeLeft { get; set; }

        public IgesPlane ViewVolumeTop { get; set; }

        public IgesPlane ViewVolumeRight { get; set; }

        public IgesPlane ViewVolumeBottom { get; set; }

        public IgesPlane ViewVolumeBack { get; set; }

        public IgesPlane ViewVolumeFront { get; set; }

        protected override int ReadParameters(List<string> parameters)
        {
            var nextIndex = base.ReadParameters(parameters);
            SubEntityIndices.Add(Integer(parameters, nextIndex)); // xvminp
            SubEntityIndices.Add(Integer(parameters, nextIndex + 1)); // yvmaxp
            SubEntityIndices.Add(Integer(parameters, nextIndex + 2)); // xvmaxp
            SubEntityIndices.Add(Integer(parameters, nextIndex + 3)); // yvminp
            SubEntityIndices.Add(Integer(parameters, nextIndex + 4)); // zvminp
            SubEntityIndices.Add(Integer(parameters, nextIndex + 5)); // zvmaxp
            return nextIndex + 6;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            ViewVolumeLeft = SubEntities[0] as IgesPlane;
            ViewVolumeTop = SubEntities[1] as IgesPlane;
            ViewVolumeRight = SubEntities[2] as IgesPlane;
            ViewVolumeBottom = SubEntities[3] as IgesPlane;
            ViewVolumeBack = SubEntities[4] as IgesPlane;
            ViewVolumeFront = SubEntities[5] as IgesPlane;
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(ViewVolumeLeft);
            SubEntities.Add(ViewVolumeTop);
            SubEntities.Add(ViewVolumeRight);
            SubEntities.Add(ViewVolumeBottom);
            SubEntities.Add(ViewVolumeBack);
            SubEntities.Add(ViewVolumeFront);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            base.WriteParameters(parameters);
            Debug.Assert(SubEntityIndices.Count == 6);
            for (int i = 0; i < SubEntityIndices.Count; i++)
            {
                parameters.Add(SubEntityIndices[i]);
            }
        }
    }
}
