﻿using System;
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
    public partial class CalculationCopyCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void CopyCalculationSaveChangedCallback();
        public event CopyCalculationSaveChangedCallback SaveChanged;

        CopyCalculationModel _Model;

        public CalculationCopyCtrl()
        {
            InitializeComponent();
        }

        private void CalculationCopyCtrl_Load(object sender, EventArgs e)
        {
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
            }
            else
            {
                _Model.ProductNo = txtNumber.Text;
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
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
            }
        }
    }
}
