using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixXmlDocumentation
{
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Xsl;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var xsl = new XslCompiledTransform();
                xsl.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AllowReferencesToOverloadPages.xslt"));

                foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories))
                {
                    var doc = new XmlDocument();
                    using (var writer = doc.CreateNavigator().AppendChild())
                    {
                        xsl.Transform(file, writer);
                    }

                    doc.Save(file);
                    Console.WriteLine("Fixed " + file);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }
    }
}
