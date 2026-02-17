using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
namespace ECCSA.BackOffice.Server.WebApp.Services
{

    public static  class OpenXmlWordEditor
    {
        public static bool LoadDocument(string templateName, Dictionary<string, string> replaceKeyValues,
            string saveAsName)
        {
            string fileName = templateName;
           //     $@"C:\Projects\eccsa\code\backoffice\server\WebApp\wwwroot\ReportTemplates\{templateName}";
           string saveAsFileName = saveAsName;
          //      $@"C:\Projects\eccsa\code\backoffice\server\WebApp\wwwroot\ReportTemplates\{saveAsName}";

            StreamWriter sw = null;
            WordprocessingDocument sourceDoc = null;
            StreamReader sr = null;
            WordprocessingDocument destDoc = null;
            try
            {
                if (File.Exists(saveAsFileName))
                {
                    File.Delete(saveAsFileName);
                }

                sourceDoc = WordprocessingDocument.Open(fileName, true);
                {
                    using (sr = new StreamReader(sourceDoc.MainDocumentPart.GetStream()))
                    {
                        var documentText = sr.ReadToEnd();
                        foreach (var key in replaceKeyValues.Keys)
                        {
                            Regex searchText = new Regex(key);
                            documentText = searchText.Replace(documentText, replaceKeyValues[key]);
                        }

                        destDoc =
                           WordprocessingDocument.Create(saveAsFileName, WordprocessingDocumentType.Document);
                        {
                            MainDocumentPart mainPart = destDoc.AddMainDocumentPart();

                            using (sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                            {
                                sw.Write(documentText);
                                sw.Flush();
                                sw.Close();
                            }
                            destDoc.Dispose();
                        }
                        sr.Close();
                    }
                    sourceDoc.Dispose();

                }
            }
            catch (Exception ex)
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }

                if (destDoc != null)
                {
                    destDoc.Dispose();
                }
                if (sourceDoc != null)
                {
                    sourceDoc.Dispose();
                }
                throw ex;
                return false;
            }

            return true;
        }
    }
}
