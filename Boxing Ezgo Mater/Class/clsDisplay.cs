using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace Boxing_Ezgo_Mater
{
    class clsDisplay
    {
        public void showNoUse(DataGridView dtg1, DataTable dt1)
        {
            dtg1.AutoGenerateColumns = false;
            dtg1.DataSource = dt1;
            dtg1.Columns.Clear();
           
            DataGridViewTextBoxColumn col_mol = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_date = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_shift = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_history = new DataGridViewTextBoxColumn();

            col_mol.DataPropertyName = "Model";
            col_date.DataPropertyName = "Date_Month";
            col_shift.DataPropertyName = "Shift";
            col_history.DataPropertyName = "HistoryUseProgram";

            col_mol.HeaderText = "Model";
            col_date.HeaderText = "Date_Month";
            col_shift.HeaderText = "Shift";
            col_history.HeaderText = "HistoryUse";

            col_mol.Name = "Model";
            col_date.Name = "Date_Month";
            col_shift.Name = "Shift";
            col_history.Name = "HistoryUse";

            col_mol.ReadOnly = true;
            col_date.ReadOnly = true;
            col_shift.ReadOnly = true;
            col_history.ReadOnly = true;

            col_mol.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_date.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_shift.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_history.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_mol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_date.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_shift.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_history.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_mol.Width = 115;
            col_date.Width = 115;
            col_shift.Width = 115;
            col_history.Width = 115;

            col_mol.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_date.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_shift.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_history.CellTemplate.Style.BackColor = Color.AliceBlue;

            col_mol.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_date.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_shift.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_history.CellTemplate.Style.Font = new Font("Times New Roman", 9);

            col_mol.CellTemplate.Style.ForeColor = Color.Black;
            col_date.CellTemplate.Style.ForeColor = Color.Black;
            col_shift.CellTemplate.Style.ForeColor = Color.Black;
            col_history.CellTemplate.Style.ForeColor = Color.Black;

            dtg1.Columns.Add(col_mol);
            dtg1.Columns.Add(col_date);
            dtg1.Columns.Add(col_shift);
            dtg1.Columns.Add(col_history);
        }

        public void showLotBoxed(DataGridView dtg2, DataTable dt2)
        {
            dtg2.AutoGenerateColumns = false;
            dtg2.DataSource = dt2;
            dtg2.Columns.Clear();

            DataGridViewTextBoxColumn liNe = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn modEl = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn codelot = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn barcodepcm = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn qtypcm = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn timetest = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn timebox = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn shiFt = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn qtyPO = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn remklot = new DataGridViewTextBoxColumn();

            liNe.DataPropertyName = "Line_No";
            modEl.DataPropertyName = "Model_Name";
            codelot.DataPropertyName = "Code_Lot";
            barcodepcm.DataPropertyName = "Barcode_PCM";
            qtypcm.DataPropertyName = "Qty_PCM_In_Lot";
            timetest.DataPropertyName = "Time_Test";
            timebox.DataPropertyName = "Time_Box";
            shiFt.DataPropertyName = "Shift_Text";
            qtyPO.DataPropertyName = "Qty_PO";
            remklot.DataPropertyName = "Remark_Lot";

            liNe.HeaderText = "Line No";
            modEl.HeaderText = "Model Name";
            codelot.HeaderText = "Code Lot";
            barcodepcm.HeaderText = "Barcode PCM";
            qtypcm.HeaderText = "Qty PCM";
            timetest.HeaderText = "Time Test";
            timebox.HeaderText = "Time Box";
            shiFt.HeaderText = "Shift Text";
            qtyPO.HeaderText = "Qty PO";
            remklot.HeaderText = "Remark";

            liNe.Name = "Line No";
            modEl.Name = "Model Name";
            codelot.Name = "Code Lot";
            barcodepcm.Name = "Barcode PCM";
            qtypcm.Name = "Qty PCM/Lot";
            timetest.Name = "Time Test";
            timebox.Name = "Time Box";
            shiFt.Name = "Shift Text";
            qtyPO.Name = "Qty PO";
            remklot.Name = "Remark";

            liNe.ReadOnly = true;
            modEl.ReadOnly = true;
            codelot.ReadOnly = true;
            barcodepcm.ReadOnly = true;
            qtypcm.ReadOnly = true;
            timetest.ReadOnly = true;
            timebox.ReadOnly = true;
            shiFt.ReadOnly = true;
            qtyPO.ReadOnly = true;
            remklot.ReadOnly = true;

            liNe.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            modEl.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            codelot.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            barcodepcm.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            qtypcm.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            timetest.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            timebox.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            shiFt.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            qtyPO.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            remklot.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            liNe.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            modEl.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            codelot.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            barcodepcm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            qtypcm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            timetest.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            timebox.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            shiFt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            qtyPO.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            remklot.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            liNe.Width = 50;
            modEl.Width = 100;
            codelot.Width = 130;
            barcodepcm.Width = 100;
            qtypcm.Width = 80;
            timetest.Width = 130;
            timebox.Width = 80;
            shiFt.Width = 100;
            qtyPO.Width = 80;
            remklot.Width = 85;

            liNe.CellTemplate.Style.BackColor = Color.AliceBlue;
            modEl.CellTemplate.Style.BackColor = Color.AliceBlue;
            codelot.CellTemplate.Style.BackColor = Color.AliceBlue;
            barcodepcm.CellTemplate.Style.BackColor = Color.AliceBlue;
            qtypcm.CellTemplate.Style.BackColor = Color.AliceBlue;
            timetest.CellTemplate.Style.BackColor = Color.AliceBlue;
            timebox.CellTemplate.Style.BackColor = Color.AliceBlue;
            shiFt.CellTemplate.Style.BackColor = Color.AliceBlue;
            qtyPO.CellTemplate.Style.BackColor = Color.AliceBlue;
            remklot.CellTemplate.Style.BackColor = Color.AliceBlue;

            liNe.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            modEl.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            codelot.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            barcodepcm.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            qtypcm.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            timetest.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            timebox.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            shiFt.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            qtyPO.CellTemplate.Style.Font = new Font("Times New Roman", 8);
            remklot.CellTemplate.Style.Font = new Font("Times New Roman", 8);

            liNe.CellTemplate.Style.ForeColor = Color.Black;
            modEl.CellTemplate.Style.ForeColor = Color.Black;
            codelot.CellTemplate.Style.ForeColor = Color.Black;
            barcodepcm.CellTemplate.Style.ForeColor = Color.Black;
            qtypcm.CellTemplate.Style.ForeColor = Color.Black;
            timetest.CellTemplate.Style.ForeColor = Color.Black;
            timebox.CellTemplate.Style.ForeColor = Color.Black;
            shiFt.CellTemplate.Style.ForeColor = Color.Black;
            qtyPO.CellTemplate.Style.ForeColor = Color.Black;
            remklot.CellTemplate.Style.ForeColor = Color.Black;

            dtg2.Columns.Add(liNe);
            dtg2.Columns.Add(modEl);
            dtg2.Columns.Add(codelot);
            dtg2.Columns.Add(barcodepcm);
            dtg2.Columns.Add(qtypcm);
            dtg2.Columns.Add(timetest);
            dtg2.Columns.Add(timebox);
            dtg2.Columns.Add(shiFt);
            dtg2.Columns.Add(qtyPO);
            dtg2.Columns.Add(remklot);
        }
    }
}
