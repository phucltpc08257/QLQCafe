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
        private void KhoiTaoControls()
        {
            btnThemSanPham = this.Controls.Find("btnThemSanPham", true).FirstOrDefault() as Button;
            btnSuaSanPham = this.Controls.Find("btnSuaSanPham", true).FirstOrDefault() as Button;
            btnXoaSanPham = this.Controls.Find("btnXoaSanPham", true).FirstOrDefault() as Button;
            btnThemKhachHang = this.Controls.Find("btnThemKhachHang", true).FirstOrDefault() as Button;
            btnSuaKhachHang = this.Controls.Find("btnSuaKhachHang", true).FirstOrDefault() as Button;
            btnXoaKhachHang = this.Controls.Find("btnXoaKhachHang", true).FirstOrDefault() as Button;
            btnThemNhanVien = this.Controls.Find("btnThemNhanVien", true).FirstOrDefault() as Button;
            btnSuaNhanVien = this.Controls.Find("btnSuaNhanVien", true).FirstOrDefault() as Button;
            btnXoaNhanVien = this.Controls.Find("btnXoaNhanVien", true).FirstOrDefault() as Button;
            tabVaiTro = this.Controls.Find("tabVaiTro", true).FirstOrDefault() as TabPage;
        }
        private Button btnThemSanPham;
        private Button btnSuaSanPham;
        private Button btnXoaSanPham;
        private Button btnThemKhachHang;
        private Button btnSuaKhachHang;
        private Button btnXoaKhachHang;
        private Button btnThemNhanVien;
        private Button btnSuaNhanVien;
        private Button btnXoaNhanVien;
        private TabPage tabVaiTro;
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
                        AnButtonVaTabChoNhanVien();
                        FormChucNang.Show();
                    }
                    /*
                    else if (user.TenVaiTro == "NhanVien")
                    {
                        MessageBox.Show("Đăng Nhập Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        FormChucNang.Show();
                        AnButtonVaTabChoNhanVien();
                    }
                    */
                }
                else
                {
                    MessageBox.Show("Mật Khẩu Hoặc Tài Khoản Không Đúng!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void AnButtonVaTabChoNhanVien()
        {
            if (btnThemSanPham != null) btnThemSanPham.Visible = false;
            if (btnSuaSanPham != null) btnSuaSanPham.Visible = false;
            if (btnXoaSanPham != null) btnXoaSanPham.Visible = false;
            if (btnThemKhachHang != null) btnThemKhachHang.Visible = false;
            if (btnSuaKhachHang != null) btnSuaKhachHang.Visible = false;
            if (btnXoaKhachHang != null) btnXoaKhachHang.Visible = false;
            if (btnThemNhanVien != null) btnThemNhanVien.Visible = false;
            if (btnSuaNhanVien != null) btnSuaNhanVien.Visible = false;
            if (btnXoaNhanVien != null) btnXoaNhanVien.Visible = false;
        }

    }
}
