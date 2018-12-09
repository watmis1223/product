using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using ProductCalculation.Library.Entity.PriceCalculation.Models;
using ProductCalculation.Library.Util;
using System.Globalization;
using ProductCalculation.Library.Business.PriceCalculation;

namespace ProductCalculation.Library.Storage
{
    static partial class StorageOperator
    {
        public static long CalPriceGetLatestID()
        {
            long iID = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cal_CalPriceGetLatestID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    iID = Convert.ToInt64(cmd.ExecuteScalar());
                    conn.Close();
                }
            }

            return iID;
        }

        public static CalculationModel CalPriceLoadByID(long id)
        {
            CalculationModel model = null;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cal_CalPriceGetByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PriceID", id));
                    conn.Open();

                    try
                    {
                        SqlDataReader drd = cmd.ExecuteReader();
                        dt.Load(drd);
                    }
                    catch { }

                    conn.Close();
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string sEncodeJson = String.Concat(
                        dt.Rows[0]["JsonData1"],
                        dt.Rows[0]["JsonData2"],
                        dt.Rows[0]["JsonData3"],
                        dt.Rows[0]["JsonData4"],
                        dt.Rows[0]["JsonData5"],
                        dt.Rows[0]["JsonData6"],
                        dt.Rows[0]["JsonData7"],
                        dt.Rows[0]["JsonData8"],
                        dt.Rows[0]["JsonData9"],
                        dt.Rows[0]["JsonData10"]
                        );

                model = Utility.JsonToObject<CalculationModel>(Zipper.Unzip(Convert.FromBase64String(sEncodeJson)));
            }

            return model;
        }

        public static void SaveCalculationModel(CalculationModel model)
        {
            try
            {
                DataTable dt = new DataTable("CalPrice");
                dt.Columns.Add(new DataColumn("PriceID", typeof(long)));
                dt.Columns.Add(new DataColumn("JsonData1", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData2", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData3", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData4", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData5", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData6", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData7", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData8", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData9", typeof(string)));
                dt.Columns.Add(new DataColumn("JsonData10", typeof(string)));

                //create new calculation row
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                //set insert columns
                List<DataColumn> oIgnoreSave = new List<DataColumn>();
                oIgnoreSave.Add(dt.Columns["PriceID"]);
                if (model.ID == 0)
                {
                    //insert new row first to get id
                    dr["JsonData1"] = "NEW";
                    model.ID = InsertRowReturnIdentity(dt.Rows[0], dt.Columns["PriceID"], oIgnoreSave.ToArray());
                }
                dr["PriceID"] = model.ID;

                //save proffix if needed
                SaveProffix(model);

                //update calculation
                model.CalculaionDateTime = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                string sJson = Utility.ObjectToJson(model);
                string sCompress = Zipper.Zip(sJson);

                //extract encode json to JsonData1 to JsonData10
                int i = 1;
                while (!String.IsNullOrWhiteSpace(sCompress))
                {
                    if (sCompress.Length >= 4000)
                    {
                        dr[String.Concat("JsonData", i)] = sCompress.Substring(0, 4000);
                        sCompress = sCompress.Remove(0, 4000);
                    }
                    else
                    {
                        dr[String.Concat("JsonData", i)] = sCompress.Substring(0);
                        sCompress = sCompress.Remove(0, sCompress.Length);
                    }

                    i += 1;
                }
                //update jsondata by particular row
                SaveTable(dt, dt.Columns["PriceID"], oIgnoreSave.ToArray());

                //save detail
                DeleteCalculationDetailByPriceID(model.ID);
                SaveCalculationDetail(model);


                //after save success
                if (model.ProffixModel != null)
                {
                    model.ProffixModel.Command = Global.Commands.Open;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static decimal RoundDown(decimal? number, int decimalPlaces)
        {
            return Convert.ToDecimal(number.GetValueOrDefault().ToString("N" + decimalPlaces));
        }

        static decimal GetMarginSummarize(CalculationNoteModel note)
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

        static decimal GetBenItemTotal(CalculationNoteModel note, string benTag)
        {
            CalculationItemModel oBEN = note.CalculationItems.Where(item => item.Tag == benTag).FirstOrDefault();

            if (oBEN != null)
            {
                return oBEN.Total;
            }

            return 0;
        }

        public static void DeleteCalculationDetailByPriceID(long priceID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("delete [CalPriceDetail] where [PriceID] = " + priceID, conn))
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

        public static void SaveCalculationDetail(CalculationModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.GeneralSetting == null)
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

            try
            {
                DataTable dt = new DataTable("CalPriceDetail");
                dt.Columns.Add(new DataColumn("PriceDetailID", typeof(long)));
                dt.Columns.Add(new DataColumn("PriceID", typeof(long)));

                dt.Columns.Add(new DataColumn("ArtikelNrLAG", typeof(string)));
                dt.Columns.Add(new DataColumn("DokumentNrADR", typeof(string)));
                dt.Columns.Add(new DataColumn("Lieferant", typeof(string)));
                dt.Columns.Add(new DataColumn("Grundlage", typeof(string)));
                dt.Columns.Add(new DataColumn("Mitarbeiter", typeof(string)));

                dt.Columns.Add(new DataColumn("Quantity", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Bareinkaufspreis", typeof(decimal))); //BEK
                //dt.Columns.Add(new DataColumn("Bezugskosten", typeof(decimal))); //BZK
                //dt.Columns.Add(new DataColumn("BEN1", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN2", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN3", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN4", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN5", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN6", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN7", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN8", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN9", typeof(decimal)));
                //dt.Columns.Add(new DataColumn("BEN10", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Einstandspreis", typeof(decimal))); //ESTP
                //dt.Columns.Add(new DataColumn("Verwaltungsgemeinkosten", typeof(decimal))); //OGK
                //dt.Columns.Add(new DataColumn("Vertriebsgemeinkosten", typeof(decimal))); //VGK
                //dt.Columns.Add(new DataColumn("Sondereinzelkosten", typeof(decimal))); //VSK
                //dt.Columns.Add(new DataColumn("Verwaltungs", typeof(decimal))); //VVK
                dt.Columns.Add(new DataColumn("Selbstkosten1", typeof(decimal))); //SK1
                //dt.Columns.Add(new DataColumn("Lagerhaltungskosten", typeof(decimal))); //LHK
                //dt.Columns.Add(new DataColumn("Verpackungsanteil", typeof(decimal))); //VPA
                //dt.Columns.Add(new DataColumn("Transportanteil", typeof(decimal))); //TRA
                dt.Columns.Add(new DataColumn("Selbstkosten2", typeof(decimal))); //SK2
                dt.Columns.Add(new DataColumn("Gewinnaufschlag", typeof(decimal))); //GA
                dt.Columns.Add(new DataColumn("Barverkaufspreis", typeof(decimal))); //VK(bar)
                //dt.Columns.Add(new DataColumn("Kundenskonto", typeof(decimal))); //SKT
                //dt.Columns.Add(new DataColumn("Verhandlungsspielraum", typeof(decimal))); //PV
                dt.Columns.Add(new DataColumn("Zielverkaufspreis", typeof(decimal))); //VK(ziel)
                //dt.Columns.Add(new DataColumn("Kundenrabatt", typeof(decimal))); //RBT
                dt.Columns.Add(new DataColumn("Nettoverkaufspreis", typeof(decimal))); //VK(liste)
                //dt.Columns.Add(new DataColumn("Mehrwertsteuer", typeof(decimal))); //MWST
                dt.Columns.Add(new DataColumn("Bruttoverkaufspreis", typeof(decimal))); //VK(brutto)
                dt.Columns.Add(new DataColumn("Deckungsbeitrag", typeof(decimal)));

                //create details rows
                //basic note
                CalculationNoteModel basicNote = model.CalculationNotes.Where(item => item.ID == 0).FirstOrDefault();
                List<CalculationNoteModel> scaleNotes = model.CalculationNotes.Where(item => item.ID > 0).ToList();

                //SaveTable()

                foreach (CalculationNoteModel note in scaleNotes)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dr["PriceDetailID"] = 0;
                    dr["PriceID"] = model.ID;

                    dr["ArtikelNrLAG"] = model.ProffixModel.LAGDokumenteArtikelNrLAG;
                    dr["DokumentNrADR"] = model.ProffixModel.ADRDokumenteDokumentNrADR;
                    dr["Lieferant"] = model.GeneralSetting.Supplier;
                    dr["Grundlage"] = model.GeneralSetting.Info;
                    dr["Mitarbeiter"] = model.GeneralSetting.Employee;

                    dr["Quantity"] = RoundDown(note.Quantity, 4);

                    //basic
                    dr["Bareinkaufspreis"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "BEK").FirstOrDefault().Total, 4); //BEK
                    //dr["Bezugskosten"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "BZK").FirstOrDefault().Total, 4); //BZK

                    //dr["BEN1"] = RoundDown(GetBenItemTotal(basicNote, "BEN 1"), 4);
                    //dr["BEN2"] = RoundDown(GetBenItemTotal(basicNote, "BEN 2"), 4);
                    //dr["BEN3"] = RoundDown(GetBenItemTotal(basicNote, "BEN 3"), 4);
                    //dr["BEN4"] = RoundDown(GetBenItemTotal(basicNote, "BEN 4"), 4);
                    //dr["BEN5"] = RoundDown(GetBenItemTotal(basicNote, "BEN 5"), 4);
                    //dr["BEN6"] = RoundDown(GetBenItemTotal(basicNote, "BEN 6"), 4);
                    //dr["BEN7"] = RoundDown(GetBenItemTotal(basicNote, "BEN 7"), 4);
                    //dr["BEN8"] = RoundDown(GetBenItemTotal(basicNote, "BEN 8"), 4);
                    //dr["BEN9"] = RoundDown(GetBenItemTotal(basicNote, "BEN 9"), 4);
                    //dr["BEN10"] = RoundDown(GetBenItemTotal(basicNote, "BEN 10"), 4);

                    dr["Einstandspreis"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "ESTP").FirstOrDefault().Total, 4); //ESTP
                    //dr["Verwaltungsgemeinkosten"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "OGK").FirstOrDefault().Total, 4); //OGK
                    //dr["Vertriebsgemeinkosten"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "VGK").FirstOrDefault().Total, 4); //VGK
                    //dr["Sondereinzelkosten"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "VSK").FirstOrDefault().Total, 4); //VSK
                    //dr["Verwaltungs"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "VVK").FirstOrDefault().Total, 4); //VVK
                    dr["Selbstkosten1"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "SK 1").FirstOrDefault().Total, 4); //SK1
                    //dr["Lagerhaltungskosten"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "LHK").FirstOrDefault().Total, 4); //LHK
                    //dr["Verpackungsanteil"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "VPA").FirstOrDefault().Total, 4); //VPA
                    //dr["Transportanteil"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "TRA").FirstOrDefault().Total, 4); //TRA
                    dr["Selbstkosten2"] = RoundDown(basicNote.CalculationItems.Where(item => item.Tag == "SK 2").FirstOrDefault().Total, 4); //SK2

                    //scale
                    dr["Gewinnaufschlag"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "GA").FirstOrDefault().Total, 4); //GA
                    dr["Barverkaufspreis"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "VK(bar)").FirstOrDefault().Total, 4); //VK(bar)
                    //dr["Kundenskonto"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "SKT").FirstOrDefault().Total, 4); //SKT
                    //dr["Verhandlungsspielraum"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "PV").FirstOrDefault().Total, 4); //PV
                    dr["Zielverkaufspreis"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "VK(ziel)").FirstOrDefault().Total, 4); //VK(ziel)
                    //dr["Kundenrabatt"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "RBT").FirstOrDefault().Total, 4); //RBT
                    dr["Nettoverkaufspreis"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault().Total, 4); //VK(liste)
                    //dr["Mehrwertsteuer"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "MWST").FirstOrDefault().Total, 4); //MWST
                    dr["Bruttoverkaufspreis"] = RoundDown(note.CalculationItems.Where(item => item.Tag == "VK(brutto)").FirstOrDefault().Total, 4); //VK(brutto)
                    dr["Deckungsbeitrag"] = RoundDown(GetMarginSummarize(note), 4);
                }

                SaveTable(dt, dt.Columns["PriceDetailID"], new DataColumn[] { dt.Columns["PriceDetailID"] });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveProffix(CalculationModel model)
        {
            if (model.ProffixModel == null)
            {
                return;
            }

            //1. save LAG_Dokumente or ADR_Dokumente
            if (!String.IsNullOrWhiteSpace(model.ProffixModel.LAGDokumenteArtikelNrLAG))
            {
                SaveLAG_Dokumente(model);
            }
            else if (!String.IsNullOrWhiteSpace(model.ProffixModel.ADRDokumenteDokumentNrADR))
            {
                SaveADR_Dokumente(model);
            }


            //2. save LAG_Artikel
            if (model.GeneralSetting.PriceScale.Scale == 1 && model.GeneralSetting.Options.Contains("A"))
            {
                if (!String.IsNullOrWhiteSpace(model.ProffixModel.LAGDokumenteArtikelNrLAG))
                {
                    SaveLAG_Artikel(model);
                }
            }

            //3. save scale if needed (active)
            if (model.GeneralSetting.PriceScale.Scale > 1 && model.GeneralSetting.Options.Contains("A"))
            {
                //delete existing scale first                
                SavePRE_PreisStaffel(model);
            }
        }
    }
}
