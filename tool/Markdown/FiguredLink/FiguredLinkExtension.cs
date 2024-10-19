using Markdig;
using Markdig.Renderers;

namespace Macad.UserGuide.Markdown;

public class FiguredLinkExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.BlockParsers.AddIfNotAlready<FiguredLinkParser>();
    }

    //--------------------------------------------------------------------------------------------------

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<HtmlFiguredLinkRenderer>())
        {
            htmlRenderer.ObjectRenderers.Add(new HtmlFiguredLinkRenderer());
        }
    }
}