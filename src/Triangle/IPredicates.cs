
namespace TriangleNet
{
    using TriangleNet.Geometry;

    public interface IPredicates
    {
        double CounterClockwise(Point a, Point b, Point c);

        double InCircle(Point a, Point b, Point c, Point p);

        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta);
    
        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta,
            double offconstant);
    }
}
