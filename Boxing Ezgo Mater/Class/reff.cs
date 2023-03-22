using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
//using Excel = Microsoft.Office.Interop.Excel;

namespace Boxing_Ezgo_Mater
{
    class reff
    {
        //public static string filePath = "D:\\Toan\\11.xlsx";
        //string constr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='" + filePath + "';Extended Properties=\"Excel 12.0;HDR=YES;\""; 
        //string path = "";
        //Excel._Application ex = new Excel.Application();
        //Excel._Workbook wb = new Excel.Workbook();
        //Excel._Worksheet ws = new Excel.Worksheet();

        //public void Excel(string path, int sheet)
        //{
        //    this.path = path;
        //    wb = ex.Workbooks.Open(path);
        //    ws = wb.Worksheets[sheet];
            
        //}
        //public string readCell(int i, int j)
        //{
        //    i++;
        //    j++;
        //    if (ws.Cells[i,j].Value != null)
        //    {
        //        return ws.Cells[i, j].Value;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        public void exportCsv(DataGridView dtg1,string path, bool check)
        {
            StringBuilder sb = new StringBuilder();
           
            for (int i = 1; i < dtg1.Columns.Count + 1; i++)
            {
                if (check == false)
                {
                    sb.Append(dtg1.Columns[i - 1].HeaderText);
                    sb.Append(",");
                }              
            }
            if (check == false)
            {
                sb.Append("\n");
            }
            

            for (int i = 0; i < dtg1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dtg1.Columns.Count; j++)
                {
                    if (dtg1.Rows[i].Cells[j].Value != null)
                    {
                        sb.Append(dtg1.Rows[i].Cells[j].Value.ToString());
                        sb.Append(",");
                    }                    
                }
                sb.Append("\n");
            }
            

            if(check == false)
            {
                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
            }
        }

        public void conExcelToTxt(string[] daTime, string[] namFil, int numFil, string namMol)
        {
            StreamReader sR;
            string str1 = null;
            string str ="";
            int i = 0, coAr = 0;
            string[] row = null;
            for (int n = 2; n >= 0;n--)
            {
                for (int j = 0; j < numFil; j++)
                {
                    if(namFil[j].Length >= 40)
                    {
                        if ((daTime[n] == namFil[j].Substring(0, 8)) && (namFil[j].Substring(34, 8) == namMol.Substring(0, 8)))
                        {
                            string strPath = @"D:\Toan2\Logfile line 3\" + daTime[n] + "\\" + namFil[j];

                            //Đếm số dòng trong file
                            sR = File.OpenText(strPath);
                            while (sR.EndOfStream == false)
                            {
                                str1 = sR.ReadLine();
                                coAr++;
                            }

                            //Gán excel vào mảng
                            string[] col = new string[coAr];
                            sR = File.OpenText(strPath);
                            i = 0;
                            while (sR.EndOfStream == false)
                            {
                                str = sR.ReadLine();
                                row = str.Split(',');
                                for (int m = 0; m < 4; m++)
                                {
                                    if (m == 0)
                                    {
                                        col[i] = "";
                                        col[i] = col[i] + row[m];
                                    }
                                    else
                                    {
                                        col[i] = col[i] + "," + row[m];
                                    }
                                }

                                i++;
                            }
                            sR.Close();

                            //Viết vào txt  
                            string dirpath = @"C:\Users\Administrator\Desktop\Toan\1. Project\1. Boxing line 3\Boxing Ezgo Mater\" + daTime[0] + "-" + namMol + ".log";


                            StreamWriter sW;
                            if (!File.Exists(dirpath))
                            {
                                sW = File.CreateText(dirpath);
                                for (int k = 0; k < i; k++)
                                {
                                    sW.WriteLine(col[k]);
                                }
                                sW.Close();
                            }
                            else
                            {
                                sW = File.AppendText(dirpath);

                                for (int k = 0; k < i; k++)
                                {
                                    sW.WriteLine(col[k]);
                                }
                                sW.Close();
                            }

                        }
                    }                   
                }    
            }
                
        }        

        public int countFile1(string[] namFol)
        {
            //namFol = Date time 
            int numFil = 0;
            for (int i = 0; i < 3; i++)
            {
                string path = "D:\\Toan2\\Logfile line 3\\" + namFol[i];
                //string path = @"\\107.107.226.225\Result\" + namFol[i];
                DirectoryInfo dir = new DirectoryInfo(path);
                numFil = numFil + dir.GetFiles().Length;
            }
            return numFil;
        }

        public string[] getNamLog1(string[] namFol, int numFil)
        {
            //namFol = Date time
            int j = 0;
            string[] namFilCSV = new string[numFil];
            for (int i = 2; i >= 0; i--)
            {
                string path = "D:\\Toan2\\Logfile line 3\\" + namFol[i];
                //string path = @"\\107.107.226.225\Result\" + namFol[i];
                DirectoryInfo dir = new DirectoryInfo(path);
                              
                foreach (FileInfo fIn in dir.GetFiles())
                {
                    namFilCSV[j] = fIn.Name;
                    j++;
                }
            }
            return namFilCSV;
        }

