
namespace TrimedBot.DatabasesInterface.Views.Forms.Connections
{
    partial class frmAddConnection
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
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mbtnSave = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.mlblConnectionState = new MaterialSkin.Controls.MaterialLabel();
            this.mtxtName = new MaterialSkin.Controls.MaterialTextBox();
            this.mmltxtAddress = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.materialCard1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.panel2);
            this.materialCard1.Controls.Add(this.panel1);
            this.materialCard1.Depth = 0;
            this.materialCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(0, 0);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(800, 450);
            this.materialCard1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mmltxtAddress);
            this.panel1.Controls.Add(this.mtxtName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(14, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 363);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mlblConnectionState);
            this.panel2.Controls.Add(this.materialLabel1);
            this.panel2.Controls.Add(this.mbtnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(14, 386);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(772, 50);
            this.panel2.TabIndex = 1;
            // 
            // mbtnSave
            // 
            this.mbtnSave.AutoSize = false;
            this.mbtnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mbtnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.mbtnSave.Depth = 0;
            this.mbtnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.mbtnSave.HighEmphasis = true;
            this.mbtnSave.Icon = null;
            this.mbtnSave.Location = new System.Drawing.Point(648, 0);
            this.mbtnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.mbtnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(124, 50);
            this.mbtnSave.TabIndex = 2;
            this.mbtnSave.Text = "Save";
            this.mbtnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.mbtnSave.UseAccentColor = false;
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(0, 0);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(107, 50);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "Connection state:";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mlblConnectionState
            // 
            this.mlblConnectionState.Depth = 0;
            this.mlblConnectionState.Dock = System.Windows.Forms.DockStyle.Left;
            this.mlblConnectionState.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mlblConnectionState.Location = new System.Drawing.Point(107, 0);
            this.mlblConnectionState.MouseState = MaterialSkin.MouseState.HOVER;
            this.mlblConnectionState.Name = "mlblConnectionState";
            this.mlblConnectionState.Size = new System.Drawing.Size(249, 50);
            this.mlblConnectionState.TabIndex = 2;
            this.mlblConnectionState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mtxtName
            // 
            this.mtxtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mtxtName.Depth = 0;
            this.mtxtName.Dock = System.Windows.Forms.DockStyle.Top;
            this.mtxtName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtName.Hint = "Name";
            this.mtxtName.LeadingIcon = null;
            this.mtxtName.Location = new System.Drawing.Point(0, 0);
            this.mtxtName.MaxLength = 50;
            this.mtxtName.MouseState = MaterialSkin.MouseState.OUT;
            this.mtxtName.Multiline = false;
            this.mtxtName.Name = "mtxtName";
            this.mtxtName.Size = new System.Drawing.Size(772, 50);
            this.mtxtName.TabIndex = 0;
            this.mtxtName.Text = "";
            this.mtxtName.TrailingIcon = null;
            // 
            // mmltxtAddress
            // 
            this.mmltxtAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mmltxtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mmltxtAddress.Depth = 0;
            this.mmltxtAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.mmltxtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mmltxtAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.mmltxtAddress.Hint = "Address";
            this.mmltxtAddress.Location = new System.Drawing.Point(0, 50);
            this.mmltxtAddress.MouseState = MaterialSkin.MouseState.HOVER;
            this.mmltxtAddress.Name = "mmltxtAddress";
            this.mmltxtAddress.Size = new System.Drawing.Size(772, 132);
            this.mmltxtAddress.TabIndex = 1;
            this.mmltxtAddress.Text = "";
            // 
            // frmAddConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.materialCard1);
            this.Name = "frmAddConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection";
            this.materialCard1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private System.Windows.Forms.Panel panel2;
        private MaterialSkin.Controls.MaterialLabel mlblConnectionState;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton mbtnSave;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialTextBox mtxtName;
        private MaterialSkin.Controls.MaterialMultiLineTextBox mmltxtAddress;
    }
}