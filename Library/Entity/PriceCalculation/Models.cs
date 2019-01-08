﻿using ProductCalculation.Library.Entity.Setting.PriceCalculation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ProductCalculation.Library.Global;

namespace ProductCalculation.Library.Entity.PriceCalculation.Models
{
    //LAG_Dokumente
    public class ProffixLAGDokumente
    {
        public int LaufNr { get; set; } //unique no.
        public string ArtikelNrLAG { get; set; }
        //public int DokumentNrADR { get; set; }
        public string Bemerkungen { get; set; }
        public string DateiName { get; set; }
    }

    public class ProffixADRDokumente
    {
        public int LaufNr { get; set; } //unique no.
        public int AdressNrADR { get; set; }
        public int DokumentNrADR { get; set; }
        public string Bemerkungen { get; set; }
        public string DateiName { get; set; }
    }

    public class ProffixLAGArtikelModel
    {
        public int LaufNr { get; set; } //unique no.
        public string ArtikelNrLAG { get; set; }
        public string Bezeichnung1 { get; set; }
        public string Bezeichnung2 { get; set; }
        public string Bezeichnung3 { get; set; }
        public string Bezeichnung4 { get; set; }
        public string Bezeichnung5 { get; set; }
    }

    public class ProffixADRAdressenModel
    {
        public int LaufNr { get; set; } //unique no.
        public int AdressNrADR { get; set; } //line 1
        public string Name { get; set; } //line 2
        public string Vorname { get; set; } //line 3
        public string Adresszeile1 { get; set; } //line 4
        public string Adresszeile2 { get; set; } // line 5       
        public string LandPRO { get; set; } //line 6 (LandPRO + PLZ + Ort)
        public string PLZ { get; set; }
        public string Ort { get; set; }
    }

    public class ProffixLAGLieferantenModel
    {
        public int LaufNr { get; set; } //unique no.
        public string ArtikelNrLAG { get; set; }
        public string Name { get; set; }
    }

    public class ProffixPREPreisStaffelModel
    {
        public int LaufNr { get; set; } //unique no.
        public string ANummer { get; set; }
        public int ArtikelTyp { get; set; }
        public int AssortiertPRE { get; set; }

        public int KNummer { get; set; }
        public int KundenTyp { get; set; }

        public decimal MengeVon { get; set; }
        public string PreisTypPRE { get; set; }
        public int Prozent { get; set; }


        public string StaffelCode { get; set; }
        public int Verkauf { get; set; }
        public string WaehrungPRO { get; set; }
        public decimal Wert { get; set; }

        public int ImportNr { get; set; }


        public DateTime ErstelltAm { get; set; }
        public string ErstelltVon { get; set; }
        public DateTime GeaendertAm { get; set; }
        public string GeaendertVon { get; set; }


        public int Geaendert { get; set; }
        public string Exportiert { get; set; }

    }

    public class ProffixModel
    {
        //[ScriptIgnore]
        //public bool IsNew { get; set; }

        //[ScriptIgnore]
        //public bool IsLoad { get; set; }

        //[ScriptIgnore]
        //public bool IsCopy { get; set; }

        //keep LAG_Dokumente
        //if calcluation for Artikel
        public string LAGDokumenteArtikelNrLAG { get; set; }

        public int LAGDokumenteLaufNr { get; set; } // primary no.


        //keep ADR_Dokumente
        //if calculation for Dokumente
        public string ADRDokumenteDokumentNrADR { get; set; }

        public int ADRDokumenteLaufNr { get; set; } // primary no.

        //keep CalPrice
        public long CalculationID { get; set; }

        [ScriptIgnore]
        public Commands Command { get; set; }

        [ScriptIgnore]
        public int CopyScale { get; set; }


        [ScriptIgnore]
        public string AppPath { get; set; }

