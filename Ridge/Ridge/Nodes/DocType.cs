namespace Ridge.Nodes
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

        internal static DocType Create(Source source, int depth)
        {
            source.Expect(NAME, true);
            source.MoveForward(NAME.Length);

            var result = new DocType
                         {
                             Depth = depth
                         };

            source.SkipWhiteSpace();
            if (source.IsNot('>'))
            {
                result.Declaration = source.TakeUntil(c => c == '>');
            }
            else
            {
                result.Declaration = string.Empty;
            }
            source.MoveForward();

            return result;
        }
    }
}