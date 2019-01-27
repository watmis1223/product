using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ProductCalculation.Library.Storage;
using ProductCalculation.Library.Entity.PriceCalculation.Models;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace ProductCalculation.Library.UI.PriceCalculation
{
    public partial class SearchArtikelDialog : DevExpress.XtraEditors.XtraForm
    {
        public string ProffixConnectionString { get; set; }
        public ProffixLAGArtikelModel Model { get; set; }

        List<ProffixLAGArtikelModel> _List;

        public SearchArtikelDialog()
        {
            InitializeComponent();
        }

        void GetData()
        {
            try
            {
                //search product
                //load proffix supplier
                _List = StorageOperator.GetProffixLAGArtikelList(txtAddressName.Text, ProffixConnectionString);
                
                if (_List != null && _List.Count > 0)
                {
                    grdResult.DataSource = _List;

                    foreach (GridColumn col in gridView1.Columns)
                    {
                        col.Visible = false;
                        col.OptionsFilter.AllowFilter = false;
                    }

                    gridView1.Columns["Bezeichnung1"].Visible = true;
                    gridView1.Columns["Bezeichnung1"].BestFit();
                }
                
            }
            finally { }
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            if (_List == null)
            {
                return;
            }

            GridHitInfo info = view.CalcHitInfo(pt);

            if (info.InRow || info.InRowCell)
            {
                Model = _List[info.RowHandle];
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAddressName.Text = "";
            grdResult.DataSource = null;
            grdResult.RefreshDataSource();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);

            DialogResult = DialogResult.OK;
        }

    }
}