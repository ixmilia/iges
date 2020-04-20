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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            Elements.Clear();

            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => GeneralNote = e as IgesGeneralNote);
            AnalysisSubcase = Integer(parameters, index++);
            AnalysisTime = DateTime(parameters, index++);
            var valueCount = Integer(parameters, index++);
            ReportingType = (IgesResultsReportingType)Integer(parameters, index++);
            var elementCount = Integer(parameters, index++);
            for (int i = 0; i < elementCount; i++)
            {
                var result = new IgesElementResult();
                result.Identifier = Integer(parameters, index++);
                binder.BindEntity(Integer(parameters, index++), e => result.Entity = e);
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

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return GeneralNote;
            foreach (var element in Elements)
            {
                yield return element.Entity;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(AnalysisSubcase);
            parameters.Add(AnalysisTime);
            parameters.Add(Elements.Count);
            parameters.Add((int)ReportingType);
            parameters.Add(Elements.Count);
            foreach (var element in Elements)
            {
                parameters.Add(element.Identifier);
                parameters.Add(binder.GetEntityId(element.Entity));
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
