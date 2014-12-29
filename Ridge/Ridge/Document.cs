﻿using System.Collections.Generic;
using System.Text;

using Ridge.Nodes;
using Ridge.Parsers;

namespace Ridge
{
    public class Document
    {
        public Document()
        {
            Nodes = new List<Node>();
        }

        public Document(string html) : this()
        {
            var strings = new LexicalAnalysis().Analyse(html);
            var doctypeParser = new DocTypeParser(strings, 0);
            doctypeParser.Parse();

            Nodes.Add(doctypeParser.DocType);

            for (var index = doctypeParser.Index; index < strings.Count;)
            {
                while (index < strings.Count
                       && (strings[index] == STRING.SPACE || strings[index] == STRING.RETURN || strings[index] == STRING.NEW_LINE))
                {
                    index++;
                }
                if (strings[index] == STRING.LESS_THAN)
                {
                    var tagParser = new TagParser(strings, index, 0);
                    tagParser.Parse();

                    Nodes.Add(tagParser.Tag);
                    index = tagParser.Index;
                }
                else
                {
                    var plainTextParser = new PlainTextParser(strings, index, 0);
                    plainTextParser.Parse();

                    Nodes.Add(plainTextParser.PlainText);
                    index = plainTextParser.Index;
                }
                while (index < strings.Count
                       && (strings[index] == STRING.SPACE || strings[index] == STRING.RETURN || strings[index] == STRING.NEW_LINE))
                {
                    index++;
                }
            }
        }

        public List<Node> Nodes { get; set; }

        public Node GetElementById(string id)
        {
            foreach (var node in Nodes)
            {
                var result = node.GetElementById(id);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in Nodes)
            {
                builder.Append(node);
            }
            return builder.ToString();
        }
    }
}