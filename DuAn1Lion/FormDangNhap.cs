using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormDangNhap : Form
    {
        public static string MaNhanVienHienTai { get; set; }
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }

        private string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text;
            string matKhau = txtNhapMatKhau.Text;
            string hashedMatKhau = HashPassword(matKhau); // Hash the input password

            var QLBanHang = new LionQuanLyQuanCaPheDataContext();
            var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email && u.MatKhau == hashedMatKhau);

            if (user != null)
            {
                MaNhanVienHienTai = user.MaNhanVien;
                string vaiTro = user.VaiTro.MaVaiTro;
                FormChucNangQuanLy formChucNangQuanLy = new FormChucNangQuanLy(vaiTro);
                formChucNangQuanLy.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.");
            }
        }

        private void btnQuenMatKhau_Click(object sender, EventArgs e)
        {
            FormQuenMatKhau formQuenMatKhau = new FormQuenMatKhau();
            formQuenMatKhau.Show();
            this.Hide();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            FormDoiMatKhau formDoiMatKhau = new FormDoiMatKhau();
            formDoiMatKhau.Show();
            this.Hide();
        }

        private void FormDangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
