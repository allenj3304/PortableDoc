

namespace ConsoleApp1
{
    using System.Collections.Generic;
    using System.IO;

    using Algorythms.PortableDoc;

    class Program
    {
        static void Main(string[] args)
        {
            var file = @"..\PortableDoc\Samples\lorem.pdf";
            FileInfo source = new FileInfo(file);

            using (var fileStream = source.OpenRead())
            {
                IEnumerable<string> output = Text.Paragraphs(fileStream);
            }
        }
    }
}
