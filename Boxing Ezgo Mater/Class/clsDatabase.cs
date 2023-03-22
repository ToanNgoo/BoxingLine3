using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.IO;

namespace Boxing_Ezgo_Mater
{
    
    class clsDatabase
    {
        //Ket noi file acess
        //string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + Application.StartupPath + @"\Database.mdb";

        public DataTable getData(string str, string conStr)
        {
            DataTable dta1 = new DataTable();
            OleDbDataAdapter da1 = new OleDbDataAdapter(str, conStr);
            da1.Fill(dta1);
            return dta1;
        }

        public DataTable insertBarPCM(ComboBox strLine, ComboBox strModel, TextBox strBarPcm, TextBox strTiTest, string strTiBox, TextBox strCodLot, string strShift, TextBox strQtyPCM, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();//Mở kết nối

                string strIns, strSle;
                strIns = "Insert Into LotBoxed (Line_No, Model_Name, Code_Lot, Barcode_PCM, Qty_PCM_In_Lot, Time_Test, Time_Box, Shift_Text) Values ('" + strLine.Text + "','" + strModel.Text + "','" + strCodLot.Text + "','" + strBarPcm.Text + "','" + strQtyPCM.Text + "','" + strTiTest.Text + "','" + strTiBox + "','" + strShift + "')";
                //getData(strIns, conStr);
                OleDbCommand cmdIns = new OleDbCommand(strIns, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                cmdIns.ExecuteNonQuery(); // thực hiện lênh SQL

                //strUpd = "Update LotBoxed Set Qty_PCM_In_Lot = '" + strQtyPCM.Text + "' Where Code_Lot = '" + strCodLot.Text + "'";
                ////getData(strUpd, conStr);
                //OleDbCommand cmdUpd = new OleDbCommand(strUpd, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                //cmdUpd.ExecuteNonQuery(); // thực hiện lênh SQL

                cn.Close();

                strSle = "Select * From LotBoxed Where Code_Lot = '" + strCodLot.Text + "'";
                return getData(strSle, conStr);            
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi update database!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dtempty = new DataTable();              
                return dtempty;
            }
        }

        public DataTable updateCodLot(TextBox strCodLot, string strRem, string qtyPO, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();

                string strUpd, strSle;
                //for (int i = 1; i <= qtyPCM ; i++)
                //{
                    strUpd = "Update LotBoxed Set Qty_PO = '" + qtyPO + "', Remark_Lot = '" + strRem + "' Where Code_Lot = '" + strCodLot.Text + "'";
                    //getData(strUpd, conStr);
                    OleDbCommand cmdUpd = new OleDbCommand(strUpd, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                    cmdUpd.ExecuteNonQuery(); // thực hiện lênh SQL  
                cn.Close();
                //}
                strSle = "Select * From LotBoxed Where Code_Lot = '" + strCodLot.Text + "'";
                return getData(strSle, conStr);                                     
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi update database!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dtempty = new DataTable();              
                return dtempty;
            }
        }

        public DataTable delNGDreLot(string txtCoLot, string strTiBox, ComboBox strModel, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();

                string strDel = "Delete * From LotBoxed Where Code_Lot ='" + txtCoLot + "'";
                //getData(strDel, conStr);
                OleDbCommand cmdDel = new OleDbCommand(strDel, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                cmdDel.ExecuteNonQuery(); // thực hiện lênh SQL  

                cn.Close();

                string strSle = "Select* From LotBoxed Where Time_Box ='" + strTiBox + "' And Model_Name = '" + strModel.Text + "'";
                return getData(strSle, conStr);               
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi delete database!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dtempty = new DataTable();
                return dtempty;
            }
        }

