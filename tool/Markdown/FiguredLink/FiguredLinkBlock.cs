using Markdig.Parsers;
using Markdig.Syntax;

namespace Macad.UserGuide.Markdown;

public class FiguredLinkBlock : LeafBlock
{
    public FiguredLinkBlock(BlockParser? parser) 
        : base(parser)
    {
        ProcessInlines = true;
    }
}