        public void SetModel(string[] arguments)
        {
            Command = Commands.New;

            //if call from proffix
            if (arguments != null && arguments.Length >= 2)
            {
                AppPath = arguments[0];

                //arguments[0] is application path, ignore it
                //arguments[1] is calculation command
                //opena 165442 53555 or open 165442 53555
                //53555 is calculation id
                string[] sSubParam = arguments[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (sSubParam != null && sSubParam.Length > 0)
                {

                    //open
                    if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("open") ||
                        sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("opena"))
                    {
                        //new
                        //IsNew = true;

                        if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("open"))
                        {
                            LAGDokumenteArtikelNrLAG = sSubParam[1];
                        }
                        else if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("opena"))
                        {
                            ADRDokumenteDokumentNrADR = sSubParam[1];
                        }

                        //Command = Commands.Open;
                        //load
                        //IsNew = false;
                        //IsLoad = true;
                        try
                        {
                            CalculationID = Convert.ToInt64(sSubParam[2]);
                        }
                        catch { }

                        if (CalculationID > 0)
                        {
                            Command = Commands.Open;
                        }
                    }
                    else if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("copy") ||
                            sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("copya"))
                    {
                        //copy
                        //IsCopy = true;
                        //IsNew = false;
                        //IsLoad = false;

                        if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("copy"))
                        {
                            LAGDokumenteArtikelNrLAG = sSubParam[1];
                        }
                        else if (sSubParam[0].TrimStart().TrimEnd().Trim().StartsWith("copya"))
                        {
                            ADRDokumenteDokumentNrADR = sSubParam[1];
                        }

                        try
                        {
                            CalculationID = Convert.ToInt64(sSubParam[2]);
                        }
                        catch { }

                        try
                        {
                            CopyScale = Convert.ToInt32(sSubParam[3]);
                        }
                        catch { }

