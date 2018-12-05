namespace ProductCalculation
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {            
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.brBrnPrice = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnPriceSetting = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnCopy = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.brBrnPrice,
            this.brBtnPriceSetting,
            this.brBtnCopy});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 11;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1748, 281);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // brBrnPrice
            // 
            this.brBrnPrice.Caption = "Calculation";
            this.brBrnPrice.Id = 4;
            this.brBrnPrice.ImageOptions.Image = global::ProductCalculation.Properties.Resources.tag_16x16;
            this.brBrnPrice.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.tag_32x32;
            this.brBrnPrice.Name = "brBrnPrice";
            this.brBrnPrice.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBrnPrice.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBrnPrice_ItemClick);
            // 
            // brBtnPriceSetting
            // 
            this.brBtnPriceSetting.Caption = "Settings";
            this.brBtnPriceSetting.Id = 9;
            this.brBtnPriceSetting.ImageOptions.Image = global::ProductCalculation.Properties.Resources.technology_16x16;
            this.brBtnPriceSetting.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.technology_32x32;
            this.brBtnPriceSetting.Name = "brBtnPriceSetting";
            this.brBtnPriceSetting.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnPriceSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnPriceSetting_ItemClick);
            // 
            // brBtnCopy
            // 
            this.brBtnCopy.Caption = "Copy";
            this.brBtnCopy.Id = 10;
            this.brBtnCopy.ImageOptions.Image = global::ProductCalculation.Properties.Resources.copy_16x16;
            this.brBtnCopy.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.copy_32x32;
            this.brBtnCopy.Name = "brBtnCopy";
            this.brBtnCopy.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnCopy_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Module";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.brBrnPrice);
            this.ribbonPageGroup1.ItemLinks.Add(this.brBtnCopy);
            this.ribbonPageGroup1.ItemLinks.Add(this.brBtnPriceSetting);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 281);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1748, 1036);
            this.pnlMain.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1748, 1317);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Calculation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }                

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem brBrnPrice;
        private DevExpress.XtraBars.BarButtonItem brBtnPriceSetting;
        private DevExpress.XtraEditors.PanelControl pnlMain;
        private DevExpress.XtraBars.BarButtonItem brBtnCopy;
    }
}