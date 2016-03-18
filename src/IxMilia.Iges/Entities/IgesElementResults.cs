// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesResultsReportingType
    {
        ElementNode = 0,
        ElementCentroid = 1,
        ConstantOnAllFaces = 2,
        GaussPoints = 3
    }

    public enum IgesDataLayerType
    {
        NotSpecial = 0,
        TopSurface = 1,
        MiddleSurface = 2,
        BottomSurface = 3,
        OrderedSet = 4
    }

    public class IgesElementResult
    {
        public int Identifier { get; set; }
        public IgesEntity Entity { get; set; }
        public int ElementTopologyType { get; set; }
        public int LayerCount { get; set; }
        public IgesDataLayerType DataLayerType { get; set; }
        public int RDRL { get; set; }
        public List<double> Results { get; } = new List<double>();
        public List<double> Values { get; } = new List<double>();
    }

    public class IgesElementResults : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ElementResults; } }

        public IgesResultType ResultsType
        {
            get { return (IgesResultType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        public IgesGeneralNote GeneralNote { get; set; }
        public int AnalysisSubcase { get; set; }
        public DateTime AnalysisTime { get; set; }
        public IgesResultsReportingType ReportingType { get; set; }
        public List<IgesElementResult> Elements { get; } = new List<IgesElementResult>();

        protected override int ReadParameters(List<string> parameters)
        {
            Elements.Clear();

            int index = 0;
            SubEntityIndices.Add(Integer(parameters, index++));
            AnalysisSubcase = Integer(parameters, index++);
            AnalysisTime = DateTime(parameters, index++);
            var valueCount = Integer(parameters, index++);
            ReportingType = (IgesResultsReportingType)Integer(parameters, index++);
            var elementCount = Integer(parameters, index++);
            for (int i = 0; i < elementCount; i++)
            {
                var result = new IgesElementResult();
                result.Identifier = Integer(parameters, index++);
                SubEntityIndices.Add(Integer(parameters, index++));
                result.ElementTopologyType = Integer(parameters, index++);
                result.LayerCount = Integer(parameters, index++);
                result.DataLayerType = (IgesDataLayerType)Integer(parameters, index++);
                var resultCount = Integer(parameters, index++);
                result.RDRL = Integer(parameters, index++);
                for (int j = 0; j < resultCount; j++)
                {
                    result.Results.Add(Double(parameters, index++));
                }

                var numberOfValues = Integer(parameters, index++); // should be `valueCount * result.LayerCount * resultCount`
                for (int j = 0; j < numberOfValues; j++)
                {
                    result.Values.Add(Double(parameters, index++));
                }

                Elements.Add(result);
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            GeneralNote = SubEntities[0] as IgesGeneralNote;
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Entity = SubEntities[i + 1];
            }
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(GeneralNote);
            SubEntities.AddRange(Elements.Select(e => e.Entity));
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(AnalysisSubcase);
            parameters.Add(AnalysisTime);
            parameters.Add(Elements.Count);
            parameters.Add((int)ReportingType);
            parameters.Add(Elements.Count);
            for (int i = 0; i < Elements.Count; i++)
            {
                var element = Elements[i];
                parameters.Add(element.Identifier);
                parameters.Add(SubEntityIndices[i + 1]);
                parameters.Add(element.ElementTopologyType);
                parameters.Add(element.LayerCount);
                parameters.Add((int)element.DataLayerType);
                parameters.Add(element.Results.Count);
                parameters.Add(element.RDRL);
                parameters.AddRange(element.Results.Cast<object>());
                parameters.Add(element.Values.Count);
                parameters.AddRange(element.Values.Cast<object>());
            }
        }
    }
}
