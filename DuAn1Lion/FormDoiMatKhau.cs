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
    public partial class FormDoiMatKhau : Form
    {
        public FormDoiMatKhau()
        {
            InitializeComponent();
            // Thiết lập ký tự ẩn mật khẩu ban đầu cho các TextBox

            txtNhapMatKhauCu.PasswordChar = '\u25CF';
            txtNhapMatKhauMoi.PasswordChar = '\u25CF';
            txtXacNhanMatKhauMoi.PasswordChar = '\u25CF';
        }

        private void FormDoiMatKhau_Load(object sender, EventArgs e)
        {

        }

        public string HashPassword(string password)
        {
            // Mã hóa mật khẩu thành mảng byte
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();

                // Chuyển đổi mảng byte thành chuỗi hexa
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Định dạng hexa, mỗi byte thành hai chữ số hexa
                }

                // Lấy chuỗi hexa
                string hexHash = sb.ToString();

                // Thay thế toàn bộ chuỗi hexa bằng dấu '*'
                string maskedHash = new string('*', hexHash.Length);

                return maskedHash;
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string email = txtXacThucEmail.Text.Trim();
            string matKhauCu = txtNhapMatKhauCu.Text;
            string matKhauMoi = txtNhapMatKhauMoi.Text;
            string nhapLaiMatKhauMoi = txtXacNhanMatKhauMoi.Text;

            // Kiểm tra các trường dữ liệu không được rỗng
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhauCu) ||
                string.IsNullOrWhiteSpace(matKhauMoi) || string.IsNullOrWhiteSpace(nhapLaiMatKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu mới và nhập lại mật khẩu mới có khớp nhau không
            if (matKhauMoi != nhapLaiMatKhauMoi)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu mới không khớp nhau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mã hóa mật khẩu cũ và mới
            string hashedMatKhauCu = HashPassword(matKhauCu);
            string hashedMatKhauMoi = HashPassword(matKhauMoi);

            using (var QLBanHang = new LionQuanLyQuanCaPheDataContext())
            {
                // Kiểm tra người dùng có tồn tại không
                var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    // Kiểm tra mật khẩu cũ
                    if (user.MatKhau != hashedMatKhauCu)
                    {
                        MessageBox.Show("Mật khẩu cũ không đúng. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cập nhật mật khẩu mới cho user và lưu vào cơ sở dữ liệu
                    user.MatKhau = hashedMatKhauMoi;
                    QLBanHang.SubmitChanges();

                    MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    this.Close();
                    FormDangNhap form = new FormDangNhap();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Email không tồn tại. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
