
namespace TriangleNet.Meshing
{
    using TriangleNet.Geometry;

    public interface IQualityMesher
    {
        IMesh Triangulate(IPolygon polygon, QualityOptions quality);
        IMesh Triangulate(IPolygon polygon, ConstraintOptions options, QualityOptions quality);
    }
}
