using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrimedBot.DatabasesInterface.Classes;

namespace TrimedBot.DatabasesInterface.Views.Forms.Connections
{
    public partial class frmAddConnection : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        public frmAddConnection()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
        }

        private async void mbtnSave_Click(object sender, EventArgs e)
        {
            var sqlHelper = new SqlHelper(mmltxtAddress.Text);
            if (await sqlHelper.ConnectionState())
            {
                mlblConnectionState.ForeColor = Color.Green;
                mlblConnectionState.Text = "Connected";

                Classes.Connections.Add(new Connection
                {
                    Name = mtxtName.Text,
                    Address = mmltxtAddress.Text
                });
            }
            else
            {
                mlblConnectionState.ForeColor = Color.Red;
                mlblConnectionState.Text = "Not connected";
            }
        }
    }
}
