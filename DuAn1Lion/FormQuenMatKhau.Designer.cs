namespace DuAn1Lion
{
    partial class FormQuenMatKhau
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
            this.txtXacNhanEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNhapMatKhauMoi = new System.Windows.Forms.TextBox();
            this.btnNhanMatKhauMoi = new System.Windows.Forms.Button();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(267, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quên mật khẩu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(103, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nhập Email xác nhận";
            // 
            // txtXacNhanEmail
            // 
            this.txtXacNhanEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXacNhanEmail.Location = new System.Drawing.Point(108, 186);
            this.txtXacNhanEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXacNhanEmail.Name = "txtXacNhanEmail";
            this.txtXacNhanEmail.Size = new System.Drawing.Size(535, 34);
            this.txtXacNhanEmail.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(103, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nhập mật khẩu mới";
            // 
            // txtNhapMatKhauMoi
            // 
            this.txtNhapMatKhauMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNhapMatKhauMoi.Location = new System.Drawing.Point(108, 303);
            this.txtNhapMatKhauMoi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNhapMatKhauMoi.Name = "txtNhapMatKhauMoi";
            this.txtNhapMatKhauMoi.PasswordChar = '*';
            this.txtNhapMatKhauMoi.Size = new System.Drawing.Size(535, 34);
            this.txtNhapMatKhauMoi.TabIndex = 4;
            // 
            // btnNhanMatKhauMoi
            // 
            this.btnNhanMatKhauMoi.BackColor = System.Drawing.Color.Turquoise;
            this.btnNhanMatKhauMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNhanMatKhauMoi.Location = new System.Drawing.Point(469, 364);
            this.btnNhanMatKhauMoi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNhanMatKhauMoi.Name = "btnNhanMatKhauMoi";
            this.btnNhanMatKhauMoi.Size = new System.Drawing.Size(172, 34);
            this.btnNhanMatKhauMoi.TabIndex = 5;
            this.btnNhanMatKhauMoi.Text = "Nhận mật khẩu mới";
            this.btnNhanMatKhauMoi.UseVisualStyleBackColor = false;
            this.btnNhanMatKhauMoi.Click += new System.EventHandler(this.btnNhanMatKhauMoi_Click);
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BackColor = System.Drawing.Color.SpringGreen;
            this.btnXacNhan.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXacNhan.Location = new System.Drawing.Point(260, 432);
            this.btnXacNhan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(263, 58);
            this.btnXacNhan.TabIndex = 6;
            this.btnXacNhan.Text = "Xác nhận";
            this.btnXacNhan.UseVisualStyleBackColor = false;
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // FormQuenMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(163)))), ((int)(((byte)(10)))));
            this.ClientSize = new System.Drawing.Size(800, 538);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.btnNhanMatKhauMoi);
            this.Controls.Add(this.txtNhapMatKhauMoi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtXacNhanEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormQuenMatKhau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormQuenMatKhau";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormQuenMatKhau_FormClosed);
            this.Load += new System.EventHandler(this.FormQuenMatKhau_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtXacNhanEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNhapMatKhauMoi;
        private System.Windows.Forms.Button btnNhanMatKhauMoi;
        private System.Windows.Forms.Button btnXacNhan;
    }
}