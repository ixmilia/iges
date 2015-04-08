// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public abstract class IgesEntity
    {
        public abstract IgesEntityType EntityType { get; }

        public IgesColorNumber Color { get; set; }
        public int LineCount { get; protected set; }
        public int FormNumber { get; protected set; }
        public IgesEntity StructureEntity { get; set; }
        public IgesTransformationMatrix TransformationMatrix { get; set; }

        public IgesBlankStatus BlankStatus { get; set; }
        public IgesSubordinateEntitySwitchType SubordinateEntitySwitchType { get; set; }
        public IgesEntityUseFlag EntityUseFlag { get; set; }
        public IgesHierarchy Hierarchy { get; set; }

        protected string EntityLabel { get; set; }
        internal int Structure { get; set; }
        protected int LineFontPattern { get; set; }
        protected int Level { get; set; }
        protected int View { get; set; }
        
        protected int LableDisplay { get; set; }
        protected int LineWeight { get; set; }
        protected int EntitySubscript { get; set; }
        protected internal List<int> SubEntityIndices { get; private set; }

        internal int TransformationMatrixPointer { get; set; }
        internal List<IgesEntity> SubEntities { get; private set; }

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

        private string GetStatusNumber()
        {
            return string.Format("{0:0#}{1:0#}{2:0#}{3:0#}",
                (int)BlankStatus,
                (int)SubordinateEntitySwitchType,
                (int)EntityUseFlag,
                (int)Hierarchy);
        }

        private void SetStatusNumber(string value)
        {
            if (value == null)
            {
                value = "00000000";
            }

            if (value.Length < 8)
            {
                value = new string('0', 8 - value.Length) + value;
            }

            if (value.Length > 8)
            {
                value = value.Substring(0, 8);
            }

            BlankStatus = (IgesBlankStatus)int.Parse(value.Substring(0, 2));
            SubordinateEntitySwitchType = (IgesSubordinateEntitySwitchType)int.Parse(value.Substring(2, 2));
            EntityUseFlag = (IgesEntityUseFlag)int.Parse(value.Substring(4, 2));
            Hierarchy = (IgesHierarchy)int.Parse(value.Substring(6, 2));
        }

        private void PopulateDirectoryData(IgesDirectoryData directoryData)
        {
            this.Structure = directoryData.Structure;
            this.LineFontPattern = directoryData.LineFontPattern;
            this.Level = directoryData.Level;
            this.View = directoryData.View;
            this.TransformationMatrixPointer = directoryData.TransformationMatrixPointer;
            this.LableDisplay = directoryData.LableDisplay;
            SetStatusNumber(directoryData.StatusNumber);
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
            dir.StatusNumber = this.GetStatusNumber();
            dir.LineWeight = this.LineWeight;
            dir.Color = this.Color;
            dir.LineCount = this.LineCount;
            dir.FormNumber = this.FormNumber;
            dir.EntityLabel = this.EntityLabel;
            dir.EntitySubscript = this.EntitySubscript;
            return dir;
        }

        internal int AddDirectoryAndParameterLines(Dictionary<IgesEntity, int> entityMap, List<string> directoryLines, List<string> parameterLines, char fieldDelimiter, char recordDelimiter)
        {
            // write transformation matrix if applicable
            if (TransformationMatrix != null && !TransformationMatrix.IsIdentity)
            {
                var matrixPointer = TransformationMatrix.AddDirectoryAndParameterLines(entityMap, directoryLines, parameterLines, fieldDelimiter, recordDelimiter);
                TransformationMatrixPointer = matrixPointer;
            }

            // write structure entity
            Structure = 0;
            if (StructureEntity != null)
            {
                if (!entityMap.ContainsKey(StructureEntity))
                {
                    Structure = -StructureEntity.AddDirectoryAndParameterLines(entityMap, directoryLines, parameterLines, fieldDelimiter, recordDelimiter);
                }
                else
                {
                    Structure = -entityMap[StructureEntity];
                }
            }

            // write sub-entities
            SubEntityIndices.Clear();
            foreach (var subEntity in SubEntities)
            {
                var index = subEntity.AddDirectoryAndParameterLines(entityMap, directoryLines, parameterLines, fieldDelimiter, recordDelimiter);
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

            entityMap[this] = nextDirectoryIndex;

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
