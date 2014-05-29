
namespace TriangleNet.Meshing
{
    using TriangleNet.Geometry;

    public interface IConstraintMesher
    {
        IMesh Triangulate(IPolygon polygon);
        IMesh Triangulate(IPolygon polygon, ConstraintOptions options);
    }
}
