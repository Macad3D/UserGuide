using Docfx.MarkdigEngine.Extensions;
using Markdig.Parsers;

namespace Macad.UserGuide.Markdown;

public class FiguredLinkParser : BlockParser
{
    private const string StartString = "^![";

    //--------------------------------------------------------------------------------------------------

    public FiguredLinkParser()
    {
        OpeningCharacters = new[] { '^' };
    }

    //--------------------------------------------------------------------------------------------------

    public override BlockState TryOpen(BlockProcessor processor)
    {
        if (processor.IsCodeIndent)
        {
            return BlockState.None;
        }

        var slice = processor.Line;
        if (!ExtensionsHelper.MatchStart(ref slice, StartString, false))
        {
            return BlockState.None;
        }

        FiguredLinkBlock figuredLinkBlock = new(this);

        processor.NewBlocks.Push(figuredLinkBlock);
        processor.Line.SkipChar();

        return BlockState.Continue;
    }
    
}
