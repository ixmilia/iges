// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        protected override int ReadParameters(List<string> parameters)
        {
            Location.X = Double(parameters, 0);
            Location.Y = Double(parameters, 1);
            Location.Z = Double(parameters, 2);
            SubEntityIndices.Add(Integer(parameters, 3));
            RawConnectionType = Integer(parameters, 4);
            ConnectionType = Enum.IsDefined(typeof(IgesConnectionType), RawConnectionType)
                ? ConnectionType = (IgesConnectionType)RawConnectionType
                : ConnectionType = IgesConnectionType.ImplementorDefined;
            FunctionType = (IgesConnectionFunctionType)Integer(parameters, 5);
            FunctionIdentifier = String(parameters, 6);
            SubEntityIndices.Add(Integer(parameters, 7));
            FunctionName = String(parameters, 8);
            SubEntityIndices.Add(Integer(parameters, 9));
            UniqueIdentifier = Integer(parameters, 10);
            RawFunctionCode = Integer(parameters, 11);
            FunctionCode = Enum.IsDefined(typeof(IgesConnectionFunctionCode), RawFunctionCode)
                ? (IgesConnectionFunctionCode)RawFunctionCode
                : IgesConnectionFunctionCode.ImplementorDefined;
            ConnectPointMayBeSwapped = !Boolean(parameters, 12);
            SubEntityIndices.Add(Integer(parameters, 13));
            return 14;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(Location?.X ?? 0.0);
            parameters.Add(Location?.Y ?? 0.0);
            parameters.Add(Location?.Z ?? 0.0);
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(ConnectionType == IgesConnectionType.ImplementorDefined ? RawConnectionType : (int)ConnectionType);
            parameters.Add((int)FunctionType);
            parameters.Add(FunctionIdentifier);
            parameters.Add(SubEntityIndices[1]);
            parameters.Add(FunctionName);
            parameters.Add(SubEntityIndices[2]);
            parameters.Add(UniqueIdentifier);
            parameters.Add(FunctionCode == IgesConnectionFunctionCode.ImplementorDefined ? RawFunctionCode : (int)FunctionCode);
            parameters.Add(ConnectPointMayBeSwapped ? 0 : 1);
            parameters.Add(SubEntityIndices[3]);
        }

        internal override void OnBeforeWrite()
        {
            base.OnBeforeWrite();
            SubEntities.Clear();
            SubEntities.Add(DisplaySymbolGeometry);
            SubEntities.Add(FunctionIdentifierTextDisplayTemplate);
            SubEntities.Add(FunctionNameTextDisplayTemplate);
            SubEntities.Add(Owner);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.LogicalOrPositional);
            DisplaySymbolGeometry = SubEntities[0];
            FunctionIdentifierTextDisplayTemplate = SubEntities[1];
            FunctionNameTextDisplayTemplate = SubEntities[2];
            Owner = SubEntities[3];
        }
    }
}
