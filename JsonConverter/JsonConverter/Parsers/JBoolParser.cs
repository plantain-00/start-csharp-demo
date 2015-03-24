﻿using JsonConverter.Nodes;

namespace JsonConverter.Parsers
{
    internal class JBoolParser : ParserBase
    {
        internal JBoolParser(Source source) : base(source)
        {
        }

        internal override void Parse()
        {
            if (Source.Is("true"))
            {
                Source.MoveForward("true".Length);

                Result = new JBool
                         {
                             Value = true
                         };
            }
            else if (Source.Is("false"))
            {
                Source.MoveForward("false".Length);

                Result = new JBool
                         {
                             Value = false
                         };
            }
            else
            {
                throw new ParseException(Source);
            }
        }
    }
}