using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;

namespace Boxing_Ezgo_Mater
{
    public partial class Form1 : Form
    {
        clsCompaLogfile cls1 = new clsCompaLogfile();
        clsDatabase cls2 = new clsDatabase();
        clsDisplay cls3 = new clsDisplay();
        clsCheckCode cls4 = new clsCheckCode();
        clsExportData cls5 = new clsExportData();
        ClsConnect clsc = new ClsConnect();

        //Array và biến số lượng PCM đang boxing
        public string[] arBarPCM = new string[21];
        public int couArBarPCM;

        //Biến tiêu chuẩn số lượng đóng Lot
        public int stanQty;

        //Biến ca ngày ca đêm
        public string strSh;
        
        //Biến PO
        public string strP;

        //Biến Data Table so sánh logfile
        public DataTable daTblFath;

        //Biến Qty PO
        public int qtPo = 0;

        //Biến load log liên tục
        public bool chek;

        //Biến reload program
        public bool reLoad = false;   
   
        //Biến thêm PCM
        public bool addPcm = false;

        //Biến Qty PCM đang Box máy treo
        public string qtbg;

        //Biến lỗi box qua qty PO
        bool erBox = false;

        //Biến remark lot
        string strLot;

        //Link PC local
        string lik_PCcal = string.Empty;

        //Biến day of log
        int dOl = 0;

        //Biến tạo PO
        public bool chkPo;

        //Biến đếm 300s
        public int cout_box;
        public int pos_X, pos_Y;
        public bool ftim3 = false;

