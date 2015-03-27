﻿using XmlConverter.Nodes;

namespace XmlConverter.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //const string XML = " <!--no need to escape <code> & such in comments--> ";
            //const string XML = " <?xml version=\"1.0\" encoding=\"UTF-8\"?> ";
            const string XML = " <Greeting name=\"sdsd\" age=\"12\" >Hello, world.<people alt='Foligno Madonna, by Raphael' /></Greeting> ";

            var document = Document.Create(XML);
            var description = document.ToString(Formatting.Indented);
        }
    }
}