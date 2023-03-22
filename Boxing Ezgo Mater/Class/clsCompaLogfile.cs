using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;


namespace Boxing_Ezgo_Mater
{
    
    class clsCompaLogfile
    {
        public int countOK = 0;

        public string changeDateTime(DateTimePicker dtp1)
        {
            dtp1.Format = DateTimePickerFormat.Custom;
            dtp1.CustomFormat = "yyyyMMdd";
            return dtp1.Text;
        }

        public string[] changeDateTime2(DateTimePicker dtp2, string [] daTime, int dOl)
        {           
            DateTime dti = dtp2.Value;
            for (int i = 0; i < dOl; i++)
            {
                daTime[i] = dti.AddDays(-i).ToString("yyyyMMdd");
            }
            return daTime;
        }

        public string[] changeDateTime3(DateTimePicker dtp3, string[] dat)
        {
            DateTime dtim = dtp3.Value;
            for (int i = 1; i < 24; i++)
            {
                dat[i] = dtim.AddMinutes(i).ToString("HH:mm:ss");
            }
            return dat;
        }

        //Count qty CSV file in folder
        public int countFile(string[] namFol, string link_Sver, int dOl)
        {
            //namFol = Date time 
                int numFil = 0;
                for (int i = 0; i < dOl; i++)
                {
                    //string path = "D:\\Toan2\\Logfile line 3\\" + namFol[i];
                    string path = link_Sver + namFol[i];
                    //string path = @"\\107.107.226.225\Result\" + namFol[i];
                    //string path = @"\\107.107.226.225\D:\EZ-GO\Result\" + namFol[i];

                    DirectoryInfo dir = new DirectoryInfo(path);
                    if (dir.Exists)
                    {
                        numFil = numFil + dir.GetFiles().Length;
                    }  
                }
                return numFil;                    
        }

        //Get name logfile
        public string[] getNamLog(string[] namFol, int numFil, string link_Sver, int dOl)
        {
            //namFol = Date time
            int j = 0;
            string[] namFilCSV = new string[numFil];
            for (int i = dOl - 1; i >= 0; i--)
            {
                //string path = "D:\\Toan2\\Logfile line 3\\" + namFol[i];
                string path = link_Sver + namFol[i];
                //string path = @"\\107.107.226.225\Result\" + namFol[i];
                //string path = @"\\107.107.226.225\D\EZ-GO\Result\" + namFol[i];

                DirectoryInfo dir = new DirectoryInfo(path);
                 
                if(dir.Exists)
                {
                    foreach (FileInfo fIn in dir.GetFiles())
                    {
                        namFilCSV[j] = fIn.Name;
                        j++;
                    }
                }             
            } 
            return namFilCSV;       
        }
      
        public void reDat()
        {
            //Hàm test kết nối 2 laptop
            DirectoryInfo dir = new DirectoryInfo(@"\\107.107.226.85\Users\Administrator\Desktop\Document");
            string[] namFilCSV = new string[10];
            int j = 0;
            foreach (FileInfo fIn in dir.GetFiles())
            {
                namFilCSV[j] = fIn.Name;
                j++;
            }
        }
              
