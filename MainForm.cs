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
using System.Threading;

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

        private delegate void LoadDataForShortCutDoneCallback(int rowCount);
        private event LoadDataForShortCutDoneCallback LoadDataForShortCutDone;

        private delegate void ShortCutCreateDoneCallback();
        private event ShortCutCreateDoneCallback ShortCutCreateDone;

        private delegate void PrepareCreateShortCutDoneCallback(string byID);
        private event PrepareCreateShortCutDoneCallback PrepareCreateShortCutDone;

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

            this.LoadDataForShortCutDone += MainForm_LoadDataForShortCutDone;
            this.ShortCutCreateDone += MainForm_ShortCutCreateDone;
            this.PrepareCreateShortCutDone += MainForm_PrepareCreateShortCutDone;

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
            brBtnPrint.Enabled = false;

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
                        brBtnPrint.Enabled = true;
                    }
                    else if (sCmdParam.Length == 2)
                    {
                        brBtnPriceSetting.Enabled = true;
                        brBtnCopy.Enabled = false;
                        brBtnNew.Enabled = true; ;
                        brBtnSave.Enabled = false;
                        brBtnPrint.Enabled = false;
                    }
                }
                else if (_Args[1].StartsWith("copy"))
                {
                    isProffixLoad = true;

                    brBtnPriceSetting.Enabled = false;
                    brBtnCopy.Enabled = false;
                    brBtnNew.Enabled = false;
                    brBtnSave.Enabled = true;
                    brBtnPrint.Enabled = false;
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
            brBtnPrint.Enabled = false;
        }

        private void brBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCopyCalculation);

            brBtnCopy.Enabled = false;
            brBtnNew.Enabled = false;
            brBtnSave.Enabled = false;
            brBtnDelete.Enabled = false;
            brBtnPrint.Enabled = false;
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
            //brBtnDelete.Enabled = true;
            //brBtnPrint.Enabled = true;
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

            brBtnPrint.Enabled = true;
            brBtnDelete.Enabled = true;
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
            CalculationModel model = _PriceModule.GetCalculationModelAndRefreshAmount();
            PriceCalculationSetting setting = _SettingModule.GetModel();

            //refresh data           

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
                    MessageBox.Show("CALCULATION CAN NOT DELETED BECAUSE IS ACTIVE, SET INACTIVE FIRST",
                        "Delete Calculation", MessageBoxButtons.OK);
                }
                else
                {
                    //allow to delete if not active
                    if (MessageBox.Show("DO YOU WANT DELETE THE CALCULATION?",
                        "Delete Calculation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _IsDeleteMode = true;
                        _PriceModule.DeleteCalculation();
                    }
                }
            }
        }

        DataTable LoadShortCutData(string proffixConnection, string byID = "AdressNrADR")
        {
            DataTable dt = new DataTable();

            if (byID == "AdressNrADR")
            {
                dt = StorageOperator.LoadProffixADRAdressen(proffixConnection);
            }
            else
            {
                dt = StorageOperator.LoadProffixLAGArtikel(proffixConnection);
            }

            if (dt != null)
            {
                if (LoadDataForShortCutDone != null)
                {
                    LoadDataForShortCutDone(dt.Rows.Count);
                }
            }

            return dt;
        }

        void DoCreateShortCut(string byID = "AdressNrADR")
        {
            PriceCalculationSetting setting = _SettingModule.GetModel();
            string sAppPath = Application.ExecutablePath;

            if (setting != null)
            {
                //pnlMain.Controls.Clear();
                //marqueeProgressBarControl1.Properties.ShowTitle = true;
                //marqueeProgressBarControl1.Left = (pnlMain.ClientSize.Width - marqueeProgressBarControl1.Width) / 2;
                //marqueeProgressBarControl1.Top = (pnlMain.ClientSize.Height - marqueeProgressBarControl1.Height) / 2;
                //pnlMain.Controls.Add(marqueeProgressBarControl1);

                DataTable dt = LoadShortCutData(setting.ProffixConnection, byID: byID);

                if (dt != null)
                {
                    if (byID == "AdressNrADR")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            StorageOperator.InsertADR_DokumenteShortCut(dr[byID].ToString(), sAppPath, setting.ProffixConnection);
                            progressBarControl1.PerformStep();
                            progressBarControl1.Update();
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            StorageOperator.InsertLAG_DokumenteShortCut(dr[byID].ToString(), sAppPath, setting.ProffixConnection);
                            progressBarControl1.PerformStep();
                            progressBarControl1.Update();
                        }
                    }
                }
            }

            if (ShortCutCreateDone != null)
            {
                ShortCutCreateDone();
            }
        }

        void PrepareCreateShortCut(string byID = "AdressNrADR")
        {
            pnlMain.Controls.Clear();
            ribbonPageGroup1.Enabled = false;
            ribbonPageGroup2.Enabled = false;
            ribbonPageGroup3.Enabled = false;

            if (PrepareCreateShortCutDone != null)
            {
                PrepareCreateShortCutDone(byID);
            }
        }

        private void MainForm_LoadDataForShortCutDone(int rowCount)
        {
            // Initializing progress bar properties
            pnlMain.Controls.Clear();
            progressBarControl1.Left = (pnlMain.ClientSize.Width - progressBarControl1.Width) / 2;
            progressBarControl1.Top = (pnlMain.ClientSize.Height - progressBarControl1.Height) / 2;
            progressBarControl1.Properties.Step = 1;
            progressBarControl1.Properties.PercentView = true;
            progressBarControl1.Properties.ShowTitle = true;
            progressBarControl1.Properties.Maximum = rowCount;
            progressBarControl1.Properties.Minimum = 0;
            pnlMain.Controls.Add(progressBarControl1);
        }

        private void MainForm_ShortCutCreateDone()
        {
            pnlMain.Controls.Clear();
            if (MessageBox.Show("Create short-cut success, application will close",
                "Create Short-cut", MessageBoxButtons.OK) == DialogResult.OK)
            {
                Application.Exit();
            }
        }        

        private void MainForm_PrepareCreateShortCutDone(string byID)
        {
            DoCreateShortCut(byID: byID);
        }

        private void brBtnShortCutOpena_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Create calculation short-cut, continue?",
                        "Create Short-cut", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            PrepareCreateShortCut();
        }

        private void btBtnShortCutOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Create calculation short-cut, continue?",
                        "Create Short-cut", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;                
            }

            PrepareCreateShortCut("ArtikelNrLAG");
        }
    }
}