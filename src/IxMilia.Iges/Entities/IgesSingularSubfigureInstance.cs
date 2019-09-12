// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSingularSubfigureInstance : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SingularSubfigureInstance; } }

        public IgesSubfigureDefinition SubfigureDefinition { get; set; }

        public double XTranslation { get; set; }
        public double YTranslation { get; set; }
        public double ZTranslation { get; set; }
        public double Scale { get; set; }

        public IgesSingularSubfigureInstance()
            : base()
        {
            this.XTranslation = 0;
            this.YTranslation = 0;
            this.ZTranslation = 0;
            this.Scale = 1;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), subfigureDefinition => SubfigureDefinition = subfigureDefinition as IgesSubfigureDefinition);
            this.XTranslation = this.Double(parameters, index++);
            this.YTranslation = Double(parameters, index++);
            this.ZTranslation = Double(parameters, index++);
            this.Scale = Double(parameters, index++);
            return index;
        }


        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(this.SubfigureDefinition));
            parameters.Add(this.XTranslation);
            parameters.Add(this.YTranslation);
            parameters.Add(this.ZTranslation);
            parameters.Add(this.Scale);

        }
    }
}