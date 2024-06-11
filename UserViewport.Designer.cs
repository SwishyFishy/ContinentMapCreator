namespace ContinentMapCreator
{
    partial class form_Window
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
            this.components = new System.ComponentModel.Container();
            this.pnl_SettingsBackground = new System.Windows.Forms.Panel();
            this.lbl_TerritoryRadius = new System.Windows.Forms.Label();
            this.nud_TerritoryRadiusBound1 = new System.Windows.Forms.NumericUpDown();
            this.nud_TerritoryRadiusBound2 = new System.Windows.Forms.NumericUpDown();
            this.lbl_TerritoryCount = new System.Windows.Forms.Label();
            this.nud_TerritoryCountBound2 = new System.Windows.Forms.NumericUpDown();
            this.nud_TerritoryCountBound1 = new System.Windows.Forms.NumericUpDown();
            this.chb_CleanBorders = new System.Windows.Forms.CheckBox();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.lbl_Header = new System.Windows.Forms.Label();
            this.pnl_MapBackground = new System.Windows.Forms.Panel();
            this.lbl_NewWindowPrompt = new System.Windows.Forms.Label();
            this.tip_SettingsDetails = new System.Windows.Forms.ToolTip(this.components);
            this.lbl_OriginSpacing = new System.Windows.Forms.Label();
            this.nud_MinimumOriginSpacing = new System.Windows.Forms.NumericUpDown();
            this.fntd_FontSelector = new System.Windows.Forms.FontDialog();
            this.btn_FontSelector = new System.Windows.Forms.Button();
            this.pnl_SettingsBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryRadiusBound1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryRadiusBound2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryCountBound2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryCountBound1)).BeginInit();
            this.pnl_MapBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MinimumOriginSpacing)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_SettingsBackground
            // 
            this.pnl_SettingsBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnl_SettingsBackground.BackColor = System.Drawing.Color.Aquamarine;
            this.pnl_SettingsBackground.Controls.Add(this.btn_FontSelector);
            this.pnl_SettingsBackground.Controls.Add(this.nud_MinimumOriginSpacing);
            this.pnl_SettingsBackground.Controls.Add(this.nud_TerritoryRadiusBound1);
            this.pnl_SettingsBackground.Controls.Add(this.nud_TerritoryRadiusBound2);
            this.pnl_SettingsBackground.Controls.Add(this.nud_TerritoryCountBound2);
            this.pnl_SettingsBackground.Controls.Add(this.nud_TerritoryCountBound1);
            this.pnl_SettingsBackground.Controls.Add(this.chb_CleanBorders);
            this.pnl_SettingsBackground.Controls.Add(this.btn_Generate);
            this.pnl_SettingsBackground.Controls.Add(this.lbl_Header);
            this.pnl_SettingsBackground.Controls.Add(this.lbl_TerritoryRadius);
            this.pnl_SettingsBackground.Controls.Add(this.lbl_OriginSpacing);
            this.pnl_SettingsBackground.Controls.Add(this.lbl_TerritoryCount);
            this.pnl_SettingsBackground.Location = new System.Drawing.Point(0, 0);
            this.pnl_SettingsBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_SettingsBackground.Name = "pnl_SettingsBackground";
            this.pnl_SettingsBackground.Size = new System.Drawing.Size(250, 862);
            this.pnl_SettingsBackground.TabIndex = 0;
            // 
            // lbl_TerritoryRadius
            // 
            this.lbl_TerritoryRadius.AutoSize = true;
            this.lbl_TerritoryRadius.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TerritoryRadius.Location = new System.Drawing.Point(14, 54);
            this.lbl_TerritoryRadius.Name = "lbl_TerritoryRadius";
            this.lbl_TerritoryRadius.Size = new System.Drawing.Size(168, 19);
            this.lbl_TerritoryRadius.TabIndex = 7;
            this.lbl_TerritoryRadius.Text = "Territory Radius (Range)";
            // 
            // nud_TerritoryRadiusBound1
            // 
            this.nud_TerritoryRadiusBound1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_TerritoryRadiusBound1.Location = new System.Drawing.Point(174, 54);
            this.nud_TerritoryRadiusBound1.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nud_TerritoryRadiusBound1.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nud_TerritoryRadiusBound1.Name = "nud_TerritoryRadiusBound1";
            this.nud_TerritoryRadiusBound1.Size = new System.Drawing.Size(49, 20);
            this.nud_TerritoryRadiusBound1.TabIndex = 13;
            this.nud_TerritoryRadiusBound1.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // nud_TerritoryRadiusBound2
            // 
            this.nud_TerritoryRadiusBound2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_TerritoryRadiusBound2.Location = new System.Drawing.Point(174, 80);
            this.nud_TerritoryRadiusBound2.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nud_TerritoryRadiusBound2.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nud_TerritoryRadiusBound2.Name = "nud_TerritoryRadiusBound2";
            this.nud_TerritoryRadiusBound2.Size = new System.Drawing.Size(49, 20);
            this.nud_TerritoryRadiusBound2.TabIndex = 12;
            this.nud_TerritoryRadiusBound2.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // lbl_TerritoryCount
            // 
            this.lbl_TerritoryCount.AutoSize = true;
            this.lbl_TerritoryCount.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TerritoryCount.Location = new System.Drawing.Point(14, 108);
            this.lbl_TerritoryCount.Name = "lbl_TerritoryCount";
            this.lbl_TerritoryCount.Size = new System.Drawing.Size(209, 19);
            this.lbl_TerritoryCount.TabIndex = 11;
            this.lbl_TerritoryCount.Text = "Number Of Territories (Range)";
            // 
            // nud_TerritoryCountBound2
            // 
            this.nud_TerritoryCountBound2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_TerritoryCountBound2.Location = new System.Drawing.Point(174, 134);
            this.nud_TerritoryCountBound2.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nud_TerritoryCountBound2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_TerritoryCountBound2.Name = "nud_TerritoryCountBound2";
            this.nud_TerritoryCountBound2.Size = new System.Drawing.Size(49, 20);
            this.nud_TerritoryCountBound2.TabIndex = 10;
            this.nud_TerritoryCountBound2.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // nud_TerritoryCountBound1
            // 
            this.nud_TerritoryCountBound1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_TerritoryCountBound1.Location = new System.Drawing.Point(174, 108);
            this.nud_TerritoryCountBound1.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nud_TerritoryCountBound1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_TerritoryCountBound1.Name = "nud_TerritoryCountBound1";
            this.nud_TerritoryCountBound1.Size = new System.Drawing.Size(49, 20);
            this.nud_TerritoryCountBound1.TabIndex = 9;
            this.nud_TerritoryCountBound1.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // chb_CleanBorders
            // 
            this.chb_CleanBorders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chb_CleanBorders.AutoSize = true;
            this.chb_CleanBorders.Checked = true;
            this.chb_CleanBorders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_CleanBorders.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chb_CleanBorders.Location = new System.Drawing.Point(18, 744);
            this.chb_CleanBorders.Name = "chb_CleanBorders";
            this.chb_CleanBorders.Size = new System.Drawing.Size(120, 23);
            this.chb_CleanBorders.TabIndex = 8;
            this.chb_CleanBorders.Text = "Clean Borders";
            this.chb_CleanBorders.UseVisualStyleBackColor = true;
            this.chb_CleanBorders.CheckedChanged += new System.EventHandler(this.chb_CleanBorders_CheckChanged);
            // 
            // btn_Generate
            // 
            this.btn_Generate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Generate.Enabled = false;
            this.btn_Generate.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Generate.Location = new System.Drawing.Point(18, 809);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(214, 40);
            this.btn_Generate.TabIndex = 5;
            this.btn_Generate.Text = "GENERATE";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // lbl_Header
            // 
            this.lbl_Header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Header.AutoSize = true;
            this.lbl_Header.Font = new System.Drawing.Font("Carlito", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Header.Location = new System.Drawing.Point(12, 9);
            this.lbl_Header.Name = "lbl_Header";
            this.lbl_Header.Size = new System.Drawing.Size(220, 33);
            this.lbl_Header.TabIndex = 0;
            this.lbl_Header.Text = "Continent Mapper";
            // 
            // pnl_MapBackground
            // 
            this.pnl_MapBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_MapBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnl_MapBackground.Controls.Add(this.lbl_NewWindowPrompt);
            this.pnl_MapBackground.Location = new System.Drawing.Point(250, 0);
            this.pnl_MapBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_MapBackground.Name = "pnl_MapBackground";
            this.pnl_MapBackground.Size = new System.Drawing.Size(750, 900);
            this.pnl_MapBackground.TabIndex = 1;
            this.pnl_MapBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_MapBackground_Paint);
            // 
            // lbl_NewWindowPrompt
            // 
            this.lbl_NewWindowPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_NewWindowPrompt.AutoSize = true;
            this.lbl_NewWindowPrompt.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NewWindowPrompt.Location = new System.Drawing.Point(3, 289);
            this.lbl_NewWindowPrompt.Name = "lbl_NewWindowPrompt";
            this.lbl_NewWindowPrompt.Size = new System.Drawing.Size(727, 19);
            this.lbl_NewWindowPrompt.TabIndex = 0;
            this.lbl_NewWindowPrompt.Text = "Use the settings panel on the left to customize the map properties, then click \'G" +
    "enerate\' to make a world map.";
            // 
            // tip_SettingsDetails
            // 
            this.tip_SettingsDetails.AutoPopDelay = 5000;
            this.tip_SettingsDetails.InitialDelay = 0;
            this.tip_SettingsDetails.ReshowDelay = 100;
            // 
            // lbl_OriginSpacing
            // 
            this.lbl_OriginSpacing.AutoSize = true;
            this.lbl_OriginSpacing.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_OriginSpacing.Location = new System.Drawing.Point(14, 160);
            this.lbl_OriginSpacing.Name = "lbl_OriginSpacing";
            this.lbl_OriginSpacing.Size = new System.Drawing.Size(192, 19);
            this.lbl_OriginSpacing.TabIndex = 14;
            this.lbl_OriginSpacing.Text = "Minimum Origin Separation";
            // 
            // nud_MinimumOriginSpacing
            // 
            this.nud_MinimumOriginSpacing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_MinimumOriginSpacing.Location = new System.Drawing.Point(174, 160);
            this.nud_MinimumOriginSpacing.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nud_MinimumOriginSpacing.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_MinimumOriginSpacing.Name = "nud_MinimumOriginSpacing";
            this.nud_MinimumOriginSpacing.Size = new System.Drawing.Size(49, 20);
            this.nud_MinimumOriginSpacing.TabIndex = 15;
            this.nud_MinimumOriginSpacing.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // fntd_FontSelector
            // 
            this.fntd_FontSelector.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // btn_FontSelector
            // 
            this.btn_FontSelector.BackColor = System.Drawing.SystemColors.Control;
            this.btn_FontSelector.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_FontSelector.Location = new System.Drawing.Point(18, 773);
            this.btn_FontSelector.Name = "btn_FontSelector";
            this.btn_FontSelector.Size = new System.Drawing.Size(120, 30);
            this.btn_FontSelector.TabIndex = 16;
            this.btn_FontSelector.Text = "Select Font";
            this.btn_FontSelector.UseVisualStyleBackColor = false;
            this.btn_FontSelector.Click += new System.EventHandler(this.btn_FontSelector_Click);
            // 
            // form_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 861);
            this.Controls.Add(this.pnl_MapBackground);
            this.Controls.Add(this.pnl_SettingsBackground);
            this.Name = "form_Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Continent Mapper";
            this.Load += new System.EventHandler(this.form_Window_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_MapBackground_Paint);
            this.pnl_SettingsBackground.ResumeLayout(false);
            this.pnl_SettingsBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryRadiusBound1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryRadiusBound2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryCountBound2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TerritoryCountBound1)).EndInit();
            this.pnl_MapBackground.ResumeLayout(false);
            this.pnl_MapBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MinimumOriginSpacing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_SettingsBackground;
        private System.Windows.Forms.Panel pnl_MapBackground;
        private System.Windows.Forms.Label lbl_Header;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.Label lbl_TerritoryRadius;
        private System.Windows.Forms.CheckBox chb_CleanBorders;
        private System.Windows.Forms.NumericUpDown nud_TerritoryCountBound2;
        private System.Windows.Forms.NumericUpDown nud_TerritoryCountBound1;
        private System.Windows.Forms.Label lbl_TerritoryCount;
        private System.Windows.Forms.Label lbl_NewWindowPrompt;
        private System.Windows.Forms.ToolTip tip_SettingsDetails;
        private System.Windows.Forms.NumericUpDown nud_TerritoryRadiusBound1;
        private System.Windows.Forms.NumericUpDown nud_TerritoryRadiusBound2;
        private System.Windows.Forms.NumericUpDown nud_MinimumOriginSpacing;
        private System.Windows.Forms.Label lbl_OriginSpacing;
        private System.Windows.Forms.FontDialog fntd_FontSelector;
        private System.Windows.Forms.Button btn_FontSelector;
    }
}

