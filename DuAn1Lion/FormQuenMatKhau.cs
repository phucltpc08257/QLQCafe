using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormQuenMatKhau : Form
    {
        public FormQuenMatKhau()
        {
            InitializeComponent();
        }

        private void FormQuenMatKhau_Load(object sender, EventArgs e)
        {

        }

        private void btnNhanMatKhauMoi_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtXacNhanEmail.Text;
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Vui lòng nhập email.");
                    return;
                }

                using (var QLBanHang = new LionQuanLyQuanCaPheDataContext())
                {
                    var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        string matKhauMoi = GenerateRandomPassword();
                        user.MatKhau = matKhauMoi;
                        QLBanHang.SubmitChanges();
                        SendEmail(email, matKhauMoi);
                    }
                    else
                    {
                        MessageBox.Show("Email không tồn tại trong hệ thống. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtXacNhanEmail.Text;
                string matKhau = txtNhapMatKhauMoi.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ email và mật khẩu.");
                    return;
                }

                using (var QLBanHang = new LionQuanLyQuanCaPheDataContext())
                {
                    var user = QLBanHang.NhanViens.FirstOrDefault(u => u.Email == email && u.MatKhau == matKhau);

                    if (user != null)
                    {
                        FormDangNhap formDangNhap = new FormDangNhap();
                        formDangNhap.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Email hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        private string GenerateRandomPassword()
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                StringBuilder stringBuilder = new StringBuilder();
                Random random = new Random();
                for (int i = 0; i < 8; i++)
                {
                    stringBuilder.Append(chars[random.Next(chars.Length)]);
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tạo mật khẩu mới: {ex.Message}");
                return null;
            }
        }

        private void SendEmail(string toEmail, string newPassword)
        {
            try
            {
                var fromAddress = new MailAddress("phucltpc08257@gmail.com", "Quan Li Quan Ca Phe");
                var toAddress = new MailAddress(toEmail);
                const string fromPassword = "ufdr koar dvdh agun"; // Mật khẩu ứng dụng của bạn
                const string subject = "Mật khẩu mới của bạn";
                string body = $"Mật khẩu mới là: {newPassword}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Sử dụng máy chủ SMTP của Gmail
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                MessageBox.Show("Mật khẩu mới đã được gửi đến email của bạn và đồng thời cũng như được cập nhật trong cơ sở dữ liệu.");
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show($"Lỗi gửi email: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void FormQuenMatKhau_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
            this.Hide();
        }
    }
}
