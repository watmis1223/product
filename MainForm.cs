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
using System.Diagnostics;

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
            _PriceModule.Copy += _PriceModule_Copy;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ribbonPageGroup2.Visible = false;

            _PriceModule.Dock = DockStyle.Fill;

            CallByProffix(_Args);            
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
                case ApplicationModules.PriceModuleCopyCalculation:
                    pnlMain.Controls.Add(_PriceModule);
                    _PriceModule.ModuleCopyCalculationMode();
                    break;

            }
        }
        public void CallByProffix(string[] arguments)
        {
            ribbonPageGroup2.Visible = false;

            _Args = arguments;

            bool isProffixLoad = false;
            if (_Args != null && _Args.Length >= 2)
            {
                if (_Args[1].StartsWith("open"))
                {
                    isProffixLoad = true;

                    if (_Args[1].Split(new string[] { " "}, StringSplitOptions.RemoveEmptyEntries).Length > 2)
                    {                        
                        ribbonPageGroup2.Visible = true;
                    }                    
                }
                else if (_Args[1].StartsWith("copy"))
                {
                    isProffixLoad = true;
                    ribbonPageGroup2.Visible = false;
                }

                ShowModule(ApplicationModules.PriceModuleCalculationByProffix, new string[] { _Args[0], _Args[1] });
            }

            if (!isProffixLoad)
            {
                brBtnPriceCalculation_ItemClick(this, null);
            }           
        }

        private void brBtnOil_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void brBtnPriceCalculation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ShowModule(ApplicationModules.PriceModuleCalculation);
            ShowModule(ApplicationModules.PriceModuleCalculationByProffix, new string[] { _Args[0], _Args[1] });
        }

        private void brBtnPriceSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleSetting);
        }

        private void brBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCopyCalculation);
        }

        private void _PriceModule_Copy(string copyCommand)
        {
            if (!String.IsNullOrWhiteSpace(copyCommand) && _Args != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _Args[0];
                startInfo.Arguments = copyCommand;
                Process.Start(startInfo);
            }
        }
    }
}