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
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace ProductCalculation.Library.UI.PriceCalculation
{
    public partial class CalculationCopyCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void CopyCalculationSaveChangedCallback();
        public event CopyCalculationSaveChangedCallback SaveChanged;

        CopyCalculationModel _Model;
        public string ProffixConnectionString { get; set; }

        //CalculationModel _CalculationModel;

        public CalculationCopyCtrl()
        {
            InitializeComponent();
        }

        private void CalculationCopyCtrl_Load(object sender, EventArgs e)
        {
            numPriceScale.Enabled = false;
        }

        public CopyCalculationModel GetModel()
        {
            return _Model;
        }

        void DoCopy()
        {
            _Model = new CopyCalculationModel();
            if (rdoCopyTypeList.EditValue.ToString() == "A")
            {
                _Model.AddressNo = txtNumber.Text;
                //_CalculationModel.ProffixModel.ADRDokumenteDokumentNrADR = _Model.AddressNo;
            }
            else
            {
                _Model.ProductNo = txtNumber.Text;
                //_CalculationModel.ProffixModel.LAGDokumenteArtikelNrLAG = _Model.ProductNo;
            }

            if (rdoCopyTypeList.EditValue.ToString() == "I")
            {
                _Model.Scale = 1;
            }
            else
            {
                _Model.Scale = Int32.Parse(numPriceScale.Text);
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DoCopy();

            if (SaveChanged != null)
            {
                SaveChanged();
            }
        }

        private void RdoCopyTypeList_EditValueChanged(object sender, System.EventArgs e)
        {
            if (rdoCopyTypeList.EditValue.ToString() == "A")
            {
                layoutControlItem2.Text = "Address-Nr";
            }
            else
            {
                layoutControlItem2.Text = "Artikel";
            }
        }

        private void RdoScaleList_EditValueChanged(object sender, System.EventArgs e)
        {
            if (rdoCopyTypeList.EditValue.ToString() == "I")
            {
                numPriceScale.Enabled = false;
            }
            else
            {
                numPriceScale.Enabled = true;
            }
        }

        private void txtNumber_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ProffixConnectionString))
            {
                return;
            }

            if (e.Button.Kind == ButtonPredefines.Search)
            {
                if (rdoCopyTypeList.EditValue.ToString() == "A")
                {
                    SearchAddressDialog dlg = new SearchAddressDialog();
                    dlg.ProffixConnectionString = ProffixConnectionString;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.Model != null)
                        {
                            txtNumber.Text = dlg.Model.AdressNrADR.ToString();                            
                        }
                    }
                }
                else
                {
                    SearchArtikelDialog dlg = new SearchArtikelDialog();
                    dlg.ProffixConnectionString = ProffixConnectionString;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.Model != null)
                        {
                            txtNumber.Text = dlg.Model.ArtikelNrLAG;
                        }
                    }
                }
            }            
        }
    }
}
