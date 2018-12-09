using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Xml;
using ProductCalculation.Library.Storage;
using ProductCalculation.Library.Entity.Setting;
using System.Globalization;
using ProductCalculation.Library.Entity.Setting.PriceCalculation;
using ProductCalculation.Library.Entity.PriceCalculation.Models;

namespace ProductCalculation.Library.UI.PriceCalculation
{
    public partial class PriceCtrlMain : DevExpress.XtraEditors.XtraUserControl
    {
        //public delegate void CopyCallback(string copyCommand);
        //public event CopyCallback Copy;

        public delegate void SavedCallback(string message);
        public event SavedCallback Saved;

        public delegate void OpenedCalculationCallback();
        public event OpenedCalculationCallback OpenedCalculation;

        PriceCalculationSetting _PriceCalculationSetting;

        bool isCalculationTabVisible = false;

        string[] _Aarguments;

        public PriceCtrlMain()
        {
            InitializeComponent();
        }

        private void PriceCtrlMain_Load(object sender, EventArgs e)
        {
            mainTabControl.SelectedTabPage = generalTabPage;

            ReloadPriceCalculationSetting(true);

            //set text line to general control
            generalCtrl1.SetTextLine(_PriceCalculationSetting.TextSetting);
        }

        public CalculationModel GetCalculationModel()
        {
            return calculationBasicCtrl1.GetModel();
        }

        public void NewCalculation()
        {
            if (generalCtrl1 != null)
            {
                generalCtrl1.NewCalculation();
            }
        }

        public void SaveCalculation()
        {
            if (generalCtrl1 != null)
            {
                generalCtrl1.UpdateModel();
            }

            if (calculationBasicCtrl1 != null)
            {

                calculationBasicCtrl1.SaveModel(generalCtrl1.GetModel());
            }
        }

        public void ModuleCalculationMode()
        {
            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = isCalculationTabVisible;
            mainTabControl.SelectedTabPage = generalTabPage;
        }

        public void ModuleCalculationByProffixMode(string[] arguments)
        {
            _Aarguments = arguments;

            //reload settings
            ReloadPriceCalculationSetting(true);

            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = isCalculationTabVisible ? calculationTabPage : generalTabPage;


            //if call from proffix, arguments should not null
            ProffixModel oProffix = new ProffixModel();
            oProffix.SetModel(arguments);

            //string[] sSubParam = arguments[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //MessageBox.Show(String.Format("{0} {1} {2} {3}",
            //sSubParam[0],
            //sSubParam[1],
            //sSubParam[2],
            //oProffix.Command.ToString()
            //));

            //MessageBox.Show(String.Format("{0}",            
            //arguments[1]
            //));

            //load cal setting from db
            CalculationModel oCal = StorageOperator.CalPriceLoadByID(Convert.ToInt64(oProffix.CalculationID));

            //set general info
            if (oCal != null)
            {
                if (oProffix.Command == Global.Commands.Copy)
                {
                    oCal.ProffixModel.CalculationID = oCal.ID;
                    oCal.ID = 0;
                    oCal.ProffixModel.ADRDokumenteLaufNr = 0;
                    oCal.ProffixModel.LAGDokumenteLaufNr = 0;
                    oCal.ProffixModel.ADRDokumenteDokumentNrADR = oProffix.ADRDokumenteDokumentNrADR;
                    oCal.ProffixModel.LAGDokumenteArtikelNrLAG = oProffix.LAGDokumenteArtikelNrLAG;
                    oCal.ProffixModel.CopyScale = oProffix.CopyScale;
                    oCal.ProffixModel.Command = Global.Commands.Copy;
                    oCal.ProffixModel.AppPath = oProffix.AppPath;
                }
                else if (oProffix.Command == Global.Commands.Open)
                {
                    oCal.ProffixModel.CalculationID = oCal.ID;
                    oCal.ProffixModel.Command = Global.Commands.Open;
                }

                //MessageBox.Show(String.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                //oCal.ID,
                //oCal.ProffixModel.CalculationID,
                //oCal.ProffixModel.ADRDokumenteLaufNr,
                //oCal.ProffixModel.LAGDokumenteLaufNr,
                //oCal.ProffixModel.ADRDokumenteDokumentNrADR,
                //oCal.ProffixModel.LAGDokumenteArtikelNrLAG,
                //oCal.ProffixModel.CopyScale,
                //oCal.ProffixModel.Command.ToString()
                //));

                generalCtrl1.SetProffixParam(oCal.ProffixModel, _PriceCalculationSetting.ProffixConnection);

                //load or copy
                if (oProffix.Command == Global.Commands.Open || oProffix.Command == Global.Commands.Copy)
                {
                    calculationTabPage.PageVisible = true;

                    generalCtrl1.LoadCalculation(oCal);

                    calculationBasicCtrl1.LoadCalculation(oCal, _PriceCalculationSetting);
                }
            }
            else
            {
                generalCtrl1.SetProffixParam(oProffix, _PriceCalculationSetting.ProffixConnection);
            }
        }

        public void ReloadPriceCalculationSetting(bool refresh)
        {
            //load module settings
            if (refresh)
            {
                if (!isCalculationTabVisible)
                {
                    _PriceCalculationSetting = ApplicationOperator.GetPriceCalculationSetting();
                }
            }
        }

        private void generalCtrl1_NewButtonClick()
        {
            isCalculationTabVisible = true;
            calculationTabPage.PageVisible = true;
            mainTabControl.SelectedTabPage = calculationTabPage;

            //reload calculation control           
            calculationBasicCtrl1.NewCalculation(generalCtrl1.GetModel(), _PriceCalculationSetting);

            if (OpenedCalculation != null)
            {
                OpenedCalculation();
            }
        }

        public string GetCopyCommand(CopyCalculationModel copyModel)
        {
            _Aarguments = null;

            string copyCommand = String.Empty;

            //generate copy command            
            CalculationModel oCalcultionModel = calculationBasicCtrl1.GetModel();

            if (copyModel != null && oCalcultionModel != null)
            {
                if (!String.IsNullOrWhiteSpace(copyModel.AddressNo))
                {
                    copyCommand = String.Concat(copyCommand, "copya ", copyModel.AddressNo, " ", oCalcultionModel.ID, " ", copyModel.Scale);
                }
                else if (!String.IsNullOrWhiteSpace(copyModel.ProductNo))
                {
                    copyCommand = String.Concat(copyCommand, "copy ", copyModel.ProductNo, " ", oCalcultionModel.ID, " ", copyModel.Scale);
                }
            }

            return copyCommand;
        }

        private void CalculationBasicCtrl1_SaveChanged(string message)
        {
            _Aarguments = null;

            if (Saved != null)
            {
                Saved(message);
            }
        }
    }
}
