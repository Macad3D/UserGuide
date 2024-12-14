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
        
        var url = linkInline.Url;
        var klass = "";
        if (Path.GetExtension(url).ToLower() == ".apng")
        {
            klass = "img-swap";
            url = Path.ChangeExtension(url, ".png");
        }

        renderer.Write("<figure>");
        renderer.Write("<p>");
        renderer.Write("<img ");
        if (!string.IsNullOrEmpty(klass))
        {
            renderer.Write("class=\"img-swap\"");
        }
        renderer.Write($" src=\"{url}\" alt=\"{Path.GetFileNameWithoutExtension(url)}\">");
        renderer.Write("</p>");
        
        renderer.Write("<figcaption>");
        renderer.WriteChildren(linkInline);
        renderer.Write("</figcaption>");

        renderer.Write("</figure>");

    }
}