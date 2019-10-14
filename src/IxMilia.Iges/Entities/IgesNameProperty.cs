namespace IxMilia.Iges.Entities
{
    using System;
    using System.Collections.Generic;

    public class IgesNameProperty : IgesProperty
    {
        public String Name { get; private set; }

        public IgesNameProperty():base()
        {
            this.FormNumber = 15;

        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var nextIndex = base.ReadParameters(parameters, binder);
            this.Name = this.String(parameters, nextIndex);
            return nextIndex + this.PropertyCount;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            this.PropertyCount = 1;
            base.WriteParameters(parameters, binder);
            parameters.Add(this.Name);
        }
    }
}