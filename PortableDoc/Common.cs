// --------------------------------------------------------------------------------------------------------------------
// <summary>
// Algorythms.PortableDoc.Common : Common.cs
//     Implementation file of Common class
//
// Purpose: Common support logic.
//           
// 
// </summary>
// <copyright company="XRSolutions" file="Common.cs">
//   Copyright XRSolutions, All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Algorythms.PortableDoc
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Common supporting locic.
    /// </summary>
    internal class Common
    {

        #region fields

        #endregion fields

        //////////////////////////////////////////////////////////
        //// public properties
        #region public properties

        #endregion public properties

        //////////////////////////////////////////////////////////
        //// public methods
        #region public methods

        /// <summary>
        /// The clean PDF text.
        /// Removes extra line breaks.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal static string CleanPdfText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Reduce multiple SP + LF to string of LF
            int ndx = text.LastIndexOf(" \n \n", StringComparison.Ordinal);
            while (ndx > -1)
            {
                text = text.Substring(0, ndx + 2) + text.Substring(ndx + 3);

                ndx = text.LastIndexOf(" \n \n", StringComparison.Ordinal);
            }

            // Replace all period + sp(s) + LF with period + LF
            Regex rx = new Regex("\\.\\s+\\n");
            text = rx.Replace(text, ".\n");

            text = RepairSentences(text);

            return text;

            // Attempt to remove breaks in sentences.  Removes indescremently causing loss of structure.
            // Replace all single SP + LF with single space.
            text = text.Replace(" \n", " ");

            // Replace all dash + LF with single space.
            text = text.Replace("-\n", " ");

            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Text to repair.</param>
        /// <param name="lineending">Line ending characters.  Default is char 13 (\n).</param>
        /// <param name="maxline">Maximum line length.  Default is 80.</param>
        /// <returns></returns>
        private static string RepairSentences(string text, string lineending = "\n", int maxline = 80)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            bool newline = true;
            StringBuilder sb = new StringBuilder();
            string[] sentenceterminators = { ".", "?", "!", ":" };
            string[] lineterminators = sentenceterminators.Union(Enumerable.Range(0, 10).Select(n => n.ToString())).ToArray();
            string[] lines = text.Split(new[] { lineending }, StringSplitOptions.None);

            // Measure left margin
            int marginsize = lines.Where(line => line.Length > 2).Select(line => line.TakeWhile(char.IsWhiteSpace).Count()).Concat(new[] { 999 }).Min();

            // Remove LF from long lines without sentence terminator.
            foreach (string line in lines.Where(line => line.Length > marginsize).Select(l => l.Substring(marginsize)))
            {
                bool isTerminated = lineterminators.Any(x => line.EndsWith(x));
                if (line.Length < maxline || isTerminated)
                {
                    sb.AppendLine(newline ? line : " " + line.Trim());

                    newline = true;
                }
                else
                {
                    sb.Append(newline ? line : " " + line.Trim());

                    newline = false;
                }
            }

            return sb.ToString();
        }
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
