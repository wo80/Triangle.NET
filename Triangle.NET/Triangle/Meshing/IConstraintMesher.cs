
namespace TriangleNet.Meshing
{
    using TriangleNet.Geometry;

    public interface IConstraintMesher
    {
        Mesh Triangulate(IPolygon polygon);
        Mesh Triangulate(IPolygon polygon, ConstraintOptions options);
    }
}
