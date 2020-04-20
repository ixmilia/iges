using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesCopiousDataPointFormat
    {
        CommonZ = 1,
        Coordinates = 2,
        CoordinatesAndVectors = 3
    }

    public enum IgesCopiousDataType
    {
        PointSet,
        LinearPath,
        CenterLine,
        Section,
        WitnessLine,
        ClosedCurve
    }

    public enum IgesCenterlineType
    {
        Simple = 20,
        Conjuction = 21
    }

    public enum IgesSectionType
    {
        ParallelSegments = 31,
        ParallelLinePairs = 32,
        AlternatingSolidAndDashed = 33,
        ParallelQuads = 34,
        ParallelTriples = 35,
        ParallelSets = 36,
        PerpendicularSets = 37,
        PerpendicularAlternatingSets = 38
    }

    public class IgesCopiousData : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CopiousData; } }

        public List<IgesPoint> DataPoints { get; private set; }
        public List<IgesVector> DataVectors { get; private set; }

        private IgesCopiousDataType _dataType;
        private IgesCopiousDataPointFormat _pointFormat;
        private IgesCenterlineType _centerlineType;
        private IgesSectionType _sectionType;

        public IgesCopiousDataType DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                UpdateFormNumber();
            }
        }

        public IgesCopiousDataPointFormat PointFormat
        {
            get { return _pointFormat; }
            set
            {
                _pointFormat = value;
                UpdateFormNumber();
            }
        }

        public IgesCenterlineType CenterlineType
        {
            get { return _centerlineType; }
            set
            {
                _centerlineType = value;
                UpdateFormNumber();
            }
        }

        public IgesSectionType SectionType
        {
            get { return _sectionType; }
            set
            {
                _sectionType = value;
                UpdateFormNumber();
            }
        }

        public IgesCopiousData()
            : base()
        {
            DataPoints = new List<IgesPoint>();
            DataVectors = new List<IgesVector>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            _pointFormat = (IgesCopiousDataPointFormat)Integer(parameters, index++);
            var tupleCount = Integer(parameters, index++);
            double x, y, z, i, j, k;
            switch (_pointFormat)
            {
                case IgesCopiousDataPointFormat.CommonZ:
                    z = Double(parameters, index++);
                    for (int n = 0; n < tupleCount; n++)
                    {
                        x = Double(parameters, index++);
                        y = Double(parameters, index++);
                        DataPoints.Add(new IgesPoint(x, y, z));
                    }

                    break;
                case IgesCopiousDataPointFormat.Coordinates:
                    for (int n = 0; n < tupleCount; n++)
                    {
                        x = Double(parameters, index++);
                        y = Double(parameters, index++);
                        z = Double(parameters, index++);
                        DataPoints.Add(new IgesPoint(x, y, z));
                    }

                    break;
                case IgesCopiousDataPointFormat.CoordinatesAndVectors:
                    for (int n = 0; n < tupleCount; n++)
                    {
                        x = Double(parameters, index++);
                        y = Double(parameters, index++);
                        z = Double(parameters, index++);
                        i = Double(parameters, index++);
                        j = Double(parameters, index++);
                        k = Double(parameters, index++);
                        DataPoints.Add(new IgesPoint(x, y, z));
                        DataVectors.Add(new IgesVector(i, j, k));
                    }

                    break;
            }

            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add((int)PointFormat);
            switch (PointFormat)
            {
                case IgesCopiousDataPointFormat.CommonZ:
                    parameters.Add(DataPoints.Count);
                    parameters.Add(DataPoints.First().Z);
                    foreach (var p in DataPoints)
                    {
                        parameters.Add(p.X);
                        parameters.Add(p.Y);
                    }

                    break;
                case IgesCopiousDataPointFormat.Coordinates:
                    parameters.Add(DataPoints.Count);
                    foreach (var p in DataPoints)
                    {
                        parameters.Add(p.X);
                        parameters.Add(p.Y);
                        parameters.Add(p.Z);
                    }

                    break;
                case IgesCopiousDataPointFormat.CoordinatesAndVectors:
                    var upper = Math.Min(DataPoints.Count, DataVectors.Count);
                    parameters.Add(upper);
                    for (int i = 0; i < upper; i++)
                    {
                        parameters.Add(DataPoints[i].X);
                        parameters.Add(DataPoints[i].Y);
                        parameters.Add(DataPoints[i].Z);
                        parameters.Add(DataVectors[i].X);
                        parameters.Add(DataVectors[i].Y);
                        parameters.Add(DataVectors[i].Z);
                    }

                    break;
            }
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            if (FormNumber >= 1 && FormNumber <= 3)
            {
                Debug.Assert((int)PointFormat == FormNumber);
                _dataType = IgesCopiousDataType.PointSet;
            }
            else if (FormNumber >= 11 && FormNumber <= 13)
            {
                Debug.Assert((int)PointFormat == FormNumber - 10);
                Debug.Assert(DataPoints.Count >= 2);
                _dataType = IgesCopiousDataType.LinearPath;
            }
            else if (FormNumber >= 20 && FormNumber <= 21)
            {
                Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Annotation);
                Debug.Assert(PointFormat == IgesCopiousDataPointFormat.CommonZ);
                Debug.Assert(DataPoints.Count % 2 == 0);
                _dataType = IgesCopiousDataType.CenterLine;
                _centerlineType = (IgesCenterlineType)FormNumber;
            }
            else if (FormNumber >= 31 && FormNumber <= 38)
            {
                Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Annotation);
                Debug.Assert(PointFormat == IgesCopiousDataPointFormat.CommonZ);
                Debug.Assert(DataPoints.Count % 2 == 0);
                _dataType = IgesCopiousDataType.Section;
                _sectionType = (IgesSectionType)FormNumber;
            }
            else if (FormNumber == 40)
            {
                Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Annotation);
                Debug.Assert(PointFormat == IgesCopiousDataPointFormat.CommonZ);
                Debug.Assert(DataPoints.Count >= 3);
                Debug.Assert(DataPoints.Count % 2 == 1);
                _dataType = IgesCopiousDataType.WitnessLine;
            }
            else if (FormNumber == 63)
            {
                Debug.Assert(DataPoints.Count >= 2);
                _dataType = IgesCopiousDataType.ClosedCurve;
            }
            else
            {
                Debug.Assert(false, "unexpected form number " + FormNumber);
            }
        }

        internal override void OnBeforeWrite()
        {
            base.OnBeforeWrite();
            if (DataVectors.Count == DataPoints.Count)
            {
                PointFormat = IgesCopiousDataPointFormat.CoordinatesAndVectors;
            }
            else if (DataPoints.Select(p => p.Z).Distinct().Count() <= 1)
            {
                PointFormat = IgesCopiousDataPointFormat.CommonZ;
            }
            else
            {
                PointFormat = IgesCopiousDataPointFormat.Coordinates;
            }

            if ((FormNumber >= 20 && FormNumber <= 21) ||
                (FormNumber >= 31 && FormNumber <= 38) ||
                FormNumber == 40)
            {
                EntityUseFlag = IgesEntityUseFlag.Annotation;
            }

            Debug.Assert(FormNumber != 0);
        }

        private void UpdateFormNumber()
        {
            switch (DataType)
            {
                case IgesCopiousDataType.PointSet:
                    FormNumber = (int)PointFormat;
                    break;
                case IgesCopiousDataType.LinearPath:
                    FormNumber = (int)PointFormat + 10;
                    break;
                case IgesCopiousDataType.CenterLine:
                    FormNumber = (int)CenterlineType;
                    break;
                case IgesCopiousDataType.Section:
                    FormNumber = (int)SectionType;
                    break;
                case IgesCopiousDataType.WitnessLine:
                    FormNumber = 40;
                    break;
                case IgesCopiousDataType.ClosedCurve:
                    FormNumber = 63;
                    break;
                default:
                    FormNumber = 0;
                    break;
            }
        }
    }
}