        public void upQtyPo(string strTime, string molNam, string strShif, string strQty, string reLot, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            
            try
            {
                string strsle = "Select * From BoxedPO";
                DataTable dt = getData(strsle, conStr);

                int errSam = 0;
                foreach(DataRow dtr in dt.Rows)
                {
                    if(strTime == dtr.ItemArray[0].ToString() && strShif == dtr.ItemArray[2].ToString())
                    {
                        errSam++;
                    }
                    else
                    {

                    }
                }

                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();
                string strIns, strUpd; //strSle;
                if(errSam == 0)
                {
                    strIns = "Insert Into BoxedPO (Time_Box, Model_Name, Shift_Text, Qty_PO, Remark_Lot) Values ('" + strTime + "','" + molNam + "','" + strShif + "','" + strQty + "','" + reLot + "')";
                    OleDbCommand cmdIns = new OleDbCommand(strIns, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                    cmdIns.ExecuteNonQuery(); // thực hiện lênh SQL  
                }
                else
                {
                    strUpd = "Update BoxedPO Set Qty_PO = '" + strQty + "' Where Shift_Text = '" + strShif + "' And Time_Box ='" + strTime + "'";
                    OleDbCommand cmdUpd = new OleDbCommand(strUpd, cn);// Khai báo và khởi tạo bộ nhớ biến cmd
                    cmdUpd.ExecuteNonQuery(); // thực hiện lênh SQL 
                }

                cn.Close();

                //strSle = "Select* From BoxedPO";
                //return getData(strSle, conStr);
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi update database!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DataTable dtempty = new DataTable();
                //return dtempty;
            }
        }
        
        public string[] modelName(string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select * From Model";
                cn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                string[] arMolname = new string[2];
                int i = 0;
                while (dr.Read())
                {
                    arMolname[i] = dr["ModelName"].ToString();
                    i++;
                }

                dr.Close();
                cn.Close();
                return arMolname;
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi lấy tên model!", "Boxing Line 3");
                string[] arEmty = new string[2];
                return arEmty;
            }
        }

        public string[,] loGin(string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select LoginName, PassWord From Dangnhap";
                cn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                string[,] arlog = new string[10, 2];
                int i = 0;
                while(dr.Read())
                {
                    arlog[i, 0] = dr["LoginName"].ToString();
                    arlog[i, 1] = dr["PassWord"].ToString();
                    i++;
                }
                dr.Close();
                cn.Close();
                return arlog;
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi lấy tài khoản Login!", "Boxing Line 3");
                string[,]arEmty = new string[10,2];
                return arEmty;
            }
        }

