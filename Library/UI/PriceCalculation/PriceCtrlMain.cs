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

            //if (_PriceCalculationSetting == null)
            //{
            //    ReloadPriceCalculationSetting(true);
            //}

            //if call from proffix, arguments should not null
            ProffixModel oProffix = new ProffixModel();
            oProffix.SetModel(arguments);

            //set general info
            generalCtrl1.SetProffixParam(oProffix, _PriceCalculationSetting.ProffixConnection);

            //load or copy
            if (oProffix.Command == Global.Commands.Open || oProffix.Command == Global.Commands.Copy)
            {
                //load cal setting from db
                CalculationModel oCal = StorageOperator.CalPriceLoadByID(Convert.ToInt64(oProffix.CalculationID));
                if (oCal != null)
                {
                    oCal.ProffixModel = oProffix;

                    if (oProffix.Command == Global.Commands.Copy)
                    {
                        oCal.ID = 0;
                    }

                    calculationTabPage.PageVisible = true;

                    generalCtrl1.LoadCalculation(oCal);

                    calculationBasicCtrl1.LoadCalculation(oCal, _PriceCalculationSetting);
                }
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
