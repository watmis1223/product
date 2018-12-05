﻿using ProductCalculation.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCalculation.Library.Storage
{
    static partial class StorageOperator
    {
        static string _ADR_Dokumente = "ADR_Dokumente";

        static void SaveADR_Dokumente(CalculationModel model)
        {
            if (model.ProffixModel.Command == Global.Commands.New ||
                model.ProffixModel.Command == Global.Commands.Copy)
            {
                InsertADR_Dokumente(model);
            }
            else
            {
                UpdateADR_Dokumente(model);
            }
        }

        static void UpdateADR_Dokumente(CalculationModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.ProffixModel == null)
            {
                return;
            }

            //save or update
            //string sCalculationID = model.ProffixModel.IsNew ? null : model.ID.ToString();
            //string sCalculationID = model.ID == 0 ? null : model.ID.ToString();
            //string sCalculationID = model.ProffixModel.IsNew ? null : model.ID.ToString();
            //ProffixADRDokumente oDokumente = GetProffixADRDokumente(
            //    model.ProffixModel.ADRDokumenteDokumentNrADR, model.ID.ToString(), model.ProffixConnection);

            ProffixADRDokumente oDokumente = GetADR_DokumenteByID(model.ProffixModel.ADRDokumenteLaufNr, model.ProffixConnection);

            if (oDokumente != null)
            {
                //update value 
                DataTable dt = new DataTable();
                dt.TableName = _LAG_Dokumente;
                dt.Columns.Add(new DataColumn("LaufNr", typeof(Int32)));
                dt.Columns.Add(new DataColumn("DateiName", typeof(string)));
                dt.Columns.Add(new DataColumn("Bezeichnung", typeof(string)));
                dt.Columns.Add(new DataColumn("Bemerkungen", typeof(string)));
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                dr["LaufNr"] = oDokumente.LaufNr;
                dr["DateiName"] = String.Concat(oDokumente.DateiName, " ", model.ID);

                //Kalk. VP 01.12.2017 Aktiv Oldenburg Kunststoff-Te
                string sBezeichnung = String.Concat(
                    "Kalk. ",
                    model.GeneralSetting.CostType == "S" ? "VP " : "EP ",
                    DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US")),
                    model.GeneralSetting.Options.Contains("A") ? " Aktiv" : " ",
                    model.GeneralSetting.Supplier);
                dr["Bezeichnung"] = sBezeichnung.Length > 100 ? sBezeichnung.Substring(0, 100) : sBezeichnung;
                dr["Bemerkungen"] = model.GeneralSetting.Remark;

                //update proffix
                UpdateRow(
                    dr,
                    dt.Columns["LaufNr"],
                    new List<DataColumn>() {
                                dt.Columns["DateiName"],
                                dt.Columns["Bezeichnung"],
                                dt.Columns["Bemerkungen"]
                    },
                    connectionString: model.ProffixConnection);
            }
        }

        static void InsertADR_Dokumente(CalculationModel model)
        {
            if (model.ID == 0)
            {
                return;
            }

            if (model.ProffixModel == null)
            {
                return;
            }

            CultureInfo oCulture = new CultureInfo("en-US");
            DateTime oNow = DateTime.Now;

            //get running no.
            model.ProffixModel.ADRDokumenteLaufNr = GetDocumentNumber(_ADR_Dokumente, model.ProffixConnection);

            DataTable dt = new DataTable();
            dt.TableName = _ADR_Dokumente;
            dt.Columns.Add(new DataColumn("LaufNr", typeof(Int32))); //40346 (running no.)
            dt.Columns.Add(new DataColumn("AdressNrADR", typeof(string))); //215048067
            dt.Columns.Add(new DataColumn("Bemerkungen", typeof(string))); //Erstellt eine neue Kalkulation
            dt.Columns.Add(new DataColumn("Bezeichnung", typeof(string))); //Kalk. VP 12/3/18 
            dt.Columns.Add(new DataColumn("DateiName", typeof(string))); //open 215048067 53576
            dt.Columns.Add(new DataColumn("Datum", typeof(string))); //2018-10-21 00:00:00.000
            dt.Columns.Add(new DataColumn("DokGruppe", typeof(string))); //Kalkulationen
            dt.Columns.Add(new DataColumn("DokumentNrADR", typeof(string))); //40346
            //dt.Columns.Add(new DataColumn("KontaktNrADR", typeof(Int32))); //NULL
            dt.Columns.Add(new DataColumn("Modul", typeof(string))); //C:\Program Files (x86)\PROFFIX\CalcModule\calc.exe            
            dt.Columns.Add(new DataColumn("ImportNr", typeof(string))); //0
            dt.Columns.Add(new DataColumn("ErstelltAm", typeof(string))); //2018-10-21 00:00:00.000
            dt.Columns.Add(new DataColumn("ErstelltVon", typeof(string))); //System (employee)
            //dt.Columns.Add(new DataColumn("GeaendertAm", typeof(string))); //NULL
            //dt.Columns.Add(new DataColumn("GeaendertVon", typeof(string))); //NULL
            dt.Columns.Add(new DataColumn("Geaendert", typeof(Int16))); //0
            dt.Columns.Add(new DataColumn("Exportiert", typeof(Int16))); //0

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr["LaufNr"] = model.ProffixModel.ADRDokumenteLaufNr;
            dr["AdressNrADR"] = model.ProffixModel.ADRDokumenteDokumentNrADR;
            dr["Bemerkungen"] = model.GeneralSetting.Remark;

            //Kalk. VP 01.12.2017 Aktiv Oldenburg Kunststoff-Te
            string sBezeichnung = String.Concat(
                "Kalk. ",
                model.GeneralSetting.CostType == "S" ? "VP " : "EP ",
                DateTime.Now.ToString("M/d/yy", oCulture),
                model.GeneralSetting.Options.Contains("A") ? " Aktiv" : " ",
                model.GeneralSetting.Supplier);
            dr["Bezeichnung"] = sBezeichnung.Length > 100 ? sBezeichnung.Substring(0, 100) : sBezeichnung;

            dr["DateiName"] = String.Format("open {0} {1}", model.ProffixModel.ADRDokumenteDokumentNrADR, model.ID);
            dr["Datum"] = oNow.ToString("yyyy-dd-MM 00:00:00.000", oCulture);
            dr["DokGruppe"] = "Kalkulationen";
            dr["DokumentNrADR"] = dr["LaufNr"];//String.Format("(select max([LaufNr]) + 1 from {0})", _ADR_Dokumente);
            //dr["KontaktNrADR"] = DBNull.Value;
            dr["Modul"] = model.ProffixModel.AppPath;
            dr["ImportNr"] = 0;
            dr["ErstelltAm"] = oNow.ToString("yyyy-dd-MM 00:00:00.000", oCulture);
            dr["ErstelltVon"] = model.GeneralSetting.Employee;
            //dr["GeaendertAm"] = DBNull.Value;
            //dr["GeaendertVon"] = DBNull.Value;
            dr["Geaendert"] = 0;
            dr["Exportiert"] = 0;

            InsertRowManualIncreaseID(dr, null, null, null, model.ProffixConnection);
        }

        public static ProffixADRDokumente GetADR_DokumenteByID(int documentID, string connectionString)
        {
            ProffixADRDokumente model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("Bemerkungen", typeof(string)),
                new DataColumn("DateiName", typeof(DateTime))
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("LaufNr", typeof(Int32));
            col.DefaultValue = documentID;
            oCondition[0] = col;

            DataTable dt = LoadTable(_ADR_Dokumente, oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixADRDokumente()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    Bemerkungen = dt.Rows[0]["Bemerkungen"].ToString(),
                    DateiName = dt.Rows[0]["DateiName"].ToString()
                };
            }

            return model;
        }
    }
}