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
        private FormChucNangQuanLy FormChucNang;
        string User = "sa";
        string Password = "1";
        public FormDangNhap()
        {
            InitializeComponent();
            FormChucNang = new FormChucNangQuanLy();
        }
        
       
        public static string MaNhanVienHienTai { get; set; }
        public bool checkDangNhap(string user, string password)
        {
            if (user == this.User && password == Password)
            {
                return true;
            }
            return false;
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string email = txtNhapEmail.Text.Trim();
            string mat_khau = txtNhapMatKhau.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Lỗi! Vui lòng không để trống Email của bạn!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(mat_khau))
            {
                MessageBox.Show("Lỗi! Vui lòng không để trống Mật Khẩu của bạn!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dbContext = new LionQuanLyQuanCaPheDataContext())
            {
                var user = (from nv in dbContext.NhanViens
                            join vt in dbContext.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                            where nv.Email == email && nv.MatKhau == mat_khau
                            select new
                            {
                                nv.MaNhanVien,
                                vt.TenVaiTro
                            }).FirstOrDefault();

                if (user != null)
                {
                    MaNhanVienHienTai = user.MaNhanVien;
                    if (user.TenVaiTro == "Admin")
                    {
                        MessageBox.Show("Đăng Nhập Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        FormChucNang.Show();
                    }
                    
                    else if (user.TenVaiTro == "Quản lý")
                    {
                        MessageBox.Show("Đăng Nhập Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        FormChucNang.Show();
                    }
                    else if (user.TenVaiTro == "Nhân viên bán hàng")
                    {
                        MessageBox.Show("Đăng Nhập Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        FormChucNang.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Mật Khẩu Hoặc Tài Khoản Không Đúng!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
       

    }
}
