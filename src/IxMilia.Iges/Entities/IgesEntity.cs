// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public abstract partial class IgesEntity
    {
        public abstract IgesEntityType EntityType { get; }

        public virtual int Structure { get; set; }
        public virtual int LineFontPattern { get; set; }
        public virtual int Level { get; set; }
        public virtual int View { get; set; }
        internal virtual int TransformationMatrixPointer { get; set; }
        public virtual int LableDisplay { get; set; }
        public virtual int StatusNumber { get; set; }
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

        protected double Double(string value)
        {
            return double.Parse(value);
        }

        protected int Integer(string value)
        {
            return int.Parse(value);
        }

        protected string String(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var sentinelIndex = value.IndexOf(IgesFile.StringSentinelCharacter);
            if (sentinelIndex < 0)
                return null;

            var lengthString = value.Substring(0, sentinelIndex);
            var length = int.Parse(lengthString);
            var calculatedLength = value.Length - lengthString.Length - 1; // 1 == sentinel length
            Debug.Assert(length == calculatedLength);
            return value.Substring(sentinelIndex + 1);
        }

        protected static string ReadParameterOrDefault(List<string> parameters, int index, string defaultValue)
        {
            if (index < parameters.Count)
                return parameters[index];
            else
                return defaultValue;
        }
    }

    public partial class IgesTransformationMatrix
    {
        public IgesPoint Transform(IgesPoint point)
        {
            return new IgesPoint(
                (R11 * point.X + R12 * point.Y + R13 * point.Z) + T1,
                (R21 * point.X + R22 * point.Y + R23 * point.Z) + T2,
                (R31 * point.X + R32 * point.Y + R33 * point.Z) + T3);
        }

        public bool IsIdentity
        {
            get
            {
                return
                    R11 == 1.0 &&
                    R12 == 0.0 &&
                    R13 == 0.0 &&
                    T1 == 0.0 &&
                    R21 == 0.0 &&
                    R22 == 1.0 &&
                    R23 == 0.0 &&
                    T2 == 0.0 &&
                    R31 == 0.0 &&
                    R32 == 0.0 &&
                    R33 == 1.0 &&
                    T3 == 0.0;
            }
        }

        public static IgesTransformationMatrix Identity
        {
            get
            {
                return new IgesTransformationMatrix()
                {
                    R11 = 1.0,
                    R12 = 0.0,
                    R13 = 0.0,
                    T1 = 0.0,
                    R21 = 0.0,
                    R22 = 1.0,
                    R23 = 0.0,
                    T2 = 0.0,
                    R31 = 0.0,
                    R32 = 0.0,
                    R33 = 1.0,
                    T3 = 0.0,
                };
            }
        }
    }
}
