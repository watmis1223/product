using ProductCalculation.Library.Entity.PriceCalculation.Models;
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
            //save or update
            //string sCalculationID = model.ProffixModel.IsNew ? null : model.ID.ToString();
            //string sCalculationID = model.ID == 0 ? null : model.ID.ToString();
            //string sCalculationID = model.ProffixModel.IsNew ? null : model.ID.ToString();
            ProffixADRDokumente oDokumente = GetProffixADRDokumente(
                model.ProffixModel.ADRDokumenteDokumentNrADR, model.ID.ToString(), model.ProffixConnection);

            if (oDokumente != null)
            {
                //update value 
                DataTable dt = new DataTable();
                dt.TableName = _LAG_Dokumente;
                dt.Columns.Add(new DataColumn("LaufNr", typeof(string)));
                dt.Columns.Add(new DataColumn("DateiName", typeof(string)));
                dt.Columns.Add(new DataColumn("Bezeichnung", typeof(string)));
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

                //update proffix
                UpdateRow(
                    dr,
                    dt.Columns["LaufNr"],
                    new List<DataColumn>() {
                                dt.Columns["DateiName"],
                                dt.Columns["Bezeichnung"]
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

            CultureInfo oCulture = new CultureInfo("en-US");
            DateTime oNow = DateTime.Now;

            //insert new document
            string sCalculationID = model.ID.ToString();

            DataTable dt = new DataTable();
            dt.TableName = _ADR_Dokumente;
            dt.Columns.Add(new DataColumn("LaufNr", typeof(Int32))); //40346 (running no.)
            dt.Columns.Add(new DataColumn("AdressNrADRmodel", typeof(string))); //215048067
            dt.Columns.Add(new DataColumn("Bemerkungenmodel", typeof(string))); //Erstellt eine neue Kalkulation
            dt.Columns.Add(new DataColumn("Bezeichnungmodel", typeof(string))); //Kalk. VP 12/3/18 
            dt.Columns.Add(new DataColumn("DateiNamemodel", typeof(string))); //open 215048067 53576
            dt.Columns.Add(new DataColumn("Datummodel", typeof(string))); //2018-10-21 00:00:00.000
            dt.Columns.Add(new DataColumn("DokGruppemodel", typeof(string))); //Kalkulationen
            dt.Columns.Add(new DataColumn("DokumentNrADRmodel", typeof(Int32))); //40346
            dt.Columns.Add(new DataColumn("KontaktNrADRmodel", typeof(string))); //NULL
            dt.Columns.Add(new DataColumn("Modulmodel", typeof(Int16))); //C:\Program Files (x86)\PROFFIX\CalcModule\calc.exe            
            dt.Columns.Add(new DataColumn("ImportNrmodel", typeof(string))); //0
            dt.Columns.Add(new DataColumn("ErstelltAmmodel", typeof(string))); //2018-10-21 00:00:00.000
            dt.Columns.Add(new DataColumn("ErstelltVonmodel", typeof(string))); //System (employee)
            dt.Columns.Add(new DataColumn("GeaendertAmmodel", typeof(string))); //NULL
            dt.Columns.Add(new DataColumn("GeaendertVonmodel", typeof(string))); //NULL
            dt.Columns.Add(new DataColumn("Geaendertmodel", typeof(string))); //0
            dt.Columns.Add(new DataColumn("Exportiertmodel", typeof(string))); //0

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr["LaufNr"] = 0;
            dr["AdressNrADR"] = model.ProffixModel.ADRDokumenteDokumentNrADR;
            dr["Bemerkungen"] = model.GeneralSetting.Remark;

            //Kalk. VP 01.12.2017 Aktiv Oldenburg Kunststoff-Te
            string sBezeichnung = String.Concat(
                "Kalk. ",
                model.GeneralSetting.CostType == "S" ? "VP " : "EP ",
                DateTime.Now.ToString("dd/MM/yy", oCulture),
                model.GeneralSetting.Options.Contains("A") ? " Aktiv" : " ",
                model.GeneralSetting.Supplier);
            dr["Bezeichnung"] = sBezeichnung.Length > 100 ? sBezeichnung.Substring(0, 100) : sBezeichnung;

            dr["DateiName"] = String.Format("open {0} {1}", model.ProffixModel.ADRDokumenteDokumentNrADR, model.ID);
            dr["Datum"] = oNow.ToString("yyyy-MM-dd 00:00:00.000", oCulture);
            dr["DokGruppe"] = "Kalkulationen";
            dr["DokumentNrADR"] = String.Format("(select max([LaufNr]) + 1 from {0})", _ADR_Dokumente);
            dr["KontaktNrADR"] = null;
            dr["Modul"] = model.ProffixModel.AppPath;
            dr["ImportNr"] = 0;
            dr["ErstelltAm"] = oNow.ToString("yyyy-MM-dd 00:00:00.000", oCulture);
            dr["ErstelltVon"] = model.GeneralSetting.Employee;
            dr["GeaendertAm"] = null;
            dr["GeaendertVon"] = null;
            dr["Geaendert"] = 0;
            dr["Exportiert"] = 0;

            InsertRowManualIncreaseID(dr, dt.Columns["[LaufNr]"], null, model.ProffixConnection);
        }
    }
}
