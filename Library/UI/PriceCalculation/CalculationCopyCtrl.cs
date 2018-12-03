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

namespace ProductCalculation.Library.UI.PriceCalculation
{
    public partial class CalculationCopyCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void CopyCalculationSaveChangedCallback();
        public event CopyCalculationSaveChangedCallback SaveChanged;

        CopyCalculationModel _Model;
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

        //public void SetCalculationModel(CalculationModel model)
        //{
        //    _CalculationModel = model;

        //    if (_CalculationModel != null)
        //    {
        //        if (_CalculationModel.CalculationNotes.Count > 2)
        //        {
        //            //use scale
        //            rdoScaleList.SelectedIndex = 1;
        //            rdoScaleList.Properties.Items[0].Enabled = false;
        //            numPriceScale.Enabled = true;
        //        }
        //        else
        //        {
        //            //non scale
        //            rdoScaleList.SelectedIndex = 0;
        //            rdoScaleList.Properties.Items[1].Enabled = false;
        //            numPriceScale.Enabled = false;
        //        }
        //    }
        //}

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

            //if (_CalculationModel != null && _CalculationModel.CalculationNotes != null)
            //{
            //    _CalculationModel.ID = 0;
            //    _CalculationModel.ProffixModel = new ProffixModel();

            //    _Model = new CopyCalculationModel();
            //    if (rdoCopyTypeList.EditValue.ToString() == "A")
            //    {
            //        _Model.AddressNo = txtNumber.Text;
            //        _CalculationModel.ProffixModel.ADRDokumenteDokumentNrADR = _Model.AddressNo;
            //    }
            //    else
            //    {
            //        _Model.ProductNo = txtNumber.Text;
            //        _CalculationModel.ProffixModel.LAGDokumenteArtikelNrLAG = _Model.ProductNo;
            //    }

            //    if (rdoCopyTypeList.EditValue.ToString() == "I")
            //    {
            //        _Model.Scale = 1;
            //    }
            //    else
            //    {
            //        _Model.Scale = Int32.Parse(numPriceScale.Text);
            //    }

            //    //if (rdoCopyTypeList.EditValue.ToString() == "I")
            //    //{
            //    //    _Model.Scale = 1;
            //    //}
            //    //else
            //    //{
            //    //    _Model.Scale = Int32.Parse(numPriceScale.Text);

            //    //    if (_Model.Scale == 1)
            //    //    {
            //    //        //calculation note count 2 means one of basic calculation plus one of scale
            //    //        //so that equals to scale 1
            //    //        if (_CalculationModel.CalculationNotes.Count > 2)
            //    //        {
            //    //            _CalculationModel.CalculationNotes.RemoveAll(item => item.ID > 1);
            //    //        }
            //    //    }

            //    //    //calculation note item at 0 is basic calculation
            //    //    if (_Model.Scale > _CalculationModel.CalculationNotes.Count(item => item.ID > 0))
            //    //    {
            //    //        //convert object to json
            //    //        //then convert json to object as new model
            //    //        //_CalculationModel.CalculationNotes.Add()
            //    //    }
            //    //}

            //}
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
    }
}
