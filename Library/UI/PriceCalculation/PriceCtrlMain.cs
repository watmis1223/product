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
        //public delegate void SettingSaveChangedCallback();
        //public event SettingSaveChangedCallback SettingSaveChanged;

        public delegate void CopyCallback(string copyCommand);
        public event CopyCallback Copy;

        PriceCalculationSetting _PriceCalculationSetting;

        List<ComboboxItemModel> _CalculationList = new List<ComboboxItemModel>();

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

            lstCalculation.DataSource = _CalculationList;
            copyCalculationTabPage.PageVisible = false;
        }

        public void ModuleSettingMode()
        {
            settingTabPage.PageVisible = true;
            generalTabPage.PageVisible = false;
            calculationTabPage.PageVisible = false;
            copyCalculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = settingTabPage;
        }

        public void ModuleCalculationMode()
        {
            settingTabPage.PageVisible = false;
            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = isCalculationTabVisible;
            copyCalculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = generalTabPage;
        }

        public void ModuleCopyCalculationMode()
        {
            mainTabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;

            settingTabPage.PageVisible = false;
            //generalTabPage.PageVisible = false;
            //calculationTabPage.PageVisible = false;
            copyCalculationTabPage.PageVisible = true;
            copyCalculationTabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            mainTabControl.SelectedTabPage = copyCalculationTabPage;

            //if (calculationBasicCtrl1 != null)
            //{
            //    calculationCopyCtrl1.SetCalculationModel(calculationBasicCtrl1.GetModel());                
            //}
        }

        void AddCalculationListItem(CalculationModel model)
        {
            //add to tree list
            if (model == null)
            {
                return;
            }

            _CalculationList.Add(new ComboboxItemModel()
            {
                Caption = model.GeneralSetting.Remark,
                Value = model.ID,
                Model = model
            });

            lstCalculation.Refresh();
        }

        public void ModuleCalculationByProffixMode(string[] arguments)
        {
            _Aarguments = arguments;

            settingTabPage.PageVisible = false;
            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = generalTabPage;

            if (_PriceCalculationSetting == null)
            {
                ReloadPriceCalculationSetting(true);
            }

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

        void ReloadPriceCalculationSetting(bool refresh)
        {
            //load module settings
            if (refresh)
            {
                _PriceCalculationSetting = ApplicationOperator.GetPriceCalculationSetting();
            }
        }

        private void generalCtrl1_NewButtonClick()
        {
            isCalculationTabVisible = true;
            calculationTabPage.PageVisible = true;
            mainTabControl.SelectedTabPage = calculationTabPage;

            //reload calculation control           
            calculationBasicCtrl1.NewCalculation(generalCtrl1.GetModel(), _PriceCalculationSetting);
            AddCalculationListItem(calculationBasicCtrl1.GetModel());
        }

        private void MainTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            generalTabPage.PageEnabled = true;
            calculationTabPage.PageEnabled = true;
            settingTabPage.PageEnabled = true;
            copyCalculationTabPage.PageEnabled = true;

            if (e.Page.Name == copyCalculationTabPage.Name)
            {
                settingTabPage.PageEnabled = false;
                generalTabPage.PageEnabled = false;
                calculationTabPage.PageEnabled = false;
                copyCalculationTabPage.PageEnabled = true;
            }
        }

        private void mainTabControl_CloseButtonClick(object sender, EventArgs e)
        {
            settingTabPage.PageVisible = settingTabPage.PageVisible;
            generalTabPage.PageVisible = generalTabPage.PageVisible;
            calculationTabPage.PageVisible = calculationTabPage.PageVisible;
            copyCalculationTabPage.PageVisible = false;

            mainTabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.Default;
        }

        private void calculationCopyCtrl1_SaveChanged()
        {
            _Aarguments = null;

            string copyCommand = String.Empty;

            //generate copy command
            CopyCalculationModel oCopyModel = calculationCopyCtrl1.GetModel();
            CalculationModel oCalcultionModel = calculationBasicCtrl1.GetModel();

            if (oCopyModel != null && oCalcultionModel != null)
            {
                if (!String.IsNullOrWhiteSpace(oCopyModel.AddressNo))
                {
                    copyCommand = String.Concat(copyCommand, "copya ", oCopyModel.AddressNo, " ", oCalcultionModel.ID, " ", oCopyModel.Scale);
                }
                else if (!String.IsNullOrWhiteSpace(oCopyModel.ProductNo))
                {
                    copyCommand = String.Concat(copyCommand, "copy ", oCopyModel.ProductNo, " ", oCalcultionModel.ID, " ", oCopyModel.Scale);
                }
            }

            if (Copy != null)
            {
                Copy(copyCommand);
            }
        }

        private void calculationBasicCtrl1_SaveChanged()
        {
            _Aarguments = null;
        }
    }
}
