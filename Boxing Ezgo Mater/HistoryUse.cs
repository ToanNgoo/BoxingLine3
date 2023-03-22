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
    public partial class HistoryUse : Form
    {
        clsDatabase clsd = new clsDatabase();
        clsDisplay clsdpl = new clsDisplay();
        public string _likPc = string.Empty;

        public HistoryUse(string lik_Pc)
        {
            InitializeComponent();
            _likPc = lik_Pc;
        }

        private void HistoryUse_Load(object sender, EventArgs e)
        {
            DataTable dt = clsd.get_NoUse(_likPc);
            clsdpl.showNoUse(dgv_NoUse, dt);
        }
    }
}
