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
        public static int GetDocumentNumber(string documentTableName, string connectionString)
        {
            int iNumber = 0;

            StringBuilder queryValues = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ///building columns
                queryValues.Append("update [LaufNummern] ");
                queryValues.AppendFormat("set [LaufNr] = [LaufNr] +1 ");
                queryValues.AppendFormat("where [Tabelle] = '{0}';", documentTableName);
                queryValues.AppendFormat(" SELECT LaufNr from [LaufNummern] where [Tabelle] = '{0}';", documentTableName);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();
                cmd.CommandText = queryValues.ToString();
                iNumber = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
                cmd.Dispose();
            }

            return iNumber;
        }

        public static ProffixLAGArtikelModel GetProffixLAGArtikelModel(string artikelNrLAG, string connectionString)
        {
            ProffixLAGArtikelModel model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("Bezeichnung1", typeof(string)),
                new DataColumn("Bezeichnung2", typeof(string)),
                new DataColumn("Bezeichnung3", typeof(string)),
                new DataColumn("Bezeichnung4", typeof(string)),
                new DataColumn("Bezeichnung5", typeof(string)),
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("LAG_Artikel", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixLAGArtikelModel()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    ArtikelNrLAG = dt.Rows[0]["ArtikelNrLAG"].ToString(),
                    Bezeichnung1 = dt.Rows[0]["Bezeichnung1"].ToString(),
                    Bezeichnung2 = dt.Rows[0]["Bezeichnung2"].ToString(),
                    Bezeichnung3 = dt.Rows[0]["Bezeichnung3"].ToString(),
                    Bezeichnung4 = dt.Rows[0]["Bezeichnung4"].ToString(),
                    Bezeichnung5 = dt.Rows[0]["Bezeichnung5"].ToString(),
                };
            }

            return model;
        }

        public static ProffixADRAdressenModel GetProffixADRAdressenModel(string adressNrADR, string connectionString)
        {
            ProffixADRAdressenModel model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("AdressNrADR", typeof(Int32)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Vorname", typeof(string)),
                new DataColumn("Adresszeile1", typeof(string)),
                new DataColumn("Adresszeile2", typeof(string)),
                new DataColumn("LandPRO", typeof(string)),
                new DataColumn("PLZ", typeof(string)),
                new DataColumn("Ort", typeof(string)),
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("AdressNrADR", typeof(Int32));

            try
            {
                col.DefaultValue = adressNrADR;
            }
            catch
            {
                col.DefaultValue = 0;
            }

            oCondition[0] = col;

            DataTable dt = LoadTable("ADR_Adressen", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixADRAdressenModel()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    AdressNrADR = Convert.ToInt32(dt.Rows[0]["AdressNrADR"].ToString()),
                    Name = dt.Rows[0]["Name"] != null ? dt.Rows[0]["Name"].ToString() : "-",
                    Vorname = dt.Rows[0]["Vorname"] != null ? dt.Rows[0]["Vorname"].ToString() : "-",
                    Adresszeile1 = dt.Rows[0]["Adresszeile1"] != null ? dt.Rows[0]["Adresszeile1"].ToString() : "-",
                    Adresszeile2 = dt.Rows[0]["Adresszeile2"] != null ? dt.Rows[0]["Adresszeile2"].ToString() : "-",
                    LandPRO = dt.Rows[0]["LandPRO"] != null ? dt.Rows[0]["LandPRO"].ToString() : String.Empty,
                    PLZ = dt.Rows[0]["PLZ"] != null ? dt.Rows[0]["PLZ"].ToString() : String.Empty,
                    Ort = dt.Rows[0]["Ort"] != null ? dt.Rows[0]["Ort"].ToString() : String.Empty,
                };
            }

            return model;
        }

        public static List<ProffixADRAdressenModel> GetProffixADRAdressenList(string name, string connectionString)
        {
            List<ProffixADRAdressenModel> model = new List<ProffixADRAdressenModel>();

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("AdressNrADR", typeof(Int32)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Vorname", typeof(string)),
                new DataColumn("Adresszeile1", typeof(string)),
                new DataColumn("Adresszeile2", typeof(string)),
                new DataColumn("LandPRO", typeof(string)),
                new DataColumn("PLZ", typeof(string)),
                new DataColumn("Ort", typeof(string)),
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("SuchIndex", typeof(String));

            try
            {
                col.DefaultValue = String.Format("%{0}%", name);
            }
            catch
            {
                col.DefaultValue = "%%";
            }

            oCondition[0] = col;

            DataTable dt = LoadTable("ADR_Adressen", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProffixADRAdressenModel oModel = new ProffixADRAdressenModel()
                    {
                        LaufNr = Convert.ToInt32(dr["LaufNr"]),
                        AdressNrADR = Convert.ToInt32(dr["AdressNrADR"].ToString()),
                        Name = dr["Name"] != null ? dr["Name"].ToString() : "-",
                        Vorname = dr["Vorname"] != null ? dr["Vorname"].ToString() : "-",
                        Adresszeile1 = dr["Adresszeile1"] != null ? dr["Adresszeile1"].ToString() : "-",
                        Adresszeile2 = dr["Adresszeile2"] != null ? dr["Adresszeile2"].ToString() : "-",
                        LandPRO = dr["LandPRO"] != null ? dr["LandPRO"].ToString() : String.Empty,
                        PLZ = dr["PLZ"] != null ? dr["PLZ"].ToString() : String.Empty,
                        Ort = dr["Ort"] != null ? dr["Ort"].ToString() : String.Empty,
                    };

                    model.Add(oModel);
                }
            }

            return model;
        }

        public static List<ProffixLAGArtikelModel> GetProffixLAGArtikelList(string name, string connectionString)
        {
            List<ProffixLAGArtikelModel> model = new List<ProffixLAGArtikelModel>();

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("Bezeichnung1", typeof(string)),
                new DataColumn("SuchIndex", typeof(string)),
                new DataColumn("SuchIndexShop", typeof(string))
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("SuchIndexShop", typeof(string));

            try
            {
                col.DefaultValue = String.Format("%{0}%", name);
            }
            catch
            {
                col.DefaultValue = "%%";
            }

            oCondition[0] = col;

            DataTable dt = LoadTable("LAG_Artikel", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProffixLAGArtikelModel oModel = new ProffixLAGArtikelModel()
                    {
                        LaufNr = Convert.ToInt32(dr["LaufNr"]),
                        ArtikelNrLAG = dr["ArtikelNrLAG"] != null ? dr["ArtikelNrLAG"].ToString() : "-",
                        Bezeichnung1 = dr["Bezeichnung1"] != null ? dr["Bezeichnung1"].ToString() : "-",
                        //Vorname = dr["Vorname"] != null ? dr["Vorname"].ToString() : "-",
                        //Adresszeile1 = dr["Adresszeile1"] != null ? dr["Adresszeile1"].ToString() : "-",
                        //Adresszeile2 = dr["Adresszeile2"] != null ? dr["Adresszeile2"].ToString() : "-",
                        //LandPRO = dr["LandPRO"] != null ? dr["LandPRO"].ToString() : String.Empty,
                        //PLZ = dr["PLZ"] != null ? dr["PLZ"].ToString() : String.Empty,
                        //Ort = dr["Ort"] != null ? dr["Ort"].ToString() : String.Empty,
                    };

                    model.Add(oModel);
                }
            }

            return model;
        }

        public static List<ProffixLAGLieferantenModel> GetProffixLAGLieferantenModelList(string artikelNrLAG, string connectionString)
        {
            List<ProffixLAGLieferantenModel> modelList = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("[Name]", typeof(string))
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("LAG_Lieferanten", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                modelList = new List<ProffixLAGLieferantenModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    modelList.Add(new ProffixLAGLieferantenModel()
                    {
                        LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                        ArtikelNrLAG = dr["ArtikelNrLAG"].ToString(),
                        Name = dr["Name"].ToString()
                    });
                }
            }

            return modelList;
        }

        public static ProffixLAGDokumente GetProffixLAGDokumente(string artikelNrLAG, string calculationID, string connectionString)
        {
            ProffixLAGDokumente model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("Bemerkungen", typeof(string)),
                new DataColumn("DateiName", typeof(DateTime))
            };

            DataColumn[] oCondition = new DataColumn[2];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            col = new DataColumn("rtrim(ltrim([DateiName]))", typeof(string));
            col.DefaultValue = String.Concat("open", "%", artikelNrLAG,
                String.IsNullOrWhiteSpace(calculationID) ? "" : String.Concat(" ", calculationID));
            oCondition[1] = col;

            DataTable dt = LoadTable("LAG_Dokumente", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixLAGDokumente()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    ArtikelNrLAG = dt.Rows[0]["ArtikelNrLAG"].ToString(),
                    Bemerkungen = dt.Rows[0]["Bemerkungen"].ToString(),
                    DateiName = dt.Rows[0]["DateiName"].ToString()
                };
            }

            return model;
        }

        public static ProffixADRDokumente GetProffixADRDokumente(string adressNrADR, string calculationID, string connectionString)
        {
            ProffixADRDokumente model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("AdressNrADR", typeof(Int32)),
                new DataColumn("DokumentNrADR", typeof(Int32)),
                new DataColumn("Bemerkungen", typeof(string)),
                new DataColumn("DateiName", typeof(DateTime))
            };

            DataColumn[] oCondition = new DataColumn[2];
            DataColumn col = new DataColumn("AdressNrADR", typeof(Int32));
            col.DefaultValue = adressNrADR;
            oCondition[0] = col;

            col = new DataColumn("rtrim(ltrim([DateiName]))", typeof(string));
            col.DefaultValue = String.Concat("opena", "%", adressNrADR,
                String.IsNullOrWhiteSpace(calculationID) ? "" : String.Concat(" ", calculationID));
            oCondition[1] = col;

            DataTable dt = LoadTable("ADR_Dokumente", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixADRDokumente()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    AdressNrADR = Convert.ToInt32(dt.Rows[0]["AdressNrADR"].ToString()),
                    DokumentNrADR = Convert.ToInt32(dt.Rows[0]["DokumentNrADR"].ToString()),
                    Bemerkungen = dt.Rows[0]["Bemerkungen"].ToString(),
                    DateiName = dt.Rows[0]["DateiName"].ToString()
                };
            }

            return model;
        }

        public static List<ProffixPREPreisStaffelModel> GetProffixPREPreisStaffelModelList(string artikelNrLAG, string connectionString)
        {
            List<ProffixPREPreisStaffelModel> modelList = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)), //id
                new DataColumn("ANummer", typeof(string)), //artikel
                new DataColumn("ArtikelTyp", typeof(Int16)), //1
                new DataColumn("AssortiertPRE", typeof(Int32)), //0

                new DataColumn("KNummer", typeof(Int32)), //0
                new DataColumn("KundenTyp", typeof(Int16)), //0

                new DataColumn("MengeVon", typeof(float)), //scale
                new DataColumn("PreisTypPRE", typeof(string)), //ART
                new DataColumn("Prozent", typeof(Int16)),

                new DataColumn("StaffelCode", typeof(string)), //10.0.1.221100200.CHF
                new DataColumn("Verkauf", typeof(Int16)), //1
                new DataColumn("WaehrungPRO", typeof(string)), //CHF
                new DataColumn("Wert", typeof(float)), //amount
                
                new DataColumn("ImportNr", typeof(Int32)), //0

                new DataColumn("ErstelltAm", typeof(DateTime)),
                new DataColumn("ErstelltVon", typeof(string)), //cs
                new DataColumn("GeaendertAm", typeof(DateTime)),
                new DataColumn("GeaendertVon", typeof(string)), //cs

                new DataColumn("Geaendert", typeof(Int16)), //1
                new DataColumn("Exportiert", typeof(Int16)), //0
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ANummer", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("PRE_PreisStaffel", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                modelList = new List<ProffixPREPreisStaffelModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    modelList.Add(new ProffixPREPreisStaffelModel()
                    {
                        LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                        ANummer = dr["ArtikelNrLAG"].ToString(),
                        ArtikelTyp = Convert.ToInt32(dr["ArtikelTyp"]),
                        AssortiertPRE = Convert.ToInt32(dr["AssortiertPRE"]),

                        KNummer = Convert.ToInt32(dr["KNummer"]),
                        KundenTyp = Convert.ToInt32(dr["KundenTyp"]),

                        MengeVon = Convert.ToDecimal(dr["MengeVon"]),
                        PreisTypPRE = dr["PreisTypPRE"].ToString(),
                        Prozent = Convert.ToInt32(dr["Prozent"]),

                        StaffelCode = dr["StaffelCode"].ToString(),
                        Verkauf = Convert.ToInt32(dr["Verkauf"]),
                        WaehrungPRO = dr["WaehrungPRO"].ToString(),
                        Wert = Convert.ToDecimal(dr["Wert"]),

                        ImportNr = Convert.ToInt32(dr["ImportNr"]),

                        ErstelltAm = Convert.ToDateTime(dr["ErstelltAm"]),
                        ErstelltVon = dr["ErstelltVon"].ToString(),
                        GeaendertAm = Convert.ToDateTime(dr["GeaendertAm"]),
                        GeaendertVon = dr["GeaendertVon"].ToString(),

                        Geaendert = Convert.ToInt32(dr["Geaendert"]),
                        Exportiert = dr["Exportiert"].ToString(),
                    });
                }
            }

            return modelList;
        }

        public static DataTable LoadProffixLAGArtikel(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ///building columns
                query.Append("select [ArtikelNrLAG] from [LAG_Artikel] ");
                query.Append("where ArtikelNrLAG not in ( ");
                query.Append("select [ArtikelNrLAG] from [LAG_Dokumente] ");
                query.AppendFormat("where ErstelltVon = '{0}'", "KalApp");
                query.Append("group by ArtikelNrLAG");
                query.Append(")");

                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(query.ToString(), connection);
                dt = new DataTable();
                da.Fill(dt);
                connection.Close();
            }

            //DataColumn[] oSelect = {
            //    //new DataColumn("LaufNr", typeof(Int32)),
            //    new DataColumn("ArtikelNrLAG", typeof(string)),
            //};

            //DataTable dt = LoadTable("LAG_Artikel", oSelect, null, null, connectionString: connectionString);

            return dt;
        }

        public static DataTable LoadProffixADRAdressen(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ///building columns
                query.Append("select AdressNrADR from [ADR_Adressen] ");
                query.Append("where AdressNrADR not in ( ");
                query.Append("select [AdressNrADR] from [dbo].[ADR_Dokumente] ");
                query.AppendFormat("where ErstelltVon = '{0}'", "KalApp");
                query.Append("group by [AdressNrADR]");
                query.Append(")");

                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(query.ToString(), connection);
                dt = new DataTable();
                da.Fill(dt);
                connection.Close();
            }

            //DataColumn[] oSelect = {
            //    //new DataColumn("LaufNr", typeof(Int32)),
            //    new DataColumn("AdressNrADR", typeof(Int32)),
            //};

            //DataTable dt = LoadTable("ADR_Adressen", oSelect, null, null, connectionString: connectionString);

            return dt;
        }
    }
}
