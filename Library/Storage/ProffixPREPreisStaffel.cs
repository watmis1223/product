using ProductCalculation.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCalculation.Library.Storage
{
    static partial class StorageOperator
    {
        static string _PRE_PreisStaffel = "PRE_PreisStaffel";

        static void SavePRE_PreisStaffel(CalculationModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.ProffixModel == null)
            {
                return;
            }

            if (model.CalculationNotes == null)
            {
                return;
            }

            //for scale, notes should more than 2
            if (model.CalculationNotes.Count == 2)
            {
                return;
            }

            //update value 
            DataTable dt = new DataTable();
            dt.TableName = _PRE_PreisStaffel;
            DataColumn[] cols = {
                new DataColumn("LaufNr", typeof(Int32)), //id
                new DataColumn("ANummer", typeof(string)), //artikel
                new DataColumn("ArtikelTyp", typeof(Int16)), //1
                new DataColumn("AssortiertPRE", typeof(Int32)), //0

                new DataColumn("KNummer", typeof(Int32)), //0
                new DataColumn("KundenTyp", typeof(Int16)), //0

                new DataColumn("MengeVon", typeof(float)), //scale
                new DataColumn("PreisTypPRE", typeof(string)), //ART
                new DataColumn("Prozent", typeof(Int16)), //0

                new DataColumn("StaffelCode", typeof(string)), //10.0.1.221100200.CHF
                new DataColumn("Verkauf", typeof(Int16)), //1
                new DataColumn("WaehrungPRO", typeof(string)), //CHF
                new DataColumn("Wert", typeof(float)), //amount
                
                new DataColumn("ImportNr", typeof(Int32)), //0

                new DataColumn("ErstelltAm", typeof(string)), //datetime
                //new DataColumn("ErstelltAm", typeof(DateTime)),
                new DataColumn("ErstelltVon", typeof(string)), //cs
                new DataColumn("GeaendertAm", typeof(string)), //datetime
                //new DataColumn("GeaendertAm", typeof(DateTime)),
                new DataColumn("GeaendertVon", typeof(string)), //cs

                new DataColumn("Geaendert", typeof(Int16)), //1
                new DataColumn("Exportiert", typeof(Int16)), //0
            };
            dt.Columns.AddRange(cols);


            //string sNow = DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000", new CultureInfo("en-US"));
            string sNow = "$CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP))";

            //basic note
            CalculationNoteModel basicNote = model.CalculationNotes.Where(item => item.ID == 0).FirstOrDefault();
            List<CalculationNoteModel> scaleNotes = model.CalculationNotes.Where(item => item.ID > 0).ToList();

            foreach (CalculationNoteModel note in scaleNotes)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                CalculationItemModel oCal = note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault();

                dr["LaufNr"] = GetDocumentNumber(_PRE_PreisStaffel, model.ProffixConnection); //new row
                dr["ANummer"] = model.ProffixModel.LAGDokumenteArtikelNrLAG;
                dr["ArtikelTyp"] = 1;
                dr["AssortiertPRE"] = 0;

                dr["KNummer"] = 0;
                dr["KundenTyp"] = 0;

                dr["MengeVon"] = note.Quantity;
                dr["PreisTypPRE"] = "ART";
                dr["Prozent"] = 0;

                dr["StaffelCode"] = String.Concat("10.0.1", ".", model.ProffixModel.LAGDokumenteArtikelNrLAG, ".", oCal.Currency.Currency); ////10.0.1.221100200.CHF
                dr["Verkauf"] = 1;
                dr["WaehrungPRO"] = oCal.Currency.Currency; //"CHF";
                dr["Wert"] = RoundDown(oCal.Total, 4);

                dr["ImportNr"] = 0;

                dr["ErstelltAm"] = sNow;
                dr["ErstelltVon"] = model.GeneralSetting.Employee; //"cs";
                dr["GeaendertAm"] = sNow;
                dr["GeaendertVon"] = model.GeneralSetting.Employee; //"cs";

                dr["Geaendert"] = 1;
                dr["Exportiert"] = 0;

            }

            //delete existing first
            DeletePREPreisStaffelByProductID(model.ProffixModel.LAGDokumenteArtikelNrLAG, model.ProffixConnection);

            //save data
            SaveTable(dt, dt.Columns["LaufNr"], null, connectionString: model.ProffixConnection);
        }

        static void DeletePREPreisStaffelByProductID(string productID, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(String.Format("delete [PRE_PreisStaffel] where [ANummer] = '{0}'", productID), conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
