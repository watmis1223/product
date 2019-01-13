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
        static void SaveLAG_Artikel(CalculationModel model, decimal defaultValue = -1)
        {
            //save or update
            //update value 
            DataTable dt = new DataTable();
            dt.TableName = "LAG_Artikel";
            dt.Columns.Add(new DataColumn("ArtikelNrLAG", typeof(string)));
            dt.Columns.Add(new DataColumn("Verkauf5", typeof(float)));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //scale calculation note id start from 1
            dr["ArtikelNrLAG"] = model.ProffixModel.LAGDokumenteArtikelNrLAG;

            if (defaultValue > -1)
            {
                dr["Verkauf5"] = defaultValue;
            }
            else
            {
                dr["Verkauf5"] = model.CalculationNotes[1].CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault().Total;
            }

            //update proffix
            UpdateRow(
                dr,
                dt.Columns["ArtikelNrLAG"],
                new List<DataColumn>() {
                    dt.Columns["Verkauf5"]
                },
                connectionString: model.ProffixConnection);
        }

        static void SaveLAG_ArtikelBewertung(string ArtikelNrLAG, decimal amout, string connectionString)
        {
            //save or update
            //update value 
            DataTable dt = new DataTable();
            dt.TableName = "LAG_Artikel";
            dt.Columns.Add(new DataColumn("ArtikelNrLAG", typeof(string)));
            dt.Columns.Add(new DataColumn("BewBasis", typeof(Int16)));
            dt.Columns.Add(new DataColumn("Bewertung", typeof(float)));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //scale calculation note id start from 1
            dr["ArtikelNrLAG"] = ArtikelNrLAG;
            dr["BewBasis"] = 6; //Kalkulationspreis
            dr["Bewertung"] = amout;

            //update proffix
            UpdateRow(
                dr,
                dt.Columns["ArtikelNrLAG"],
                new List<DataColumn>() {
                    dt.Columns["BewBasis"],
                    dt.Columns["Bewertung"]
                },
                connectionString: connectionString);
        }
    }
}
