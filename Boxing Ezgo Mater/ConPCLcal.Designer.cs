namespace Boxing_Ezgo_Mater
{
    partial class ConPCLcal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_likPC = new System.Windows.Forms.TextBox();
            this.btn_ktraCon = new System.Windows.Forms.Button();
            this.btn_ConPcL = new System.Windows.Forms.Button();
            this.btn_conFct = new System.Windows.Forms.Button();
            this.txt_likFct = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kiểm tra kết nối PC Local";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Link PC_Local";
            // 
            // txt_likPC
            // 
            this.txt_likPC.Location = new System.Drawing.Point(96, 63);
            this.txt_likPC.Name = "txt_likPC";
            this.txt_likPC.Size = new System.Drawing.Size(398, 20);
            this.txt_likPC.TabIndex = 2;
            // 
            // btn_ktraCon
            // 
            this.btn_ktraCon.Location = new System.Drawing.Point(419, 159);
            this.btn_ktraCon.Name = "btn_ktraCon";
            this.btn_ktraCon.Size = new System.Drawing.Size(75, 35);
            this.btn_ktraCon.TabIndex = 3;
            this.btn_ktraCon.Text = "Kiểm tra";
            this.btn_ktraCon.UseVisualStyleBackColor = true;
            this.btn_ktraCon.Click += new System.EventHandler(this.btn_ktraCon_Click);
            // 
            // btn_ConPcL
            // 
            this.btn_ConPcL.Location = new System.Drawing.Point(510, 62);
            this.btn_ConPcL.Name = "btn_ConPcL";
            this.btn_ConPcL.Size = new System.Drawing.Size(35, 23);
            this.btn_ConPcL.TabIndex = 4;
            this.btn_ConPcL.UseVisualStyleBackColor = true;
            // 
            // btn_conFct
            // 
            this.btn_conFct.Location = new System.Drawing.Point(510, 110);
            this.btn_conFct.Name = "btn_conFct";
            this.btn_conFct.Size = new System.Drawing.Size(35, 23);
            this.btn_conFct.TabIndex = 7;
            this.btn_conFct.UseVisualStyleBackColor = true;
            // 
            // txt_likFct
            // 
            this.txt_likFct.Location = new System.Drawing.Point(96, 111);
            this.txt_likFct.Name = "txt_likFct";
            this.txt_likFct.Size = new System.Drawing.Size(398, 20);
            this.txt_likFct.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Link FCT";
            // 
            // ConPCLcal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 222);
            this.Controls.Add(this.btn_conFct);
            this.Controls.Add(this.txt_likFct);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_ConPcL);
            this.Controls.Add(this.btn_ktraCon);
            this.Controls.Add(this.txt_likPC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ConPCLcal";
            this.Text = "ConPCLcal";
            this.Load += new System.EventHandler(this.ConPCLcal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_likPC;
        private System.Windows.Forms.Button btn_ktraCon;
        private System.Windows.Forms.Button btn_ConPcL;
        private System.Windows.Forms.Button btn_conFct;
        private System.Windows.Forms.TextBox txt_likFct;
        private System.Windows.Forms.Label label3;
    }
}