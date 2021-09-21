
namespace TrimedBot.DatabasesInterface.Views.Forms
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mtcSteps = new MaterialSkin.Controls.MaterialTabControl();
            this.tpConnections = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mbtnAddConnection = new MaterialSkin.Controls.MaterialButton();
            this.tpQueries = new System.Windows.Forms.TabPage();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.mlbConnections = new MaterialSkin.Controls.MaterialListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mmltxtQueries = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.mtcSteps.SuspendLayout();
            this.tpConnections.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpQueries.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mtcSteps
            // 
            this.mtcSteps.Controls.Add(this.tpConnections);
            this.mtcSteps.Controls.Add(this.tpQueries);
            this.mtcSteps.Depth = 0;
            this.mtcSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtcSteps.ImageList = this.ilIcons;
            this.mtcSteps.Location = new System.Drawing.Point(2, 56);
            this.mtcSteps.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mtcSteps.MouseState = MaterialSkin.MouseState.HOVER;
            this.mtcSteps.Multiline = true;
            this.mtcSteps.Name = "mtcSteps";
            this.mtcSteps.SelectedIndex = 0;
            this.mtcSteps.Size = new System.Drawing.Size(903, 635);
            this.mtcSteps.TabIndex = 0;
            // 
            // tpConnections
            // 
            this.tpConnections.BackColor = System.Drawing.Color.White;
            this.tpConnections.Controls.Add(this.panel2);
            this.tpConnections.Controls.Add(this.panel1);
            this.tpConnections.ImageKey = "outline_cable_black_24dp.png";
            this.tpConnections.Location = new System.Drawing.Point(4, 39);
            this.tpConnections.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpConnections.Name = "tpConnections";
            this.tpConnections.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpConnections.Size = new System.Drawing.Size(895, 592);
            this.tpConnections.TabIndex = 0;
            this.tpConnections.Text = "Connections";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mbtnAddConnection);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(2, 531);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(891, 58);
            this.panel2.TabIndex = 3;
            // 
            // mbtnAddConnection
            // 
            this.mbtnAddConnection.AutoSize = false;
            this.mbtnAddConnection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mbtnAddConnection.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.mbtnAddConnection.Depth = 0;
            this.mbtnAddConnection.Dock = System.Windows.Forms.DockStyle.Right;
            this.mbtnAddConnection.HighEmphasis = true;
            this.mbtnAddConnection.Icon = null;
            this.mbtnAddConnection.Location = new System.Drawing.Point(744, 0);
            this.mbtnAddConnection.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.mbtnAddConnection.MouseState = MaterialSkin.MouseState.HOVER;
            this.mbtnAddConnection.Name = "mbtnAddConnection";
            this.mbtnAddConnection.Size = new System.Drawing.Size(147, 58);
            this.mbtnAddConnection.TabIndex = 2;
            this.mbtnAddConnection.Text = "Add connection";
            this.mbtnAddConnection.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.mbtnAddConnection.UseAccentColor = false;
            this.mbtnAddConnection.UseVisualStyleBackColor = true;
            this.mbtnAddConnection.Click += new System.EventHandler(this.mbtnAddConnection_Click);
            // 
            // tpQueries
            // 
            this.tpQueries.BackColor = System.Drawing.Color.White;
            this.tpQueries.Controls.Add(this.mmltxtQueries);
            this.tpQueries.ImageKey = "outline_code_black_24dp.png";
            this.tpQueries.Location = new System.Drawing.Point(4, 39);
            this.tpQueries.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpQueries.Name = "tpQueries";
            this.tpQueries.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpQueries.Size = new System.Drawing.Size(895, 592);
            this.tpQueries.TabIndex = 1;
            this.tpQueries.Text = "Queries";
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.White;
            this.ilIcons.Images.SetKeyName(0, "outline_cable_black_24dp.png");
            this.ilIcons.Images.SetKeyName(1, "outline_code_black_24dp.png");
            // 
            // mlbConnections
            // 
            this.mlbConnections.BackColor = System.Drawing.Color.White;
            this.mlbConnections.BorderColor = System.Drawing.Color.LightGray;
            this.mlbConnections.Depth = 0;
            this.mlbConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mlbConnections.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mlbConnections.Location = new System.Drawing.Point(0, 0);
            this.mlbConnections.MouseState = MaterialSkin.MouseState.HOVER;
            this.mlbConnections.Name = "mlbConnections";
            this.mlbConnections.SelectedIndex = -1;
            this.mlbConnections.SelectedItem = null;
            this.mlbConnections.Size = new System.Drawing.Size(891, 528);
            this.mlbConnections.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mlbConnections);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 528);
            this.panel1.TabIndex = 2;
            // 
            // mmltxtQueries
            // 
            this.mmltxtQueries.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mmltxtQueries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mmltxtQueries.Depth = 0;
            this.mmltxtQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mmltxtQueries.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mmltxtQueries.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.mmltxtQueries.Location = new System.Drawing.Point(2, 3);
            this.mmltxtQueries.MouseState = MaterialSkin.MouseState.HOVER;
            this.mmltxtQueries.Name = "mmltxtQueries";
            this.mmltxtQueries.Size = new System.Drawing.Size(891, 586);
            this.mmltxtQueries.TabIndex = 0;
            this.mmltxtQueries.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 694);
            this.Controls.Add(this.mtcSteps);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.mtcSteps;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(2, 56, 2, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Databases interface";
            this.mtcSteps.ResumeLayout(false);
            this.tpConnections.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tpQueries.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl mtcSteps;
        private System.Windows.Forms.TabPage tpConnections;
        private System.Windows.Forms.TabPage tpQueries;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.Panel panel2;
        private MaterialSkin.Controls.MaterialButton mbtnAddConnection;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialListBox mlbConnections;
        private MaterialSkin.Controls.MaterialMultiLineTextBox mmltxtQueries;
    }
}