        public Form1()
        {
            InitializeComponent();       
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Khóa các mục khi chưa Login
            daTiPic1.Enabled = false;
            rabtnDay.Enabled = false;
            rabtnNight.Enabled = false;
            radbtnPOm.Enabled = false;
            radbtnOBA.Enabled = false;
            labLine.Enabled = false;
            cobbLine.Enabled = false;
            labModel.Enabled = false;
            cobbModel.Enabled = false;
            labBarPCM.Enabled = false;
            txtBarPCM.Enabled = false;
            labRelFu.Enabled = false;
            txtRelFu.Enabled = false;
            labQty.Enabled = false;
            txtQty.Enabled = false;
            labCodLot.Enabled = false;
            txtCodLot.Enabled = false;
            labttPO.Enabled = false;
            txtttPO.Enabled = false;           
            btnCreLotLe.Enabled = false;
            tabControl1.Enabled = false;
            grBoSet.Enabled = false;
            btnCrePo.Enabled = false;
            btnCloPO.Enabled = false;
            radBtnRelo.Enabled = false;
            labqtPO.Enabled = false;
            txtqtPO.Enabled = false;
            btnAddPcm.Enabled = false;                       
           
            //Get link PC local  
            try
            {
                lik_PCcal = clsc.get_likPC(txt_PcLocal.Text);
                if(clsc.getCN(lik_PCcal) == true)
                {
                    toolStripStatusLabel1.Text = "PC Local Connected";
                    toolStripStatusLabel1.ForeColor = Color.Black;
                    toolStripStatusLabel1.BackColor = Color.Green;
                    //Kiem tra xem co lsu ko connect ko
                    if(cls2.chkHisCon() == "No")
                    {
                        //Copy logfile -> xoa logfile tai PC Boxing
                        cls2.cpy_files(@Application.StartupPath + "\\Result\\", "\\Result\\", lik_PCcal, false);
                        //Copy txtBoxing
                        cls2.upTxtBox(lik_PCcal);
                        //Delete database history no connect PC Local
                        cls2.DelConPC(); 
                        //Update database history no use program
                        cls2.upNoUse(lik_PCcal);
                        //Update database BoxedPO
                        cls2.upboxPOPCLocal(lik_PCcal);
                        //Update database LotBoxed
                        //cls2.upLotboxPCLocal(lik_PCcal);
                    }                                       
                }
                else
                {
                    lik_PCcal = @Application.StartupPath;
                    if(clsc.getCN(lik_PCcal) == true)
                    {
                        toolStripStatusLabel1.Text = "Database Available";
                        toolStripStatusLabel1.ForeColor = Color.Black;
                        toolStripStatusLabel1.BackColor = Color.Green;
                        //Save database history no connect to PC Local
                        cls2.SaConPC();                        
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Database Unavailable";
                        toolStripStatusLabel1.ForeColor = Color.Black;
                        toolStripStatusLabel1.BackColor = Color.Red;
                    }
                }               
            }
            catch (Exception)
            {   }

            //Check setup  link
            if(clsc.getCN(txt_PcLocal.Text) == true)
            {
                btn_PcLocal.BackColor = Color.Green;
            }
            else
            {
                btn_PcLocal.BackColor = Color.Red;
            }
            if(clsc.getFct(txt_Fct.Text) == true)
            {
                btn_Fct.BackColor = Color.Green;
                toolStripStatusLabel2.Text = "FCT Connected Success";
                toolStripStatusLabel2.BackColor = Color.Green;
            }
            else
            {
                btn_Fct.BackColor = Color.Red;
                toolStripStatusLabel2.Text = "FCT Connected Fail";
                toolStripStatusLabel2.BackColor = Color.Red;
            }

            //Update txt Qty tiêu chuan
            StreamReader sr = new StreamReader(@Application.StartupPath + "\\QtyofStd.log");
            int count_qtSt = 0;
            while(sr.EndOfStream == false)
            {
                if (count_qtSt >= 2)
                {
                    MessageBox.Show("Lỗi file QtyofStd.log\nThông báo PE kiểm tra", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                string str = sr.ReadLine();
                string[] arrstr = str.Split(':');
                if(arrstr[0] == "Master")
                {
                    txtMas.Text = arrstr[1];
                }
                else if(arrstr[0] == "Slave")
                {
                    txtSla.Text = arrstr[1];
                }
                else
                { }                
                count_qtSt++;
            }
            sr.Close();

            //Day of logtest
            StreamReader srr = new StreamReader(@Application.StartupPath + "\\DayofLog.log");
            while(srr.EndOfStream == false)
            {
                string str = srr.ReadLine();
                dOl = int.Parse(str);
            }
            srr.Close();
            if(dOl == 0)
            {
                MessageBox.Show("Lỗi file DayofLog.log\nThông báo PE kiểm tra", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Update model name
            cobbModel.Items.AddRange(cls2.modelName(lik_PCcal));

            //Xoa file in folder History FCT
            File.Delete(lik_PCcal + "\\HistoryUse\\txtFCT.txt");

            //Xoa database LotBoxed
            cls2.delLotBx(daTiPic1.Text, lik_PCcal);

            //Timer 20h, 8h
            timer2.Start();
        }

        private void rabtnDay_CheckedChanged(object sender, EventArgs e)
        {
            strSh = rabtnDay.Text;
            //Reset qty PO
            qtPo = 0;
            txtttPO.ResetText();
        }

        private void rabtnNight_CheckedChanged(object sender, EventArgs e)
        {
            strSh = rabtnNight.Text;
            //Reset qty PO
            qtPo = 0;
            txtttPO.ResetText();
        }

        private void radbtnPOm_CheckedChanged(object sender, EventArgs e)
        {
            strP = "New";
            qtPo = 0;
            txtttPO.ResetText();
        }

        private void radbtnOBA_CheckedChanged(object sender, EventArgs e)
        {
            strP = "OBA";
            qtPo = 0;
            txtttPO.ResetText();
        }    

        private void cobbModel_TextChanged(object sender, EventArgs e)
        {
            if(cobbModel.Text != "")
            {
                radBtnRelo.Enabled = true;
                labBarPCM.Enabled = false;
                txtBarPCM.Enabled = false;
                qtPo = 0;
                txtttPO.ResetText();

                if (cobbModel.Text != "")
                {
                    MessageBox.Show("Hãy tạo PO mới!", "Boxing Line 3");
                }

                if (cobbModel.Text == "Ezgo Master")
                {
                    toolStripTextBox1.Text = "  20210802_Boxing Ezgo Master PCB V06_Ver01";
                    stanQty = int.Parse(txtMas.Text);
                }
                else
                {
                    toolStripTextBox1.Text = "  20210802_Boxing Ezgo Slave PCB V06_Ver01";
                    stanQty = int.Parse(txtSla.Text);
                }
            }    
        }

        private void btnCrePo_Click(object sender, EventArgs e)
        {
            chkPo = true;
            string dTi = cls1.changeDateTime(daTiPic1);
            bool nePo = true;
            chek = true;
            //txtBarPCM.Text = "";
            //txtCodLot.Text = "";
            //Kiểm tra có đủ thông tin
            if (cobbLine.Text == "")
            {
                MessageBox.Show("Chưa có thông tin về Line!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((rabtnDay.Checked == false) && (rabtnNight.Checked == false))
            {
                MessageBox.Show("Chưa có thông tin về ca sản xuất!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((radbtnPOm.Checked == false) && (radbtnOBA.Checked == false))
            {
                MessageBox.Show("Chưa có thông tin về PO sản xuất!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    //Tạo đường dấn lưu file record data
                    cls5.createFoler(dTi, lik_PCcal);

                    //Tạo Data table để so sánh logfile               
                    string[] lisDatTim = new string[dOl];
                    cls1.changeDateTime2(daTiPic1, lisDatTim, dOl);

                    int numF = 0;
                    numF = cls1.countFile(lisDatTim, cls1.loadLink(txt_Fct.Text), dOl);

                    string[] namF = new string[numF];
                    namF = cls1.getNamLog(lisDatTim, numF, cls1.loadLink(txt_Fct.Text), dOl);                 

                    cls1.conExcelToTxt(lisDatTim, namF, numF, cobbModel.Text, nePo, cls1.loadLink(txt_Fct.Text), dOl);//=== dùng txt file

                    DialogResult rell = MessageBox.Show("Tạo PO thành công!\n === Nhập Qty PO ===", "Boxing Line 3", MessageBoxButtons.OK);
                    if(rell == DialogResult.OK)
                    {
                        chek = true;                     
                        labRelFu.Enabled = true;
                        txtRelFu.Enabled = true;
                        labQty.Enabled = true;
                        txtQty.Enabled = true;
                        labCodLot.Enabled = true;
                        txtCodLot.Enabled = true;
                        labttPO.Enabled = true;
                        txtttPO.Enabled = true;                      
                        btnCreLotLe.Enabled = true;
                        btnCloPO.Enabled = true;             
                        timer1.Enabled = true;
                        //radBtnRelo.Enabled = false;
                        labqtPO.Enabled = true;
                        txtqtPO.Enabled = true;
                        txtqtPO.Focus();
                                            
                    }              
                }
                catch (Exception)
                {
                    MessageBox.Show("Tạo PO thất bại!", "Boxing Line 3", MessageBoxButtons.RetryCancel);
                }

                //Update Qty PO khi program treo                
                if ((cls5.checkExitLog(dTi, cobbModel.Text, strP, strSh, lik_PCcal) == true) && (reLoad == true))
                {
                    string qtt1 = cls1.redCSV(dTi, strP, strSh, cobbModel, lik_PCcal);
                    string[] arrtg = qtt1.Split('/');
                    qtPo = int.Parse(arrtg[0]);
                    //qtbg = qtPo.ToString();
                }                                
            }           
        }       

        private void btnCreLotLe_Click(object sender, EventArgs e)
        {
            //Biến thay đổi format date time
            string dTime = cls1.changeDateTime(daTiPic1);
            //Kiểm tra số lượng PCM đang boxing
            string[] qtpm = txtQty.Text.Split('/');

            DialogResult diCreLe = MessageBox.Show("Bạn có chắc muốn đóng Lot lẻ " + qtpm[0] + " PCM ?", "Boxing Line 3", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //Hiển thị Lot đã Boxed
            if ((diCreLe == DialogResult.Yes) && (txtCodLot.Text != ""))
            {
                try
                {
                    strLot = "Lot Le";

                    if (cls5.checkExitPO(dTime, lik_PCcal) == true)
                    {
                        
                        cls1.savLot(txtCodLot);//save code lot                                             

                        //Reset all data sau khi tạo lot                          
                        txtBarPCM.ResetText();
                        labBarPCM.Enabled = false;
                        txtBarPCM.Enabled = false;
                        txtRelFu.ResetText();                        
                        txtCodLot.Enabled = true;
                        txtCodLot.Focus();
                        reLoad = false;

                        //Đếm số lượng PCM boxing trong PO
                        string[] qtP = txtQty.Text.Split('/');
                        int qtyP = int.Parse(qtP[0]);
                        qtPo = qtPo + qtyP;
                        txtttPO.Text = qtPo.ToString() + "/" + txtqtPO.Text;
                        
                        if(erBox == true)
                        {
                            qtPo = qtPo - qtyP;
                            txtttPO.Text = qtPo.ToString() + "/" + txtqtPO.Text;
                            erBox = false;
                        }
                        txtQty.ResetText();
                        cls1.reSetCouOK();
                        txtCodLot.ResetText();

                        //Tạo file record                                                   
                        cls5.exportCsv(dataGridView2, lik_PCcal + "\\Result\\" + dTime + "\\" + strP + " " + strSh + " Boxing " + cobbModel.Text + " PCB V06 Ver01.CSV", cls5.checkExitLog(dTime, cobbModel.Text, strP, strSh, lik_PCcal));
                    }
                    else
                    {
                        MessageBox.Show("Chưa tạo PO!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Đã xảy ra lỗi. Đóng Lot thất bại.", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cls3.showLotBoxed(dataGridView2, cls2.delNGDreLot(txtCodLot.Text, DateTime.Now.ToString().Substring(0, 10), cobbModel, lik_PCcal));
                    cls1.reSetCouOK();
                }
            }
            else
            {
                MessageBox.Show("Chưa đủ điều kiện Boxing!", "Boxing Line 3", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }                             
        }
        private async void txtBarPCM_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(500);
            if (txtBarPCM.Text != "" && txtBarPCM.Text.Length == 14)
            {                
                txtRelFu.Clear();
                //Biến thay đổi format date time
                string dTime = cls1.changeDateTime(daTiPic1);
                //Kiểm tra định dạng code
                int chCodPcm = cls4.checkBarCodePCM(txtBarPCM.Text);
                //Kiểm tra model đúng hay sai
                string strWrgMol = cls4.checkWrongModel(txtBarPCM.Text);
                //Kiểm tra double barcode PCM
                //int chSamCod = cls4.checkSameCodePCM(txtBarPCM.Text, dTime);
                //Biến check exit logfile
                bool chekExLog = cls5.checkExitLog(dTime, cobbModel.Text, strP, strSh, lik_PCcal);

                if (chCodPcm != 0 && txtBarPCM.Text != "")
                {
                    txtBarPCM.ResetText();
                    MessageBox.Show("Sai định dạng Barcode PCM. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarPCM.Focus();//Trỏ chuột tại text Box Barcode PCM

                }
                else if (strWrgMol != cobbModel.Text && txtBarPCM.Text != "")
                {
                    txtBarPCM.ResetText();
                    MessageBox.Show("Boxing sai model. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarPCM.Focus();//Trỏ chuột tại text Box Barcode PCM                     
                }
                //else if (chSamCod != 0)
                //{
                //    txtBarPCM.ResetText();
                //    MessageBox.Show("Trùng Barcode PCM đã boxing. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtBarPCM.Focus();//Trỏ chuột tại text Box Barcode PCM                      
                //}
                //===dùng txtx file
                else if (cls1.chekSamBar(txtBarPCM) == false)
                {
                    txtBarPCM.ResetText();
                    MessageBox.Show("Trùng Barcode PCM đã boxing. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarPCM.Focus();//Trỏ chuột tại text Box Barcode PCM

                }
                else
                {
                    cls1.usiListStrig(txtBarPCM, txtRelFu, txtQty, dTime, cobbModel.Text, chekExLog, reLoad, stanQty.ToString(), qtbg);

                    //Hiển thị PCM đang boxing                         
                    try
                    {
                        if ((txtRelFu.Text.Substring(0, 2) == "OK") && (txtBarPCM.Text != ""))
                        {
                            cls3.showLotBoxed(dataGridView2, cls2.insertBarPCM(cobbLine, cobbModel, txtBarPCM, txtRelFu, DateTime.Now.ToString(), txtCodLot, strSh, txtQty, lik_PCcal));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Boxing PCM khác!", "Boxing Line 3");
                    }

                    //txtBarPCM.Text = "";

                    //Đếm PCM theo tiêu chuẩn và boxing
                    if (cls1.countPCMinLot(txtCodLot, txtBarPCM, stanQty) == true)
                    {
                        try
                        {
                            strLot = "Lot Chuan";

                            if (cls5.checkExitPO(dTime, lik_PCcal) == true)
                            {                        
                                txtBarPCM.Enabled = true;

                                cls1.savLot(txtCodLot);//save code lot                                       

                                //Reset all data sau khi tạo lot                           
                                
                                labBarPCM.Enabled = false;
                                txtBarPCM.Enabled = false;
                                txtRelFu.ResetText();                              
                                txtCodLot.Enabled = true;
                                txtCodLot.Focus();
                                reLoad = false;

                                //Đếm số lượng PCM boxing trong PO
                                string[] qtP = txtQty.Text.Split('/');
                                int qtyP = int.Parse(qtP[0]);                            
                                qtPo = qtPo + qtyP;
                                txtttPO.Text = qtPo.ToString() + "/" + txtqtPO.Text;
                                if (erBox == true)
                                {
                                    qtPo = qtPo - qtyP;
                                    txtttPO.Text = qtPo.ToString() + "/" + txtqtPO.Text;
                                    erBox = false;
                                }
                                txtQty.ResetText();
                                cls1.reSetCouOK();
                                //txtBarPCM.ResetText();
                                txtCodLot.ResetText();

                                //Tạo file record
                                cls5.exportCsv(dataGridView2, lik_PCcal + "\\Result\\" + dTime + "\\" + strP + " " + strSh + " Boxing " + cobbModel.Text + " PCB V06 Ver01.CSV", chekExLog);                               
                            }
                            else
                            {
                                MessageBox.Show("Chưa tạo PO!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarPCM.ResetText();
                                txtCodLot.ResetText();
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Đã xảy ra lỗi. Đóng Lot thất bại.", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cls3.showLotBoxed(dataGridView2, cls2.delNGDreLot(txtCodLot.Text, DateTime.Now.ToString().Substring(0, 10), cobbModel, lik_PCcal));
                            cls1.reSetCouOK();
                            txtBarPCM.ResetText();
                            txtCodLot.ResetText();
                        }
                    }
                    else
                    {
                        txtBarPCM.ResetText();
                    }
                }                    
            }
        }

        private async void txtCodLot_TextChanged(object sender, EventArgs e)
        {            
            if (txtCodLot.Text != "" && txtCodLot.Text.Length == 17)
            {
                if (txtqtPO.Text == "")
                {
                    MessageBox.Show("Chưa nhập Qty PO!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodLot.Text = "";
                }
                else
                {
                    await Task.Delay(500);
                    //Biến thay đổi format date time
                    string dTime = cls1.changeDateTime(daTiPic1);
                    //Kiểm tra định dạng Code Lot
                    int chCodLot = cls4.checkCodeLot(txtCodLot.Text, dTime);
                    //Kiểm tra đúng model hay không
                    string chMolCoLot = cls4.checkWrongModelCodeLot(txtCodLot.Text);
                    //Kiểm tra double code Lot
                    //int chSaCoLo = cls4.checkSameCodeLot(txtCodLot.Text, dTime);         

                    //switch (txtCodLot.Text != "")
                    //{
                    //    case true:
                    if (chCodLot != 0 && txtCodLot.Text != "")
                    {
                        txtCodLot.ResetText();
                        MessageBox.Show("Sai định dạng Code Lot. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCodLot.Focus();//Trỏ chuột tại textBox Code Lot
                        txtBarPCM.Enabled = false;

                    }
                    else if (chMolCoLot != cobbModel.Text && txtCodLot.Text != "")
                    {
                        txtCodLot.ResetText();
                        MessageBox.Show("Boxing sai model. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCodLot.Focus();//Trỏ chuột tại textBox Code Lot
                        txtBarPCM.Enabled = false;

                    }
                    //else if (chSaCoLo != 0)
                    //{
                    //    txtCodLot.ResetText();
                    //    MessageBox.Show("Trùng Code Lot đã boxing. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    txtCodLot.Focus();//Trỏ chuột tại textBox Code Lot                       
                    //}                   
                    //===dùng txt file
                    else if (cls1.chekSamLot(txtCodLot) == false)
                    {
                        txtCodLot.ResetText();
                        MessageBox.Show("Trùng Code Lot đã boxing. Hãy kiểm tra lại!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCodLot.Focus();//Trỏ chuột tại textBox Code Lot 
                        txtBarPCM.Enabled = false;
                    }
                    else
                    {
                        txtCodLot.Enabled = false;
                        labBarPCM.Enabled = true;
                        txtBarPCM.Enabled = true;
                        txtBarPCM.Focus();  //Trỏ chuột tại textBox barcode    

                        //Update data khi program treo
                        //if ((cls5.checkExitLog(dTime, cobbModel.Text, strP, strSh) == true) && (reLoad == true))
                        if (reLoad == true)
                        {
                            qtbg = cls2.UpDatExitPro1(txtCodLot, lik_PCcal).ToString();
                            txtQty.Text = qtbg + "/" + stanQty.ToString();
                        }
                    }
                    //        break;
                    //    default:
                    //        break;
                    //}
                }              
            }
        }

        private void btnCloPO_Click(object sender, EventArgs e)
        {
            string[] strPo = txtttPO.Text.Split('/');
            if(float.Parse(strPo[0])/float.Parse(strPo[1]) < 0.9f)
            {
                DialogResult rel1 = MessageBox.Show("PO đang boxing thiếu " + (int.Parse(strPo[1]) - int.Parse(strPo[0])).ToString() + " PCM\nNếu bạn đóng PO, chương trình sẽ ghi nhận lỗi\nBạn có muốn tiếp tục không?", "Boxing Line 3", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if(rel1 == DialogResult.OK)
                {
                    //Update data table
                    //Biến thay đổi format date time
                    string dTime = cls1.changeDateTime(daTiPic1);
                    cls2.upQtyPo(dTime, cobbModel.Text, strSh, txtttPO.Text, strP, lik_PCcal);

                    //Luu lich su txt1 file (model, date_month, shift, qty)                    
                    string strtxt = cobbModel.Text + " " + Convert.ToDateTime(daTiPic1.Text.Substring(4, 2) + "/" + daTiPic1.Text.Substring(6, 2) + "/" + daTiPic1.Text.Substring(0, 4)).ToShortDateString() + " " + find_shift() + " " + strPo[0];
                    if (!File.Exists(lik_PCcal + "\\HistoryUse\\txtBoxing.txt"))//chua ton tai file txt
                    {
                        StreamWriter sw = File.CreateText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    else//da ton tai file txt
                    {
                        StreamWriter sw = File.AppendText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    //Kiem tra log function -> luu txt2 (model, date_month, shift, qty)
                    //-list date can ktra
                    string[] lisDatTim = new string[dOl];
                    cls1.changeDateTime2(daTiPic1, lisDatTim, dOl);
                    //Tạo txt2
                    cls2.get_ttlogFu(cls1.loadLink(txt_Fct.Text), lisDatTim, lik_PCcal);
                    //so sanh file txt1 va txt2 (ko matching, matching < 90% qty)
                    string[] erNoUse = cls2.ssFCTBox(lik_PCcal);
                    //luu data base
                    cls2.savDatNoUse(erNoUse, lik_PCcal);

                    //Ẩn chức năng
                    labBarPCM.Enabled = false;
                    txtBarPCM.Enabled = false;
                    labRelFu.Enabled = false;
                    txtRelFu.Enabled = false;
                    labQty.Enabled = false;
                    txtQty.Enabled = false;
                    labCodLot.Enabled = false;
                    txtCodLot.Enabled = false;
                    labttPO.Enabled = false;
                    txtttPO.Enabled = false;
                    btnCreLotLe.Enabled = false;
                    btnCloPO.Enabled = false;

                    //Hien thi historyUse
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "HistoryUse")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        HistoryUse hsu = new HistoryUse(lik_PCcal);
                        hsu.Show();
                    }
                }
            }
            else
            {
                DialogResult relCP = MessageBox.Show("Bạn có muốn đóng PO không?", "Boxing Line 3", MessageBoxButtons.OKCancel);
                if (relCP == DialogResult.OK)
                {
                    //Update data table
                    //Biến thay đổi format date time
                    string dTime = cls1.changeDateTime(daTiPic1);
                    cls2.upQtyPo(dTime, cobbModel.Text, strSh, txtttPO.Text, strP, lik_PCcal);

                    //Luu lich su txt1 file (model, date_month, shift, qty)                    
                    string strtxt = cobbModel.Text + " " + Convert.ToDateTime(daTiPic1.Text.Substring(4, 2) + "/" + daTiPic1.Text.Substring(6, 2) + "/" + daTiPic1.Text.Substring(0, 4)).ToShortDateString() + " " + find_shift() + " " + strPo[0];
                    if (!File.Exists(lik_PCcal + "\\HistoryUse\\txtBoxing.txt"))//chua ton tai file txt
                    {
                        StreamWriter sw = File.CreateText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    else//da ton tai file txt
                    {
                        StreamWriter sw = File.AppendText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    //Kiem tra log function -> luu txt2 (model, date_month, shift, qty)
                    //-list date can ktra
                    string[] lisDatTim = new string[dOl];
                    cls1.changeDateTime2(daTiPic1, lisDatTim, dOl);
                    //Tạo txt2
                    cls2.get_ttlogFu(cls1.loadLink(txt_Fct.Text), lisDatTim, lik_PCcal);
                    //so sanh file txt1 va txt2 (ko matching, matching < 90% qty)
                    string[] erNoUse = cls2.ssFCTBox(lik_PCcal);
                    //luu data base
                    cls2.savDatNoUse(erNoUse, lik_PCcal);

                    //Ẩn chức năng
                    labBarPCM.Enabled = false;
                    txtBarPCM.Enabled = false;
                    labRelFu.Enabled = false;
                    txtRelFu.Enabled = false;
                    labQty.Enabled = false;
                    txtQty.Enabled = false;
                    labCodLot.Enabled = false;
                    txtCodLot.Enabled = false;
                    labttPO.Enabled = false;
                    txtttPO.Enabled = false;
                    btnCreLotLe.Enabled = false;
                    btnCloPO.Enabled = false;
                    timer3.Start();
                    cout_box = 900;
                    ftim3 = true;
                    pos_X = Cursor.Position.X;
                    pos_Y = Cursor.Position.Y;                   

                    //Hien thi historyUse
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "HistoryUse")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        HistoryUse hsu = new HistoryUse(lik_PCcal);
                        hsu.Show();
                    }
                }   
            }           
        }  
       
        private void btnLogin_Click(object sender, EventArgs e)
        {
            int entLog = 0;
            string[,] listUser = new string[10, 2];
            listUser = cls2.loGin(lik_PCcal);
            for (int i = 0; i < 10; i++)
            {
                if ((txtID.Text == listUser[i, 0]) && (txtPass.Text == listUser[i, 1]))
                {
                    if ((txtID.Text == "toan.ngh") && (txtPass.Text == "1"))
                    {
                        daTiPic1.Enabled = true;
                        rabtnDay.Enabled = true;
                        rabtnNight.Enabled = true;
                        radbtnPOm.Enabled = true;
                        radbtnOBA.Enabled = true;
                        labLine.Enabled = true;
                        cobbLine.Enabled = true;
                        labModel.Enabled = true;
                        cobbModel.Enabled = true;                   
                        tabControl1.Enabled = true;
                        grBoSet.Enabled = true;
                        btnCrePo.Enabled = true;                      
                        txtID.Enabled = false;
                        txtPass.Enabled = false;
                        txtPass.Clear();
                        entLog++;
                        radBtnRelo.Enabled = true;                      
                        break;
                    }
                    else
                    {
                        daTiPic1.Enabled = true;
                        rabtnDay.Enabled = true;
                        rabtnNight.Enabled = true;
                        radbtnPOm.Enabled = true;
                        radbtnOBA.Enabled = true;
                        labLine.Enabled = true;
                        cobbLine.Enabled = true;
                        labModel.Enabled = true;
                        cobbModel.Enabled = true;                       
                        tabControl1.Enabled = true;
                        btnCrePo.Enabled = true;                    
                        txtID.Enabled = false;
                        txtPass.Enabled = false;
                        txtPass.Clear();
                        entLog++;
                        radBtnRelo.Enabled = true;                
                        break;
                    }                   
                }
            }  
            if (entLog == 0)
            {
                MessageBox.Show("Đăng nhập sai!", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void butExit_Click(object sender, EventArgs e)
        {
            txtID.Enabled = true;
            txtID.ResetText();
            txtPass.Enabled = true;
            txtPass.ResetText();
            cobbLine.ResetText();
            cobbModel.ResetText();
            txtBarPCM.ResetText();
            txtRelFu.ResetText();
            txtQty.ResetText();
            txtCodLot.ResetText();
            daTiPic1.Enabled = false;
            rabtnDay.Checked = false;
            rabtnNight.Checked = false;
            radbtnPOm.Checked = false;
            radbtnOBA.Checked = false;
            rabtnDay.Enabled = false;
            rabtnNight.Enabled = false;
            radbtnPOm.Enabled = false;
            radbtnOBA.Enabled = false;
            labLine.Enabled = false;
            cobbLine.Enabled = false;
            labModel.Enabled = false;
            cobbModel.Enabled = false;
            labBarPCM.Enabled = false;
            txtBarPCM.Enabled = false;
            labRelFu.Enabled = false;
            txtRelFu.Enabled = false;
            labQty.Enabled = false;
            txtQty.Enabled = false;
            labCodLot.Enabled = false;
            txtCodLot.Enabled = false;
            labttPO.Enabled = false;
            txtttPO.Enabled = false;          
            btnCreLotLe.Enabled = false;
            tabControl1.Enabled = false;
            grBoSet.Enabled = false;
            btnCrePo.Enabled = false;
            btnCloPO.Enabled = false;
            radBtnRelo.Enabled = false;
            txtID.Focus();
            labqtPO.Enabled = false;
            txtqtPO.Enabled = false;
            txtqtPO.ResetText();
            btnAddPcm.Enabled = false;
            qtPo = 0;
            addPcm = false;          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if((DateTime.Now.Minute % 5 == 0) && (DateTime.Now.Second == 1))
            {
                bool nePo = true;

                //Tạo Data table để so sánh logfile               
                string[] lisDatTim = new string[dOl];
                cls1.changeDateTime2(daTiPic1, lisDatTim, dOl);

                int numF = 0;
                numF = cls1.countFile(lisDatTim, cls1.loadLink(txt_Fct.Text), dOl);

                string[] namF = new string[numF];
                namF = cls1.getNamLog(lisDatTim, numF, cls1.loadLink(txt_Fct.Text), dOl);              

                cls1.conExcelToTxt(lisDatTim, namF, numF, cobbModel.Text, nePo, cls1.loadLink(txt_Fct.Text), dOl);
            }
        }

        private void radBtnRelo_CheckedChanged(object sender, EventArgs e)
        {
            if (radBtnRelo.Checked == true)
            {
                reLoad = true;
            }
            radBtnRelo.Enabled = false;
        }       

        private void btnAddPcm_Click(object sender, EventArgs e)
        {
            addPcm = true;
            labCodLot.Enabled = true;
            txtCodLot.Enabled = true;
            txtCodLot.Focus();
            labRelFu.Enabled = true;
            txtRelFu.Enabled = true;
            labQty.Enabled = true;
            txtQty.Enabled = true;
            labttPO.Enabled = true;
            txtttPO.Enabled = true;
            btnCreLotLe.Enabled = true;
            btnCloPO.Enabled = true;
        }

        private void txtttPO_TextChanged(object sender, EventArgs e)
        {
            if (txtttPO.Text != "")
            {              
                string[] qqt1 = txtttPO.Text.Split('/');
                if (int.Parse(qqt1[0]) >= int.Parse(txtqtPO.Text) && int.Parse(qqt1[0]) <= (int.Parse(txtqtPO.Text) + 5))//qqt1[0] == txtqtPO.Text
                {
                    //Hiển thị data
                    cls3.showLotBoxed(dataGridView2, cls2.updateCodLot(txtCodLot, strLot, txtttPO.Text, lik_PCcal));

                    //Biến thay đổi format date time
                    string dTime = cls1.changeDateTime(daTiPic1);
                    cls2.upQtyPo(dTime, cobbModel.Text, strSh, txtttPO.Text, strP, lik_PCcal);

                    //Luu lich su txt1 file (model, date_month, shift, qty)                    
                    string strtxt = cobbModel.Text + " " + Convert.ToDateTime(daTiPic1.Text.Substring(4,2) + "/" + daTiPic1.Text.Substring(6,2) + "/" + daTiPic1.Text.Substring(0,4)).ToShortDateString() + " " + find_shift() + " " + txtqtPO.Text;
                    if (!File.Exists(lik_PCcal + "\\HistoryUse\\txtBoxing.txt"))//chua ton tai file txt
                    {
                        StreamWriter sw = File.CreateText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    else//da ton tai file txt
                    {
                        StreamWriter sw = File.AppendText(lik_PCcal + "\\HistoryUse\\txtBoxing.txt");
                        sw.WriteLine(strtxt);
                        sw.Close();
                    }
                    //Kiem tra log function -> luu txt2 (model, date_month, shift, qty)
                    //-list date can ktra
                    string[] lisDatTim = new string[dOl];
                    cls1.changeDateTime2(daTiPic1, lisDatTim, dOl);
                    //Tạo txt2
                    cls2.get_ttlogFu(cls1.loadLink(txt_Fct.Text), lisDatTim, lik_PCcal);
                    //so sanh file txt1 va txt2 (ko matching, matching < 90% qty)
                    string[] erNoUse = cls2.ssFCTBox(lik_PCcal);
                    //luu data base
                    cls2.savDatNoUse(erNoUse, lik_PCcal);                    

                    //Ẩn chức năng
                    labBarPCM.Enabled = false;
                    txtBarPCM.Enabled = false;
                    labRelFu.Enabled = false;
                    txtRelFu.Enabled = false;
                    labQty.Enabled = false;
                    txtQty.Enabled = false;
                    labCodLot.Enabled = false;
                    txtCodLot.Enabled = false;
                    labttPO.Enabled = false;
                    txtttPO.Enabled = false;
                    btnCreLotLe.Enabled = false;
                    btnCloPO.Enabled = false;
                    btnAddPcm.Enabled = true;                   
                    labqtPO.Enabled = false;
                    txtqtPO.Enabled = false;
                    timer3.Start();
                    cout_box = 900;
                    ftim3 = true;
                    pos_X = Cursor.Position.X;
                    pos_Y = Cursor.Position.Y;

                    //Hien thi historyUse
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "HistoryUse")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        HistoryUse hsu = new HistoryUse(lik_PCcal);
                        hsu.Show();
                    }
                                               
                }
                if (int.Parse(qqt1[0]) > (int.Parse(txtqtPO.Text) + 5))
                {
                    if (addPcm == false)
                    {
                        MessageBox.Show("Boxing vượt quá số lượng cho phép (" + (int.Parse(txtqtPO.Text) + 5).ToString() + ").\nĐóng đủ số lượng PO và thêm PCM sau.", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                        erBox = true;
                    }
                    else
                    {
                        //Hiển thị data
                        cls3.showLotBoxed(dataGridView2, cls2.updateCodLot(txtCodLot, strLot, txtttPO.Text, lik_PCcal));
                    }
                }
                if (int.Parse(qqt1[0]) < int.Parse(txtqtPO.Text))
                {
                    if (erBox == false)
                    {
                        //Hiển thị data
                        cls3.showLotBoxed(dataGridView2, cls2.updateCodLot(txtCodLot, strLot, txtttPO.Text, lik_PCcal));
                    }
                    else
                    {
                        cls3.showLotBoxed(dataGridView2, cls2.delNGDreLot(txtCodLot.Text, DateTime.Now.ToString().Substring(0, 10), cobbModel, lik_PCcal));
                    }
                }
            }
        }

        private void lịchSửSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "HistoryUse")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                HistoryUse hsu = new HistoryUse(lik_PCcal);
                hsu.Show();
            }
        }

        private void kếtNốiPCLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "ConPCLcal")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                ConPCLcal cP = new ConPCLcal(txt_PcLocal.Text, txt_Fct.Text);
                cP.Show();
            }
        }

        private async void toolStripStatusLabel1_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(500);
            if(toolStripStatusLabel1.Text != "")
            {
                switch(toolStripStatusLabel1.Text)
                {
                    case "PC Local Connected":
                        break;
                    case "Database Available":
                        MessageBox.Show("Không thể kết nối PC Local\nChương trình sẽ thao tác với PC Boxing", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case "Database Unavailable":
                        MessageBox.Show("Lỗi kết nối database.\nHãy khởi động lại chương trình", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        break;
                }
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

        public string find_shift()
        {
            string shift;
            DateTime dateTime = DateTime.Now;

            DateTime startDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
            DateTime endDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 20, 0, 0);

            if (TimeBetween(dateTime, startDateTime, endDateTime))
            {
                shift = "Ngày";
            }
            else
            {
                shift = "Đêm";
            }
            return shift;
        }
        
        private async void txtqtPO_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(10000);
            if(txtqtPO.Text != "")
            {             
                int ouQt = 0;                                   
                if(int.TryParse(txtqtPO.Text, out ouQt) == true)
                {
                    if (txtqtPO.Text.Contains(" "))
                    {
                        MessageBox.Show("Qty PO chứa dấu cách. Hãy nhập lại", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtqtPO.Text = "";
                    }
                    else
                    {
                        txtqtPO.Enabled = false;
                    }                     
                }
                else
                {
                    MessageBox.Show("Hãy nhập số", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtqtPO.Text = "";
                }                
            }          
        }
       
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now.Hour == 20 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 1 && chkPo == true)
            {
                chkPo = false;
                if(txtttPO.Text != "")
                {
                    string[] arr1 = txtttPO.Text.Split('/');
                    if (float.Parse(arr1[0]) / float.Parse(arr1[1]) < 0.9f)
                    {
                        MessageBox.Show("PO đang boxing thiếu " + (int.Parse(arr1[1]) - int.Parse(arr1[0])).ToString() + " PCM", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }               
            }

            if(DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 1 && chkPo == true)
            {
                chkPo = false;
                if (txtttPO.Text != "")
                {
                    string[] arr2 = txtttPO.Text.Split('/');
                    if (float.Parse(arr2[0]) / float.Parse(arr2[1]) < 0.9f)
                    {
                        MessageBox.Show("PO đang boxing thiếu " + (int.Parse(arr2[1]) - int.Parse(arr2[0])).ToString() + " PCM", "Boxing Line 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }    
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if(ftim3 == true)
            {
                cout_box--;
                if (cout_box == 0)
                {
                    this.Close();
                }
            }           
        }

        private void txtCodLot_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if(x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void txtBarPCM_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void btnAddPcm_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void btnCreLotLe_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void txtID_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void txtPass_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void btnLogin_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }

        private void butExit_MouseMove(object sender, MouseEventArgs e)
        {
            var x = pos_X;
            var y = pos_Y;

            pos_X = Cursor.Position.X;
            pos_Y = Cursor.Position.Y;

            if (x != pos_X || y != pos_Y)
            {
                cout_box = 900;
                timer3.Start();
            }
        }                            
    }
}
