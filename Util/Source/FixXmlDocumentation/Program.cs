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
                bool pre;
                if (args[0] == "--pre")
                    pre = true;
                else if (args[0] == "--post")
                    pre = false;
                else
                    throw new InvalidOperationException("Invalid command line: specify either --pre or --post.");

                if (pre)
                    Pre();
                else
                    Post();

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }

        static void Pre()
        {
            var xsl = new XslCompiledTransform();
            xsl.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AllowReferencesToOverloadPages.xslt"));

            foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                var doc = new XmlDocument();
                using (var writer = doc.CreateNavigator().AppendChild())
                {
                    xsl.Transform(file, writer);
                }

                doc.Save(file);

                // Workaround Sandcastle bug by naming one of our components incorrectly (!)
                File.WriteAllText(file, File.ReadAllText(file).Replace(
                    "M:Nito.ConnectedProperties.IConnectibleProperty`1.GetOrCreate(System.Func{`0})",
                    "M:Nito.ConnectedProperties.IConnectibleProperty`1.GetOrCreate(System.Func`1)"));

                Console.WriteLine("Prepared " + file + " for Sandcastle");
            }
        }

        static void Post()
        {
            foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                // Replace the incorrect name with the correct name after Sandcastle runs, so VS will be OK with it.
                File.WriteAllText(file, File.ReadAllText(file).Replace(
                    "M:Nito.ConnectedProperties.IConnectibleProperty`1.GetOrCreate(System.Func`1)",
                    "M:Nito.ConnectedProperties.IConnectibleProperty`1.GetOrCreate(System.Func{`0})"));

                Console.WriteLine("Fixed " + file);
            }
        }
    }
}
