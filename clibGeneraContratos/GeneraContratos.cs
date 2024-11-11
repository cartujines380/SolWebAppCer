using DocumentFormat.OpenXml.Packaging;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace clibGeneraContratos
{
    public class GeneraContratos
    {
        public byte[] writeToWordDocx(DataTable dt, string PathTempFile)
        {
            byte[] Res = null;
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(PathTempFile, true))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lcambio(wordprocessingDocument, "&amp;" + dr["campo"].ToString(), dr["valor"].ToString());
                }
            }

            Res = File.ReadAllBytes(PathTempFile);
            return Res;
        }

        private void lcambio(WordprocessingDocument docu, string nombreword, string nombreReplace)
        {
            try
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(docu.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                docText = Regex.Replace(docText, nombreword, nombreReplace);

                using (StreamWriter sw = new StreamWriter(docu.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
