using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Boxing_Ezgo_Mater
{
    class ClsConnect
    {
        public string get_likPC(string strtg)
        {
            string lik_PcSev = string.Empty;
            try
            {
                FileStream fs = new FileStream(@Application.StartupPath + "\\Link_PCLocal.log", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                while (sr.EndOfStream == false)
                {
                    lik_PcSev = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi file Link_PCLocal.log", "Lỗi");
                lik_PcSev = strtg;
            }

            return lik_PcSev;
        }

        public bool getCN(string str)
        {
            try
            {
                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + str + @"\Database.mdb");
                con.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }  
        
        public bool getFct(string strfct)
        {
            //string strf = strfct.Substring(0, 24);
            if(!Directory.Exists(strfct))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
