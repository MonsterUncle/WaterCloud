using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WaterCloud.Code
{
    public class PdfHelper
    {
    }
    public class PDFFooter : PdfPageEventHelper
    {
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Header"));
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            //PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            //tabFot.TotalWidth = 700f;
            //tabFot.DefaultCell.Border = 0;
            ////  var footFont = FontFactory.GetFont("Lato", 12 * 0.667f, new Color(60, 60, 60));
            //string fontpath = HttpContext.Current.Server.MapPath("~/App_Data");
            //BaseFont customfont = BaseFont.CreateFont(fontpath + "\\Lato-Regular.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            //var footFont = new Font(customfont, 12 * 0.667f, Font.NORMAL, new Color(170, 170, 170));

            //PdfPCell cell;
            //cell = new PdfPCell(new Phrase("@ 2016 . All Rights Reserved", footFont));
            //cell.VerticalAlignment = Element.ALIGN_CENTER;
            //cell.Border = 0;
            //cell.PaddingLeft = 100f;
            //tabFot.AddCell(cell);
            //tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}
