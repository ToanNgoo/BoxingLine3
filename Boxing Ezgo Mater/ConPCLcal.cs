using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boxing_Ezgo_Mater
{
    public partial class ConPCLcal : Form
    {
        ClsConnect clsc = new ClsConnect();
        clsCompaLogfile clsp = new clsCompaLogfile();
        public string _strPclacl = string.Empty;
        public string _strFct = string.Empty;

        public ConPCLcal(string strPclcal, string strFct)
        {
            InitializeComponent();
            _strPclacl = strPclcal;
            _strFct = strFct;
        }

        private void ConPCLcal_Load(object sender, EventArgs e)
        {
            txt_likPC.Text = clsc.get_likPC(_strPclacl);
            txt_likFct.Text = clsp.loadLink(_strFct);
        }

        private void btn_ktraCon_Click(object sender, EventArgs e)
        {
            bool cnn = clsc.getCN(txt_likPC.Text);
            if (cnn == true)
            {
                btn_ConPcL.BackColor = Color.Green;
            }
            else
            {
                btn_ConPcL.BackColor = Color.Red;
            }

            bool cnf = clsc.getFct(txt_likFct.Text);
            if(cnf == true)
            {
                btn_conFct.BackColor = Color.Green;
            }
            else
            {
                btn_conFct.BackColor = Color.Red;
            }
        }
    }
}
