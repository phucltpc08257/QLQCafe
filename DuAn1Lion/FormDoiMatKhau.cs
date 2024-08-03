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
        private bool isPasswordVisible = false;
        // Sử dụng hình ảnh từ Resources
     

        public FormDoiMatKhau()
        {
            InitializeComponent();
            // Thêm PictureBox cho nút con mắt cho txtNhapMatKhauMoi
            AddShowPasswordPictureBox(txtNhapMatKhauMoi);

            // Thêm PictureBox cho nút con mắt cho txtXacNhanMatKhauMoi
            AddShowPasswordPictureBox(txtXacNhanMatKhauMoi);
            // Thiết lập ký tự ẩn mật khẩu ban đầu cho các TextBox

            txtNhapMatKhauCu.PasswordChar = '\u25CF';
            txtNhapMatKhauMoi.PasswordChar = '\u25CF';
            txtXacNhanMatKhauMoi.PasswordChar = '\u25CF';

            // Thêm PictureBox cho nút con mắt
            PictureBox pbShowPassword = new PictureBox();
            pbShowPassword.Image = Properties.Resources.eye_closed; // Hình ảnh mặc định của nút con mắt
            pbShowPassword.Size = new System.Drawing.Size(20, 20);
            pbShowPassword.SizeMode = PictureBoxSizeMode.StretchImage;
            pbShowPassword.Cursor = Cursors.Hand;
            pbShowPassword.Click += PbShowPassword_Click;

            // Đặt PictureBox vào bên phải của TextBox txtNhapMatKhauCu
            int textBoxWidth = txtNhapMatKhauCu.Size.Width;
            int textBoxHeight = txtNhapMatKhauCu.Size.Height;
            pbShowPassword.Location = new System.Drawing.Point(textBoxWidth - 25, 2); // Điều chỉnh vị trí cho phù hợp
            txtNhapMatKhauCu.Controls.Add(pbShowPassword);
          

        }
        private void AddShowPasswordPictureBox(TextBox textBox)
        {
            // Tạo PictureBox cho TextBox được chuyền vào
            PictureBox pbShowPassword = new PictureBox();
            pbShowPassword.Image = Properties.Resources.eye_closed; // Hình ảnh mặc định của nút con mắt (mật khẩu ẩn)
            pbShowPassword.Size = new System.Drawing.Size(20, 20);
            pbShowPassword.SizeMode = PictureBoxSizeMode.StretchImage;
            pbShowPassword.Cursor = Cursors.Hand;
            pbShowPassword.Tag = textBox; // Lưu trữ thông tin TextBox liên quan

            // Đặt vị trí của PictureBox vào bên phải của TextBox
            int textBoxWidth = textBox.Size.Width;
            int textBoxHeight = textBox.Size.Height;
            pbShowPassword.Location = new System.Drawing.Point(textBoxWidth - 25, 2); // Điều chỉnh vị trí cho phù hợp
            textBox.Controls.Add(pbShowPassword);

            // Xử lý sự kiện Click của PictureBox
            pbShowPassword.Click += PbShowPassword1_Click;
           
        }
        private void FormDoiMatKhau_Load(object sender, EventArgs e)
        {

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
        private void PbShowPassword1_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            TextBox textBox = (TextBox)pb.Tag; // Lấy TextBox mà PictureBox này đang liên kết đến

            // Đảo ngược trạng thái hiển thị mật khẩu của TextBox tương ứng
            if (textBox.PasswordChar == '\u25CF')
            {
                textBox.PasswordChar = '\0'; // Hiển thị mật khẩu
                pb.Image = Properties.Resources.eye_open; // Thay đổi hình ảnh nút con mắt
            }
            else
            {
                textBox.PasswordChar = '\u25CF'; // Ẩn mật khẩu
                pb.Image = Properties.Resources.eye_closed; // Thay đổi hình ảnh nút con mắt
            }
        }

        private void PbShowPassword_Click(object sender, EventArgs e)
        {
            // Đảo ngược trạng thái hiển thị mật khẩu
            isPasswordVisible = !isPasswordVisible;

            // Cập nhật ký tự hiển thị cho các TextBox txtXacThucEmail.PasswordChar = isPasswordVisible ? '\0' : '\u25CF';
            txtNhapMatKhauCu.PasswordChar = isPasswordVisible ? '\0' : '\u25CF';
      
            PictureBox pb = (PictureBox)sender;
            pb.Image = isPasswordVisible ? Properties.Resources.eye_open : Properties.Resources.eye_closed;
        }

        private void pcdoimatkhau_Click(object sender, EventArgs e)
        {

            this.Hide();
            this.Close();
            FormDangNhap form = new FormDangNhap();
            form.ShowDialog();


        }
    }
}
