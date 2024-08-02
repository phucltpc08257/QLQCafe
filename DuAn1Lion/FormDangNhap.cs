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
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        public static string MaNhanVienHienTai { get; set; }
        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            /*
            string email = txtNhapEmail.Text;
            string matKhau = txtNhapMatKhau.Text;

            var QLBanHang = new LionQuanLyQuanCaPheDataContext();
            var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email && u.MatKhau == matKhau);



            if (user != null)
            {
                string vaiTro = user.VaiTro.MaVaiTro;
                FormChucNangQuanLy formChucNang = new FormChucNangQuanLy(vaiTro);
                formChucNang.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.");
            }



            /*
            FormChucNangQuanLy formChucNangQuanLy = new FormChucNangQuanLy();
            formChucNangQuanLy.Show();
            this.Hide();
            */

        }
    }
}
