using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System.IO;
using iText.IO.Image;
using iText.Layout.Borders;
using Microsoft.EntityFrameworkCore;

public class PdfGenerator
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "<Pending>")]
    public static byte[] GeneratePdfOrderForm(string id_devis, HistoriqueDevisTravauxRepository _historique_travaux,ApplicationDbContext _context)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            PdfWriter writer = new PdfWriter(memoryStream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Color headerColor = new DeviceRgb(0, 51, 102);
            Color lightGray = new DeviceRgb(240, 240, 240);
            string sql = $"SELECT pourcentage FROM type_finition WHERE id = (SELECT id_type_finition FROM devis WHERE id = '{id_devis}')";
            var pourcentage = _context._type_finition
                .FromSqlRaw(sql)
                .Select(x => x.Pourcentage)
                .FirstOrDefault();

        Image img = new Image(ImageDataFactory.Create("D:/ITU/S6/Mr Rojo/EVALUATION STAGE 2024/Evaluation/wwwroot/assets/img/logo2.png"))
            .SetWidth(100)
            .SetTextAlignment(TextAlignment.RIGHT);
            document.Add(img);


            // Adding client information
            document.Add(new Paragraph("Client Information")
                .SetFont(boldFont)
                .SetFontSize(12)
                .SetMarginBottom(5));
            document.Add(new Paragraph("Client Name\nClient Address\nCity, State, Zip\nPhone: (098) 765-4321\nEmail: client@example.com")
                .SetFont(font)
                .SetFontSize(10)
                .SetMarginBottom(20));


            // Adding the invoice title
            document.Add(new Paragraph("Facture")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetMarginBottom(10));

            // Adding invoice details
            document.Add(new Paragraph("Numero facture: " + id_devis)
                .SetFont(font)
                .SetFontSize(12)
                .SetMarginBottom(5));
            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy"))
                .SetFont(font)
                .SetFontSize(12)
                .SetMarginBottom(20));

            // Creating the table with header
            Table table = new Table(new float[] { 30, 200, 50, 70, 70, 100 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginBottom(20);

            // Adding header cells
            table.AddHeaderCell(CreateHeaderCell("N°", headerColor, boldFont));
            table.AddHeaderCell(CreateHeaderCell("Désignation", headerColor, boldFont));
            table.AddHeaderCell(CreateHeaderCell("Unité", headerColor, boldFont));
            table.AddHeaderCell(CreateHeaderCell("Quantité", headerColor, boldFont));
            table.AddHeaderCell(CreateHeaderCell("Prix unitaire", headerColor, boldFont));
            table.AddHeaderCell(CreateHeaderCell("Total", headerColor, boldFont));

            double? totalAmount = 0;

            // Adding rows to the table
            foreach (var detail in _historique_travaux.FindAllByIdDevis(id_devis))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                table.AddCell(CreateCell(detail.Travaux.CodeTravaux.ToString(), font));
                table.AddCell(CreateCell(detail.Travaux.Designation.ToString(), font));
                table.AddCell(CreateCell(detail.Unite.Nom.ToString(), font));
#pragma warning disable CS8604 // Possible null reference argument.
                table.AddCell(CreateCell(detail.Quantite.ToString(), font));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                string formattedPrixUnitaire = FormatNumberWithSpaces(detail.PrixUnitaire) + " Ar";
                table.AddCell(CreateCell(formattedPrixUnitaire, font));

                double? total = (detail.PrixUnitaire * detail.Quantite) + (detail.PrixUnitaire * detail.Quantite) * (pourcentage/100);
                string formattedTotal = FormatNumberWithSpaces(total) + " Ar";
                table.AddCell(CreateCell(formattedTotal, font));

                totalAmount += total;
            }

            // Adding empty cells for spacing
            table.AddCell(new Cell(1, 4).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetBackgroundColor(lightGray)
                .Add(new Paragraph("Total général").SetFont(boldFont).SetFontSize(12).SetTextAlignment(TextAlignment.RIGHT)));

            // Display the total amount
            string formattedTotalAmount = FormatNumberWithSpaces(totalAmount) + " Ar";
            table.AddCell(new Cell().SetBackgroundColor(lightGray)
                .Add(new Paragraph(formattedTotalAmount).SetFont(boldFont).SetFontSize(12).SetTextAlignment(TextAlignment.RIGHT)));

            document.Add(table);

            // Adding footer
            document.Add(new Paragraph("Merci pour votre confiance.")
                .SetFont(boldFont)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(20));
            document.Add(new Paragraph("Conditions de paiement: À régler sous 30 jours.")
                .SetFont(font)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("Pour toute question concernant cette facture, contactez-nous à l'adresse info@company.com.")
                .SetFont(font)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Close();

            return memoryStream.ToArray();
        }
    }

    private static Cell CreateHeaderCell(string content, Color backgroundColor, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(content).SetFont(font).SetFontSize(12).SetFontColor(ColorConstants.WHITE))
            .SetBackgroundColor(backgroundColor)
            .SetTextAlignment(TextAlignment.CENTER);
    }

    private static Cell CreateCell(string content, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(content).SetFont(font).SetFontSize(10))
            .SetTextAlignment(TextAlignment.CENTER);
    }

    public static string FormatNumberWithSpaces(double? number)
    {
        if (number == null)
            return string.Empty;
        return number.Value.ToString("N0", new System.Globalization.CultureInfo("fr-FR")).Replace(",", "\u00A0");
    }
}
