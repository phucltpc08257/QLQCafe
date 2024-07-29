using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormDoiMatKhau : Form
    {
        public FormDoiMatKhau()
        {
            InitializeComponent();
        }

        private void FormDoiMatKhau_Load(object sender, EventArgs e)
        {

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string email = txtXacThucEmail.Text;
            string matKhauCu = txtNhapMatKhauCu.Text;
            string matKhauMoi = txtNhapMatKhauMoi.Text;
            string nhapLaiMatKhauMoi = txtXacNhanMatKhauMoi.Text;
            if (matKhauMoi != nhapLaiMatKhauMoi)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu mới không khớp nhau.");
                return;
            }
            var QLBanHang = new LionQuanLyQuanCaPheDataContext();
            var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email && u.MatKhau == matKhauCu);

            if (user != null)
            {
                user.MatKhau = matKhauMoi;
                QLBanHang.SubmitChanges();
                MessageBox.Show("Đổi mật khẩu thành công.");
                FormDangNhap formDangNhap = new FormDangNhap();
                formDangNhap.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu cũ không đúng. Vui lòng kiểm tra lại.");
            }
        }
    }
}
