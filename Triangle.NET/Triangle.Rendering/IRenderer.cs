
namespace TriangleNet.Rendering
{
    public interface IRenderer
    {
        IRenderContext Context { get; set; }

        void Render();
    }
}
