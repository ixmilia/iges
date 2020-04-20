using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesColorDefinition : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ColorDefinition; } }

        // properties
        public double RedIntensity { get; set; }
        public double GreenIntensity { get; set; }
        public double BlueIntensity { get; set; }
        public string Name { get; set; }

        public IgesColorDefinition()
            : this(1.0, 1.0, 1.0, null)
        {
        }

        public IgesColorDefinition(double r, double g, double b, string name = null)
            : base()
        {
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
            this.RedIntensity = r;
            this.GreenIntensity = g;
            this.BlueIntensity = b;
            this.Name = name;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.RedIntensity = Double(parameters, 0);
            this.GreenIntensity = Double(parameters, 1);
            this.BlueIntensity = Double(parameters, 2);
            this.Name = String(parameters, 3);
            return 4;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.RedIntensity);
            parameters.Add(this.GreenIntensity);
            parameters.Add(this.BlueIntensity);
            parameters.Add(this.Name);
        }
    }
}
