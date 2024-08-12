using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormDoiMatKhau : Form
    {
        public FormDoiMatKhau()
        {
            InitializeComponent();
            txtNhapMatKhauCu.PasswordChar = '\u25CF';
            txtNhapMatKhauMoi.PasswordChar = '\u25CF';
            txtXacNhanMatKhauMoi.PasswordChar = '\u25CF';
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string email = txtXacThucEmail.Text.Trim();
            string matKhauCu = txtNhapMatKhauCu.Text;
            string matKhauMoi = txtNhapMatKhauMoi.Text;
            string nhapLaiMatKhauMoi = txtXacNhanMatKhauMoi.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhauCu) ||
                string.IsNullOrWhiteSpace(matKhauMoi) || string.IsNullOrWhiteSpace(nhapLaiMatKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi != nhapLaiMatKhauMoi)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu mới không khớp nhau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedMatKhauCu = HashPassword(matKhauCu); // Hash the current password
            string hashedMatKhauMoi = HashPassword(matKhauMoi); // Hash the new password

            using (var QLBanHang = new LionQuanLyQuanCaPheDataContext())
            {
                var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email && u.MatKhau == hashedMatKhauCu);

                if (user != null)
                {
                    user.MatKhau = hashedMatKhauMoi; // Update with new hashed password
                    QLBanHang.SubmitChanges();

                    MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    this.Close();
                    FormDangNhap form = new FormDangNhap();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Email hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void FormDoiMatKhau_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
            this.Hide();
        }
    }
}