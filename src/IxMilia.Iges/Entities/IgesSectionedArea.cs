using System;
using System.Collections.Generic;
using System.Linq;
using IxMilia.Iges.Collections;

namespace IxMilia.Iges.Entities
{
    public class IgesSectionedArea : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SectionedArea; } }

        private IgesEntity _exteriorDefinitionCurve;

        public IgesEntity ExteriorDefinitionCurve
        {
            get => _exteriorDefinitionCurve;
            set
            {
                if (!IsValidExteriorDefinitionCurve(value))
                {
                    throw new InvalidOperationException("Specified entity is not a valid exterior definition curve.");
                }

                _exteriorDefinitionCurve = value;
            }
        }

        public int FillPattern { get; set; }
        public IgesPoint ReferenceLocation { get; set; }
        public double PatternZDepth { get; set; }
        public double NormalDistance { get; set; }
        public IList<IgesEntity> InteriorDefinitionCurves { get; } = new ListNonNullWithPredicate<IgesEntity>(IsValidExteriorDefinitionCurve);

        /// <summary>
        /// Angle in radians.
        /// </summary>
        public double RotationAngle { get; set; } = Math.PI / 4.0;

        public IgesSectionedArea(IgesEntity exteriorDefinitionCurve, int fillPattern, IgesPoint referenceLocation, double zDepth, double normalDistance, double rotationAngle, params IgesEntity[] islandCurves)
            : this()
        {
            ExteriorDefinitionCurve = exteriorDefinitionCurve;
            FillPattern = fillPattern;
            ReferenceLocation = referenceLocation;
            PatternZDepth = zDepth;
            NormalDistance = normalDistance;
            RotationAngle = rotationAngle;
            foreach (var islandCurve in islandCurves)
            {
                InteriorDefinitionCurves.Add(islandCurve);
            }
        }

        internal IgesSectionedArea()
        {
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => ExteriorDefinitionCurve = e);
            FillPattern = Integer(parameters, index++);
            ReferenceLocation = Point2(parameters, ref index);
            PatternZDepth = Double(parameters, index++);
            NormalDistance = Double(parameters, index++);
            RotationAngle = Double(parameters, index++);
            var islandCurveCount = Integer(parameters, index++);
            for (int i = 0; i < islandCurveCount; i++)
            {
                binder.BindEntity(Integer(parameters, index++), e => InteriorDefinitionCurves.Add(e));
            }
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return ExteriorDefinitionCurve;
            foreach (var island in InteriorDefinitionCurves)
            {
                yield return island;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(ExteriorDefinitionCurve));
            parameters.Add(FillPattern);
            parameters.Add(ReferenceLocation.X);
            parameters.Add(ReferenceLocation.Y);
            parameters.Add(PatternZDepth);
            parameters.Add(NormalDistance);
            parameters.Add(RotationAngle);
            parameters.Add(InteriorDefinitionCurves.Count);
            parameters.AddRange(InteriorDefinitionCurves.Select(binder.GetEntityId).Cast<object>());
        }

        private static bool IsValidExteriorDefinitionCurve(IgesEntity candidateDefinitionCurve)
        {
            switch (candidateDefinitionCurve)
            {
                case IgesCircularArc _:
                case IgesCompositeCurve _:
                case IgesParametricSplineCurve _:
                case IgesRationalBSplineCurve _:
                case IgesConicArc _ when candidateDefinitionCurve.FormNumber == 1:
                case IgesCopiousData _ when candidateDefinitionCurve.FormNumber == 63:
                    return true;
                default:
                    return false;
            }
        }
    }
}
