using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesTextMirroringAxis
    {
        None = 0,
        PerpendicularToTextBase = 1,
        TextBase = 2
    }

    public enum IgesTextRotationType
    {
        Horizontal = 0,
        Vertical = 1
    }

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

        public string Comment { get; set; }
        public int LineWeight { get; set; }

        private int _structurePointer;
        private int _labelDisplayPointer;
        private IgesLabelDisplayAssociativity _labelDisplay;

        public IgesLabelDisplayAssociativity LabelDisplay
        {
            get { return _labelDisplay; }
            set
            {
                _labelDisplay = value;
                if (_labelDisplay != null)
                {
                    _labelDisplay.AssociatedEntity = this;
                }
            }
        }
        

        private int _viewPointer;
        private int _transformationMatrixPointer;

        public List<IgesEntity> AssociatedEntities { get; }
        public List<IgesEntity> Properties { get; }

        protected IgesEntity()
        {
            Levels = new HashSet<int>();
            AssociatedEntities = new List<IgesEntity>();
            Properties = new List<IgesEntity>();
            if (!(this is IgesTransformationMatrix))
            {
                TransformationMatrix = IgesTransformationMatrix.Identity;
            }
        }

        internal abstract int ReadParameters(List<string> parameters, IgesReaderBinder binder);

        internal virtual void OnAfterRead(IgesDirectoryData directoryData)
        {
        }

        internal virtual void OnBeforeWrite()
        {
        }

        internal abstract void WriteParameters(List<object> parameters, IgesWriterBinder binder);

        internal virtual IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield break;
        }

        internal virtual void UnMarkEntitiesForTrimming(HashSet<int> entitiesToTrim)
        {
        }

        internal virtual IgesEntity PostProcess()
        {
            return this;
        }

        internal void ReadCommonPointers(List<string>parameters, int nextIndex, IgesReaderBinder binder)
        {
            var associatedPointerCount = Integer(parameters, nextIndex++);
            for (int i = 0; i < associatedPointerCount; i++)
            {
                binder.BindEntity(Integer(parameters, nextIndex++), e =>
                {
                    if (e != null)
                    {
                        Debug.Assert(e.EntityType == IgesEntityType.AssociativityInstance || e.EntityType == IgesEntityType.GeneralNote || e.EntityType == IgesEntityType.TextDisplayTemplate);
                        AssociatedEntities.Add(e);
                    }
                });
            }

            var propertyPointerCount = Integer(parameters, nextIndex++);
            for (int i = 0; i < propertyPointerCount; i++)
            {
                binder.BindEntity(Integer(parameters, nextIndex++), e => Properties.Add(e));
            }
        }

        internal void BindPointers(IgesDirectoryData dir, IgesReaderBinder binder)
        {
            if (EntityType == IgesEntityType.Null)
            {
                // null entities don't parse anything
                return;
            }

            // link to structure entities (field 3)
            if (_structurePointer < 0)
            {
                binder.BindEntity(-_structurePointer, e => StructureEntity = e);
            }

            // line font definition (field 4)
            if (dir.LineFontPattern < 0)
            {
                binder.BindEntity(-dir.LineFontPattern, e => CustomLineFont = e as IgesLineFontDefinitionBase);
            }

            // level (field 5)
            Levels.Clear();
            if (_levelsPointer < 0)
            {
                binder.BindEntity(-_levelsPointer, e =>
                {
                    var customLevels = e as IgesDefinitionLevelsProperty;
                    if (customLevels != null)
                    {
                        foreach (var customLevel in customLevels.DefinedLevels)
                        {
                            Levels.Add(customLevel);
                        }
                    }
                });
            }
            else
            {
                Levels.Add(_levelsPointer);
            }

            // populate view (field 6)
            if (_viewPointer > 0)
            {
                binder.BindEntity(_viewPointer, e => View = e as IgesViewBase);
            }

            // populate transformation matrix (field 7)
            if (_transformationMatrixPointer > 0)
            {
                binder.BindEntity(_transformationMatrixPointer, e => TransformationMatrix = e as IgesTransformationMatrix);
            }
            else
            {
                TransformationMatrix = IgesTransformationMatrix.Identity;
            }

            // label display (field 8)
            if (dir.LableDisplay > 0)
            {
                binder.BindEntity(dir.LableDisplay, e => LabelDisplay = e as IgesLabelDisplayAssociativity);
            }

            // link to custom colors (field 13)
            if (dir.Color < 0)
            {
                binder.BindEntity(-dir.Color, e => CustomColor = e as IgesColorDefinition);
            }
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

            BlankStatus = (IgesBlankStatus)IgesParser.ParseIntStrict(value.Substring(0, 2));
            SubordinateEntitySwitchType = (IgesSubordinateEntitySwitchType)IgesParser.ParseIntStrict(value.Substring(2, 2));
            EntityUseFlag = (IgesEntityUseFlag)IgesParser.ParseIntStrict(value.Substring(4, 2));
            Hierarchy = (IgesHierarchy)IgesParser.ParseIntStrict(value.Substring(6, 2));
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
            this._labelDisplayPointer = directoryData.LableDisplay;
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
            dir.LableDisplay = this._labelDisplayPointer;
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
            OnBeforeWrite();
            writerState.ReportReferencedEntities(GetReferencedEntities());

            // write structure entity (field 3)
            if (StructureEntity != null)
            {
                _structurePointer = -writerState.GetOrWriteEntityIndex(StructureEntity);
            }
            else
            {
                _structurePointer = 0;
            }

            // write line font pattern (field 4)
            int lineFontPattern = 0;
            if (CustomLineFont != null)
            {
                lineFontPattern = -writerState.GetOrWriteEntityIndex(CustomLineFont);
            }
            else
            {
                lineFontPattern = (int)LineFont;
            }

            // write levels (field 5)
            if (Levels.Count <= 1)
            {
                _levelsPointer = Levels.FirstOrDefault();
            }
            else
            {
                _levelsPointer = -writerState.GetLevelsPointer(Levels);
            }

            // write view (field 6)
            if (View != null)
            {
                _viewPointer = writerState.GetOrWriteEntityIndex(View);
            }
            else
            {
                _viewPointer = 0;
            }

            // write transformation matrix (field 7)
            if (TransformationMatrix != null && !TransformationMatrix.IsIdentity)
            {
                _transformationMatrixPointer = writerState.GetOrWriteEntityIndex(TransformationMatrix);
            }
            else
            {
                _transformationMatrixPointer = 0;
            }

            // write label display associativity (field 8)
            if (LabelDisplay != null)
            {
                _labelDisplayPointer = writerState.GetOrWriteEntityIndex(LabelDisplay);
            }
            else
            {
                _labelDisplayPointer = 0;
            }

            // write custom color entity (field 13)
            int color = 0;
            if (CustomColor != null)
            {
                color = -writerState.GetOrWriteEntityIndex(CustomColor);
            }
            else
            {
                color = (int)Color;
            }

            var parameters = new List<object>();
            parameters.Add((int)EntityType);
            this.WriteParameters(parameters, writerState.WriterBinder);

            if (AssociatedEntities.Any() || Properties.Any())
            {
                writerState.ReportReferencedEntities(AssociatedEntities);
                writerState.ReportReferencedEntities(Properties);
                parameters.Add(AssociatedEntities.Count);
                parameters.AddRange(AssociatedEntities.Select(writerState.WriterBinder.GetEntityId).Cast<object>());

                if (Properties.Any())
                {
                    parameters.Add(Properties.Count);
                    parameters.AddRange(Properties.Select(writerState.WriterBinder.GetEntityId).Cast<object>());
                }
            }

            var nextDirectoryIndex = writerState.DirectoryLines.Count + 1;
            var nextParameterIndex = writerState.ParameterLines.Count + 1;
            this._lineCount = IgesFileWriter.AddParametersToStringList(parameters.ToArray(), writerState.ParameterLines, writerState.FieldDelimiter, writerState.RecordDelimiter,
                lineSuffix: string.Format(" {0,7}", nextDirectoryIndex),
                comment: Comment);
            var dir = GetDirectoryData(color, lineFontPattern);
            dir.ParameterPointer = nextParameterIndex;
            dir.ToString(writerState.DirectoryLines);

            writerState.EntityMap[this] = nextDirectoryIndex;

            return nextDirectoryIndex;
        }

        protected double Double(List<string> values, int index)
        {
            return IgesParameterReader.Double(values, index);
        }

        protected double DoubleOrDefault(List<string> values, int index, double defaultValue)
        {
            return IgesParameterReader.DoubleOrDefault(values, index, defaultValue);
        }

        protected int Integer(List<string> values, int index)
        {
            return IgesParameterReader.Integer(values, index);
        }

        protected int IntegerOrDefault(List<string> values, int index, int defaultValue)
        {
            return IgesParameterReader.IntegerOrDefault(values, index, defaultValue);
        }

        protected string String(List<string> values, int index)
        {
            return IgesParameterReader.String(values, index);
        }

        protected string StringOrDefault(List<string> values, int index, string defaultValue)
        {
            return IgesParameterReader.StringOrDefault(values, index, defaultValue);
        }

        protected bool Boolean(List<string> values, int index)
        {
            return IgesParameterReader.Boolean(values, index);
        }

        protected bool BooleanOrDefault(List<string> values, int index, bool defaultValue)
        {
            return IgesParameterReader.BooleanOrDefault(values, index, defaultValue);
        }

        protected DateTime DateTime(List<string> values, int index)
        {
            return IgesParameterReader.DateTime(values, index);
        }

        protected DateTime DateTimeOrDefault(List<string> values, int index, DateTime defaultValue)
        {
            return IgesParameterReader.DateTimeOrDefault(values, index, defaultValue);
        }

        protected IgesPoint Point3(List<string> values, ref int index)
        {
            return IgesParameterReader.Point3(values, ref index);
        }

        protected IgesPoint Point2(List<string> values, ref int index)
        {
            return IgesParameterReader.Point2(values, ref index);
        }

        protected IgesPoint PointOrDefault(List<string> values, ref int index, IgesPoint defaultValue)
        {
            return IgesParameterReader.PointOrDefault(values, ref index, defaultValue);
        }

        protected IgesVector Vector(List<string> values, ref int index)
        {
            return IgesParameterReader.Vector(values, ref index);
        }

        protected IgesVector VectorOrDefault(List<string> values, ref int index, IgesVector defaultValue)
        {
            return IgesParameterReader.VectorOrDefault(values, ref index, defaultValue);
        }
    }
}
