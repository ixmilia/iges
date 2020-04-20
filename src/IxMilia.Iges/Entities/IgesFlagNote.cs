using System;
using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesFlagNote : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.FlagNote; } }

        public IgesPoint Location { get; set; }

        /// <summary>
        /// The flag's rotation angle in radians.
        /// </summary>
        public double RotationAngle { get; set; }
        public IgesGeneralNote GeneralNote { get; set; }
        public IList<IgesLeader> Leaders { get; private set; }

        // computed properties
        private IgesTextString NoteString { get { return GeneralNote?.Strings?.FirstOrDefault(); } }

        /// <summary>
        /// The height of the flag.  May never be less than 0.3 inches.
        /// </summary>
        public double Height { get { return Math.Max((NoteString?.BoxHeight ?? 0.0) * 2.0, 0.3); } }

        /// <summary>
        /// The height of a single character.  Inherited from <see cref="IgesTextString.BoxHeight"/>.
        /// </summary>
        public double CharacterHeight { get { return NoteString?.BoxHeight ?? 0.0; } }

        /// <summary>
        /// The length of the flag.  May never be less than 0.6 inches.
        /// </summary>
        public double Length { get { return Math.Max(Width + CharacterHeight * 0.4, 0.6); } }

        /// <summary>
        /// The width of the flag.  Inherited from <see cref="IgesTextString.BoxWidth"/>.
        /// </summary>
        public double Width { get { return NoteString?.BoxWidth ?? 0.0; } }

        /// <summary>
        /// The length of the tip.
        /// </summary>
        public double TipLength { get { return Height * 0.5 / Math.Tan(35.0 * Math.PI / 180.0); } }

        public IgesFlagNote()
        {
            EntityUseFlag = IgesEntityUseFlag.Annotation;
            Location = IgesPoint.Origin;
            Leaders = new List<IgesLeader>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            Location = Point3(parameters, ref index);
            RotationAngle = Double(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), note => GeneralNote = note as IgesGeneralNote);
            var leaderCount = Integer(parameters, index++);
            Leaders = new IgesLeader[leaderCount].ToList();
            for (int i = 0; i < leaderCount; i++)
            {
                var idx = i;
                binder.BindEntity(Integer(parameters, index++), leader => Leaders[idx] = leader as IgesLeader);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return GeneralNote;
            foreach (var leader in Leaders)
            {
                yield return leader;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(Location.X);
            parameters.Add(Location.Y);
            parameters.Add(Location.Z);
            parameters.Add(RotationAngle);
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(Leaders.Count);
            foreach (var leader in Leaders)
            {
                parameters.Add(binder.GetEntityId(leader));
            }
        }
    }
}
