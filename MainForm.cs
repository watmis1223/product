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
using ProductCalculation.Library.Entity.PriceCalculation.Models;
using ProductCalculation.Library.Entity.Report;
using ProductCalculation.Library.Entity.Setting.PriceCalculation;

namespace ProductCalculation
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        PriceCtrlMain _PriceModule = new PriceCtrlMain();
        CalculationCopyCtrl _CopyModule = new CalculationCopyCtrl();
        SettingCtrl _SettingModule = new SettingCtrl();
        bool _IsDeleteMode = false;

        //argument form prffix
        string[] _Args;

        public MainForm(string[] arguments = null)
        {
            InitializeComponent();

            _Args = arguments;

            if (_Args != null && _Args.Length >= 2)
            {
                if (_Args[1].StartsWith("copy"))
                {
                    string app = _Args[0];
                    string cmd = _Args[1];
                    for (int i = 2; i < _Args.Length; i++)
                    {
                        cmd = String.Concat(cmd + " " + _Args[i]);
                    }

                    _Args = new string[] { app, cmd };
                }
            }

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
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = false;
            brBtnDelete.Enabled = false;

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
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = false;

            bool isProffixLoad = false;
            if (_Args != null && _Args.Length >= 2)
            {
                if (_Args[1].StartsWith("open"))
                {
                    isProffixLoad = true;

                    string[] sCmdParam = _Args[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (sCmdParam.Length > 2)
                    {
                        brBtnPriceSetting.Enabled = false;
                        brBtnCopy.Enabled = true;
                        brBtnNew.Enabled = false;
                        brBtnSave.Enabled = true;
                        brBtnDelete.Enabled = true;
                    }
                    else if (sCmdParam.Length == 2)
                    {
                        brBtnPriceSetting.Enabled = true;
                        brBtnCopy.Enabled = false;
                        brBtnNew.Enabled = true; ;
                        brBtnSave.Enabled = false;
                    }
                }
                else if (_Args[1].StartsWith("copy"))
                {
                    isProffixLoad = true;

                    brBtnPriceSetting.Enabled = false;
                    brBtnCopy.Enabled = false;
                    brBtnNew.Enabled = false;
                    brBtnSave.Enabled = true;
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

            brBtnNew.Enabled = true;
            brBtnDelete.Enabled = false;
        }

        private void brBtnPriceSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleSetting);

            brBtnCopy.Enabled = false;
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = false;
            brBtnDelete.Enabled = false;
        }

        private void brBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCopyCalculation);

            brBtnCopy.Enabled = false;
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = false;
            brBtnDelete.Enabled = false;
        }

        private void brBtnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _PriceModule.NewCalculation();
        }

        private void brBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _PriceModule.SaveCalculation();
        }

        private void _PriceModule_OpenedCalculation()
        {
            brBtnPriceSetting.Enabled = false;
            brBtnCopy.Enabled = false;
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = true;
            brBtnDelete.Enabled = true;
        }

        private void _PriceModule_Saved(string message)
        {
            _Args = null;            
            MessageBox.Show(message, "Calculation", MessageBoxButtons.OK);

            if (_IsDeleteMode)
            {
                //close app
                _IsDeleteMode = false;
                Application.Exit();
            }
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

        private void brBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //get calculation model
            CalculationModel model = _PriceModule.GetCalculationModel();
            PriceCalculationSetting setting = _SettingModule.GetModel();
            //save to pdf
            if (model != null && setting != null)
            {
                string sReportPath = "";
                try
                {
                    sReportPath = setting.ReportPathSetting.ReportPath;
                }
                catch { }

                if (String.IsNullOrWhiteSpace(sReportPath))
                {
                    MessageBox.Show("Report path empty", "Print PDF", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        Invoice invoice = new Invoice();
                        invoice.CreateInvoice(model, sReportPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Print PDF", MessageBoxButtons.OK);
                    }

                }
            }
        }

        private void brBtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //get calculation model
            CalculationModel model = _PriceModule.GetCalculationModelWithUpdateGeneralSettings();

            if (model != null)
            {
                if (model.GeneralSetting.Options.Contains("A"))
                {
                    //not allow to delete
                    MessageBox.Show("KALKULATION KANN NICHT GELÖSCHT WERDEN, DA AKTIV. BITTE ZUERST INAKTIV SETZEN!", 
                        "Delete Calculation", MessageBoxButtons.OK);
                }
                else
                {
                    //allow to delete if not active
                    if (MessageBox.Show("WOLLEN SIE DIE KALKULATION LÖSCHEN?", 
                        "Delete Calculation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _IsDeleteMode = true;
                        _PriceModule.DeleteCalculation();
                    }
                }
            }
        }
    }
}