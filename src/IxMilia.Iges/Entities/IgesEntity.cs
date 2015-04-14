// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public abstract partial class IgesEntity
    {
        public abstract IgesEntityType EntityType { get; }

        private int _lineCount;
        public int FormNumber { get; protected set; }
        public IgesEntity StructureEntity { get; set; }
        public IgesViewBase View { get; set; }
        public IgesTransformationMatrix TransformationMatrix { get; set; }

        public IgesBlankStatus BlankStatus { get; set; }
        public IgesSubordinateEntitySwitchType SubordinateEntitySwitchType { get; set; }
        public IgesEntityUseFlag EntityUseFlag { get; set; }
        public IgesHierarchy Hierarchy { get; set; }

        private IgesColorDefinition _customColor;
        private IgesColorNumber _color;

        public IgesColorNumber Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _customColor = null;
            }
        }

        public IgesColorDefinition CustomColor
        {
            get { return _customColor; }
            set
            {
                if (value == null)
                {
                    _color = IgesColorNumber.Default;
                }
                else
                {
                    _color = IgesColorNumber.Custom;
                }

                _customColor = value;
            }
        }

        private IgesLineFontDefinitionBase _customLineFont;
        private IgesLineFontPattern _lineFont;

        public IgesLineFontPattern LineFont
        {
            get { return _lineFont; }
            set
            {
                _lineFont = value;
                _customLineFont = null;
            }
        }

        public IgesLineFontDefinitionBase CustomLineFont
        {
            get { return _customLineFont; }
            set
            {
                if (value == null)
                {
                    _lineFont = IgesLineFontPattern.Default;
                }
                else
                {
                    _lineFont = IgesLineFontPattern.Custom;
                }

                _customLineFont = value;
            }
        }

        private int _levelsPointer;
        public HashSet<int> Levels { get; private set; }

        private string _entityLabel;
        public string EntityLabel
        {
            get { return _entityLabel; }
            set { _entityLabel = value == null || value.Length <= 8 ? value : value.Substring(0, 8); }
        }

        private uint _entitySubscript;
        public uint EntitySubscript
        {
            get { return _entitySubscript; }
            set { _entitySubscript = Math.Min(99999999u, value); } // max 8 digits
        }

        private int _structurePointer;
        
        protected int LableDisplay { get; set; }
        protected int LineWeight { get; set; }
        protected internal List<int> SubEntityIndices { get; private set; }

        private int _viewPointer;
        private int _transformationMatrixPointer;
        protected List<IgesEntity> SubEntities { get; private set; }

        private List<int> _associatedEntityIndices;
        public List<IgesEntity> AssociatedEntities { get; private set; }

        private List<int> _propertyIndices;
        public List<IgesEntity> Properties { get; private set; }

        protected IgesEntity()
        {
            SubEntities = new List<IgesEntity>();
            SubEntityIndices = new List<int>();
            AssociatedEntities = new List<IgesEntity>();
            _associatedEntityIndices = new List<int>();
            Properties = new List<IgesEntity>();
            _propertyIndices = new List<int>();
            Levels = new HashSet<int>();
        }

        protected abstract int ReadParameters(List<string> parameters);

        protected abstract void WriteParameters(List<object> parameters);

        internal virtual void OnAfterRead(IgesDirectoryData directoryData)
        {
        }

        internal void ReadCommonPointers(List<string>parameters, int nextIndex)
        {
            var associatedPointerCount = Integer(parameters, nextIndex++);
            for (int i = 0; i < associatedPointerCount; i++)
            {
                _associatedEntityIndices.Add(Integer(parameters, nextIndex++));
            }

            var propertyPointerCount = Integer(parameters, nextIndex++);
            for (int i = 0; i < propertyPointerCount; i++)
            {
                _propertyIndices.Add(Integer(parameters, nextIndex++));
            }
        }

        internal void BindPointers(IgesDirectoryData dir, Dictionary<int, IgesEntity> entityMap, HashSet<int> entitiesToTrim)
        {
            if (EntityType == IgesEntityType.Null)
            {
                // null entities don't parse anything
                return;
            }

            // link to structure entities (field 3)
            if (_structurePointer < 0)
            {
                StructureEntity = entityMap[-_structurePointer];
                entitiesToTrim.Add(-_structurePointer);
            }

            // line font definition (field 4)
            if (dir.LineFontPattern < 0)
            {
                var custom = entityMap[-dir.LineFontPattern] as IgesLineFontDefinitionBase;
                if (custom != null)
                {
                    CustomLineFont = custom;
                    entitiesToTrim.Add(-dir.LineFontPattern);
                }
                else
                {
                    Debug.Assert(false, "line font pointer was not an IgesLineFontDefinitionBase");
                    LineFont = IgesLineFontPattern.Default;
                }
            }

            // level (field 5)
            Levels.Clear();
            if (_levelsPointer < 0)
            {
                var customLevels = entityMap[-_levelsPointer] as IgesDefinitionLevelsProperty;
                if (customLevels != null)
                {
                    foreach (var customLevel in customLevels.DefinedLevels)
                    {
                        Levels.Add(customLevel);
                    }

                    entitiesToTrim.Add(-_levelsPointer);
                }
                else
                {
                    Debug.Assert(false, "level pointer was not an IgesDefinitionLevelsProperty");
                }
            }
            else
            {
                Levels.Add(_levelsPointer);
            }

            // populate view (field 6)
            if (_viewPointer > 0)
            {
                View = entityMap[_viewPointer] as IgesViewBase;
                entitiesToTrim.Add(_viewPointer);
            }

            // populate transformation matrix (field 7)
            if (_transformationMatrixPointer > 0)
            {
                TransformationMatrix = entityMap[_transformationMatrixPointer] as IgesTransformationMatrix;
                entitiesToTrim.Add(_transformationMatrixPointer);
            }
            else
            {
                TransformationMatrix = IgesTransformationMatrix.Identity;
            }

            // TODO: label display (field 8)

            // TODO: line weight (field 12)

            // link to custom colors (field 13)
            if (dir.Color < 0)
            {
                var custom = entityMap[-dir.Color] as IgesColorDefinition;
                if (custom != null)
                {
                    CustomColor = custom;
                    entitiesToTrim.Add(-dir.Color);
                }
                else
                {
                    Debug.Assert(false, "color pointer was not an IgesColorDefinition");
                    Color = IgesColorNumber.Default;
                }
            }

            // link sub entities
            SubEntities.Clear();
            foreach (var pointer in SubEntityIndices)
            {
                if (entityMap.ContainsKey(pointer))
                {
                    SubEntities.Add(entityMap[pointer]);
                    entitiesToTrim.Add(pointer);
                }
                else
                {
                    SubEntities.Add(null);
                }
            }

            // link common pointers
            AssociatedEntities.Clear();
            foreach (var pointer in _associatedEntityIndices)
            {
                var entity = entityMap[pointer];
                Debug.Assert(entity.EntityType == IgesEntityType.AssociativityInstance || entity.EntityType == IgesEntityType.GeneralNote || entity.EntityType == IgesEntityType.TextDisplayTemplate);
                AssociatedEntities.Add(entity);
            }

            Properties = _propertyIndices.Select(pointer => entityMap[pointer]).ToList();
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
            this._structurePointer = directoryData.Structure;
            if (directoryData.LineFontPattern < 0)
            {
                this.LineFont = IgesLineFontPattern.Custom;
            }
            else
            {
                this.LineFont = (IgesLineFontPattern)directoryData.LineFontPattern;
            }

            this._levelsPointer = directoryData.Level;
            this._viewPointer = directoryData.View;
            this._transformationMatrixPointer = directoryData.TransformationMatrixPointer;
            this.LableDisplay = directoryData.LableDisplay;
            SetStatusNumber(directoryData.StatusNumber);
            this.LineWeight = directoryData.LineWeight;
            if (directoryData.Color < 0)
            {
                this.Color = IgesColorNumber.Custom;
            }
            else
            {
                this.Color = (IgesColorNumber)directoryData.Color;
            }

            this._lineCount = directoryData.LineCount;
            this.FormNumber = directoryData.FormNumber;
            this.EntityLabel = directoryData.EntityLabel;
            this.EntitySubscript = directoryData.EntitySubscript;
        }

        private IgesDirectoryData GetDirectoryData(int color, int lineFontPattern)
        {
            var dir = new IgesDirectoryData();
            dir.EntityType = EntityType;
            dir.Structure = this._structurePointer;
            dir.LineFontPattern = lineFontPattern;
            dir.Level = this._levelsPointer;
            dir.View = this._viewPointer;
            dir.TransformationMatrixPointer = this._transformationMatrixPointer;
            dir.LableDisplay = this.LableDisplay;
            dir.StatusNumber = this.GetStatusNumber();
            dir.LineWeight = this.LineWeight;
            dir.Color = color;
            dir.LineCount = this._lineCount;
            dir.FormNumber = this.FormNumber;
            dir.EntityLabel = this.EntityLabel;
            dir.EntitySubscript = this.EntitySubscript;
            return dir;
        }

        internal int AddDirectoryAndParameterLines(WriterState writerState)
        {
            // write view
            if (View != null)
            {
                _viewPointer = writerState.GetOrWriteEntityIndex(View);
            }

            // write transformation matrix if applicable
            if (TransformationMatrix != null && !TransformationMatrix.IsIdentity)
            {
                _transformationMatrixPointer = writerState.GetOrWriteEntityIndex(TransformationMatrix);
            }

            // write structure entity
            _structurePointer = 0;
            if (StructureEntity != null)
            {
                _structurePointer = -writerState.GetOrWriteEntityIndex(StructureEntity);
            }

            // write levels
            if (Levels.Count <= 1)
            {
                _levelsPointer = Levels.FirstOrDefault();
            }
            else
            {
                _levelsPointer = -writerState.GetLevelsPointer(Levels);
            }

            // write line font pattern
            int lineFontPattern = 0;
            if (CustomLineFont != null)
            {
                lineFontPattern = -writerState.GetOrWriteEntityIndex(CustomLineFont);
            }
            else
            {
                lineFontPattern = (int)LineFont;
            }

            // write custom color entity
            int color = 0;
            if (CustomColor != null)
            {
                color = -writerState.GetOrWriteEntityIndex(CustomColor);
            }
            else
            {
                color = (int)Color;
            }

            // write sub-entities
            SubEntityIndices.Clear();
            foreach (var subEntity in SubEntities)
            {
                var index = subEntity == null
                    ? 0
                    : writerState.GetOrWriteEntityIndex(subEntity);
                SubEntityIndices.Add(index);
            }

            // write common pointers
            _associatedEntityIndices.Clear();
            foreach (var assoc in AssociatedEntities)
            {
                var index = writerState.GetOrWriteEntityIndex(assoc);
                _associatedEntityIndices.Add(index);
            }

            _propertyIndices.Clear();
            foreach (var prop in Properties)
            {
                var index = writerState.GetOrWriteEntityIndex(prop);
                _propertyIndices.Add(index);
            }

            var nextDirectoryIndex = writerState.DirectoryLines.Count + 1;
            var nextParameterIndex = writerState.ParameterLines.Count + 1;
            var parameters = new List<object>();
            parameters.Add((int)EntityType);
            this.WriteParameters(parameters);

            if (_associatedEntityIndices.Any())
            {
                parameters.Add(_associatedEntityIndices.Count);
                parameters.AddRange(_associatedEntityIndices.Cast<object>());
            }

            if (_propertyIndices.Any())
            {
                parameters.Add(_propertyIndices.Count);
                parameters.AddRange(_propertyIndices.Cast<object>());
            }

            this._lineCount = IgesFileWriter.AddParametersToStringList(parameters.ToArray(), writerState.ParameterLines, writerState.FieldDelimiter, writerState.RecordDelimiter,
                lineSuffix: string.Format(" {0,7}", nextDirectoryIndex));
            var dir = GetDirectoryData(color, lineFontPattern);
            dir.ParameterPointer = nextParameterIndex;
            dir.ToString(writerState.DirectoryLines);

            writerState.EntityMap[this] = nextDirectoryIndex;

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
                case IgesEntityType.ColorDefinition:
                    entity = new IgesColorDefinition();
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
                case IgesEntityType.LineFontDefinition:
                    switch (directoryData.FormNumber)
                    {
                        case 1:
                            entity = new IgesTemplateLineFontDefinition();
                            break;
                        case 2:
                            entity = new IgesPatternLineFontDefinition();
                            break;
                    }
                    break;
                case IgesEntityType.Null:
                    entity = new IgesNull();
                    break;
                case IgesEntityType.Plane:
                    entity = new IgesPlane();
                    break;
                case IgesEntityType.Point:
                    entity = new IgesLocation();
                    break;
                case IgesEntityType.Property:
                    switch (directoryData.FormNumber)
                    {
                        case 1:
                            entity = new IgesDefinitionLevelsProperty();
                            break;
                    }
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
                case IgesEntityType.View:
                    switch (directoryData.FormNumber)
                    {
                        case 0:
                            entity = new IgesView();
                            break;
                        case 1:
                            entity = new IgesPerspectiveView();
                            break;
                    }
                    break;
            }

            if (entity != null)
            {
                entity.PopulateDirectoryData(directoryData);
                int nextIndex = entity.ReadParameters(parameters);
                entity.ReadCommonPointers(parameters, nextIndex);
                entity.OnAfterRead(directoryData);
            }

            return entity;
        }
    }
}
