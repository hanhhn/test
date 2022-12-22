using Aspose.Words;
using System;
using System.IO;

namespace aspose.words.linux
{
    class Program
    {
        static void Main(string[] args)
        {
            License lic = new License();
            lic.SetLicense(Path.Combine(Directory.GetCurrentDirectory(), "Aspose.Total.NET.lic"));

            Document document = new Document(Path.Combine(Directory.GetCurrentDirectory(), "Monthly-report-template.docx"));
            document.Save(string.Format("{0}.pdf", DateTime.Now.ToFileTimeUtc()));

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
