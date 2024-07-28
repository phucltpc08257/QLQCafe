using System;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
           
            this.txtNhapMatKhau.PasswordChar = '*';
          
        }
        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
          

          
            MessageBox.Show("Chúc mừng! Bạn đã đăng nhập thành công vào hệ thống mặc dù bạn eo có tài khoản và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Hệ thống đang Load.......", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("successfully", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FormChucNangQuanLy form = new FormChucNangQuanLy();
                form.ShowDialog();
                form.Dispose(); 

                
                this.Close();   

          
        }
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chúc mừng! Bạn đã đăng nhập thành công vào hệ thống mặc dù bạn không có tài khoản và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            FormDoiMatKhau form = new FormDoiMatKhau();

            form.ShowDialog();
            form = null;
            this.Show();
            this.Close();
        }

        private void picThoat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hệ thống đang Load.......", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Xác nhận Thoát hệ hệ thống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show(" Cúc lẹ......", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
