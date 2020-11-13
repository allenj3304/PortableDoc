// --------------------------------------------------------------------------------------------------------------------
// <summary>
// Algorythms.PortableDoc.Text : Text.cs
//     Implementation file of Text class
//
// Purpose: Text extractor for Portable Document Files.
//           
// 
// </summary>
// <copyright company="XRSolutions" file="Text.cs">
//   Copyright XRSolutions, All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------




namespace Algorythms.PortableDoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using iText7 = iText.Kernel.Pdf;
    /// <summary>
    /// Portable Document Text Reader.
    /// </summary>
    public class Text
    {
        #region fields

        #endregion fields

        //////////////////////////////////////////////////////////
        //// public properties
        #region public properties

        /// <summary>
        /// Convert the Portable Document to text.
        /// </summary>
        /// <param name="file">The portable document file stream.</param>
        /// <param name="errors">Collection of errors that occur during reading the file stream.  Null if sucessful.</param>
        /// <returns>The text contents of the file.</returns>
        public static string Convert(Stream file, out List<Exception> errors)
        {
            StringBuilder result = new StringBuilder();
            errors = null;
            
            using iText7.PdfReader reader = new iText7.PdfReader(file);
            using iText7.PdfDocument doc = new iText7.PdfDocument(reader);

            int numberOfPages = doc.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                try
                {
                    var page = doc.GetPage(i);
                    string pagetext = iText7.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page);
                    result.Append(Common.CleanPdfText(pagetext));
                }
                catch(Exception e)
                {
                    if(errors == null) errors = new List<Exception>();
                    errors.Add(e);
                }
            }

            return result.ToString();
        }
    
        /// <summary>
        /// An enumeration of paragraphs of the portable document.
        /// </summary>
        /// <param name="file">The portable document file stream.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of paragraphs.</returns>
        public static IEnumerable<string> Paragraphs(Stream file)
        {
            using iText7.PdfReader reader = new iText7.PdfReader(file);
            using iText7.PdfDocument doc = new iText7.PdfDocument(reader);

            int numberOfPages = doc.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                iText7.PdfPage page = doc.GetPage(i);
                string pagetext = iText7.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page);
                pagetext = Common.CleanPdfText(pagetext);

                // Parse paragraphs.
                IEnumerable<string> paragraphs = pagetext.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in paragraphs) 
                {
                    yield return item; 
                }
            }
        }

        /// <summary>
        /// An enumeration of sentences of the portable document.
        /// </summary>
        /// <param name="file">The portable document file stream.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of sentences.</returns>
        public static IEnumerable<string> Sentences(Stream file)
        {
            throw new NotImplementedException("TODO:  Add sentence parsing.");
        }


        #endregion public properties

        //////////////////////////////////////////////////////////
        //// public methods
        #region public methods

        #endregion public methods

        //////////////////////////////////////////////////////////
        //// private methods
        #region private methods

        #endregion private methods

        //////////////////////////////////////////////////////////
        //// event handlers
        #region event handlers

        #endregion event handlers

    }
}
