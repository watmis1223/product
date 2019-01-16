using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Data;
using System.IO;
using PdfSharp.Drawing;
using ProductCalculation.Library.Entity.PriceCalculation.Models;
using System.Globalization;

namespace ProductCalculation.Library.Entity.Report
{
    public class Invoice
    {
        CalculationModel _Model;
        Document _Document;
        string _ReportID;
        string _ReportHeader;

        // Some pre-defined colors
#if true
        // RGB colors
        readonly static Color TableBorder = new Color(81, 125, 192);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);
#else
    // CMYK colors
    readonly static Color tableBorder = Color.FromCmyk(100, 50, 0, 30);
    readonly static Color tableBlue = Color.FromCmyk(0, 80, 50, 30);
    readonly static Color tableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif

        public Invoice()
        {

        }

        public void CreateInvoice(CalculationModel model, string reportPath)
        {
            if (model == null)
            {
                return;
            }

            _Model = model;

            //if (_Model.ProffixModel != null)
            //{
            //    _ReportHeader = string.Format("Kalkulation {0} {1}",
            //    (!String.IsNullOrWhiteSpace(_Model.ProffixModel.LAGDokumenteArtikelNrLAG) ? "Artikel" : "Adress"),
            //    (!String.IsNullOrWhiteSpace(_Model.ProffixModel.LAGDokumenteArtikelNrLAG) ? _Model.ProffixModel.LAGDokumenteArtikelNrLAG : _Model.ProffixModel.ADRDokumenteDokumentNrADR));

            //    _ReportID = string.Format("Kalkulation_{0}_{1}_{2}.pdf",
            //        (!String.IsNullOrWhiteSpace(_Model.ProffixModel.LAGDokumenteArtikelNrLAG) ? "Artikel" : "Adress"),
            //        (!String.IsNullOrWhiteSpace(_Model.ProffixModel.LAGDokumenteArtikelNrLAG) ? _Model.ProffixModel.LAGDokumenteArtikelNrLAG : _Model.ProffixModel.ADRDokumenteDokumentNrADR),
            //        DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            //}
            //else
            //{
            //    _ReportHeader = string.Format("Kalkulation {0}", DateTime.Now.ToString("yyyyMMdd HHmmss"));
            //    _ReportID = string.Format("Kalkulation_{0}.pdf", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            //}

            if (String.IsNullOrWhiteSpace(_Model.ReportHeader))
            {
                _Model.ReportHeader = string.Format("Kalkulation {0}", DateTime.Now.ToString("yyyyMMdd HHmmss"));
            }
            if (String.IsNullOrWhiteSpace(_Model.ReportID))
            {
                _Model.ReportID = string.Format("Kalkulation_{0}.pdf", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            }
            _ReportHeader = _Model.ReportHeader;
            _ReportID = _Model.ReportID;


            CreateDocument();

            _Document.UseCmykColor = true;

            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // Create a renderer for PDF that uses Unicode font encoding
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Set the MigraDoc document
            pdfRenderer.Document = _Document;

            // Create the PDF document
            pdfRenderer.RenderDocument();

            pdfRenderer.Save(string.Format("{0}\\{1}", reportPath, _ReportID));
            // ...and start a viewer.
            Process.Start(string.Format("{0}\\{1}", reportPath, _ReportID));
        }

        Document CreateDocument()
        {
            // Create a new MigraDoc document
            this._Document = new Document();

            DefineStyles();

            CreateCalculationSection();

            return this._Document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this._Document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.

            style.Font.Name = "Verdana";

            style = this._Document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this._Document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this._Document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            //style.Font.Name = "Times New Roman";

            style.Font.Size = 7;

            // Create a new style called Reference based on style Normal
            style = this._Document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        Section reportSection;

        void CreateCalculationSection()
        {
            // Each MigraDoc document needs at least one section.
            reportSection = this._Document.AddSection();

            Paragraph paragraph = null;
            paragraph = reportSection.Headers.Primary.AddParagraph();
            paragraph.AddText(_ReportHeader);
            paragraph.Format.Font.Size = 15;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            paragraph = reportSection.AddParagraph();
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US")));
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.Supplier) ? "-" : _Model.GeneralSetting.Supplier);
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.ProductDesc.Line1) ? "-" : _Model.GeneralSetting.ProductDesc.Line1);
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.ProductDesc.Line2) ? "-" : _Model.GeneralSetting.ProductDesc.Line2);
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.ProductDesc.Line3) ? "-" : _Model.GeneralSetting.ProductDesc.Line3);
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.ProductDesc.Line4) ? "-" : _Model.GeneralSetting.ProductDesc.Line4);
            paragraph.AddLineBreak();
            paragraph.AddText(String.IsNullOrWhiteSpace(_Model.GeneralSetting.ProductDesc.Line5) ? "-" : _Model.GeneralSetting.ProductDesc.Line5);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Left;

            // Add the print date field
            paragraph = reportSection.AddParagraph();
            paragraph.Format.SpaceBefore = "0.5cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Kalkulation", TextFormat.Bold);


            Table orderDetailTable = null;
            Row headerRow = null;

            // Create the item table
            orderDetailTable = reportSection.AddTable();
            orderDetailTable.Style = "Table";
            orderDetailTable.Borders.Color = TableBorder;
            orderDetailTable.Borders.Width = 0.25;
            orderDetailTable.Borders.Left.Width = 0.5;
            orderDetailTable.Borders.Right.Width = 0.5;
            orderDetailTable.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns            
            Column column = orderDetailTable.AddColumn("11cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = orderDetailTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = orderDetailTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            headerRow = orderDetailTable.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = TableBlue;

            headerRow.Cells[0].AddParagraph("Kostenanteil");
            headerRow.Cells[0].Format.Font.Bold = true;
            headerRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            //headerRow.Cells[0].MergeDown = 1;

            headerRow.Cells[1].AddParagraph("Währung");
            headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            headerRow.Cells[2].AddParagraph("Betrag");
            headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            orderDetailTable.SetEdge(0, 0, 3, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            FillOrderDetailContent(orderDetailTable);
        }
        decimal RoundDown(decimal? number, int decimalPlaces)
        {
            return Convert.ToDecimal(number.GetValueOrDefault().ToString("N" + decimalPlaces));
        }

        decimal GetMarginSummarize(CalculationNoteModel note)
        {
            decimal iSummary = 0;

            if (note.CalculationMarginItems != null)
            {
                CalculationItemModel oVK = note.CalculationMarginItems.Find(item => item.Tag == "VK");
                CalculationItemModel oVKbrutto = note.CalculationMarginItems.Find(item => item.Tag == "VK(brutto)");

                if (oVK != null && (oVKbrutto != null && oVKbrutto.VariableTotal > 0))
                {
                    iSummary = (oVK.VariableTotal / oVKbrutto.VariableTotal) * 100;
                }
            }

            return iSummary;
        }

        string GetMarginSummarizeDesc(CalculationNoteModel note)
        {
            string desc = "Deckungsbeitrag";

            if (note.CalculationMarginItems != null)
            {
                CalculationItemModel oVK = note.CalculationMarginItems.Find(item => item.Tag == "VK");
                CalculationItemModel oVKbrutto = note.CalculationMarginItems.Find(item => item.Tag == "VK(brutto)");

                if (oVK != null && (oVKbrutto != null && oVKbrutto.VariableTotal > 0))
                {
                    desc = String.Format("{0} {1}/{2}", desc, oVK.VariableTotal, oVKbrutto.VariableTotal);
                }
            }

            return desc;
        }

        void FillOrderDetailContent(Table orderDetailTable)
        {
            CalculationNoteModel basicNote = _Model.CalculationNotes.Where(item => item.ID == 0).FirstOrDefault();
            List<CalculationNoteModel> scaleNotes = _Model.CalculationNotes.Where(item => item.ID > 0).ToList();

            CalculationItemModel oCal;
            Row orderDetailRow = null;

            oCal = basicNote.CalculationItems.Where(item => item.Tag == "BEK").FirstOrDefault();
            orderDetailRow = orderDetailTable.AddRow();
            orderDetailRow.Cells[0].AddParagraph(oCal.Description);
            orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
            orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

            oCal = basicNote.CalculationItems.Where(item => item.Tag == "ESTP").FirstOrDefault();
            orderDetailRow = orderDetailTable.AddRow();
            orderDetailRow.Cells[0].AddParagraph(oCal.Description);
            orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
            orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

            oCal = basicNote.CalculationItems.Where(item => item.Tag == "SK 1").FirstOrDefault();
            orderDetailRow = orderDetailTable.AddRow();
            orderDetailRow.Cells[0].AddParagraph(oCal.Description);
            orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
            orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

            oCal = basicNote.CalculationItems.Where(item => item.Tag == "SK 2").FirstOrDefault();
            orderDetailRow = orderDetailTable.AddRow();
            orderDetailRow.Cells[0].AddParagraph(oCal.Description);
            orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
            orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

            if (scaleNotes.Count == 1)
            {
                foreach (CalculationNoteModel note in scaleNotes)
                {
                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(bar)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(ziel)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(brutto)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    if (note.CalculationMarginItems != null)
                    {
                        oCal = note.CalculationMarginItems.Find(item => item.Tag == "VK");
                        if (oCal != null)
                        {
                            orderDetailRow = orderDetailTable.AddRow();
                            orderDetailRow.Cells[0].AddParagraph("Deckungsbeitrag");
                            orderDetailRow.Cells[1].AddParagraph("");
                            orderDetailRow.Cells[2].AddParagraph(String.Format("{0}", RoundDown(oCal.VariableTotal, 4).ToString()));

                            orderDetailRow = orderDetailTable.AddRow();
                            orderDetailRow.Cells[0].AddParagraph(GetMarginSummarizeDesc(note));
                            orderDetailRow.Cells[1].AddParagraph("");
                            orderDetailRow.Cells[2].AddParagraph(String.Format("{0}%", RoundDown(GetMarginSummarize(note), 4).ToString()));
                        }
                    }
                }
            }
            else
            {
                foreach (CalculationNoteModel note in scaleNotes)
                {
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(String.Format("Quantity {0}", RoundDown(note.Quantity, 4)));
                    orderDetailRow.Cells[1].AddParagraph("");
                    orderDetailRow.Cells[2].AddParagraph("");
                    orderDetailRow.Format.Alignment = ParagraphAlignment.Center;
                    orderDetailRow.Format.Font.Bold = true;
                    orderDetailRow.Shading.Color = TableBlue;

                    oCal = note.CalculationItems.Where(item => item.Tag == "GA").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(bar)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(ziel)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    oCal = note.CalculationItems.Where(item => item.Tag == "VK(brutto)").FirstOrDefault();
                    orderDetailRow = orderDetailTable.AddRow();
                    orderDetailRow.Cells[0].AddParagraph(oCal.Description);
                    orderDetailRow.Cells[1].AddParagraph(oCal.Currency.Currency);
                    orderDetailRow.Cells[2].AddParagraph(RoundDown(oCal.Total, 4).ToString());

                    if (note.CalculationMarginItems != null)
                    {
                        oCal = note.CalculationMarginItems.Find(item => item.Tag == "VK");
                        if (oCal != null)
                        {
                            orderDetailRow = orderDetailTable.AddRow();
                            orderDetailRow.Cells[0].AddParagraph("Deckungsbeitrag");
                            orderDetailRow.Cells[1].AddParagraph("");
                            orderDetailRow.Cells[2].AddParagraph(String.Format("{0}", RoundDown(oCal.VariableTotal, 4).ToString()));

                            orderDetailRow = orderDetailTable.AddRow();
                            orderDetailRow.Cells[0].AddParagraph(GetMarginSummarizeDesc(note));
                            orderDetailRow.Cells[1].AddParagraph("");
                            orderDetailRow.Cells[2].AddParagraph(String.Format("{0}%", RoundDown(GetMarginSummarize(note), 4).ToString()));
                        }
                    }
                }
            }


            //orderDetailRow = orderDetailTable.AddRow();
            //orderDetailRow.Cells[0].Shading.Color = TableGray;
            //orderDetailRow.Cells[1].MergeRight = 1;
            //orderDetailRow.Cells[1].AddParagraph("Total");
            //orderDetailRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            //orderDetailRow.Cells[1].Format.Font.Bold = true;
            //orderDetailRow.Cells[3].AddParagraph(string.Format("{0:n2}", subSurchargePrice));
            //orderDetailRow.Cells[3].Format.Font.Bold = true;
            //orderDetailTable.SetEdge(3, orderDetailTable.Rows.Count - 1, 1, 1, Edge.Bottom, BorderStyle.Single, 1);
            //orderDetailTable.SetEdge(3, orderDetailTable.Rows.Count - 2, 1, 1, Edge.Bottom, BorderStyle.Single, 1);

        }
    }
}
