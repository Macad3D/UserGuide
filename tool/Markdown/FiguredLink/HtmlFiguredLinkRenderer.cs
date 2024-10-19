using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax.Inlines;

namespace Macad.UserGuide.Markdown;

public class HtmlFiguredLinkRenderer  : HtmlObjectRenderer<FiguredLinkBlock>
{
    protected override void Write(HtmlRenderer renderer, FiguredLinkBlock block)
    {
        var linkInline = block.Inline?.FirstChild as LinkInline;
        if (linkInline?.Url is null || !linkInline.IsImage)
        {
            return;
        }

        renderer.Write("<figure>");
        renderer.Write("<p>");
        renderer.Write($"<img src=\"{linkInline.Url}\" alt=\"{Path.GetFileNameWithoutExtension(linkInline.Url)}\">");
        renderer.Write("</p>");
        
        renderer.Write("<figcaption>");
        renderer.WriteChildren(linkInline);
        renderer.Write("</figcaption>");

        renderer.Write("</figure>");

    }
}