        public int UpDatExitPro1(TextBox strCoLo, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();

                string strSle;               
                strSle = "Select* From LotBoxed Where Code_Lot = '" + strCoLo.Text + "'";
                DataTable dt1 = getData(strSle, conStr);
                cn.Close();
             
                return dt1.Rows.Count;

            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi update data!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public void SaConPC()
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();
                string strUp = "Insert Into ConnectPCLocal (Date_Month, Hour_Text, Status) Values ('" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','No')";
                OleDbCommand cmd = new OleDbCommand(strUp, cn);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi update ConnectPCLocal database", "Boxing Line 3");
            }
        }

        public void DelConPC()
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            try
            {
                OleDbConnection cn = new OleDbConnection(conStr);
                cn.Open();
                string strDel = "Delete * From ConnectPCLocal";
                OleDbCommand cmd = new OleDbCommand(strDel, cn);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi delete ConnectPCLocal database", "Boxing Line 3");
            }
        }

        public string chkHisCon()
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            string strSel = "Select * From ConnectPCLocal";
            DataTable dt = getData(strSel, conStr);

            string rel = string.Empty;
            foreach(DataRow dtr in dt.Rows)
            {
                rel = dtr.ItemArray[2].ToString();
            }
            return rel;
        }

        public void upNoUse(string lik_Pc)
        {
            string contrPCBoxing = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            string strSel = "Select * From HistoryUse";
            DataTable dt = getData(strSel, contrPCBoxing);

            OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb");
            cn.Open();
            foreach(DataRow dtr in dt.Rows)
            {
                string strIn = string.Empty;
                strIn = "Insert Into HistoryUse Values('" + dtr.ItemArray[0].ToString() + "','" + dtr.ItemArray[1].ToString() + "','" + dtr.ItemArray[2].ToString() + "','" + dtr.ItemArray[3].ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(strIn, cn);
                cmd.ExecuteNonQuery();
            }
            cn.Close();

            //Delete database PCboxing
            string del = "Delete * From HistoryUse";
            OleDbConnection cnd = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb");
            cnd.Open();
            OleDbCommand cmdd = new OleDbCommand(del, cnd);
            cmdd.ExecuteNonQuery();
            cnd.Close();           
        }

        public void upboxPOPCLocal(string lik_Pc)
        {
            string contrPCBoxing = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            string strSel = "Select * From BoxedPO";
            DataTable dt = getData(strSel, contrPCBoxing);

            OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb");
            cn.Open();
            foreach (DataRow dtr in dt.Rows)
            {
                string strIn = string.Empty;
                strIn = "Insert Into BoxedPO Values('" + dtr.ItemArray[0].ToString() + "','" + dtr.ItemArray[1].ToString() + "','" + dtr.ItemArray[2].ToString() + "','" + dtr.ItemArray[3].ToString() + "','" + dtr.ItemArray[4].ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(strIn, cn);
                cmd.ExecuteNonQuery();
            }
            cn.Close();

            //Delete database PCboxing
            string del = "Delete * From BoxedPO";
            OleDbConnection cnd = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb");
            cnd.Open();
            OleDbCommand cmdd = new OleDbCommand(del, cnd);
            cmdd.ExecuteNonQuery();
            cnd.Close(); 
        }

        public void upLotboxPCLocal(string lik_Pc)
        {
            string contrPCBoxing = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb";
            string strSel = "Select * From LotBoxed";
            DataTable dt = getData(strSel, contrPCBoxing);

            OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb");
            cn.Open();
            foreach (DataRow dtr in dt.Rows)
            {
                string strIn = string.Empty;
                strIn = "Insert Into LotBoxed Values('" + dtr.ItemArray[0].ToString() + "','" + dtr.ItemArray[1].ToString() + "','" + dtr.ItemArray[2].ToString() + "','" + dtr.ItemArray[3].ToString() + "','" + dtr.ItemArray[4].ToString() + "','" 
                                                        + dtr.ItemArray[5].ToString() + "','" + dtr.ItemArray[6].ToString() + "','" + dtr.ItemArray[7].ToString() + "','" + dtr.ItemArray[8].ToString() + "','" + dtr.ItemArray[9].ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(strIn, cn);
                cmd.ExecuteNonQuery();
            }
            cn.Close();

            //Delete database PCboxing
            string del = "Delete * From LotBoxed";
            OleDbConnection cnd = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @Application.StartupPath + @"\Database.mdb");
            cnd.Open();
            OleDbCommand cmdd = new OleDbCommand(del, cnd);
            cmdd.ExecuteNonQuery();
            cnd.Close();
        }

        public DataTable get_NoUse(string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            string str = "Select * From HistoryUse order by Date_Month";
            return getData(str, conStr);
        }

        public void cpy_files(string pthPCboxing, string folder_Address, string lik_Pc, bool _overW)
        {
            string targetpth = lik_Pc + folder_Address;
            string[] folrPCboxing = Directory.GetDirectories(pthPCboxing);
            for(int i = 0; i < folrPCboxing.Length; i++)
            {
                string[] filPCboxing = Directory.GetFiles(folrPCboxing[i]);
                foreach(string fil in filPCboxing)
                {
                    string filName = Path.GetFileName(fil);
                    string destFile = Path.Combine(targetpth, filName);
                    File.Copy(fil, destFile, _overW);
                }
                Directory.Delete(folrPCboxing[i], true);
            }           
        }

        public void get_ttlogFu(string lik_logFu, string[] foldeer, string lik_Pc)
        {
            for (int i = 0; i < foldeer.Length; i++)
            {
                if (Directory.Exists(lik_logFu + "\\" + foldeer[i]))
                {
                    string[] files = Directory.GetFiles(lik_logFu + "\\" + foldeer[i]);

                    string[] modl = new string[foldeer.Length]; 
                    string[] dt_month = new string[foldeer.Length];
                    string[] shf = new string[foldeer.Length];
                    int[] qty = new int[foldeer.Length];
                    int j = 0;
                    foreach(string fil in files)
                    {
                        if (fil.Contains("20210802_Function Test"))
                        {
                            //get model                                                                      
                            if (Path.GetFileName(fil).Contains("20210802_Function Test Ezgo Master"))
                            {
                                modl[j] = "Ezgo Master";
                            }
                            else if (Path.GetFileName(fil).Contains("20210802_Function Test Ezgo Slave"))
                            {
                                modl[j] = "Ezgo Slave";
                            }
                            else
                            { }

                            //get date_month, shift
                            DateTime dt = File.GetLastWriteTime(fil);
                            //string[] ardt = dt.ToString().Split(' ');
                            string t = Convert.ToDateTime(foldeer[i].Substring(4, 2) + "/" + foldeer[i].Substring(6, 2) + "/" + foldeer[i].Substring(0, 4)).ToShortDateString();
                            dt_month[j] = t;
                            shf[j] = find_shift(dt);

                            //get qty
                            StreamReader sr = new StreamReader(fil);
                            int count_qty = 0;
                            while (sr.EndOfStream == false)
                            {
                                string strL = sr.ReadLine();
                                string[] arrStrL = strL.Split(',');
                                if (arrStrL[2] == "OK")
                                {
                                    count_qty++;
                                }
                            }
                            sr.Close();
                            qty[j] = count_qty;

                            //Chuyen file khac
                            j++;
                        }                      
                    }

                    //Xu ly cac array thong tin cua folder
                    for(int m = 0; m < foldeer.Length; m++)
                    {
                        if (modl[m] != "" && dt_month[m] != "" && shf[m] != "" && qty[m] != 0)
                        {
                            for (int n = m + 1; n < foldeer.Length; n++)
                            {
                                if (modl[n] != null && dt_month[n] != null && shf[n] != null)
                                {
                                    if ((modl[m] == modl[n]) && (dt_month[m] == dt_month[n]) && (shf[m] == shf[n]))
                                    {
                                        qty[m] = qty[m] + qty[n];
                                        modl[n] = "";
                                        dt_month[n] = "";
                                        shf[n] = "";
                                        qty[n] = 0;
                                    }
                                }
                                if (modl[n] == null || dt_month[n] == null || shf[n] == null)
                                { break; }
                            }
                        }   
                        if(modl[m] == null || dt_month[m] == null || shf[m] == null)
                        { break; }
                    }
                    //Viet txt file
                    for(int k = 0; k < foldeer.Length; k++)
                    {
                        if (modl[k] != "" && dt_month[k] != "" && shf[k] != "" && qty[k] != 0)
                        {
                            string strtxt = modl[k] + " " + dt_month[k] + " " + shf[k] + " " + qty[k].ToString();
                            if (!File.Exists(lik_Pc + "\\HistoryUse\\txtFCT.txt"))//chua ton tai file txt
                            {
                                StreamWriter sw = File.CreateText(lik_Pc + "\\HistoryUse\\txtFCT.txt");
                                sw.WriteLine(strtxt);
                                sw.Close();
                            }
                            else//da ton tai file txt
                            {
                                StreamWriter sw = File.AppendText(lik_Pc + "\\HistoryUse\\txtFCT.txt");
                                sw.WriteLine(strtxt);
                                sw.Close();
                            }
                        }
                        if (modl[k] == null || dt_month[k] == null || shf[k] == null)
                        { break; }
                    }                   
                }
            }            
        }

        public string[] ssFCTBox(string lik_Pc)
        {
            try
            {
                StreamReader swF = new StreamReader(lik_Pc + "\\HistoryUse\\txtFCT.txt");                
                string[] err = new string[100];
                int i = 0;
                while (swF.EndOfStream == false)
                {
                    StreamReader swB = new StreamReader(lik_Pc + "\\HistoryUse\\txtBoxing.txt");
                    int ersame = 0;
                    bool erss = false;
                    string strF = swF.ReadLine();
                    string[] arStrF = strF.Split(' ');
                    while (swB.EndOfStream == false)
                    {
                        string strB = swB.ReadLine();
                        string[] arStrB = strB.Split(' ');
                        if (arStrF[0] == arStrB[0] && arStrF[1] == arStrB[1] && arStrF[2] == arStrB[2] && arStrF[3] == arStrB[3])//Ezgo[0] Master[1] 20220114[2] Ngày[3] ...[4] 
                        {
                            float t = float.Parse(arStrB[4]) / float.Parse(arStrF[4]);
                            if (t < 0.9f)
                            {
                                err[i] = arStrF[0] + " " + arStrF[1] + " " + arStrF[2] + " " + arStrF[3] + " Less(B/F=" + t.ToString("0.00") + ")";
                                i++;
                            }
                            erss = true;
                        }
                        if (erss == false)
                        {
                            ersame++;
                        }
                    }
                    swB.Close();
                    if (ersame != 0)
                    {
                        err[i] = arStrF[0] + " " + arStrF[1] + " " + arStrF[2] + " " + arStrF[3] + " NO";
                        i++;
                    }
                }
                swF.Close();                
                return err;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không thể truy cập txtFCT/txtBoxing", "Boxing Line 3");
                string[] arrer = new string[5] {"", "", "", "", ""};
                return arrer;
            }           
        }

        public void savDatNoUse(string[] noUse, string lik_Pc)
        {                
            DataTable dt = getData("Select * From HistoryUse", @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb");
            OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb");
            cn.Open();
            for (int i = 0; i < noUse.Length; i++)
            {
                bool sam = false;                
                if(noUse[i] != null)
                {
                    string[] arrNoUse = noUse[i].Split(' ');
                    foreach(DataRow dtr in dt.Rows)
                    {                        
                        if(arrNoUse[0] + " " + arrNoUse[1] == dtr.ItemArray[0].ToString() && arrNoUse[2] == dtr.ItemArray[1].ToString() && arrNoUse[3] == dtr.ItemArray[2].ToString())
                        {
                            sam = true;
                        }
                        else
                        {
                            
                        }
                    }
                    if(sam == false)
                    {
                        string strIn = string.Empty;
                        strIn = "Insert Into HistoryUse Values('" + arrNoUse[0] + " " + arrNoUse[1] + "','" + arrNoUse[2] + "','" + arrNoUse[3] + "','" + arrNoUse[4] + "')";
                        OleDbCommand cmd = new OleDbCommand(strIn, cn);
                        cmd.ExecuteNonQuery();
                    }                     
                } 
                else
                {
                    break;
                }
            }               
            cn.Close();
        }

        public void delLotBx(string datee, string lik_Pc)
        {
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + lik_Pc + @"\Database.mdb";
            string sel = "Select * From LotBoxed";
            DataTable dt = getData(sel, conStr);

            string[] arrdt = new string[dt.Rows.Count];
            int i = 0;
            foreach(DataRow dtr in dt.Rows)
            {
                if(!dtr.ItemArray[6].ToString().Contains(datee))
                {
                    dtr.ItemArray[6] = arrdt[1];
                    i++;
                }                
            }

            OleDbConnection cn = new OleDbConnection(conStr);
            cn.Open();
            for (int j = 0; j < arrdt.Length; j++)
            {
                string strDel = "Delete * From LotBoxed Where Time_Box='" + arrdt[j] + "'";
                OleDbCommand cmd = new OleDbCommand(strDel, cn);
                cmd.ExecuteNonQuery();
            }
                cn.Close();
        }

        public void upTxtBox(string lik_Pc)
        {
            try
            {
                StreamReader srB = new StreamReader(@Application.StartupPath + "\\HistoryUse\\txtBoxing.txt");
                StreamWriter swF = File.AppendText(lik_Pc + "\\HistoryUse\\txtBoxing.txt");
                while (srB.EndOfStream == false)
                {
                    string strB = srB.ReadLine();
                    swF.WriteLine(strB);
                }
                swF.Close();
                srB.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi khi update txtBoxing (Không tìm thays file)", "Boxing Line 3");
            }            
        }

        public bool TimeBetween(DateTime time, DateTime startDateTime, DateTime endDateTime)
        {
            // get TimeSpan
            TimeSpan start = new TimeSpan(startDateTime.Hour, startDateTime.Minute, 0);
            TimeSpan end = new TimeSpan(endDateTime.Hour, endDateTime.Minute, 0);

            // convert datetime to a TimeSpan
            TimeSpan now = time.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        public string find_shift(DateTime dt)
        {
            string shift;
            DateTime dateTime = DateTime.Now;

            DateTime startDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
            DateTime endDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 20, 0, 0);

            if (TimeBetween(dt, startDateTime, endDateTime))
            {
                shift = "Ngày";
            }
            else
            {
                shift = "Đêm";
            }
            return shift;
        }
    }
}
