// --------------------------------------------------------------------------------------------------------------------
// <summary>
// Algorythms.PortableDoc.UnitTests.TextTest : classname.cs
//     Implementation file of classname class
//
// Purpose: Unit test for the Text class.
//           
// 
// </summary>
// <copyright company="XRSolutions" file="classname.cs">
//   Copyright XRSolutions, All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Algorythms.PortableDoc.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    [TestClass]
    public class TextTest
    {
        private const string TestDir = @"Samples\";
        private const string ConvertDir = "UnitTests-Convert";
        private const string ParaDir = "UnitTests-ParaTest";

        private string samplesDir;

        private TestContext context;

        public TestContext TestContext { get => this.context; set { this.context = value; } }

        //[ClassInitialize]
        //public static void TextTestInitialize(TestContext testContext)
        //{
        //}

        [TestInitialize]
        public void Initialize()
        {
            this.SetSamplesDir();            
        }

        [TestCleanup]
        public void Cleanup()
        {            
        }

        [TestMethod]
        public void ConvertTest()
        {
            //this.CleanupOutput(ConvertDir);

            foreach (string file in Directory.EnumerateFiles(this.samplesDir, "*.pdf"))
            {
                FileInfo source = new FileInfo(file);

                using var fileStream = source.OpenRead();
                string output = Text.Convert(fileStream, out List<Exception> errors);

                if (errors != null)
                {
                    errors.ForEach(e => Assert.Fail(e.Message));
                }
                else
                {
                    Assert.IsNotNull(output);
                    var outfile = this.GetOutput(source, ConvertDir);

                    SaveToTxtFile(outfile, output);
                }
            }
        }

        [TestMethod]
        public void ParagraphsTest()
        {
            //this.CleanupOutput(ParaDir);

            foreach (string file in Directory.EnumerateFiles(this.samplesDir, "*.pdf"))
            {
                FileInfo source = new FileInfo(file);

                using var fileStream = source.OpenRead();
                IEnumerable<string> output = Text.Paragraphs(fileStream);

                FileInfo outfile = this.GetOutput(source, ParaDir);
                
                using StreamWriter stream = new StreamWriter(outfile.FullName, false, System.Text.Encoding.Unicode);
                foreach (string para in output)
                {
                    Assert.IsNotNull(para);

                    stream.WriteLine(para);
                }
            }
        }

        private FileInfo GetOutput(FileInfo sourceFile, string subDir)
        {
            // NOTE: Folder is automatically deleted for Sucessfull tests.
            FileInfo output =
                new FileInfo(
                    Path.ChangeExtension(
                        Path.Combine(this.TestContext.TestRunResultsDirectory, $"{subDir}-{sourceFile.Name}"),
                        ".txt"));
            this.TestContext.AddResultFile(output.FullName);

            if (output.Directory != null && !output.Directory.Exists)
            {
                output.Directory.Create();
            }

            return output;
        }

        private void SetSamplesDir()
        {
            DirectoryInfo dir = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName);
            
            this.samplesDir = Path.Combine(dir.FullName, TestDir);
        }

        [Obsolete("Using TestContext result directory for output.")]
        private void CleanupOutput(string subDir)
        {
            DirectoryInfo outDir = new DirectoryInfo(Path.Combine(this.samplesDir, subDir));

            // Remove previous output.
            if (outDir.Exists)
            {
                outDir.Delete(true);
            }
        }

        private static void SaveToTxtFile(FileInfo fi, string contents)
        {
            using StreamWriter file = new System.IO.StreamWriter(fi.FullName, false, System.Text.Encoding.UTF8);
            file.Write(contents);
        }
    }
}
