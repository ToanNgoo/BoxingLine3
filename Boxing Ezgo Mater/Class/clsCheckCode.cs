using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace Boxing_Ezgo_Mater
{
    class clsCheckCode
    {
        //Ket noi file acess
        //string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + Application.StartupPath + @"\Database.mdb";
        public int checkBarCodePCM(string barCoPCM)
        {
            int erBarcod = 0;
            int iBar = 1;
            int ib3it0 = 0, ib3it1 = 0;
            int ib5it012 = 0, ib5it3 = 0;

            //Check format
            if (barCoPCM.Length != 14)
            {
                erBarcod++;
            }
            else
            {
                foreach (char item in barCoPCM)
                {
                    switch(iBar)
                    {
                        case 1:
                            if((item < 48) || (item > 50))
                            {
                                erBarcod++;
                            }
                            break;
                        case 2:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 3:
                            if (item == '0')
                            {
                                ib3it0++;
                            }
                            else if (item == '1')
                            {
                                ib3it1++;
                            }
                            else
                            {
                                erBarcod++;
                            }
                            break;
                        case 4:
                            if ((ib3it0 != 0) && ((item < 48) || (item > 57)))
                            {
                                erBarcod++;
                            }
                            else if ((ib3it1 != 0) && ((item < 48) || (item > 50)))
                            {
                                erBarcod++;
                            }
                            else
                            { }
                            break;
                        case 5:
                            if ((item == '0') || (item == '1' || (item == '2')))
                            {
                                ib5it012++;
                            }
                            else if (item == '3')
                            {
                                ib5it3++;
                            }
                            else
                            {
                                erBarcod++;
                            }
                            break;
                        case 6:
                            if ((ib5it012 != 0) && ((item < 48) || (item > 57)))
                            {
                                erBarcod++;
                            }
                            else if ((ib5it3 != 0) && ((item < 48) || (item > 49)))
                            {
                                erBarcod++;
                            }
                            else
                            { }
                            break;
                        case 7:
                            if(item != 'V')
                            {
                                erBarcod++;
                            }
                            break;
                        case 8:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 9:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 10:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 11:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 12:
                            if((item < 48) || (item > 57))
                            {
                                erBarcod++;
                            }
                            break;
                        case 13:
                            if(item != '0')
                            {
                                erBarcod++;
                            }
                            break;
                        case 14 :
                            if((item != 'V') && (item != 'I'))
                            {
                                erBarcod++;
                            }
                            break;
                        default:
                            break;
                    }                   
                    iBar++;
                }                    
            }
            return erBarcod;
        }
        public int checkCodeLot(string coLot, string datime)
        {
            int erCoLot = 0;
            int iLot = 1;
            int il13itp = 0, il13ita = 0;

            char[] arDateTime = new char[9];
            int coArDaTi = 0;
            //Check date time
            foreach (char charTime in datime)
            {
                arDateTime[coArDaTi] = charTime;
                coArDaTi++;
            }
            //Check format code
            if (coLot.Length != 17)
            {
                erCoLot++;
            }
            else
            {
                foreach (char item in coLot)
                {
                    switch(iLot)
                    {
                        case 1:
                            if(item != 'V')
                            {
                                erCoLot++;
                            }
                            break;
                        case 2:
                            if(item != 'P')
                            {
                                erCoLot++;
                            }
                            break;
                        case 3:
                            if(item != 'M')
                            {
                                erCoLot++;
                            }
                            break;
                        case 4:
                            if(item != arDateTime[2])
                            {
                                erCoLot++;
                            }
                            break;
                        case 5:
                            if(item != arDateTime[3])
                            {
                                erCoLot++;
                            }
                            break;
                        case 6:
                            if(item != arDateTime[4])
                            {
                                erCoLot++;
                            }
                            break;
                        case 7:
                            if(item != arDateTime[5])
                            {
                                erCoLot++;
                            }
                            break;
                        case 8:
                            if(item != arDateTime[6])
                            {
                                erCoLot++;
                            }
                            break;
                        case 9:
                            if(item != arDateTime[7])
                            {
                                erCoLot++;
                            }
                            break;
                        case 10:
                            if(item != '0')
                            {
                                erCoLot++;
                            }
                            break;
                        case 11:
                            if(item != '1')
                            {
                                erCoLot++;
                            }
                            break;
                        case 12:
                            if((item != 'B') && (item != 'S'))
                            {
                                erCoLot++;
                            }
                            break;
                        case 13:
                            if (item == 'P')
                            {
                                il13itp++;
                            }
                            else if (item == 'A')
                            {
                                il13ita++;
                            }
                            else
                            {
                                erCoLot++;
                            }
                            break;
                        case 14:
                            if ((il13itp != 0) && (item != '1'))
                            {
                                erCoLot++;
                            }
                            else if ((il13ita != 0) && (item != '3') && (item != '4'))
                            {
                                erCoLot++;
                            }                           
                            else
                            { }
                            break;
                        case 15:
                            if((item < 48) || (item > 57))
                            {
                                erCoLot++;
                            }
                            break;
                        case 16:
                            if((item < 48) || (item > 57))
                            {
                                erCoLot++;
                            }
                            break;
                        case 17:
                            if((item < 48) || (item > 57))
                            {
                                erCoLot++;
                            }
                            break;
                        default:
                            break;
                    }
                    iLot++;                                      
                }
            }
            return erCoLot;          
        }

        public int checkSameCodePCM(string cod, string strTime, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            // Gán code đã boxing vào array trung gian
            int i = 0;
            string[] arBaPcm = new string[500];
            int erSame = 0;
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Barcode_PCM From LotBoxed";// Where Time_Box ='" + strTime + "'";
                cn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();                          
                while (dr.Read())
                {
                    arBaPcm[i] = dr["Barcode_PCM"].ToString();
                    i++;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm Barcode PCM giống nhau!", "Boxing Line 3");
            }

            //So sánh để tìm code Same
            for (int j = 0; j < i; j++)
            {
                if (cod == arBaPcm[j])
                {
                    erSame++;
                }               
            }
            return erSame;
        }

        public int checkSameCodeLot(string cod, string strTime, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            // Gán code đã boxing vào array trung gian
            int i = 0;
            string[] arCodLot = new string[500];
            int erSame = 0;
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Code_Lot From LotBoxed";// Where Time_Box ='" + strTime + "'";
                cn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    arCodLot[i] = dr["Code_Lot"].ToString();
                    i++;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm Code Lot giống nhau!", "Boxing Line 3");
            }

            //So sánh để tìm code Same
            for (int j = 0; j < i; j++)
            {
                if (cod == arCodLot[j])
                {
                    erSame++;
                }
            }
            return erSame;
        }

        public string checkWrongModel(string codPcm)
        {
            //Check boxing nhầm model
            string mod ="";
            foreach (char item in codPcm)
            {
                mod = item.ToString();
            }
            if (mod == "V")
            {
                return "Ezgo Master";
            }
            else if (mod == "I")
            {
                return "Ezgo Slave";
            }           
            else
            {
                return "No model of line 3";
            }
        }    
  
        public string checkWrongModelCodeLot(string codLot)
        {
            //Check wrong model code Lot
            string modLot = "";
            int i = 1;
            foreach (char item in codLot)
            {
                if (i == 14)
                {
                    modLot = item.ToString();
                }
                i++;
            }
            if (modLot == "4")
            {
                return "Ezgo Slave";
            }
            else if (modLot == "3")
            {
                return "Ezgo Master";
            }
            else
            {
                return "No model of line 3";
            }
        }
    }
}
