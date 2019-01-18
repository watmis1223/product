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
            this.brBtnNew = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnSave = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnShortCutOpena = new DevExpress.XtraBars.BarButtonItem();
            this.btBtnShortCutOpen = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.brBrnPrice,
            this.brBtnPriceSetting,
            this.brBtnCopy,
            this.brBtnNew,
            this.brBtnSave,
            this.brBtnPrint,
            this.brBtnDelete,
            this.brBtnShortCutOpena,
            this.btBtnShortCutOpen});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 17;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1748, 281);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // brBrnPrice
            // 
            this.brBrnPrice.Caption = "General";
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
            // brBtnNew
            // 
            this.brBtnNew.Caption = "New";
            this.brBtnNew.Id = 11;
            this.brBtnNew.ImageOptions.Image = global::ProductCalculation.Properties.Resources.insert_16x16;
            this.brBtnNew.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.insert_32x32;
            this.brBtnNew.Name = "brBtnNew";
            this.brBtnNew.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnNew_ItemClick);
            // 
            // brBtnSave
            // 
            this.brBtnSave.Caption = "Save";
            this.brBtnSave.Id = 12;
            this.brBtnSave.ImageOptions.Image = global::ProductCalculation.Properties.Resources.saveas_16x16;
            this.brBtnSave.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.saveas_32x32;
            this.brBtnSave.Name = "brBtnSave";
            this.brBtnSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnSave_ItemClick);
            // 
            // brBtnPrint
            // 
            this.brBtnPrint.Caption = "Print";
            this.brBtnPrint.Id = 13;
            this.brBtnPrint.ImageOptions.Image = global::ProductCalculation.Properties.Resources.exporttopdf_16x16;
            this.brBtnPrint.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.exporttopdf_32x32;
            this.brBtnPrint.Name = "brBtnPrint";
            this.brBtnPrint.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnPrint_ItemClick);
            // 
            // brBtnDelete
            // 
            this.brBtnDelete.Caption = "Delete";
            this.brBtnDelete.Id = 14;
            this.brBtnDelete.ImageOptions.Image = global::ProductCalculation.Properties.Resources.deletelist_32x32;
            this.brBtnDelete.ImageOptions.LargeImage = global::ProductCalculation.Properties.Resources.deletelist_32x32;
            this.brBtnDelete.Name = "brBtnDelete";
            this.brBtnDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnDelete_ItemClick);
            // 
            // brBtnShortCutOpena
            // 
            this.brBtnShortCutOpena.Caption = "Shortcut für Adressen hinzufügen";
            this.brBtnShortCutOpena.Id = 14;
            this.brBtnShortCutOpena.ImageOptions.Image = global::ProductCalculation.Properties.Resources.undo_32x32;
            this.brBtnShortCutOpena.Name = "brBtnShortCutOpena";
            this.brBtnShortCutOpena.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnShortCutOpena.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnShortCutOpena_ItemClick);
            // 
            // btBtnShortCutOpen
            // 
            this.btBtnShortCutOpen.Caption = "Shortcut für Artikel hinzufügen";
            this.btBtnShortCutOpen.Id = 15;
            this.btBtnShortCutOpen.ImageOptions.Image = global::ProductCalculation.Properties.Resources.undo_32x321;
            this.btBtnShortCutOpen.Name = "btBtnShortCutOpen";
            this.btBtnShortCutOpen.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btBtnShortCutOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btBtnShortCutOpen_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Module";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.brBrnPrice);
            this.ribbonPageGroup1.ItemLinks.Add(this.brBtnPriceSetting);
            this.ribbonPageGroup1.ItemLinks.Add(this.brBtnPrint);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.brBtnNew);
            this.ribbonPageGroup2.ItemLinks.Add(this.brBtnCopy);
            this.ribbonPageGroup2.ItemLinks.Add(this.brBtnDelete);
            this.ribbonPageGroup2.ItemLinks.Add(this.brBtnSave);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.brBtnShortCutOpena);
            this.ribbonPageGroup3.ItemLinks.Add(this.btBtnShortCutOpen);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMain.Controls.Add(this.marqueeProgressBarControl1);
            this.pnlMain.Controls.Add(this.progressBarControl1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 281);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1748, 1036);
            this.pnlMain.TabIndex = 1;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(504, 411);
            this.progressBarControl1.MenuManager = this.ribbonControl1;
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(718, 64);
            this.progressBarControl1.TabIndex = 0;
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = "Loading";
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(504, 490);
            this.marqueeProgressBarControl1.MenuManager = this.ribbonControl1;
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(718, 64);
            this.marqueeProgressBarControl1.TabIndex = 1;
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
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem brBtnNew;
        private DevExpress.XtraBars.BarButtonItem brBtnDelete;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem brBtnSave;
        private DevExpress.XtraBars.BarButtonItem brBtnPrint;
        private DevExpress.XtraBars.BarButtonItem brBtnShortCutOpena;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem btBtnShortCutOpen;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
    }
}