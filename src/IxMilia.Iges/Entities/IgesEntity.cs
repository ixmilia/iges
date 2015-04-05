// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public abstract class IgesEntity
    {
        public abstract IgesEntityType EntityType { get; }

        public virtual int Structure { get; set; }
        public virtual int LineFontPattern { get; set; }
        public virtual int Level { get; set; }
        public virtual int View { get; set; }
        internal virtual int TransformationMatrixPointer { get; set; }
        public virtual int LableDisplay { get; set; }
        public virtual string StatusNumber { get; set; }
        public virtual int LineWeight { get; set; }
        public virtual IgesColorNumber Color { get; set; }
        public virtual int LineCount { get; set; }
        public virtual int FormNumber { get; set; }
        public virtual string EntityLabel { get; set; }
        public virtual int EntitySubscript { get; set; }
        public IgesTransformationMatrix TransformationMatrix { get; set; }
        internal List<IgesEntity> SubEntities { get; private set; }
        protected internal List<int> SubEntityIndices { get; private set; }

        protected IgesEntity()
        {
            SubEntities = new List<IgesEntity>();
            SubEntityIndices = new List<int>();
        }

        protected abstract void ReadParameters(List<string> parameters);

        protected abstract void WriteParameters(List<object> parameters);

        internal virtual void OnAfterRead(IgesDirectoryData directoryData)
        {
        }

        private void PopulateDirectoryData(IgesDirectoryData directoryData)
        {
            this.Structure = directoryData.Structure;
            this.LineFontPattern = directoryData.LineFontPattern;
            this.Level = directoryData.Level;
            this.View = directoryData.View;
            this.TransformationMatrixPointer = directoryData.TransformationMatrixPointer;
            this.LableDisplay = directoryData.LableDisplay;
            this.StatusNumber = directoryData.StatusNumber;
            this.LineWeight = directoryData.LineWeight;
            this.Color = directoryData.Color;
            this.LineCount = directoryData.LineCount;
            this.FormNumber = directoryData.FormNumber;
            this.EntityLabel = directoryData.EntityLabel;
            this.EntitySubscript = directoryData.EntitySubscript;
        }

        private IgesDirectoryData GetDirectoryData()
        {
            var dir = new IgesDirectoryData();
            dir.EntityType = EntityType;
            dir.Structure = this.Structure;
            dir.LineFontPattern = this.LineFontPattern;
            dir.Level = this.Level;
            dir.View = this.View;
            dir.TransformationMatrixPointer = this.TransformationMatrixPointer;
            dir.LableDisplay = this.LableDisplay;
            dir.StatusNumber = this.StatusNumber;
            dir.LineWeight = this.LineWeight;
            dir.Color = this.Color;
            dir.LineCount = this.LineCount;
            dir.FormNumber = this.FormNumber;
            dir.EntityLabel = this.EntityLabel;
            dir.EntitySubscript = this.EntitySubscript;
            return dir;
        }

        internal int AddDirectoryAndParameterLines(List<string> directoryLines, List<string> parameterLines, char fieldDelimiter, char recordDelimiter)
        {
            // write transformation matrix if applicable
            if (TransformationMatrix != null && !TransformationMatrix.IsIdentity)
            {
                var matrixPointer = TransformationMatrix.AddDirectoryAndParameterLines(directoryLines, parameterLines, fieldDelimiter, recordDelimiter);
                TransformationMatrixPointer = matrixPointer;
            }

            // write sub-entities
            SubEntityIndices.Clear();
            foreach (var subEntity in SubEntities)
            {
                var index = subEntity.AddDirectoryAndParameterLines(directoryLines, parameterLines, fieldDelimiter, recordDelimiter);
                SubEntityIndices.Add(index);
            }

            var nextDirectoryIndex = directoryLines.Count + 1;
            var nextParameterIndex = parameterLines.Count + 1;
            var dir = GetDirectoryData();
            dir.ParameterPointer = nextParameterIndex;
            dir.ToString(directoryLines);
            var parameters = new List<object>();
            parameters.Add((int)EntityType);
            this.WriteParameters(parameters);
            IgesFileWriter.AddParametersToStringList(parameters.ToArray(), parameterLines, fieldDelimiter, recordDelimiter,
                lineSuffix: string.Format(" {0,7}", nextDirectoryIndex));

            return nextDirectoryIndex;
        }

        protected double Double(List<string> values, int index)
        {
            return DoubleOrDefault(values, index, 0.0);
        }

        protected double DoubleOrDefault(List<string> values, int index, double defaultValue)
        {
            if (index < values.Count)
            {
                return double.Parse(values[index]);
            }
            else
            {
                return defaultValue;
            }
        }

        protected int Integer(List<string> values, int index)
        {
            return IntegerOrDefault(values, index, 0);
        }

        protected int IntegerOrDefault(List<string> values, int index, int defaultValue)
        {
            if (index < values.Count)
            {
                return int.Parse(values[index]);
            }
            else
            {
                return defaultValue;
            }
        }

        protected string String(List<string> values, int index)
        {
            return StringOrDefault(values, index, null);
        }

        protected string StringOrDefault(List<string> values, int index, string defaultValue)
        {
            if (index < values.Count)
            {
                return values[index];
            }
            else
            {
                return defaultValue;
            }
        }

        protected static string ReadParameterOrDefault(List<string> parameters, int index, string defaultValue)
        {
            if (index < parameters.Count)
                return parameters[index];
            else
                return defaultValue;
        }

        internal static IgesEntity FromData(IgesDirectoryData directoryData, List<string> parameters)
        {
            IgesEntity entity = null;
            switch (directoryData.EntityType)
            {
                case IgesEntityType.CircularArc:
                    entity = new IgesCircularArc();
                    break;
                case IgesEntityType.CompositeCurve:
                    entity = new IgesCompositeCurve();
                    break;
                case IgesEntityType.Direction:
                    entity = new IgesDirection();
                    break;
                case IgesEntityType.Line:
                    entity = new IgesLine();
                    break;
                case IgesEntityType.Null:
                    entity = new IgesNull();
                    break;
                case IgesEntityType.Point:
                    entity = new IgesLocation();
                    break;
                case IgesEntityType.Sphere:
                    entity = new IgesSphere();
                    break;
                case IgesEntityType.SubfigureDefinition:
                    entity = new IgesSubfigureDefinition();
                    break;
                case IgesEntityType.Torus:
                    entity = new IgesTorus();
                    break;
                case IgesEntityType.TransformationMatrix:
                    entity = new IgesTransformationMatrix();
                    break;
            }

            if (entity != null)
            {
                entity.PopulateDirectoryData(directoryData);
                entity.ReadParameters(parameters);
                entity.OnAfterRead(directoryData);
            }

            return entity;
        }
    }
}
