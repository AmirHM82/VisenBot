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
using TrimedBot.DatabasesInterface.Views.Forms.Connections;

namespace TrimedBot.DatabasesInterface.Views.Forms
{
    public partial class frmMain : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        public frmMain()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
        }

        private void mbtnAddConnection_Click(object sender, EventArgs e)
        {
            var addConnectionForm = new frmAddConnection();
            addConnectionForm.ShowDialog();
        }
    }
}
