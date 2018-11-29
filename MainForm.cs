using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ProductCalculation.Library.Entity;
using ProductCalculation.Library.Storage;
using ProductCalculation.Library.UI;
using ProductCalculation.Library.UI.PriceCalculation;
using ProductCalculation.Library.Global;

namespace ProductCalculation
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {        
        PriceCtrlMain _PriceModule = new PriceCtrlMain();
        string[] _Args;

        public MainForm(string[] arguments = null)
        {
            InitializeComponent();

            _Args = arguments;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            _PriceModule.Dock = DockStyle.Fill;

            bool isProffixLoad = false;
            if (_Args != null && _Args.Length > 0)
            {
                if (_Args[0].StartsWith("open"))
                {
                    isProffixLoad = true;
                    CallByProffix(new string[] { "", _Args[0] });
                }
            }

            if (!isProffixLoad)
            {
                brBtnPriceCalculation_ItemClick(this, null);
            }            
        }

        public void ShowModule(ApplicationModules module, params string[] arguments)
        {
            pnlMain.Controls.Clear();

            switch (module)
            {
                case ApplicationModules.PriceModuleCalculation:
                    pnlMain.Controls.Add(_PriceModule);
                    _PriceModule.ModuleCalculationMode();
                    break;
                case ApplicationModules.PriceModuleSetting:
                    pnlMain.Controls.Add(_PriceModule);
                    _PriceModule.ModuleSettingMode();
                    break;
                case ApplicationModules.PriceModuleCalculationByProffix:
                    pnlMain.Controls.Add(_PriceModule);
                    _PriceModule.ModuleCalculationByProffixMode(arguments);
                    break;

            }
        }

        public void CallByProffix(string[] arguments)
        {
            ShowModule(ApplicationModules.PriceModuleCalculationByProffix, arguments);
        }

        private void brBtnOil_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void brBtnPriceCalculation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCalculation);
        }

        private void brBtnPriceSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleSetting);
        }
    }
}