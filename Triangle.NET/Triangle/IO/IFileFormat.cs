
namespace TriangleNet.IO
{
    public interface IFileFormat
    {
        bool IsSupported(string file);
    }
}
