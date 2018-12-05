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
        CalculationCopyCtrl _CopyModule = new CalculationCopyCtrl();
        SettingCtrl _SettingModule = new SettingCtrl();

        //argument form prffix
        string[] _Args;

        public MainForm(string[] arguments = null)
        {
            InitializeComponent();

            _Args = arguments;

            //_PriceModule
            //_PriceModule.Copy += _PriceModule_Copy;
            _PriceModule.Saved += _PriceModule_Saved;
            _PriceModule.OpenedCalculation += _PriceModule_OpenedCalculation;

            //_SettingModule
            _SettingModule.SaveChanged += _SettingModule_SaveChanged;

            //_CopyModule
            _CopyModule.SaveChanged += _CopyModule_SaveChanged;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _PriceModule.Dock = DockStyle.Fill;
            _CopyModule.Dock = DockStyle.Fill;
            _SettingModule.Dock = DockStyle.Fill;

            brBtnCopy.Enabled = false;

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
                    pnlMain.Controls.Add(_SettingModule);
                    //_PriceModule.ModuleSettingMode();
                    break;
                case ApplicationModules.PriceModuleCalculationByProffix:
                    pnlMain.Controls.Add(_PriceModule);
                    _PriceModule.ModuleCalculationByProffixMode(arguments);
                    break;
                case ApplicationModules.PriceModuleCopyCalculation:
                    pnlMain.Controls.Add(_CopyModule);
                    //_PriceModule.ModuleCopyCalculationMode();
                    break;

            }
        }
        public void CallByProffix(string[] arguments)
        {
            _Args = arguments;

            brBtnPriceSetting.Enabled = true;
            brBtnCopy.Enabled = false;

            bool isProffixLoad = false;
            if (_Args != null && _Args.Length >= 2)
            {
                if (_Args[1].StartsWith("open"))
                {
                    isProffixLoad = true;

                    if (_Args[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length > 2)
                    {
                        brBtnPriceSetting.Enabled = false;
                        brBtnCopy.Enabled = true;
                    }
                }
                else if (_Args[1].StartsWith("copy"))
                {
                    isProffixLoad = true;

                    brBtnPriceSetting.Enabled = false;
                    brBtnCopy.Enabled = false;
                }

                ShowModule(ApplicationModules.PriceModuleCalculationByProffix, new string[] { _Args[0], _Args[1] });
            }

            if (!isProffixLoad)
            {
                brBrnPrice_ItemClick(this, null);
            }
        }

        private void brBrnPrice_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ShowModule(ApplicationModules.PriceModuleCalculation);
            if (_Args != null && _Args.Length >= 2)
            {
                ShowModule(ApplicationModules.PriceModuleCalculationByProffix, new string[] { _Args[0], _Args[1] });
            }
            else
            {
                ShowModule(ApplicationModules.PriceModuleCalculation);
            }
        }

        private void brBtnPriceSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleSetting);
        }

        private void brBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCopyCalculation);
        }

        private void _PriceModule_OpenedCalculation()
        {
            brBtnPriceSetting.Enabled = false;
        }

        private void _PriceModule_Saved(string message)
        {
            _Args = null;
            MessageBox.Show(message, "Calculation", MessageBoxButtons.OK);
        }

        private void _SettingModule_SaveChanged(string message)
        {
            MessageBox.Show(message, "Settings", MessageBoxButtons.OK);
        }

        private void _CopyModule_SaveChanged()
        {
            string command = _PriceModule.GetCopyCommand(_CopyModule.GetModel());

            if (!String.IsNullOrWhiteSpace(command) && (_Args != null && _Args.Length >= 2))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _Args[0];
                startInfo.Arguments = command;
                Process.Start(startInfo);
            }
        }
    }
}