                        Command = Commands.Copy;
                    }
                }
            }
        }
    }

    public class CopyCalculationModel
    {
        public string AddressNo { get; set; }
        public string ProductNo { get; set; }
        public int Scale { get; set; }
    }

    public class ComboboxItemModel
    {
        public long Value { get; set; }
        public string Caption { get; set; }
        public CalculationModel Model { get; set; }
        public override string ToString()
        {
            return Caption;
        }
    }

    #region GeneralSetting Model
    public class GeneralProductDesc
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line4 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line5 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line6 { get; set; }
    }
    public class GeneralPriceScale
    {
        private decimal _Scale;
        public decimal Scale
        {
            get { return _Scale; }
            set { _Scale = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarkUp { get; set; }

        private decimal _MinProfit;
        public decimal MinProfit
        {
            get { return _MinProfit; }
            set { _MinProfit = decimal.Round(value, 4); }
        }

        private decimal _MaxProfit;
        public decimal MaxProfit
        {
            get { return _MaxProfit; }
            set { _MaxProfit = decimal.Round(value, 4); }
        }
    }
    public class GeneralConvert
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        //ShopUnit
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShopUnit { get; set; }

        //SaleUnit
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SaleUnit { get; set; }

        //ShopUnit
        private decimal _EEUnitNumber;
        public decimal EEUnitNumber
        {
            get { return _EEUnitNumber; }
            set { _EEUnitNumber = decimal.Round(value, 4); }
        }

        //SaleUnit
        private decimal _VEUnitNumber;
        public decimal VEUnitNumber
        {
            get { return _VEUnitNumber; }
            set { _VEUnitNumber = decimal.Round(value, 4); }
        }

        private decimal _UnitNumber;
        public decimal UnitNumber
        {
            get { return _UnitNumber; }
            set { _UnitNumber = decimal.Round(value, 4); }
        }
    }
    public class GeneralCurrency
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        private decimal _Rate;
        public decimal Rate
        {
            get { return _Rate; }
            set { _Rate = decimal.Round(value, 4); }
        }
    }
    public class GeneralSettingModel
    {
        public long ID { get; set; }
        public string CostType { get; set; }
        public List<string> Options { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Employee { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreateDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralProductDesc ProductDesc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralPriceScale PriceScale { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralConvert Convert { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralCurrency Currency { get; set; }

        [ScriptIgnore]
        public List<string> TextLines { get; set; }

        [ScriptIgnore]
        public ProffixModel ProffixModel { get; set; }
    }
    #endregion

    #region Calculation
    public class CalculationItemModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sign { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }


        private decimal _AmountPercent;
        public decimal AmountPercent
        {
            get { return _AmountPercent; }
            set { _AmountPercent = decimal.Round(value, 4); }
        }

        private decimal _AmountFix;
        public decimal AmountFix
        {
            get { return _AmountFix; }
            set { _AmountFix = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CurrencyModel Currency { get; set; }

        //keep scale unit
        //EE = original, VE = convert
        //get convert number from GeneralUnit model
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ConvertModel Convert { get; set; }


        private decimal _Total;
        public decimal Total
        {
            get { return _Total; }
            set { _Total = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }
        public int Group { get; set; }
        public int ItemOrder { get; set; }
        public bool IsSummary { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> SummaryGroups { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BaseCalculationGroupRows { get; set; }

        //keep cost calculation
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CostCalculatonGroupModel CostCalculatonGroup { get; set; }

        //keep margin
        private decimal _VariableTotal;
        public decimal VariableTotal
        {
            get { return _VariableTotal; }
            set { _VariableTotal = decimal.Round(value, 4); }
        }

        //P percent, F fix, V variable total, T total
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EditedField { get; set; }

    }

    public class ConvertModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }

        private decimal _OriginalAmount;
        public decimal OriginalAmount
        {
            get { return _OriginalAmount; }
            set { _OriginalAmount = decimal.Round(value, 4); }
        }

        //[Description("P=Percent, F=Fix, T=Total, S=Special")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public String ConvertAmountField { get; set; }

        public override string ToString()
        {
            return Unit;
        }
    }

    public class CostCalculatonGroupModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> SummaryGroups { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BaseCalculationGroupRows { get; set; }
    }

    public class CurrencyModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        private decimal _OriginalAmount;
        public decimal OriginalAmount
        {
            get { return _OriginalAmount; }
            set { _OriginalAmount = decimal.Round(value, 4); }
        }

        //[Description("P=Percent, F=Fix, T=Total, S=Special")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public String CurrencyBaseAmountField { get; set; }
        public override string ToString()
        {
            return Currency;
        }
    }

    //main model
    public class CalculationModel
    {
        //database row id
        public long ID { get; set; }

        //from general's settings
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralSettingModel GeneralSetting { get; set; }

        ////from module's settings
        //[ScriptIgnore]
        //public PriceCalculationSetting ModuleSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationNoteModel> CalculationNotes { get; set; }

        //basic calculation tempt view, not serialize to json
        [ScriptIgnore]
        public List<CalculationItemModel> CalculationViewItems { get; set; }

        //keep margin
        [ScriptIgnore]
        public List<CalculationItemModel> CalculationMarginViewItems { get; set; }

        //keep Proffix
        public ProffixModel ProffixModel { get; set; }

        [ScriptIgnore]
        public string ProffixConnection { get; set; }

        public string CalculaionDateTime { get; set; }

        //[ScriptIgnore]
        //public bool IsCopy { get; set; }

        //[ScriptIgnore]
        //public int CopyScale { get; set; }

        public bool IsDelete { get; set; }

        public override string ToString()
        {
            return DateTime.Now.ToString();
        }
    }

    //calculation note
    public class CalculationNoteModel
    {
        //database ros id
        public long ID { get; set; }

        //basic calculation
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationItemModel> CalculationItems { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationItemModel> CalculationMarginItems { get; set; }

        //keep scale unit number
        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = decimal.Round(value, 4); }
        }
    }
    #endregion
}
