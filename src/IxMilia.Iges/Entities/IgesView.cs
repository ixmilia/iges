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
            SubEntities.Add(left);
            SubEntities.Add(top);
            SubEntities.Add(right);
            SubEntities.Add(bottom);
            SubEntities.Add(back);
            SubEntities.Add(front);
        }

        public IgesPlane ViewVolumeLeft
        {
            get { return SubEntities[0] as IgesPlane; }
            set { SubEntities[0] = value; }
        }

        public IgesPlane ViewVolumeTop
        {
            get { return SubEntities[1] as IgesPlane; }
            set { SubEntities[1] = value; }
        }

        public IgesPlane ViewVolumeRight
        {
            get { return SubEntities[2] as IgesPlane; }
            set { SubEntities[2] = value; }
        }

        public IgesPlane ViewVolumeBottom
        {
            get { return SubEntities[3] as IgesPlane; }
            set { SubEntities[3] = value; }
        }

        public IgesPlane ViewVolumeBack
        {
            get { return SubEntities[4] as IgesPlane; }
            set { SubEntities[4] = value; }
        }

        public IgesPlane ViewVolumeFront
        {
            get { return SubEntities[5] as IgesPlane; }
            set { SubEntities[5] = value; }
        }

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

        protected override void WriteParameters(List<object> parameters)
        {
            base.WriteParameters(parameters);
            Debug.Assert(SubEntityIndices.Count == 6);
            for (int i = 0; i < SubEntityIndices.Count; i++)
            {
                parameters.Add(SubEntityIndices[i]);
            }
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
        }
    }
}
