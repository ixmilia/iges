using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesConnectionType
    {
        None = 0,
        NonSpecificLocalPoint = 1,
        NonSpecificPhysicalPoint = 2,
        LogicalComponentPin = 101,
        LogicalPortConnector = 102,
        LogicalOffpageConnector = 103,
        LogicalGlobalSignalConnector = 104,
        PhysicalPWASurfaceMountPin = 201,
        PhysicalPWABlindPin = 202,
        PhysicalPWAThruPin = 203,
        ImplementorDefined = -1
    }

    public enum IgesConnectionFunctionType
    {
        NotSpecified = 0,
        ElectricalSignal = 1,
        FluidFlowPath = 2
    }

    public enum IgesConnectionFunctionCode
    {
        Unspecified = 0,
        Input = 1,
        Output = 2,
        InputAndOutput = 3,
        Power = 4,
        Ground = 5,
        Anode = 6,
        Cathode = 7,
        Emitter = 8,
        Base = 9,
        Collector = 10,
        Source = 11,
        Gate = 12,
        Drain = 13,
        Case = 14,
        Shield = 15,
        InvertingInput = 16,
        RegulatedInput = 17,
        BoosterInput = 18,
        UnregulatedInput = 19,
        InvertingOutput = 20,
        RegulatedOutput = 21,
        BoosterOutput = 22,
        UnregulatedOutput = 23,
        Sink = 24,
        Strobe = 25,
        Enable = 26,
        Data = 27,
        Clock = 28,
        Set = 29,
        Reset = 30,
        Blanking = 31,
        Test = 32,
        Address = 33,
        Control = 34,
        Carry = 35,
        Sum = 36,
        Write = 37,
        Sense = 38,
        V_Plus = 39,
        Read = 40,
        Load = 41,
        Sync = 42,
        TriStateOutput = 43,
        VDD = 44,
        V_Negative = 45,
        VEE = 46,
        Reference = 47,
        ReferenceBypass = 48,
        ReferenceSupply = 49,
        Deferral = 98,
        NoConnection = 99,
        ImplementorDefined = -1
    }

    public class IgesConnectPoint : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ConnectPoint; } }

        public IgesPoint Location { get; set; }
        public IgesEntity DisplaySymbolGeometry { get; set; }
        public IgesConnectionType ConnectionType { get; set; }
        public int RawConnectionType { get; set; }
        public IgesConnectionFunctionType FunctionType { get; set; }
        public string FunctionIdentifier { get; set; }
        public IgesEntity FunctionIdentifierTextDisplayTemplate { get; set; }
        public string FunctionName { get; set; }
        public IgesEntity FunctionNameTextDisplayTemplate { get; set; }
        public int UniqueIdentifier { get; set; }
        public IgesConnectionFunctionCode FunctionCode { get; set; }
        public int RawFunctionCode { get; set; }
        public bool ConnectPointMayBeSwapped { get; set; }
        public IgesEntity Owner { get; set; }

        public IgesConnectPoint()
            : base()
        {
            EntityUseFlag = IgesEntityUseFlag.LogicalOrPositional;
            Location = IgesPoint.Origin;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            Location = Point3(parameters, ref index);
            binder.BindEntity(Integer(parameters, index++), e => DisplaySymbolGeometry = e);
            RawConnectionType = Integer(parameters, index++);
            ConnectionType = Enum.IsDefined(typeof(IgesConnectionType), RawConnectionType)
                ? ConnectionType = (IgesConnectionType)RawConnectionType
                : ConnectionType = IgesConnectionType.ImplementorDefined;
            FunctionType = (IgesConnectionFunctionType)Integer(parameters, index++);
            FunctionIdentifier = String(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => FunctionIdentifierTextDisplayTemplate = e);
            FunctionName = String(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => FunctionNameTextDisplayTemplate = e);
            UniqueIdentifier = Integer(parameters, index++);
            RawFunctionCode = Integer(parameters, index++);
            FunctionCode = Enum.IsDefined(typeof(IgesConnectionFunctionCode), RawFunctionCode)
                ? (IgesConnectionFunctionCode)RawFunctionCode
                : IgesConnectionFunctionCode.ImplementorDefined;
            ConnectPointMayBeSwapped = !Boolean(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => Owner = e);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return DisplaySymbolGeometry;
            yield return FunctionIdentifierTextDisplayTemplate;
            yield return FunctionNameTextDisplayTemplate;
            yield return Owner;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(Location.X);
            parameters.Add(Location.Y);
            parameters.Add(Location.Z);
            parameters.Add(binder.GetEntityId(DisplaySymbolGeometry));
            parameters.Add(ConnectionType == IgesConnectionType.ImplementorDefined ? RawConnectionType : (int)ConnectionType);
            parameters.Add((int)FunctionType);
            parameters.Add(FunctionIdentifier);
            parameters.Add(binder.GetEntityId(FunctionIdentifierTextDisplayTemplate));
            parameters.Add(FunctionName);
            parameters.Add(binder.GetEntityId(FunctionNameTextDisplayTemplate));
            parameters.Add(UniqueIdentifier);
            parameters.Add(FunctionCode == IgesConnectionFunctionCode.ImplementorDefined ? RawFunctionCode : (int)FunctionCode);
            parameters.Add(ConnectPointMayBeSwapped ? 0 : 1);
            parameters.Add(binder.GetEntityId(Owner));
        }
    }
}
