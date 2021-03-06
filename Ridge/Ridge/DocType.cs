using ParseLibrary;

namespace Ridge
{
    public class DocType : Node
    {
        internal const string NAME = "<!DOCTYPE";
        public string Declaration { get; set; }

        public override string ToString(Formatting formatting, int spaceNumber = 4)
        {
            if (formatting == Formatting.None)
            {
                return string.Format("{0} {1}>", NAME, Declaration);
            }
            return string.Format("{0} {1}>\n", NAME, Declaration);
        }

        internal static DocType Create(Source source, Node parent, int depth)
        {
            source.Expect(NAME, true);
            source.Skip(NAME);

            var result = new DocType
                         {
                             Depth = depth,
                             Parent = parent
                         };

            source.SkipBlankSpaces();
            if (source.IsNot('>'))
            {
                result.Declaration = source.TakeUntil('>');
            }
            else
            {
                result.Declaration = string.Empty;
            }
            source.SkipIt();

            return result;
        }
    }
}