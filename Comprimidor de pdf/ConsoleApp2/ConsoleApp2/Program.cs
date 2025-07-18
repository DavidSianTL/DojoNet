using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using iText.Kernel.Pdf;

class Program
{
    static void Main()
    {
        string inputPath = @"C:\Users\Admin\Desktop\Evaluacion.pdf";
        string cleanedPath = @"C:\Users\Admin\Desktop\Evaluacion_sin_metadata.pdf";
        string compressedPath = @"C:\Users\Admin\Desktop\Evaluacion_comprimido.pdf";
        string brotliPath = @"C:\Users\Admin\Desktop\Evaluacion_comprimido.brotli";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("❌ El archivo no existe.");
            return;
        }

        long originalSize = new FileInfo(inputPath).Length;

        // Paso 1: Eliminar metadata
        using (var reader = new PdfReader(inputPath))
        using (var writer = new PdfWriter(cleanedPath))
        using (var pdfDoc = new PdfDocument(reader, writer))
        {
            var info = pdfDoc.GetDocumentInfo();
            info.SetAuthor(""); info.SetTitle(""); info.SetSubject("");
            info.SetCreator(""); info.SetKeywords(""); info.SetProducer("");

            var catalog = pdfDoc.GetCatalog().GetPdfObject();
            catalog.Remove(PdfName.Metadata);
            catalog.Remove(PdfName.ViewerPreferences);
            catalog.Remove(PdfName.OpenAction);
            catalog.Remove(PdfName.Trapped);
            catalog.Remove(PdfName.Names);
            catalog.Remove(PdfName.EmbeddedFiles);
            catalog.Remove(PdfName.Outlines);
            pdfDoc.GetTrailer().Remove(PdfName.ID);
        }

        long cleanedSize = new FileInfo(cleanedPath).Length;

        // Paso 2: Comprimir con Ghostscript
        string gsPath = @"C:\Program Files\gs\gs10.05.1\bin\gswin64c.exe";
        string arguments = $"-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS=/ebook -dNOPAUSE -dQUIET -dBATCH -sOutputFile=\"{compressedPath}\" \"{cleanedPath}\"";

        if (!File.Exists(gsPath))
        {
            Console.WriteLine($"❌ Ghostscript no encontrado en: {gsPath}");
            return;
        }

        Console.WriteLine(" Comprimiendo PDF con Ghostscript...");
        var gs = new ProcessStartInfo
        {
            FileName = gsPath,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = Process.Start(gs);
        process.WaitForExit();

        long finalSize = new FileInfo(compressedPath).Length;
        double percent = ((double)(originalSize - finalSize) / originalSize) * 100;

        Console.WriteLine($"\n Tamaño original:    {originalSize / 1024.0:F2} KB");
        Console.WriteLine($"Tras limpieza:      {cleanedSize / 1024.0:F2} KB");
        Console.WriteLine($" Tras compresión:    {finalSize / 1024.0:F2} KB");
        Console.WriteLine($" Reducción total:    {percent:F2}%");

        // Paso 3: Convertir a Brotli
        Console.WriteLine("Generando versión Brotli...");

        using (FileStream inputFile = File.OpenRead(compressedPath))
        using (FileStream outputFile = File.Create(brotliPath))
        using (BrotliStream brotliStream = new BrotliStream(outputFile, CompressionLevel.Optimal))
        {
            inputFile.CopyTo(brotliStream);
        }

        long brotliSize = new FileInfo(brotliPath).Length;
        double brotliPercent = ((double)(finalSize - brotliSize) / finalSize) * 100;

        Console.WriteLine($" Brotli creado:       {brotliSize / 1024.0:F2} KB");
        Console.WriteLine($"Reducción Brotli:    {brotliPercent:F2}%");
    }
}