        //Đổ data từ excel file vào data table
        public DataTable[] converExtoDta(DateTimePicker dtp1, string[] daTime, string[] namFil, int numFil, string namMol, string link_Sver)
        {
            DataTable[] dta = new DataTable[numFil];

            for (int i = 0; i < numFil; i++)
            {
                try
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((daTime[j] == namFil[i].Substring(0, 8)) && (namFil[i].Contains(namMol.Substring(5, 5)))) //(namFil[i].Substring(34, 8) == namMol.Substring(0, 8)))
                        {
                            string path = link_Sver + daTime[j] + "\\" + namFil[i];
                            //string path = @"\\107.107.226.225\Result\" + daTime[j] + "\\" + namFil[i];
                            //string path = link_Sver + namFol[i];
                        
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
        
        //Tổng hợp mảng Data Table thành 1 Data Table
        public DataTable merDtaTable(DataTable[] dt, int numFil)
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
        
        public void conBarLog(DataTable dta1, string barCoPCM, TextBox relTest, TextBox qtyPCM, string namMol)
        {

            int countBar = 0, countBar2 = 0, countBar3 = 0, countBar4 = 0, countBar5 = 0, h = 0, t = 0;
            DataRow[] dtr = dta1.Select();//string.Format("LogDate Like '%" + logDate[i] + "%'")               

            for (int j = 0; j < dta1.Rows.Count; j++)
            {
                if (dtr[j]["BarcodeScan (" + namMol + ")"].ToString() == barCoPCM)
                {
                    countBar++;
                    if (dtr[j]["Result"].ToString() == "OK")
                    {
                        if (countBar == 1)
                        {
                            for (int n = j + 1; n < dta1.Rows.Count; n++)
                            {
                                if(dtr[n]["BarcodeScan (" + namMol + ")"].ToString() != barCoPCM)
                                {
                                    countBar5++;                                   
                                }
                                if (dtr[n]["BarcodeScan (" + namMol + ")"].ToString() == barCoPCM)
                                {
                                    if(dtr[n]["Result"].ToString() == "OK")
                                    {
                                        countBar4++;
                                        if(countBar4 >= 2)
                                        {
                                            h = n;
                                        }                                      
                                    }
                                    else //dtr[n]["Result"].ToString() == "NG"
                                    {
                                        countBar4 = 0;
                                        h = 0;                                                                              
                                    }                                     
                                }
                            }

                            if(countBar5 != 0)
                            {
                                relTest.Text = dtr[j]["Result"].ToString() + " " + dtr[j]["LogTime"].ToString() + " " + dtr[j]["LogDate"].ToString();
                                countOK++;
                                qtyPCM.Text = countOK.ToString();
                                countBar3++;
                                break;
                            }

                            if (countBar4 >= 2)
                            {
                                relTest.Text = dtr[h]["Result"].ToString() + " " + dtr[h]["LogTime"].ToString() + " " + dtr[h]["LogDate"].ToString();
                                countOK++;
                                qtyPCM.Text = countOK.ToString();
                                countBar3++;
                                break;
                            }
                            else
                            {
                                countBar3 = 0;
                            } 
                        }
                    }
                    else if (dtr[j]["Result"].ToString() == "NG")
                    {
                        countBar2 = 0;
                        for (int k = j + 1; k < dta1.Rows.Count; k++)
                        {
                            if (dtr[k]["BarcodeScan (" + namMol + ")"].ToString() == barCoPCM)
                            {
                                if (dtr[k]["Result"].ToString() == "OK")
                                {
                                    countBar2++;
                                    if (countBar2 >= 2)
                                    {
                                        t = k;
                                    }
                                }
                                else
                                {
                                    countBar2 = 0;
                                    t = 0;
                                }
                            }
                        }
                        if (countBar2 >= 2)
                        {
                            relTest.Text = dtr[t]["Result"].ToString() + " " + dtr[t]["LogTime"].ToString() + " " + dtr[t]["LogDate"].ToString();
                            countOK++;
                            qtyPCM.Text = countOK.ToString();
                            countBar3++;
                        }
                        else
                        {
                            countBar3 = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("PCM không có kết quả test Function", "Boxing Line 3");
                    }
                }                                               
            }
            if (countBar == 0)
            {
                MessageBox.Show("PCM không test Function", "Boxing Line 3");               
            }
            if (countBar3 == 0)
            {
                MessageBox.Show("PCM NG Funtion!", "Boxing Line 3");
            }
        }

        public void coBarLog(DataTable dta1, string barCoPCM, TextBox relTest, TextBox qtyPCM, string namMol)
        {
            int countBar = 0;
            try
            {
                DataRow[] dtr = dta1.Select(string.Format("BarcodeScan (" + namMol + ") Like '%" + barCoPCM + "%'"));
                countBar++;                              
                
                for (int j = 0; j < dta1.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        if (dtr[j]["Result"].ToString() == "OK")
                        {
                            relTest.Text = dtr[j]["Result"].ToString() + " " + dtr[j]["LogTime"].ToString() + " " + dtr[j]["LogDate"].ToString();
                            countOK++;
                            qtyPCM.Text = countOK.ToString();
                            break;
                        }
                    }
                    else
                    {
                        if (dtr[j]["Result"].ToString() == "OK")
                        {
                            if ((dtr[j + 1]["Result"].ToString() == "OK") && (dtr[j + 2]["BarcodeScan (" + namMol + ")"].ToString() == ""))
                            {
                                relTest.Text = dtr[j + 1]["Result"].ToString() + " " + dtr[j + 1]["LogTime"].ToString() + " " + dtr[j + 1]["LogDate"].ToString();
                                countOK++;
                                qtyPCM.Text = countOK.ToString();
                                break;
                            }
                        }
                        else if (dtr[j]["Result"].ToString() == "NG")
                        {
                            if (dtr[j + 1]["Result"].ToString() == "")
                            {
                                MessageBox.Show("PCM NG Funtion!", "Boxing Line 3");
                            }
                        }
                        else
                        {
                            if (dtr[j + 1]["Result"].ToString() == "")
                            {
                                MessageBox.Show("PCM NG Funtion!", "Boxing Line 3");
                            }
                        }
                    }

                    if(dtr[j]["BarcodeScan (" + namMol + ")"].ToString() == "")
                    {
                        break;
                    }                            
                }
            }
            catch (Exception)
            {

                MessageBox.Show("PCM không test Function", "Boxing Line 3");
            }                       
        }     

        //Đếm số dòng trong file Excel
        public int countRowExcel(string dTime, string strP, string strSh, ComboBox cb1, string lik_Pc)
        {
            FileStream fS = new FileStream(lik_Pc + "\\Result\\" + dTime + "\\" + strP + " " + strSh + " Boxing " + cb1.Text + " PCB V06 Ver01.CSV", FileMode.Open);
            StreamReader sR = new StreamReader(fS);
            try
            {
                int rExcel = 0;
                while (sR.EndOfStream == false)
                {
                    string str = sR.ReadLine();
                    rExcel++;
                }
                sR.Close();
                fS.Close();
                return rExcel;
            }
            catch (Exception)
            {
                sR.Close();
                fS.Close();
                MessageBox.Show("Xảy ra lỗi truy cập Excel!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }         
        }

        //Tìm PCM OK/NG theo barcode
        public void finBarPCM(TextBox txt1, TextBox txt2, TextBox txt3, DateTimePicker dtp1, int qtyRow, string strAccMol)
        {
            //Truy cập file Excel
            Excel.Application xlApp = new Excel.Application();          
            Excel.Workbook xlWB = xlApp.Workbooks.Open(@"D:\Toan2\Logfile line 3\" + changeDateTime(dtp1) + "\\" + changeDateTime(dtp1) + " 1 20210802_Function Test " + strAccMol + " PCB V06_Ver01.CSV");
            Excel.Worksheet xlWS = xlWB.Worksheets[changeDateTime(dtp1) + " 1 20210802_Function Te"];
            Excel.Range colRange = xlWS.Columns["D:D"];
            string searStr = txt1.Text;

            //Nếu có thông tin barcode mới tìm kiếm
            try
            {
                if (txt1.Text != "")
                {
                    Excel.Range relRange = colRange.Find(
                    What: searStr,
                    LookIn: Excel.XlFindLookIn.xlValues,
                    LookAt: Excel.XlLookAt.xlPart,
                    SearchOrder: Excel.XlSearchOrder.xlByColumns,
                    SearchDirection: Excel.XlSearchDirection.xlNext);

                    if (relRange.Text != txt1.Text)
                    {
                        MessageBox.Show("PCM chưa test function!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    }
                    else
                    {
                        int countRow = 1;
                        int[] row = new int[1000];
                        for (int i = 1; i < qtyRow; i++)
                        {
                            if (xlWS.Cells[i, 4].Value.ToString() == relRange.Text)
                            {
                                row[countRow] = i;
                                countRow++;
                            }
                        }
                        if (xlWS.Cells[row[1], 3].Value.ToString() == "OK")
                        {
                            txt2.Text = "OK";
                            countOK++;
                            txt3.Text = countOK.ToString();                           
                        }
                        else
                        {
                            try
                            {
                                for (int i = 1; i < countRow; i++)
                                {
                                    if ((xlWS.Cells[row[i + 1], 3].Value.ToString() == "OK") && (xlWS.Cells[row[i + 2], 3].Value.ToString() == "OK"))
                                    {
                                        txt2.Text = "OK";
                                        countOK++;
                                        txt3.Text = countOK.ToString();
                                        break;
                                    }
                                    else
                                    {
                                        txt2.Text = "NG";
                                        txt2.BackColor = Color.Red;                                    
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("PCM fucntion NG!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);                         
                            }
                        }
                    }
                }
                xlWB.Close(false);
                xlApp.Quit();  
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể check barcode PCM!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);             
            }                             
        }

        //Reset biến đếm PCM boxing
        public bool countPCMinLot(TextBox txt1, TextBox txt2, int qtyPcmBoxed)
        {
            if (countOK == qtyPcmBoxed)
            {
                txt2.Enabled = false;
                txt2.Text = "";
                MessageBox.Show("Đã đủ " + countOK + " PCM. Hãy đóng Lot chuẩn?", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Question);
                countOK = 0;
                return true;                 
            }
            else
            {
                return false;
            }
        }

        public void reSetCouOK()
        {
            countOK = 0;
        }     
 
        //Sử dụng file .txt
        public void conExcelToTxt(string[] daTime, string[] namFil, int numFil, string namMol, bool nePo, string link_Sver, int dOl)
        {
            StreamReader sR;
            string str1 = null;
            string str = "";
            int i = 0, coAr = 0;
            string[] row = null;
            for (int n = dOl - 1; n >= 0; n--)
            {
                for (int j = 0; j < numFil; j++)
                {
                    if (namFil[j].Length >= 8)
                    {
                        if ((daTime[n] == namFil[j].Substring(0, 8)) && (namFil[j].Contains(namMol.Substring(5, 5)))) //(namFil[j].Substring(34, 8) == namMol.Substring(0, 8)))
                        {
                            //string strPath = @"D:\Toan2\Logfile line 3\" + daTime[n] + "\\" + namFil[j];
                            string strPath = link_Sver + daTime[n] + "\\" + namFil[j];
                            //string strpath = @"\\107.107.226.225\Result\" + daTime[n] + "\\" + namFil[j];
                            //string strPath = @"\\107.107.226.225\D\EZ-GO\Result\" + daTime[n] + "\\" + namFil[j];

                            //Đếm số dòng trong file
                            sR = File.OpenText(strPath);
                            while (sR.EndOfStream == false)
                            {
                                str1 = sR.ReadLine();
                                coAr++;
                            }
                            sR.Close();

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
                            string dirpath = @Application.StartupPath + @"\Compare\" + daTime[0] + "-" + namMol + ".txt";
                            //string dirpath = @Application.StartupPath + @"\" + daTime[0] + "-" + namMol + ".txt";

                            StreamWriter sW;
                            if ((!File.Exists(dirpath)) || (nePo == true))
                            {
                                sW = File.CreateText(dirpath);
                                for (int k = 0; k < i; k++)
                                {
                                    sW.WriteLine(col[k]);
                                }
                                sW.Close();
                                nePo = false;
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

        public void usiListStrig(TextBox txtbar, TextBox txtRel, TextBox txtqTy, string daTime, string namMol, bool chkExLog, bool reLod, string qtStan, string qtbg)
        {
            try
            {
                int haCod = 0, tiOk = 0;// tiOk2 = 0;
                int i = 0;
                //int oK1 = 0, oK2 = 0;
                //List<string> comDatTest = new List<string>();
                //List<string> comTimTest = new List<string>();
                //List<string> comRelTest = new List<string>();
                //List<string> comBarPcm = new List<string>();  

                string[] comDatTest = new string[5];
                string[] comTimTest = new string[5];
                string[] comRelTest = new string[5];
                string[] comBarPcm = new string[5];

                string cdt = "", ctt = "", crt = "", cbp = "";
                //======================================check, so sanh barcode va .log
                OptionDefine.clsCheckTrungInformation chekInf = new OptionDefine.clsCheckTrungInformation();
                List<string> litInf = new List<string>();
                List<string> litBar = new List<string>();

                //chekInf.LoadList("BarPCM", ref litBar);

                chekInf.LoadList(ref litInf, @Application.StartupPath + @"\Compare\" + daTime + "-" + namMol + ".txt");
                //chekInf.LoadList(ref litInf, @Application.StartupPath + @"\" + daTime + "-" + namMol + ".txt"); 

                //if (chekInf.CheckDuplicateInforamation(txtbar.Text, litBar) == true)
                //{
                string[] arrStr = new string[4];
                string rel1 = "", tim1 = "", dat1 = "";
                string rel2 = "", tim2 = "", dat2 = "";
                foreach (string strr in litInf)
                {
                    arrStr = strr.Split(',');
                    if (arrStr[3] == txtbar.Text) //(strr.Substring(21, 14) == txtbar.Text)
                    {
                        haCod++;

                        comDatTest[haCod] = arrStr[0];
                        comTimTest[haCod] = arrStr[1];
                        comRelTest[haCod] = arrStr[2];
                        comBarPcm[haCod] = arrStr[3];
                        if (haCod == 1)
                        {
                            switch (arrStr[2])
                            {
                                case "OK":
                                    tiOk++;
                                    //oK1 = i;
                                    rel1 = arrStr[2];
                                    tim1 = arrStr[1];
                                    dat1 = arrStr[0];

                                    //if (tiOk >= 2)
                                    //{
                                    //    oK2 = i;
                                    //    rel2 = arrStr[2];
                                    //    tim2 = arrStr[1];
                                    //    dat2 = arrStr[0];
                                    //}
                                    break;
                                default:
                                    tiOk = 0;
                                    rel1 = "";
                                    tim1 = "";
                                    dat1 = "";
                                    break;
                            }
                        }
                        else //Have PCM test again
                        {
                            tiOk = 0;
                            for (int t = 1; t < haCod; t++)
                            {
                                for (int h = t + 1; h <= haCod; h++)
                                {
                                    if (comTimTest[h] != "")
                                    {
                                        switch (comDatTest[t] == comDatTest[h])
                                        {
                                            case true://cung ngay test
                                                if (Convert.ToDateTime(comTimTest[t]) > Convert.ToDateTime(comTimTest[h]))
                                                {
                                                    //Doi cho date time
                                                    cdt = comDatTest[t];
                                                    comDatTest[t] = comDatTest[h];
                                                    comDatTest[h] = cdt;

                                                    //Doi cho time test
                                                    ctt = comTimTest[t];
                                                    comTimTest[t] = comTimTest[h];
                                                    comTimTest[h] = ctt;

                                                    //Doi cho ket qua test
                                                    crt = comRelTest[t];
                                                    comRelTest[t] = comRelTest[h];
                                                    comRelTest[h] = crt;

                                                    //Doi cho barcode PCM
                                                    cbp = comBarPcm[t];
                                                    comBarPcm[t] = comBarPcm[h];
                                                    comBarPcm[h] = cbp;
                                                }
                                                break;
                                            default://khac ngay test, PO ca dem
                                                if (Convert.ToDateTime(comTimTest[t]) < Convert.ToDateTime(comTimTest[h]))
                                                {
                                                    //Doi cho date time
                                                    cdt = comDatTest[t];
                                                    comDatTest[t] = comDatTest[h];
                                                    comDatTest[h] = cdt;

                                                    //Doi cho time test
                                                    ctt = comTimTest[t];
                                                    comTimTest[t] = comTimTest[h];
                                                    comTimTest[h] = ctt;

                                                    //Doi cho ket qua test
                                                    crt = comRelTest[t];
                                                    comRelTest[t] = comRelTest[h];
                                                    comRelTest[h] = crt;

                                                    //Doi cho barcode PCM
                                                    cbp = comBarPcm[t];
                                                    comBarPcm[t] = comBarPcm[h];
                                                    comBarPcm[h] = cbp;
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            if ((comRelTest[haCod] == "OK") && (comRelTest[haCod - 1] == "OK"))
                            {
                                tiOk = 2;
                                rel2 = comRelTest[haCod];
                                tim2 = comTimTest[haCod];
                                dat2 = comDatTest[haCod];

                            }
                            else
                            {
                                tiOk = 0;
                                rel2 = "";
                                tim2 = "";
                                dat2 = "";

                            }
                        }
                    }
                    i++;
                }
                switch (haCod)
                {
                    case 0:
                        txtbar.ResetText();
                        MessageBox.Show("PCM không test Function");
                        break;
                    case 1:
                        if (tiOk == 1)
                        {
                            //if (chkExLog == true)
                            //{
                            if (reLod == true)
                            {
                                countOK++;
                                countOK = countOK + int.Parse(qtbg);
                            }
                            else
                            {
                                countOK++;
                            }
                            //}
                            //else
                            //{
                            //    countOK++;
                            //}
                            chekInf.SaveList(txtbar.Text, "BarPCM");
                            //txtRel.Text = litInf[oK1].Substring(18, 2) + " " + litInf[oK1].Substring(9, 8) + " " + litInf[oK1].Substring(0, 8);
                            txtRel.Text = rel1 + " " + tim1 + " " + dat1;
                            txtqTy.Text = countOK.ToString() + "/" + qtStan;
                            //MessageBox.Show("PCM OK");
                        }
                        else
                        {
                            txtbar.ResetText();
                            MessageBox.Show("PCM NG Function", "Boxing Line 3");
                        }
                        break;
                    default:
                        if (tiOk == 2)
                        {
                            //if (chkExLog == true)
                            //{
                            if (reLod == true)
                            {
                                countOK++;
                                countOK = countOK + int.Parse(qtbg);
                            }
                            else
                            {
                                countOK++;
                            }
                            //}
                            //else
                            //{
                            //    countOK++;
                            //}
                            chekInf.SaveList(txtbar.Text, "BarPCM");
                            //txtRel.Text = litInf[oK2].Substring(18, 2) + " " + litInf[oK2].Substring(9, 8) + " " + litInf[oK2].Substring(0, 8);
                            txtRel.Text = rel2 + " " + tim2 + " " + dat2;
                            txtqTy.Text = countOK.ToString() + "/" + qtStan;
                            //MessageBox.Show("PCM OK");
                        }
                        else
                        {
                            txtbar.ResetText();
                            MessageBox.Show("PCM NG Function, test lại OK Function 1 lần", "Boxing Line 3");
                        }
                        break;
                }         
            }
            catch (Exception)
            {
                MessageBox.Show("PCM test Function quá 5 lần/Lỗi file logfunction.txt", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }                  
        }

        public bool chekSamLot(TextBox txtLot)
        {
            OptionDefine.clsCheckTrungInformation chekInf = new OptionDefine.clsCheckTrungInformation();
            List<string> litLot = new List<string>();

            chekInf.LoadList("CodLot", ref litLot);

            return chekInf.CheckDuplicateInforamation(txtLot.Text, litLot);
        }

        public void savLot(TextBox txtLot)
        {
            OptionDefine.clsCheckTrungInformation chekInf = new OptionDefine.clsCheckTrungInformation();

            chekInf.SaveList(txtLot.Text, "CodLot");
        }

        public bool chekSamBar(TextBox txtBar)
        {
            OptionDefine.clsCheckTrungInformation chekInf = new OptionDefine.clsCheckTrungInformation();
            List<string> litBar = new List<string>();

            chekInf.LoadList("BarPCM", ref litBar);

            return chekInf.CheckDuplicateInforamation(txtBar.Text, litBar);
        }

        public string loadLink(string strtg)
        {
            string link_Sver = string.Empty;

            try
            {
                FileStream FS = new FileStream(@Application.StartupPath + "\\LinkSever.log", FileMode.Open);
                StreamReader SR = new StreamReader(FS);
                while (SR.EndOfStream == false)
                {
                    link_Sver = SR.ReadLine();
                }
                SR.Close();
                FS.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi file LinkSever.log", "Lỗi");
                link_Sver = strtg;
            }
            
            return link_Sver;
        }

        public string redCSV(string dTime, string strP, string strSh, ComboBox cb1, string lik_Pc)
        {
            FileStream fS = new FileStream(lik_Pc + "\\Result\\" + dTime + "\\" + strP + " " + strSh + " Boxing " + cb1.Text + " PCB V06 Ver01.CSV", FileMode.Open);
            StreamReader sR = new StreamReader(fS);
           
            string qty = "";
            try
            {
                string str;
                string[] accStr = null;
                while (sR.EndOfStream == false)
                {
                    str = sR.ReadLine();
                    accStr = str.Split(',');

                    qty = accStr[8];                 
                }
                sR.Close();
                fS.Close();
                return qty;
            }
            catch (Exception)
            {
                sR.Close();
                fS.Close();
                MessageBox.Show("Xảy ra lỗi truy cập logfile!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }       
        }      
    }
}