        public DataTable[] converExtoDta1(DateTimePicker dtp1, string[] daTime, string[] namFil, int numFil, string namMol)
        {
            DataTable[] dta = new DataTable[numFil];

            for (int i = 0; i < numFil; i++)
            {
                try
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((daTime[j] == namFil[i].Substring(0, 8)) && (namFil[i].Substring(34, 8) == namMol.Substring(0, 8)))
                        {
                            string path = @"D:\Toan2\Logfile line 3\" + daTime[j] + "\\" + namFil[i];
                            //string path = @"\\107.107.226.225\Result\" + daTime[i] + "\\" + namFil[i];
                            Excel.Application app = new Excel.Application();
                            Excel.Workbook wB = app.Workbooks.Open(path);
                            Excel.Worksheet wS = wB.Worksheets[1];

                            //Đếm số dòng, số cột file excel
                            int rows = wS.UsedRange.Rows.Count;

                            //Khai báo new Data Table
                            DataTable dt = new DataTable();
                            //Khai báo vị trí header excel
                            int no_r = 1;

                            //Đổ header text vào data table
                            for (int c = 1; c <= 4; c++)
                            {
                                string colNam = wS.Cells[1, c].Text;
                                dt.Columns.Add(colNam);
                                no_r = 2;
                            }

                            //Đổ excel vào data table
                            for (int r = no_r; r <= rows; r++)
                            {
                                DataRow dtr = dt.NewRow();
                                for (int c = 1; c <= 4; c++)
                                {
                                    dtr[c - 1] = wS.Cells[r, c].Text;
                                }
                                dt.Rows.Add(dtr);
                            }
                            wB.Close();
                            app.Quit();

                            dta[i] = dt;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            //Return mảng Data Table
            return dta;
        }

        public DataTable merDtaTable1(DataTable[] dt, int numFil)
        {
            DataTable dtall = new DataTable();
            for (int i = 0; i < numFil; i++)
            {
                if (dt[i] != null)
                {
                    dtall.Merge(dt[i]);
                }
            }
            return dtall;
        }  

        public void usiListStrig(TextBox txtbar, TextBox txtRel, TextBox txtqTy, string daTime, string namMol)
        {
            int haCod = 0, tiOk = 0;// tiOk2 = 0;
            int i = 0;
            int countOk = 0;
            int oK1 = 0, oK2 = 0;
            //======================================check, so sanh barcode va .log
            OptionDefine.clsCheckTrungInformation chekInf = new OptionDefine.clsCheckTrungInformation();
            List<string> litInf = new List<string>();
            List<string> litBar = new List<string>();
            
            chekInf.LoadList("BarPCM", ref litBar);

            chekInf.LoadList(ref litInf, @"C:\Users\Administrator\Desktop\Toan\1. Project\1. Boxing line 3\Boxing Ezgo Mater\" + daTime + "-" + namMol + ".log");

            if(chekInf.CheckDuplicateInforamation(txtbar.Text, litBar) == true)
            {
                foreach (string strr in litInf)
                {
                    if (strr.Length >= 30)
                    {
                        if (strr.Substring(21, 14) == txtbar.Text)
                        {
                            haCod++;
                            if (strr.Substring(18, 2) == "OK")
                            {
                                tiOk++;
                                oK1 = i;
                                if (tiOk == 2)
                                {
                                    oK2 = i;
                                }
                            }
                            else if (strr.Substring(18, 2) == "NG")
                            {
                                tiOk = 0;
                            }
                            else
                            { }
                        }
                    }
                    i++;
                }

                if (haCod == 0)
                {
                    MessageBox.Show("PCM không test Function");
                }
                else if (haCod == 1)
                {
                    if (tiOk == 1)
                    {
                        chekInf.SaveList(txtbar.Text, "BarPCM");
                        txtRel.Text = litInf[oK1].Substring(18, 2) + litInf[oK1].Substring(9, 8) + litInf[oK1].Substring(0, 8);
                        countOk++;
                        txtqTy.Text = countOk.ToString();
                        MessageBox.Show("PCM OK");
                    }
                    else
                    {
                        MessageBox.Show("PCM NG");
                    }
                }
                else
                {
                    if (tiOk >= 2)
                    {
                        chekInf.SaveList(txtbar.Text, "BarPCM");
                        txtRel.Text = litInf[oK2].Substring(18, 2) + litInf[oK2].Substring(9, 8) + litInf[oK2].Substring(0, 8);
                        countOk++;
                        txtqTy.Text = countOk.ToString();
                        MessageBox.Show("PCM OK");
                    }
                    else
                    {
                        MessageBox.Show("PCM NG");
                    }
                }      
            }
            else
            {
                MessageBox.Show("PCM đã boxing");
            }                  
        }
    }
}
