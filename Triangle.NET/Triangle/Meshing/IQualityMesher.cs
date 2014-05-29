
namespace TriangleNet.Meshing
{
    using TriangleNet.Geometry;

    public interface IQualityMesher
    {
        Mesh Triangulate(IPolygon polygon, QualityOptions quality);
        Mesh Triangulate(IPolygon polygon, ConstraintOptions options, QualityOptions quality);
    }
}
