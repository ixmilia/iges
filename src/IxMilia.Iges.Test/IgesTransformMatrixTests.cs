using IxMilia.Iges.Entities;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesTransformMatrixTests
    {
        private static void TestTransform(IgesPoint input, IgesTransformationMatrix matrix, IgesPoint expected)
        {
            var result = matrix.Transform(input);
            Assert.Equal(expected.X, result.X);
            Assert.Equal(expected.Y, result.Y);
            Assert.Equal(expected.Z, result.Z);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Entities)]
        public void IdentityTransformTest()
        {
            var point = new IgesPoint(0.0, 0.0, 0.0);
            TestTransform(point, IgesTransformationMatrix.Identity, point);

            point = new IgesPoint(1.0, 2.0, 3.0);
            TestTransform(point, IgesTransformationMatrix.Identity, point);
        }
    }
}
