using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormChucNangQuanLy : Form
    {
        private string UserRole;
        public FormChucNangQuanLy(string VaiTro)
        {
            InitializeComponent();
            UserRole = VaiTro;
            SetupUI();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            dtgvThongTinNhanVien.CellFormatting += dtgvThongTinNhanVien_CellFormatting;
        }
        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            AnMaVaiTro();
            AnMaNhanVien();
            anMaKH();
        }

        /*---Phân quyền---*/
        private void SetupUI()
        {
            if (UserRole == "VT001")
            {

            }
            else if (UserRole == "VT002")
            {
                // Ẩn các tab không phù hợp
                tclFormChucNang.TabPages.Remove(tpVaiTro);
                // Ẩn các chức năng không cần thiết cho Quản Lý
                btnThemNhanVien.Enabled = false;
                btnSuaNhanVien.Enabled = false;
                btnXoaNhanVien.Enabled = false;
                btnThemNhanVien.Enabled = false;
                btnSuaKhachHang.Enabled = false;
                btnXoaKhachHang.Enabled = false;
                btnSuaSanPham.Enabled = false;
                btnXoaSanPham.Enabled = false;
            }
            else if (UserRole == "VT003")
            {
                // Ẩn các tab không phù hợp
                tclFormChucNang.TabPages.Remove(tpNhanVien);
                tclFormChucNang.TabPages.Remove(tpThongKe);
                tclFormChucNang.TabPages.Remove(tpVaiTro);
                tclFormChucNang.TabPages.Remove(tpNguyenLieu);
                // Ẩn các chức năng không cần thiết cho Nhân viên
                btnSuaKhachHang.Enabled = false;
                btnXoaKhachHang.Enabled = false;
                btnThemSanPham.Enabled = false;
                btnSuaSanPham.Enabled = false;
                btnXoaSanPham.Enabled = false;
            }
        }

        private void tclFormChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tclFormChucNang.SelectedTab == tpNhanVien)
            {
                HienThiNhanVien();
            }

            if (tclFormChucNang.SelectedTab == tpVaiTro)
            {
                HienThioVaiTro();
            }

            if (tclFormChucNang.SelectedTab == tpKhachHang)
            {
                hienThiKhachHang();
                lamMoiKhachHang();
            }

            if (tclFormChucNang.SelectedTab == tpThongKe)
            {
                //HienThiThongKeKhachHang();
                //HienThiThongKeHoaDon();
            }
        }

        /*--Ẩn các mã*/
        private void AnMaNhanVien()
        {
            txtMaNhanVien.ReadOnly = true;
            txtMaNhanVien.TabStop = false;
        }
        private void AnMaVaiTro()
        {
            txtMaVaiTro.ReadOnly = true;
            txtMaVaiTro.TabStop = false;
        }
        private void anMaKH()
        {
            txtMaKhachHang.ReadOnly = true;
            txtMaKhachHang.TabStop = false;
        }

        /*---Vai trò---*/
        /*---Thêm vai trò---*/
        private void ThemVaiTro()
        {
            if (ValidateVaiTroInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    VaiTro ThemVT = new VaiTro()
                    {
                        MaVaiTro = GenerateMaVaiTro(),
                        TenVaiTro = cbbvaitro.Text
                    };

                    try
                    {
                        QLNV.VaiTros.InsertOnSubmit(ThemVT);
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThioVaiTro();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm vai trò: " + ex.Message);
                    }
                }
            }
        }

        /*---Sửa vai trò---*/
        private void SuaVaiTro()
        {
            if (ValidateVaiTroInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string maVT = txtMaVaiTro.Text;
                    var vaiTro = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == maVT);

                    if (vaiTro != null)
                    {
                        vaiTro.TenVaiTro = cbbvaitro.Text;

                        try
                        {
                            QLNV.SubmitChanges();
                            MessageBox.Show("Cập nhật thông tin vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThioVaiTro();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật vai trò: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy vai trò để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /*---Xóa vai trò---*/
        private void XoaVaiTro()
        {
            string maVT = txtMaVaiTro.Text;

            if (!string.IsNullOrEmpty(maVT))
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa vai trò này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                    {
                        var vaiTro = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == maVT);

                        if (vaiTro != null)
                        {
                            try
                            {
                                QLNV.VaiTros.DeleteOnSubmit(vaiTro);
                                QLNV.SubmitChanges();
                                MessageBox.Show("Xóa vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                HienThioVaiTro();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi xóa vai trò: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy vai trò để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vai trò để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /*---Tìm kiếm vai trò---*/
        private void TimKiemVaiTro()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var list = from vt in QLNV.VaiTros
                           where vt.MaVaiTro.Contains(txttimkiemVaiTro.Text) || vt.TenVaiTro.Contains(txttimkiemVaiTro.Text)
                           select new
                           {
                               vt.MaVaiTro,
                               vt.TenVaiTro
                           };

                dtgvVaiTro.DataSource = list.ToList();
            }
        }

        /*---Hiển thị vai trò---*/
        private void HienThioVaiTro()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var list = from vt in QLNV.VaiTros
                           select new
                           {
                               vt.MaVaiTro,
                               vt.TenVaiTro
                           };

                dtgvVaiTro.DataSource = list.ToList();

                var predefinedRoles = new List<string> { "Admin", "Quản lý", "Nhân viên bán hàng " }; // Add more roles as needed
                cbbvaitro.DataSource = predefinedRoles;
            }
        }
        /*---Bắt lỗi vai trò---*/
        private bool ValidateVaiTroInput()
        {
            if (string.IsNullOrWhiteSpace(cbbvaitro.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
        /*---Random Mã vai trò---*/
        private string GenerateMaVaiTro()
        {
            using (var context = new LionQuanLyQuanCaPheDataContext())
            {
                int nextId = 1;
                string newMaVaiTro = $"VT{nextId:D3}";

                while (context.VaiTros.Any(vt => vt.MaVaiTro == newMaVaiTro))
                {
                    nextId++;
                    newMaVaiTro = $"VT{nextId:D3}";
                }

                return newMaVaiTro;
            }
        }

        /*---NHÂN VIÊN---*/
        /*---Thêm nhân viên---*/
        private void ThemNhanVien()
        {
            if (ValidateNhanVienInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string randomPassword = RandomMatKhau();
                    NhanVien ThemNV = new NhanVien()
                    {
                        MaNhanVien = GenerateMaNhanVien(),
                        TenNhanVien = txtTenNhanVien.Text,
                        Email = txtEmail.Text,
                        SDT = txtSDTNhanVien.Text,
                        DiaChi = txtDiaChi.Text,
                        MaVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString(),
                        NgaySinh = dttpNgaySinhNhanVien.Value,
                        GioiTinh = cbbGioiTinhNhanVien.Text,
                        NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value,
                        MatKhau = randomPassword
                    };

                    try
                    {
                        QLNV.NhanViens.InsertOnSubmit(ThemNV);
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        HienThiNhanVien();
                        ClearTextBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm thông tin nhân viên: " + ex.Message);
                    }
                }
            }
        }

        /*---Sửa Nhân viên---*/
        private void SuaNhanVien()
        {
            if (ValidateNhanVienInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string maNV = txtMaNhanVien.Text;
                    var nhanVien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == maNV);

                    if (nhanVien != null)
                    {
                        nhanVien.TenNhanVien = txtTenNhanVien.Text;
                        nhanVien.Email = txtEmail.Text;
                        nhanVien.SDT = txtSDTNhanVien.Text;
                        nhanVien.DiaChi = txtDiaChi.Text;
                        nhanVien.MaVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString();
                        nhanVien.NgaySinh = dttpNgaySinhNhanVien.Value;
                        nhanVien.GioiTinh = cbbGioiTinhNhanVien.Text;
                        nhanVien.NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value;

                        try
                        {
                            QLNV.SubmitChanges();
                            MessageBox.Show("Cập nhật thông tin nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThiNhanVien();
                            ClearTextBox();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /*---Xóa nhân viên---*/
        private void XoaNhanVien()
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                    {
                        string maNV = txtMaNhanVien.Text;
                        var nhanvien = QLNV.NhanViens.FirstOrDefault(k => k.MaNhanVien == maNV);
                        if (nhanvien != null)
                        {
                            QLNV.NhanViens.DeleteOnSubmit(nhanvien);
                            QLNV.SubmitChanges();
                            HienThiNhanVien();
                            MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTextBox();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên có mã số này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }

        /*---Hiển thị nhân viên---*/
        private void HienThiNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var list = from nv in QLNV.NhanViens
                           join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                           select new
                           {
                               nv.MaNhanVien,
                               nv.TenNhanVien,
                               nv.SDT,
                               nv.Email,
                               nv.DiaChi,
                               vt.MaVaiTro,
                               nv.NgaySinh,
                               nv.GioiTinh,
                               nv.MatKhau,
                               nv.NgayBatDauLamViec,
                               vt.TenVaiTro
                           };

                dtgvThongTinNhanVien.DataSource = list.ToList();

                var gioiTinhList = new List<string> { "Nam", "Nữ", };
                cbbGioiTinhNhanVien.DataSource = gioiTinhList;

                var vaiTroList = QLNV.VaiTros.ToList();
                cbbVaiTroCuaNhanVien.DataSource = vaiTroList;
                cbbVaiTroCuaNhanVien.DisplayMember = "TenVaiTro";
                cbbVaiTroCuaNhanVien.ValueMember = "MaVaiTro";
            }
        }
        /*---Bắt lỗi nhập của Nhân viên---*/
        private bool ValidateNhanVienInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Kiểm tra nếu tên có chứa ký tự số
            if (txtTenNhanVien.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên nhân viên không được chứa số. Vui lòng nhập lại.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email của nhân viên");
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Địa chỉ email không hợp lệ. Vui lòng nhập đúng định dạng");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDTNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại của nhân viên");
                return false;
            }

            if (!IsValidPhoneNumber(txtSDTNhanVien.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập lại");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ của nhân viên");
                return false;
            }

            if (cbbVaiTroCuaNhanVien.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò của nhân viên");
                return false;
            }

            return true;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            long number;
            return long.TryParse(phoneNumber, out number) && phoneNumber.Length == 10;
        }
        /*---Làm mới Nhân viên---*/
        private void ClearTextBox()
        {
            txtMaVaiTro.Clear();

            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtEmail.Clear();
            txtSDTNhanVien.Clear();
            txtDiaChi.Clear();

            dttpNgaySinhNhanVien.Value = DateTime.Now;

            dttpNgayBatDauLamCuaNhanVien.Value = DateTime.Now;
        }
        /*---Mã Nhân viên tự sinh---*/
        private string GenerateMaNhanVien()
        {
            using (var context = new LionQuanLyQuanCaPheDataContext())
            {
                int nextId = 1;
                string newMaNhanVien = $"NV{nextId:D3}";

                while (context.NhanViens.Any(nv => nv.MaNhanVien == newMaNhanVien))
                {
                    nextId++;
                    newMaNhanVien = $"NV{nextId:D3}";
                }

                return newMaNhanVien;
            }
        }

        /*---Random mật khẩu---*/
        private string RandomMatKhau()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*---Tìm kiếm Nhân viên---*/
        private void TimKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var list = from nv in QLNV.NhanViens
                           join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                           where nv.MaNhanVien.Contains(txtTimKiemNhanVien.Text) || nv.TenNhanVien.Contains(txtTimKiemNhanVien.Text)
                           select new
                           {
                               nv.MaNhanVien,
                               nv.TenNhanVien,
                               nv.SDT,
                               nv.Email,
                               nv.DiaChi,
                               nv.MaVaiTro,
                               nv.NgaySinh,
                               nv.GioiTinh,
                               nv.MatKhau,
                               nv.NgayBatDauLamViec,
                               TenVaiTro = vt.TenVaiTro
                           };

                dtgvThongTinNhanVien.DataSource = list.ToList();
            }
        }

        /*---KHÁCH HÀNG---*/
        /*---Thêm khách hàng---*/
        private void themKhachHang()
        {

            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Lỗi! Mã nhân viên không hợp lệ. Vui lòng đăng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                FormDangNhap formDangNhap = new FormDangNhap();
                formDangNhap.Show();
                return;
            }

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) || string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
           string.IsNullOrEmpty(txtSDTKhachHang.Text) || string.IsNullOrEmpty(dttpNgaySinhKhachHang.Text) ||
           string.IsNullOrEmpty(txtEmailKhachHang.Text))
            {
                MessageBox.Show("Bạn không thể thêm khi để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) || Regex.IsMatch(txtTenKhachHang.Text, @"\d"))
            {
                MessageBox.Show("Tên không được bỏ trống và không được chứa số!");
                return;
            }

            if (string.IsNullOrEmpty(txtSDTKhachHang.Text) || !Regex.IsMatch(txtSDTKhachHang.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không được bỏ trống và phải có 10 số!");
                return;
            }

            if (string.IsNullOrEmpty(txtEmailKhachHang.Text) || !Regex.IsMatch(txtEmailKhachHang.Text, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("Email không được bỏ trống và phải có định dạng @gmail.com!");
                return;
            }

            try
            {
                var QLKH = new LionQuanLyQuanCaPheDataContext();
                KhachHang Themkh = new KhachHang()
                {
                    MaNhanVien = maNhanVien,
                    TenKhachHang = txtTenKhachHang.Text,
                    DiaChi = txtDiaChiKhachHang.Text,
                    SDT = txtSDTKhachHang.Text,
                    NgaySinh = dttpNgaySinhKhachHang.Value,
                    Email = txtEmailKhachHang.Text


                };
                QLKH.KhachHangs.InsertOnSubmit(Themkh);
                Themkh.MaKhachHang = "KH" + maKh.ToString("D3");
                maKh += 1;

                QLKH.SubmitChanges();
                MessageBox.Show("Thêm thành công");
                hienThiKhachHang();
                lamMoiKhachHang();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi khi thêm");
            }


        }

        /*---Sửa khách hàng---*/
        private void suaKhachHang()
        {
            if (string.IsNullOrEmpty(txtTenKhachHang.Text) || string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                string.IsNullOrEmpty(txtSDTKhachHang.Text) || string.IsNullOrEmpty(dttpNgaySinhKhachHang.Text) ||
                string.IsNullOrEmpty(txtEmailKhachHang.Text))
            {
                MessageBox.Show("Bạn không thể sửa khi để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var QLKH = new LionQuanLyQuanCaPheDataContext();
                var SuaKhachHang = (from kh in QLKH.KhachHangs

                                    where kh.MaKhachHang == dtgvThongTinKhachHang.CurrentRow.
                                    Cells["MaKhachHang"].Value.ToString()
                                    select kh).SingleOrDefault();



                SuaKhachHang.TenKhachHang = txtTenKhachHang.Text;
                SuaKhachHang.DiaChi = txtDiaChiKhachHang.Text;
                SuaKhachHang.SDT = txtSDTKhachHang.Text;
                SuaKhachHang.NgaySinh = dttpNgaySinhKhachHang.Value;
                SuaKhachHang.Email = txtEmailKhachHang.Text;


                try
                {
                    QLKH.SubmitChanges();
                    MessageBox.Show("Cập nhật thành công");
                    hienThiKhachHang();
                    lamMoiKhachHang();

                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi");
                }
            }
        }

        /*---Xóa khách hàng---*/
        private void xoaKhachHang()
        {

            DialogResult dl = MessageBox.Show("Bạn chắc chắn muốn Xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(txtTenKhachHang.Text) || string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                string.IsNullOrEmpty(txtSDTKhachHang.Text) || string.IsNullOrEmpty(dttpNgaySinhKhachHang.Text) ||
                string.IsNullOrEmpty(txtEmailKhachHang.Text))
                {
                    MessageBox.Show("Bạn chưa chọn khách hàng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string maKhToDelete = dtgvThongTinKhachHang.CurrentRow.Cells["MaKhachHang"].Value.ToString();
                    var QLBH = new LionQuanLyQuanCaPheDataContext();
                    var xoaKhachHang = QLBH.KhachHangs.SingleOrDefault(x => x.MaKhachHang == maKhToDelete);
                    if (xoaKhachHang != null)
                    {
                        QLBH.KhachHangs.DeleteOnSubmit(xoaKhachHang);
                        try{
                                QLBH.SubmitChanges();
                                MessageBox.Show("Xóa thành công");
                                hienThiKhachHang();
                                lamMoiKhachHang();
                            }
                        catch (Exception)
                        {
                            MessageBox.Show("Lỗi");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng cần xóa");
                    }

                }
            }
        }

        /*---Tìm kiếm khách hàng---*/
        private void TimKiemKhachHang()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string timKiemValue = txtTimKiemKhachHang.Text.Trim(); // Lấy giá trị tìm kiếm từ textbox

                // Query lấy thông tin nhân viên từ database dựa vào mã nhân viên hoặc tên nhân viên nhập vào
                var timKiem = from kh in QLNV.KhachHangs
                              join nv in QLNV.NhanViens on kh.MaNhanVien equals nv.MaNhanVien into vtGroup
                              from vt in vtGroup.DefaultIfEmpty()
                              where kh.MaKhachHang.Contains(timKiemValue) || kh.TenKhachHang.Contains(timKiemValue)
                              select new
                              {
                                  kh.MaKhachHang,
                                  kh.MaNhanVien,
                                  kh.TenKhachHang,
                                  kh.DiaChi,
                                  kh.SDT,
                                  kh.NgaySinh,
                                  kh.Email
                              };

                dtgvThongTinKhachHang.DataSource = timKiem.ToList();
            }
        }
        /*---Hiển thị khách hàng---*/
        private int maKh = 01;
        string maNhanVien = FormDangNhap.MaNhanVienHienTai;

        private void hienThiKhachHang()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();

            var list = from kh in QLKH.KhachHangs
                       where kh.MaKhachHang == kh.MaKhachHang
                       select new
                       {
                           kh.MaKhachHang,
                           kh.TenKhachHang,
                           kh.DiaChi,
                           kh.SDT,
                           kh.NgaySinh,
                           kh.Email
                       };



            dtgvThongTinKhachHang.DataSource = list.ToList();

        }

        /*---Làm mới khách hàng---*/
        private void lamMoiKhachHang()
        {

            txtMaKhachHang.Clear();
            txtTenKhachHang.Clear();
            txtDiaChiKhachHang.Clear();
            txtSDTKhachHang.Clear();
            txtEmailKhachHang.Clear();
            txtTenKhachHang.Focus();
        }

        /*---Đẩy data lên textbox sử dụng hàm*/
        private void DisplayNhanVienDetails(DataGridViewRow selectedRow)
        {
            txtMaNhanVien.Text = selectedRow.Cells["MaNhanVien"].Value.ToString();
            txtTenNhanVien.Text = selectedRow.Cells["TenNhanVien"].Value.ToString();
            txtEmail.Text = selectedRow.Cells["Email"].Value.ToString();
            txtSDTNhanVien.Text = selectedRow.Cells["SDT"].Value.ToString();
            txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value.ToString();

            dttpNgaySinhNhanVien.Value = Convert.ToDateTime(selectedRow.Cells["NgaySinh"].Value);
            cbbGioiTinhNhanVien.Text = selectedRow.Cells["GioiTinh"].Value.ToString();
            dttpNgayBatDauLamCuaNhanVien.Value = Convert.ToDateTime(selectedRow.Cells["NgayBatDauLamViec"].Value);

            string maVaiTro = selectedRow.Cells["MaVaiTro"].Value.ToString();
            string tenVaiTro = selectedRow.Cells["TenVaiTro"].Value.ToString();

            var vaiTroList = cbbVaiTroCuaNhanVien.DataSource as List<VaiTro>;
            if (vaiTroList != null)
            {
                VaiTro selectedVaiTro = vaiTroList.FirstOrDefault(vt => vt.MaVaiTro == maVaiTro);
                if (selectedVaiTro != null)
                {
                    cbbVaiTroCuaNhanVien.SelectedItem = selectedVaiTro;
                }
            }
        }

        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Sản phẩm---*/
        private void btnThemSanPham_Click(object sender, EventArgs e)
        {

        }

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
        {

        }


        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Khách hàng*/
        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            themKhachHang();
        }

        private void btnSuaKhachHang_Click(object sender, EventArgs e)
        {
            suaKhachHang();
        }

        private void btnXoaKhachHang_Click(object sender, EventArgs e)
        {
            xoaKhachHang();
        }

        private void btnTimKiemKhachHang_Click(object sender, EventArgs e)
        {
            TimKiemKhachHang() ;
        }

        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Nhân viên---*/
        private void btnThemNhanVien_Click(object sender, EventArgs e)
        {
            ThemNhanVien();
        }

        private void btnSuaNhanVien_Click(object sender, EventArgs e)
        {
            SuaNhanVien();
        }

        private void btnXoaNhanVien_Click(object sender, EventArgs e)
        {
            XoaNhanVien();
        }

        private void btnTimKiemNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemNhanVien();
        }

        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Nguyên liệu---*/
        private void btnThemNguyenLieu_Click(object sender, EventArgs e)
        {

        }

        private void btnSuaNguyenLieu_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaNguyenLieu_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiemNguyenLieu_Click(object sender, EventArgs e)
        {

        }


        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Vai trò---*/
        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {

        }

        private void btnsuaVaiTro_Click(object sender, EventArgs e)
        {

        }

        private void btnxoaVaiTro_Click(object sender, EventArgs e)
        {

        }

        private void btntimkiemVaiTro_Click(object sender, EventArgs e)
        {

        }

        /*---Đẩy data lên textbox---*/
        private void dtgvThongTinKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var HTKhachHang = (from kh in QLKH.KhachHangs

                               where kh.MaKhachHang == dtgvThongTinKhachHang.CurrentRow.
                               Cells["MaKhachHang"].Value.ToString()
                               select kh).SingleOrDefault();

            txtMaKhachHang.Text = HTKhachHang.MaKhachHang.ToString();
            txtTenKhachHang.Text = HTKhachHang.TenKhachHang.ToString();
            txtDiaChiKhachHang.Text = HTKhachHang.DiaChi.ToString();
            txtSDTKhachHang.Text = HTKhachHang.SDT.ToString();
            dttpNgaySinhKhachHang.Text = HTKhachHang.NgaySinh.ToString();
            txtEmailKhachHang.Text = HTKhachHang.Email.ToString();
        }

        private void dtgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtgvThongTinNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvThongTinNhanVien.Columns[e.ColumnIndex].Name == "MatKhau")
            {
                if (e.Value != null)
                {
                    e.Value = "**";
                }
            }
        }
        private void dtgvThongTinNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvThongTinNhanVien.Rows[e.RowIndex];
                DisplayNhanVienDetails(selectedRow);
            }
        }

        private void dttgvThongTinNguyenLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtgvVaiTro_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DataGridViewRow row = dtgvVaiTro.Rows[e.RowIndex];
            string maVaiTro = row.Cells["MaVaiTro"].Value.ToString();
            string tenVaiTro = row.Cells["TenVaiTro"].Value.ToString();

            txtMaVaiTro.Text = maVaiTro;
            cbbvaitro.Text = tenVaiTro;
        }

       
    }


}



