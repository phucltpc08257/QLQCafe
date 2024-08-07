using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using System.Xml.Linq;

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
            Console.WriteLine(RandomMatKhau());
        }
        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            AnMaVaiTro();
            AnMaNhanVien();
            anMaKH();
            hienThiSan_Pham();
            Hien_Thi_Nguyen_Lieu();
            hienThiKhachHang();
            anSanPham_NguyenLieu();
            Hien_Thi_Gia_Text_Box();
            KhongChoNguoiDungNhapTXT();
            hienThiOrder();
            So_Luong_Ban_Ra();
            hienThi_ThongKe_SanPham();
            Hien_Thi_Thong_Ke_Nguyen_Lieu();
            Order();
            ChanKiTuOrder();
            ThongKeNV_KH();
            HienThiThongKeKhachHang();
            HienThiThongKeNhanVien();
            HienThiThongKeHoaDon();
        }

        private void KhongChoNguoiDungNhapTXT()
        {
            cbbVaiTroCuaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGioiTinhNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            /*---Giam gia---*/
            cbbGiamGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGiamGia.Items.Add("0%");
            cbbGiamGia.Items.Add("5%");
            cbbGiamGia.Items.Add("10%");
            cbbGiamGia.Items.Add("20%");
            cbbGiamGia.Items.Add("30%");
            cbbGiamGia.Items.Add("40%");
            cbbGiamGia.Items.Add("50%");
            cbbGiamGia.SelectedIndex = 0;
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
                ClearTextBox();
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

            if (tclFormChucNang.SelectedTab == tpSanPham)
            {
                hienThiSan_Pham();
                LamMoi_SP();
            }

            if (tclFormChucNang.SelectedTab == tpNguyenLieu)
            {
                Hien_Thi_Nguyen_Lieu();
                LamMoi_NguyenLieu();
            }

            if (tclFormChucNang.SelectedTab == tpThongKe)
            {

            }

            if (tclFormChucNang.SelectedTab == tpOrder)
            {
                flowLayoutPanelMenu.Controls.Clear();
                hienThiOrder();
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
        private void anSanPham_NguyenLieu()
        {
            txtMaSanPham.ReadOnly = true;
            txtMaSanPham.TabStop = false;
            txtMaNguyenLieu.ReadOnly = true;
            txtMaNguyenLieu.TabStop = false;
        }

        private void ThongKeNV_KH()
        {
            //Combobox Thống kê khách hàng
            cbb_Chon_Soluong_Ban_Ra_KhachHang.Items.AddRange(new object[] {
                "", "Số Lượng Hóa Đơn Tháng", "Số Lượng Hóa Đơn Năm", "Tổng Giá Trị Tháng", "Tổng Giá Trị Năm"
            });
            cbb_Chon_Soluong_Ban_Ra_KhachHang.SelectedIndex = 0;
            cbb_Chon_Soluong_Ban_Ra_KhachHang.SelectedIndexChanged += cbb_Chon_Soluong_Ban_Ra_KhachHang_SelectedIndexChanged;

            //Combobox Thống kê nhân viên
            cbb_Chon_Soluong_Ban_Ra_NhanVien.Items.AddRange(new object[] {
                 "Số Lượng Hóa Đơn Tuần", "Số Lượng Hóa Đơn Tháng", "Số Lượng Hóa Đơn Năm"
                });
            cbb_Chon_Soluong_Ban_Ra_NhanVien.SelectedIndex = 0;
            cbb_Chon_Soluong_Ban_Ra_NhanVien.SelectedIndexChanged += cbb_Chon_Soluong_Ban_Ra_NhanVien_SelectedIndexChanged;

        }
        private void Order()
        {
            txtTongTien.ReadOnly = true;
            txtTongTien.TabStop = false;
            txtCanThanhToan.ReadOnly = true;
            txtCanThanhToan.TabStop = false;
            txtTienThua.ReadOnly = true;
            txtTienThua.TabStop = false;

            cbbGiamGia.SelectedIndexChanged += cbbGiamGia_SelectedIndexChanged;
        }

        /*---Vai trò---*/
        /*---Thêm vai trò---*/
        private void ThemVaiTro()
        {
            if (ValidateVaiTroInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    // Lấy tên vai trò từ combobox
                    string tenVaiTro = cbbvaitro.Text;

                    // Kiểm tra xem vai trò đã tồn tại chưa
                    var existingVaiTro = QLNV.VaiTros.FirstOrDefault(vt => vt.TenVaiTro == tenVaiTro);
                    if (existingVaiTro != null)
                    {
                        MessageBox.Show("Vai trò này đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng quá trình thêm vai trò
                    }

                    // Tạo mới vai trò
                    VaiTro ThemVT = new VaiTro()
                    {
                        MaVaiTro = GenerateMaVaiTro(),
                        TenVaiTro = tenVaiTro
                    };

                    try
                    {
                        // Thêm vai trò vào cơ sở dữ liệu
                        QLNV.VaiTros.InsertOnSubmit(ThemVT);
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThioVaiTro(); // Cập nhật giao diện hoặc danh sách vai trò
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
                // Lấy từ khóa tìm kiếm từ textbox và loại bỏ khoảng trắng
                string keyword = txttimkiemVaiTro.Text.Trim();

                // Kiểm tra nếu từ khóa tìm kiếm là rỗng
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Kết thúc phương thức nếu không có từ khóa tìm kiếm
                }

                // Tìm kiếm vai trò
                var list = from vt in QLNV.VaiTros
                           where vt.MaVaiTro.Contains(keyword) || vt.TenVaiTro.Contains(keyword)
                           select new
                           {
                               vt.MaVaiTro,
                               vt.TenVaiTro
                           };

                // Chuyển danh sách tìm được thành danh sách
                var resultList = list.ToList();

                // Gán dữ liệu cho DataGridView
                dtgvVaiTro.DataSource = resultList;

                // Hiển thị thông báo tìm kiếm
                if (resultList.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy vai trò nào với thông tin bạn đã nhập.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Đã tìm thấy {resultList.Count} vai trò.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

                var predefinedRoles = new List<string> { " ", "Admin", "Quản lý", "Nhân viên bán hàng " }; // Add more roles as needed
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

            // Kiểm tra nếu tên vai trò có chứa ký tự đặc biệt (không tính khoảng trắng)
            if (ContainsSpecialCharactersIgnoringWhitespace(cbbvaitro.Text))
            {
                MessageBox.Show("Tên vai trò không được chứa ký tự đặc biệt. Vui lòng nhập lại.");
                return false;
            }

            if (ContainsDigits(cbbvaitro.Text))
            {
                MessageBox.Show("Tên vai trò không được chứa số. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
        private bool ContainsSpecialCharactersIgnoringWhitespace(string input)
        {
            foreach (char c in input)
            {
                // Kiểm tra nếu là ký tự đặc biệt và không phải khoảng trắng
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ContainsDigits(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
        /*---Random Mã vai trò---*/
        private string GenerateMaVaiTro()
        {
            using (var context = new LionQuanLyQuanCaPheDataContext())
            {
                // Lấy mã vai trò lớn nhất hiện tại trong cơ sở dữ liệu
                var maxMaVaiTro = context.VaiTros
                    .OrderByDescending(vt => vt.MaVaiTro)
                    .Select(vt => vt.MaVaiTro)
                    .FirstOrDefault();

                int nextId = 1;

                if (maxMaVaiTro != null)
                {
                    // Tách phần số của mã vai trò
                    nextId = int.Parse(maxMaVaiTro.Substring(2)) + 1;
                }

                // Tạo mã vai trò mới với định dạng "VTxxx"
                string newMaVaiTro = $"VT{nextId:D3}";

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
                               nv.NgaySinh,
                               nv.GioiTinh,
                               nv.MatKhau,
                               nv.NgayBatDauLamViec,
                               vt.TenVaiTro,
                               vt.MaVaiTro,
                           };

                dtgvThongTinNhanVien.DataSource = list.ToList();


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

            // Kiểm tra nếu tên có chứa ký tự đặc biệt
            if (ContainsSpecialCharacters(txtTenNhanVien.Text))
            {
                MessageBox.Show("Tên nhân viên không được chứa ký tự đặc biệt. Vui lòng nhập lại.");
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

            // Kiểm tra nếu địa chỉ có chứa ký tự đặc biệt
            if (ContainsSpecialCharacters(txtDiaChi.Text))
            {
                MessageBox.Show("Địa chỉ không được chứa ký tự đặc biệt. Vui lòng nhập lại.");
                return false;
            }

            if (dttpNgaySinhNhanVien.Value == DateTime.Now || dttpNgaySinhNhanVien.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng chọn ngày sinh hợp lệ.");
                return false;
            }

            DateTime ngaySinhNhanVien = dttpNgaySinhNhanVien.Value;
            DateTime ngayHienTai = DateTime.Today;
            DateTime ngayDuocPhepSinh = ngayHienTai.AddYears(-18);

            if (ngaySinhNhanVien >= ngayHienTai || ngaySinhNhanVien >= ngayDuocPhepSinh)
            {
                MessageBox.Show("Tuổi của nhân viên chưa đủ 18 tuổi");
                return false;
            }

            if (cbbVaiTroCuaNhanVien.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò của nhân viên");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbbGioiTinhNhanVien.Text))
            {
                MessageBox.Show("Vui lòng chọn giới tính của nhân viên");
                return false;
            }

            // Kiểm tra giới tính có chứa ký tự đặc biệt
            if (ContainsSpecialCharacters(cbbGioiTinhNhanVien.Text))
            {
                MessageBox.Show("Giới tính không được chứa ký tự đặc biệt. Vui lòng nhập lại.");
                return false;
            }

            return true;
        }

        private bool ContainsSpecialCharacters(string input)
        {
            // Pattern để kiểm tra có ký tự đặc biệt (không phải chữ cái, số, khoảng trắng, và các ký tự có dấu)
            string pattern = @"[^\p{L}\p{N}\s,]"; // Chỉ cho phép chữ cái, số và khoảng trắng, bao gồm các ký tự Unicode

            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private void BatLoiVaiTro()
        {
            // Kiểm tra xem có chọn vai trò từ ComboBox hay không
            if (cbbVaiTroCuaNhanVien.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vai trò cho nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng quá trình lưu nếu chưa chọn vai trò
            }
            string vaiTro = cbbVaiTroCuaNhanVien.SelectedItem as string;

            // Kiểm tra xem người dùng đã chọn vai trò hay chưa
            if (string.IsNullOrEmpty(vaiTro))
            {
                MessageBox.Show("Vui lòng chọn vai trò của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Kiểm tra số điện thoại phải có số 0 ở đầu
            if (!phoneNumber.StartsWith("0"))
            {
                return false;
            }

            long number;
            return long.TryParse(phoneNumber, out number) && phoneNumber.Length == 10;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                // Kiểm tra xem email có hợp lệ không
                var addr = new System.Net.Mail.MailAddress(email);

                // Kiểm tra xem địa chỉ email có phải là @gmail.com không
                return addr.Address == email && email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
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
                // Lấy mã nhân viên lớn nhất hiện tại trong cơ sở dữ liệu
                var maxMaNhanVien = context.NhanViens
                    .OrderByDescending(nv => nv.MaNhanVien)
                    .Select(nv => nv.MaNhanVien)
                    .FirstOrDefault();

                int nextId = 1;

                if (maxMaNhanVien != null)
                {
                    // Tách phần số của mã nhân viên
                    nextId = int.Parse(maxMaNhanVien.Substring(2)) + 1;
                }

                // Tạo mã nhân viên mới với định dạng "NVxxx"
                string newMaNhanVien = $"NV{nextId:D3}";

                return newMaNhanVien;
            }
        }

        /*---Random mật khẩu---*/
        private static string RandomMatKhau()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string randomString = new string(Enumerable.Repeat(chars, 8)
                                          .Select(s => s[random.Next(s.Length)]).ToArray());

            // Tạo đối tượng mã hóa MD5
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi ngẫu nhiên sang mảng byte
                byte[] inputBytes = Encoding.ASCII.GetBytes(randomString);

                // Tính toán mã băm MD5
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Định dạng hexa, mỗi byte thành hai chữ số hexa
                }

                // Lấy 8 ký tự đầu tiên của chuỗi hexa và thay thế bằng dấu ''
                string md5Hash = sb.ToString();
                string maskedHash = new string('*', 8); // Dự kiến là dấu '*'

                return maskedHash;
            }
        }

        /*---Tìm kiếm Nhân viên---*/
        private void TimKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                // Lấy từ khóa tìm kiếm từ textbox và loại bỏ khoảng trắng thừa
                string keyword = txtTimKiemNhanVien.Text.Trim();

                // Kiểm tra nếu từ khóa tìm kiếm là rỗng
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Kết thúc phương thức nếu không có từ khóa tìm kiếm
                }

                // Tìm kiếm nhân viên
                var list = from nv in QLNV.NhanViens
                           join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                           where nv.MaNhanVien.Contains(keyword) || nv.TenNhanVien.Contains(keyword)
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

                // Chuyển danh sách tìm được thành danh sách
                var resultList = list.ToList();

                // Gán dữ liệu cho DataGridView
                dtgvThongTinNhanVien.DataSource = resultList;

                // Hiển thị thông báo tìm kiếm
                if (resultList.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào với thông tin bạn đã nhập.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Đã tìm thấy {resultList.Count} nhân viên.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) ||
                Regex.IsMatch(txtTenKhachHang.Text, @"\d") ||
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) // Chấp nhận tất cả các ký tự chữ cái Unicode
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            // Kiểm tra địa chỉ khách hàng (chấp nhận dấu)
            if (string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                Regex.IsMatch(txtDiaChiKhachHang.Text, @"[^\p{L}\d\s,.-]")) // Chấp nhận tất cả các ký tự chữ cái Unicode và các ký tự dấu câu hợp lệ
            {
                MessageBox.Show("Địa chỉ không được bỏ trống và không được chứa ký tự đặc biệt ngoài dấu câu hợp lệ!");
                return;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrEmpty(txtSDTKhachHang.Text) ||
                !Regex.IsMatch(txtSDTKhachHang.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không được bỏ trống, không được chứa ký tự đặc biệt và phải có 10 số!");
                return;
            }

            // Kiểm tra email
            if (string.IsNullOrEmpty(txtEmailKhachHang.Text) ||
                !Regex.IsMatch(txtEmailKhachHang.Text, @"^[a-zA-Z0-9._%+-]+@gmail\.com$") ||
                Regex.IsMatch(txtEmailKhachHang.Text, @"[^\w.@+-]")) // Chấp nhận các ký tự chữ cái, số và một số ký tự đặc biệt
            {
                MessageBox.Show("Email không được bỏ trống, phải có định dạng @gmail.com và không chứa ký tự đặc biệt ngoài những ký tự được phép!");
                return;
            }

            // Kiểm tra ngày sinh
            DateTime ngaySinhKhachHang = dttpNgaySinhKhachHang.Value;
            DateTime ngayHienTai = DateTime.Today;

            if (ngaySinhKhachHang == ngayHienTai || ngaySinhKhachHang > ngayHienTai)
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng chọn ngày sinh hợp lệ.");
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
                return;
            }

            // Kiểm tra tên khách hàng (chấp nhận dấu)
            if (string.IsNullOrEmpty(txtTenKhachHang.Text) ||
                Regex.IsMatch(txtTenKhachHang.Text, @"\d") ||
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) // Chấp nhận tất cả các ký tự chữ cái Unicode
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) ||
                Regex.IsMatch(txtTenKhachHang.Text, @"\d") ||
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) // Chấp nhận tất cả các ký tự chữ cái Unicode
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            // Kiểm tra địa chỉ khách hàng (chấp nhận dấu)
            if (string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                Regex.IsMatch(txtDiaChiKhachHang.Text, @"[^\p{L}\d\s,.-]")) // Chấp nhận tất cả các ký tự chữ cái Unicode và các ký tự dấu câu hợp lệ
            {
                MessageBox.Show("Địa chỉ không được bỏ trống và không được chứa ký tự đặc biệt ngoài dấu câu hợp lệ!");
                return;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrEmpty(txtSDTKhachHang.Text) ||
                !Regex.IsMatch(txtSDTKhachHang.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không được bỏ trống, không được chứa ký tự đặc biệt và phải có 10 số!");
                return;
            }

            // Kiểm tra email
            if (string.IsNullOrEmpty(txtEmailKhachHang.Text) ||
                !Regex.IsMatch(txtEmailKhachHang.Text, @"^[a-zA-Z0-9._%+-]+@gmail\.com$") ||
                Regex.IsMatch(txtEmailKhachHang.Text, @"[^\w.@+-]")) // Chấp nhận các ký tự chữ cái, số và một số ký tự đặc biệt
            {
                MessageBox.Show("Email không được bỏ trống, phải có định dạng @gmail.com và không chứa ký tự đặc biệt ngoài những ký tự được phép!");
                return;
            }

            // Kiểm tra ngày sinh
            DateTime ngaySinh = dttpNgaySinhKhachHang.Value;
            DateTime ngayHienTai = DateTime.Now;
            if (ngaySinh > ngayHienTai || ngaySinh == ngayHienTai)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn hoặc bàng ngày hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var SuaKhachHang = (from kh in QLKH.KhachHangs
                                where kh.MaKhachHang == dtgvThongTinKhachHang.CurrentRow.Cells["MaKhachHang"].Value.ToString()
                                select kh).SingleOrDefault();

            if (SuaKhachHang != null)
            {
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
                    MessageBox.Show("Lỗi khi cập nhật khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        try
                        {
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
        //  TIM KIEM KHÁCH HÀNG
        private void TimKiemKhachHang()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();

            // Kiểm tra nếu người dùng chưa nhập khách hàng cần tìm
            if (string.IsNullOrEmpty(txtTimKiemKhachHang.Text))
            {
                MessageBox.Show("Bạn chưa nhập khách hàng cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string timKiemValue = txtTimKiemKhachHang.Text.Trim();

            // Kiểm tra nếu giá trị tìm kiếm chứa ký tự đặc biệt (chỉ cho phép ký tự chữ cái, số và dấu)
            if (Regex.IsMatch(timKiemValue, @"[^\p{L}\d\s\-\,\.\/]")) // Chấp nhận tất cả các ký tự chữ cái Unicode, số, khoảng trắng, dấu gạch ngang, dấu phẩy, dấu chấm, và dấu gạch chéo
            {
                MessageBox.Show("Giá trị tìm kiếm không được chứa ký tự đặc biệt ngoài các ký tự có dấu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                // Query lấy thông tin khách hàng từ database dựa vào mã khách hàng, tên khách hàng, địa chỉ hoặc ngày sinh nhập vào
                var timKiem = from kh in QLNV.KhachHangs
                              join nv in QLNV.NhanViens on kh.MaNhanVien equals nv.MaNhanVien into vtGroup
                              from vt in vtGroup.DefaultIfEmpty()
                              where kh.MaKhachHang.Contains(timKiemValue) ||
                                    kh.TenKhachHang.Contains(timKiemValue) ||
                                    kh.DiaChi.Contains(timKiemValue) ||
                                    kh.SDT.Contains(timKiemValue) ||
                                    kh.NgaySinh.ToString().Contains(timKiemValue) || // Convert ngày sinh sang chuỗi để tìm kiếm
                                    kh.Email.Contains(timKiemValue) ||
                                    kh.MaNhanVien.Contains(timKiemValue)
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
                if (timKiem.ToList().Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng với thông tin đã nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        /*---SẢN PHẨM---*/
        /*---Thêm sản phẩm---*/
        private void Them_San_Pham()
        {
            if (string.IsNullOrEmpty(txtTenSanPham.Text) || string.IsNullOrEmpty(txtGiaNhap.Text) || string.IsNullOrEmpty(txtGiaBan.Text))
            {
                MessageBox.Show("Vui Lòng Điền Đầy Đủ Dữ Liệu!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Regex regex = new Regex(@"^[\p{L}\s]+$");
            if (!regex.IsMatch(txtTenSanPham.Text))
            {
                MessageBox.Show("Lỗi! Vui Lòng Chỉ Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Lỗi! Giá Nhập Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out decimal giaBan) || giaBan <= 0)
            {
                MessageBox.Show("Lỗi! Giá Bán Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (giaNhap >= giaBan)
            {
                MessageBox.Show("Lỗi! Giá Nhập Phải Nhỏ Hơn Giá Bán", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (giaNhap >= 70000)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Quá 70", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (giaBan >= 100000)
            {
                MessageBox.Show("Lỗi! Giá Bán Không Được Quá 100", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            giaNhap *= 1000;
            giaBan *= 1000;

            if (pic_AnhSanPham.Image == null)
            {
                MessageBox.Show("Vui lòng chọn ảnh cho sản phẩm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] imgData = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pic_AnhSanPham.Image.Save(ms, pic_AnhSanPham.Image.RawFormat);
                    imgData = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý ảnh: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maNhanVien = FormDangNhap.MaNhanVienHienTai;
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Lỗi! Mã nhân viên không hợp lệ. Vui lòng đăng nhập lại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                FormDangNhap loginForm = new FormDangNhap();
                loginForm.ShowDialog();
                this.Show();
                return;
            }

            using (var sp = new LionQuanLyQuanCaPheDataContext())
            {
                string auto_Ma_Sp = GetNewMaSanPham(sp);
                SanPham ThemSp = new SanPham
                {
                    MaSanPham = auto_Ma_Sp,
                    MaNhanVien = maNhanVien,
                    TenSanPham = txtTenSanPham.Text,
                    GiaBan = giaBan,
                    GiaNhap = giaNhap,
                    HinhAnh = imgData != null ? new System.Data.Linq.Binary(imgData) : null
                };

                sp.SanPhams.InsertOnSubmit(ThemSp);

                try
                {
                    sp.SubmitChanges();
                    MessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hienThiSan_Pham();
                    hienThi_ThongKe_SanPham();
                    LamMoi_SP();
                    Hien_Thi_Gia_Text_Box();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string GetNewMaSanPham(LionQuanLyQuanCaPheDataContext sp_Quan_Tri)
        {
            var existingMaHangs = sp_Quan_Tri.SanPhams.Select(sp => sp.MaSanPham).ToList();

            int newMaHang = 1;
            while (existingMaHangs.Contains("SP" + newMaHang.ToString("D3")))
            {
                newMaHang++;
            }

            return "SP" + newMaHang.ToString("D3");
        }
        private void LamMoi_SP()
        {
            txtTenSanPham.Clear();
            txtMaSanPham.Clear();
            txtGiaBan.Clear();
            txtGiaNhap.Clear();
            pic_AnhSanPham.Image = null;
        }

        /*---Sửa sản phẩm---*/
        private string imagePath = "";
        private void Sua_San_Pham()
        {
            decimal donGiaBan;
            decimal donGiaNhap;

            if (!decimal.TryParse(txtGiaBan.Text, out donGiaBan) || donGiaBan <= 0)
            {
                MessageBox.Show("Đơn giá bán phải là số dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out donGiaNhap) || donGiaNhap <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (donGiaNhap > 70000)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Quá 70", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (donGiaBan > 100000)
            {
                MessageBox.Show("Lỗi! Giá Bán Không Được Quá 100", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (donGiaNhap > donGiaBan)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Lớn Hơn Giá Bán", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (donGiaNhap == donGiaBan)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Bằng Giá Bán", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            donGiaBan *= 1000;
            donGiaNhap *= 1000;

            var sua_Sp = new LionQuanLyQuanCaPheDataContext();

            string idSanPham = dtgvSanPham.CurrentRow.Cells["MaSanPham"].Value.ToString();
            var SuaSanPham = sua_Sp.SanPhams.FirstOrDefault(s => s.MaSanPham == idSanPham);

            if (SuaSanPham != null)
            {
                string tenSanPham = txtTenSanPham.Text;
                if (!Regex.IsMatch(tenSanPham, @"^[\p{L}\s]+$"))
                {
                    MessageBox.Show("Tên sản phẩm chỉ được nhập chữ, dấu và khoảng trắng, không được nhập số", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SuaSanPham.TenSanPham = tenSanPham;
                SuaSanPham.GiaBan = donGiaBan;
                SuaSanPham.GiaNhap = donGiaNhap;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    try
                    {
                        using (Image image = Image.FromFile(imagePath))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                image.Save(ms, ImageFormat.Jpeg);
                                SuaSanPham.HinhAnh = ms.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

                try
                {
                    sua_Sp.SubmitChanges();
                    MessageBox.Show("Cập nhật thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hienThiSan_Pham();
                    hienThi_ThongKe_SanPham();
                    LamMoi_SP();
                    Hien_Thi_Gia_Text_Box();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm để cập nhật", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*---Xóa sản phẩm---*/
        private void Xoa_San_Pham()
        {
            if (dtgvSanPham.SelectedRows.Count > 0)
            {
                DialogResult dl = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm đã chọn không?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dtgvSanPham.SelectedRows)
                    {
                        string tenSP = row.Cells["TenSanPham"].Value.ToString();

                        using (var Sp = new LionQuanLyQuanCaPheDataContext())
                        {
                            var XoaSanPham = Sp.SanPhams.FirstOrDefault(sp => sp.TenSanPham == tenSP);

                            if (XoaSanPham != null)
                            {
                                if (XoaSanPham.HinhAnh != null)
                                {
                                }

                                Sp.SanPhams.DeleteOnSubmit(XoaSanPham);

                                try
                                {
                                    Sp.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Xóa Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hienThiSan_Pham();
                    hienThi_ThongKe_SanPham();
                    LamMoi_SP();
                    Hien_Thi_Gia_Text_Box();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*---Tìm kiếm sản phẩm---*/
        private void TimKiem_SanPham()
        {
            string tuKhoa_SP = txtTimKiemSanPham.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa_SP))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var list_Tim_Kiem_Sp = new LionQuanLyQuanCaPheDataContext();

            var List = from sp in list_Tim_Kiem_Sp.SanPhams
                       where sp.MaSanPham.ToLower().Contains(tuKhoa_SP) ||
                             sp.TenSanPham.ToLower().Contains(tuKhoa_SP) ||
                             sp.GiaBan.ToString().Contains(tuKhoa_SP)
                       select new
                       {
                           sp.MaSanPham,
                           sp.TenSanPham,
                           sp.GiaNhap,
                           sp.GiaBan,
                           sp.MaNhanVien,
                           sp.HinhAnh,
                       };
            if (dtgvSanPham.Columns.Contains("GiaBan"))
            {
                dtgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dtgvSanPham.Columns["GiaBan"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
            if (dtgvSanPham.Columns.Contains("GiaNhap"))
            {
                dtgvSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                dtgvSanPham.Columns["GiaNhap"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
            if (List.Any())
            {
                var resultList = List.ToList();
                MessageBox.Show("Tìm Kiếm Sản Phẩm Thành Công!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dtgvSanPham.DataSource = resultList;

                if (dtgvSanPham.Columns["HinhAnh"] != null)
                {
                    dtgvSanPham.Columns["HinhAnh"].Visible = false;
                }

                if (!dtgvSanPham.Columns.Contains("AnhSanPham"))
                {
                    DataGridViewImageColumn Column = new DataGridViewImageColumn();
                    Column.Name = "AnhSanPham";
                    Column.HeaderText = "Ảnh Sản Phẩm";
                    Column.Width = 100;
                    Column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    dtgvSanPham.Columns.Add(Column);
                }

                foreach (DataGridViewRow row in dtgvSanPham.Rows)
                {
                    var cellValue = row.Cells["HinhAnh"].Value;
                    if (cellValue != null && cellValue != DBNull.Value)
                    {
                        byte[] DataImg = ((System.Data.Linq.Binary)cellValue).ToArray();
                        using (var ms = new MemoryStream(DataImg))
                        {
                            var image = Image.FromStream(ms);
                            row.Cells["AnhSanPham"].Value = image;
                        }
                    }
                    else
                    {
                        row.Cells["AnhSanPham"].Value = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                hienThiSan_Pham();
                if (dtgvSanPham.Columns.Contains("AnhSanPham"))
                {
                    dtgvSanPham.Columns.Remove("AnhSanPham");
                }
            }
        }
        private void Hien_Thi_Gia_Text_Box()
        {
            //PHẦN HIỆN THỊ .000 LÊN TEXTBOX

            txtGiaBan.Text = ".000";
            txtGiaNhap.Text = ".000";
            txtGiaNhapNguyenLieu.Text = ".000";
            txtGiaBan.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);
            txtGiaNhap.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);
            txtGiaNhapNguyenLieu.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);
            txtTenSanPham.KeyPress += new KeyPressEventHandler(ChanSo_KeyPress);
            txtThanhPhan.KeyPress += new KeyPressEventHandler(ChanSo_KeyPress);
            txtNhaSanXuat.KeyPress += new KeyPressEventHandler(ChanSo_KeyPress);
            txtTenNguyenLieu.KeyPress += new KeyPressEventHandler(ChanSo_KeyPress);
            txtSoLuongNguyenLieu.KeyPress += new KeyPressEventHandler(ChanVanBan_KiTuDacBiet_KeyPress);
            txtTimKiemSanPham.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
            txtTimKiemNguyenLieu.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
            txtTimKiemThongKeSanPham.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
            txtTimKiemThongKeNguyenLieu.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);

        }
        private void ChanVanBan_KiTuDacBiet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //CHẶN KÝ TỰ ĐẶC BIỆT
        private void Chan_KiTuDacBiet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void ChanSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private string giaBanMacDinh = ".000";
        private string giaNhapMacDinh = ".000";
        public class TabState
        {
            public string GiaBan { get; set; }
            public string GiaNhap { get; set; }
        }

        private TabState currentState = new TabState();

        private void TxtGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            int dotIndex = textBox.Text.IndexOf('.');
            if (dotIndex >= 0)
            {
                if (textBox.SelectionStart > dotIndex)
                {
                    e.Handled = true;
                }
                if (e.KeyChar == (char)Keys.Back && textBox.SelectionStart == textBox.Text.Length)
                {
                    e.Handled = true;
                }
            }
        }
        private void TxtGia_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            if (e.KeyCode == Keys.Delete && textBox.SelectionStart >= textBox.Text.Length - 4)
            {
                e.SuppressKeyPress = true;
            }
        }
        private void TxtGia_SelectionChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            if (textBox.SelectedText == textBox.Text)
            {
                int dotIndex = textBox.Text.IndexOf('.');
                textBox.SelectionStart = dotIndex;
            }
        }

        private void TxtGia_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string textWithoutSuffix = textBox.Text.EndsWith(".000") && textBox.Text.Length >= 4
                ? textBox.Text.Substring(0, textBox.Text.Length - 4) : textBox.Text;

            textWithoutSuffix = textWithoutSuffix.Replace(".", "").Replace(",", "");

            if (decimal.TryParse(textWithoutSuffix, out decimal value))
            {
                NumberFormatInfo nfi = new NumberFormatInfo
                {
                    NumberGroupSeparator = ".",
                    NumberDecimalSeparator = ",",
                    NumberGroupSizes = new int[] { 3 }
                };

                int selectionStart = textBox.SelectionStart;
                textBox.Text = value.ToString("N0", nfi) + ".000";
                textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length - 4);
            }
            if (textBox.Text.Length <= 4)
            {
                textBox.Text = ".000";
                textBox.SelectionStart = 0;
            }
        }

        /*---Hiển thị sản phẩm---*/
        private void hienThiSan_Pham()
        {

            var list_SP = new LionQuanLyQuanCaPheDataContext();

            var List_SP = from Sp in list_SP.SanPhams
                          select new
                          {
                              Sp.MaSanPham,
                              Sp.TenSanPham,
                              Sp.GiaNhap,
                              Sp.GiaBan,
                              Sp.MaNhanVien,
                              Sp.HinhAnh,
                          };

            var resultList = List_SP.ToList();
            dtgvSanPham.DataSource = resultList;

            if (dtgvSanPham.Columns.Contains("GiaBan"))
            {
                dtgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dtgvSanPham.Columns["GiaBan"].DefaultCellStyle.FormatProvider = new CultureInfo("vi-VN")
                {
                    NumberFormat = { NumberGroupSeparator = "." }
                };
            }

            if (dtgvSanPham.Columns.Contains("GiaNhap"))
            {
                dtgvSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                dtgvSanPham.Columns["GiaNhap"].DefaultCellStyle.FormatProvider = new CultureInfo("vi-VN")
                {
                    NumberFormat = { NumberGroupSeparator = "." }
                };
            }

            if (!dtgvSanPham.Columns.Contains("AnhSanPham"))
            {
                DataGridViewImageColumn Column = new DataGridViewImageColumn();
                Column.Name = "AnhSanPham";
                Column.HeaderText = "Ảnh Sản Phẩm";
                Column.Width = 100;
                Column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dtgvSanPham.Columns.Add(Column);
            }

            foreach (DataGridViewRow row in dtgvSanPham.Rows)
            {
                var cellValue = row.Cells["HinhAnh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    byte[] DataImg = ((System.Data.Linq.Binary)cellValue).ToArray();
                    using (var ms = new MemoryStream(DataImg))
                    {
                        var image = Image.FromStream(ms);
                        row.Cells["AnhSanPham"].Value = image;
                    }
                }
                else
                {
                    row.Cells["AnhSanPham"].Value = null;
                }
            }

            if (dtgvSanPham.Columns["HinhAnh"] != null)
            {
                dtgvSanPham.Columns["HinhAnh"].Visible = false;
            }
        }
        private void Chon_Anh_San_Pham()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (.jpg,.jpeg, .png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                try
                {
                    pic_AnhSanPham.Image = Image.FromFile(imagePath);
                    MessageBox.Show("Ảnh đã được chọn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnAnhSanPham_Click(object sender, EventArgs e)
        {
            Chon_Anh_San_Pham();
        }

        /*---NGUYÊN LIỆU---*/
        /*---Thêm nguyên liệu---*/
        private void Them_Nguyen_Lieu()
        {
            if (string.IsNullOrEmpty(txtTenNguyenLieu.Text) ||
                string.IsNullOrEmpty(txtSoLuongNguyenLieu.Text) ||
                string.IsNullOrEmpty(txtNhaSanXuat.Text))
            {
                MessageBox.Show("Vui Lòng Điền Đầy Đủ Dữ Liệu!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Regex regexName = new Regex(@"^[\p{L}\s]+$");
            if (!regexName.IsMatch(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Lỗi! Vui Lòng Chỉ Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Regex regexManufacturer = new Regex(@"^[\p{L}\s]+$");
            if (!regexManufacturer.IsMatch(txtNhaSanXuat.Text))
            {
                MessageBox.Show("Lỗi! Nhà Sản Xuất Chỉ Được Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string giaNhapText = txtGiaNhapNguyenLieu.Text.EndsWith(".000") && txtGiaNhapNguyenLieu.Text.Length >= 4
                ? txtGiaNhapNguyenLieu.Text.Substring(0, txtGiaNhapNguyenLieu.Text.Length - 4) : txtGiaNhapNguyenLieu.Text;

            if (!decimal.TryParse(giaNhapText.Replace(".", "").Replace(",", ""), out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Lỗi! Giá Nhập Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            giaNhap *= 1000;

            if (!int.TryParse(txtSoLuongNguyenLieu.Text, out int soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Lỗi! Số Lượng Nhập Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime ngayNhap = dttpNgayNhapNguyenLieu.Value;
            DateTime ngayHetHan = dttpNgayHethanNguyenLieu.Value;

            if (ngayHetHan < ngayNhap || ngayHetHan == ngayNhap)
            {
                MessageBox.Show("Ngày Hết Hạn Phải Sau, Không Cùng Ngày Nhập!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maNhanVien = FormDangNhap.MaNhanVienHienTai;
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Lỗi! Mã nhân viên không hợp lệ. Vui lòng đăng nhập lại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                FormDangNhap loginForm = new FormDangNhap();
                loginForm.ShowDialog();
                this.Show();
                return;
            }

            using (var nl = new LionQuanLyQuanCaPheDataContext())
            {
                string auto_Ma_NL = GetNewMaNguyenLieu(nl);
                NguyenLieu ThemNguyenLieu = new NguyenLieu
                {
                    MaNguyenLieu = auto_Ma_NL,
                    ThanhPhan = txtThanhPhan.Text,
                    MaNhanVien = maNhanVien,
                    TenNguyenLieu = txtTenNguyenLieu.Text,
                    GiaNhap = giaNhap,
                    SoLuongNhap = soLuongNhap,
                    NhaSanXuat = txtNhaSanXuat.Text,
                    NgayNhap = ngayNhap,
                    NgayHetHan = ngayHetHan
                };

                nl.NguyenLieus.InsertOnSubmit(ThemNguyenLieu);

                try
                {
                    nl.SubmitChanges();
                    MessageBox.Show("Thêm Nguyên Liệu Thành Công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Nguyen_Lieu();
                    LamMoi_NguyenLieu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetNewMaNguyenLieu(LionQuanLyQuanCaPheDataContext Nguyen_lieu)
        {
            var existingMaHangs = Nguyen_lieu.NguyenLieus.Select(nl => nl.MaNguyenLieu).ToList();

            int newMaNguyenLieu = 1;
            while (existingMaHangs.Contains("NL" + newMaNguyenLieu.ToString("D3")))
            {
                newMaNguyenLieu++;
            }

            return "NL" + newMaNguyenLieu.ToString("D3");
        }
        private void LamMoi_NguyenLieu()
        {
            txtMaNguyenLieu.Clear();
            txtGiaNhapNguyenLieu.Clear();
            txtTenNguyenLieu.Clear();
            txtThanhPhan.Clear();
            txtNhaSanXuat.Clear();
            txtSoLuongNguyenLieu.Clear();

            dttpNgayNhapNguyenLieu.Value = DateTime.Now;
            dttpNgayHethanNguyenLieu.Value = DateTime.Now;
        }
        private void Hien_Thi_Nguyen_Lieu()
        {
            var list_NL = new LionQuanLyQuanCaPheDataContext();

            var List_NL = from Nl in list_NL.NguyenLieus
                          select new
                          {
                              Nl.MaNguyenLieu,
                              Nl.TenNguyenLieu,
                              Nl.ThanhPhan,
                              Nl.SoLuongNhap,
                              Nl.GiaNhap,
                              Nl.NgayNhap,
                              Nl.NgayHetHan,
                              Nl.NhaSanXuat,
                          };

            var resultList = List_NL.ToList();
            dtgvThongTinNguyenLieu.DataSource = resultList;
            if (dtgvThongTinNguyenLieu.Columns.Contains("GiaNhap"))
            {
                dtgvThongTinNguyenLieu.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                dtgvThongTinNguyenLieu.Columns["GiaNhap"].DefaultCellStyle.FormatProvider = new CultureInfo("vi-VN")
                {
                    NumberFormat = { NumberGroupSeparator = "." }
                };
            }
        }

        /*---Sửa nguyên liệu---*/
        private void Sua_Nguyen_Lieu()
        {
            decimal donGiaNhap;

            if (!decimal.TryParse(txtGiaNhapNguyenLieu.Text, out donGiaNhap) || donGiaNhap <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            donGiaNhap *= 1000;

            if (!int.TryParse(txtSoLuongNguyenLieu.Text, out int soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập phải là số nguyên dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtgvThongTinNguyenLieu.CurrentRow == null)
            {
                MessageBox.Show("Không có dòng nào được chọn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string idNguyenLieu = dtgvThongTinNguyenLieu.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            var sua_Nl = new LionQuanLyQuanCaPheDataContext();
            var SuaNguyenLieu = sua_Nl.NguyenLieus.FirstOrDefault(s => s.MaNguyenLieu == idNguyenLieu);

            if (SuaNguyenLieu != null)
            {
                if (dttpNgayHethanNguyenLieu.Value <= dttpNgayNhapNguyenLieu.Value)
                {
                    MessageBox.Show("Ngày hết hạn không được trước ngày nhập hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SuaNguyenLieu.ThanhPhan = txtThanhPhan.Text;
                SuaNguyenLieu.GiaNhap = donGiaNhap;
                SuaNguyenLieu.NhaSanXuat = txtNhaSanXuat.Text;
                SuaNguyenLieu.TenNguyenLieu = txtTenNguyenLieu.Text;
                SuaNguyenLieu.SoLuongNhap = soLuongNhap;
                SuaNguyenLieu.NgayNhap = dttpNgayNhapNguyenLieu.Value;
                SuaNguyenLieu.NgayHetHan = dttpNgayHethanNguyenLieu.Value;

                try
                {
                    sua_Nl.SubmitChanges();
                    MessageBox.Show("Cập nhật thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Nguyen_Lieu();
                    LamMoi_NguyenLieu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy nguyên liệu để cập nhật", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*---Xóa nguyên liệu---*/
        private void Xoa_Nguyen_Lieu()
        {
            if (dtgvThongTinNguyenLieu.SelectedRows.Count > 0)
            {
                DialogResult dl = MessageBox.Show("Bạn có chắc chắn muốn xóa Nguyên Liệu đã chọn không?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dtgvThongTinNguyenLieu.SelectedRows)
                    {
                        string tenNL = row.Cells["TenNguyenLieu"].Value.ToString();

                        using (var Nl = new LionQuanLyQuanCaPheDataContext())
                        {
                            var XoaNguyenLieu = Nl.NguyenLieus.FirstOrDefault(nl => nl.TenNguyenLieu == tenNL);

                            if (XoaNguyenLieu != null)
                            {
                                Nl.NguyenLieus.DeleteOnSubmit(XoaNguyenLieu);

                                try
                                {
                                    Nl.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Xóa Nguyên Liệu Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Nguyen_Lieu();
                    LamMoi_NguyenLieu();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /*---Tìm kiếm nguyên liệu---*/
        private void TimKiem_NguyenLieu()
        {
            string NguyennLieu = txtTimKiemNguyenLieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtTimKiemNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_NL = txtTimKiemNguyenLieu.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(tuKhoa_NL))
            {
                var list_nguyenlieu = new LionQuanLyQuanCaPheDataContext();

                var List = from nl in list_nguyenlieu.NguyenLieus
                           where nl.MaNguyenLieu.ToLower().Contains(tuKhoa_NL) ||
                                 nl.TenNguyenLieu.ToLower().Contains(tuKhoa_NL)
                           select new
                           {
                               nl.MaNguyenLieu,
                               nl.TenNguyenLieu,
                               nl.ThanhPhan,
                               nl.SoLuongNhap,
                               nl.GiaNhap,
                               nl.NgayNhap,
                               nl.NgayHetHan,
                               nl.NhaSanXuat,
                           };

                if (List.Any())
                {
                    MessageBox.Show("Tìm kiếm nguyên liệu thành công!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgvThongTinNguyenLieu.DataSource = List.ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nguyên liệu phù hợp", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Nguyen_Lieu();
                }
            }
            else
            {
                Hien_Thi_Nguyen_Lieu();
            }
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

        /*---THỐNG KÊ*/
        /*---Thống kê sản phẩm---*/
        private void hienThi_ThongKe_SanPham()
        {
            var list_TK_SP = new LionQuanLyQuanCaPheDataContext();
            var resultList_NK = list_TK_SP.ThongKeSanPham().ToList();

            dtgvThongKeSanPham.DataSource = resultList_NK;

            string[] hiddenColumns = new string[] { "SoLuongBanRaTuan", "SoLuongBanRaThang", "SoLuongBanRaNam", "TongGiaBanRaTuan", "TongGiaBanRaThang", "TongGiaBanRaNam" };
            foreach (string columnName in hiddenColumns)
            {
                if (dtgvThongKeSanPham.Columns.Contains(columnName))
                {
                    dtgvThongKeSanPham.Columns[columnName].Visible = false;
                }
            }

            if (dtgvThongKeSanPham.Columns.Contains("GiaBan"))
            {
                dtgvThongKeSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["GiaBan"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
            if (dtgvThongKeSanPham.Columns.Contains("GiaNhap"))
            {
                dtgvThongKeSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["GiaNhap"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
        }

        private void TimKiem_ThongKe_SanPham()
        {
            string SanPham = txtTimKiemThongKeSanPham.Text.Trim();
            if (string.IsNullOrWhiteSpace(SanPham))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_SP = SanPham.ToLower();

            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var List_SP = QLBH.ThongKeSanPham().ToList();

            var filteredList = from s in List_SP
                               where s.MaSanPham.ToLower().Contains(tuKhoa_SP) ||
                                     s.TenSanPham.ToLower().Contains(tuKhoa_SP)
                               select new
                               {
                                   s.MaSanPham,
                                   s.TenSanPham,
                                   s.GiaBan,
                                   s.SoLuongBanRaTuan,
                                   s.SoLuongBanRaThang,
                                   s.SoLuongBanRaNam,
                                   s.TongGiaBanRaTuan,
                                   s.TongGiaBanRaThang,
                                   s.TongGiaBanRaNam
                               };

            if (filteredList.Any())
            {
                var resultList = filteredList.ToList();
                dtgvThongKeSanPham.DataSource = resultList;

                DinhDangDataGridView();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hienThi_ThongKe_SanPham();
            }
        }

        private void TimKiemThongKe_SanPham()
        {
            string SanPham = txtTimKiemThongKeSanPham.Text.Trim();
            if (string.IsNullOrWhiteSpace(SanPham))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_SP = SanPham.ToLower();

            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var List_SP = QLBH.ThongKeSanPham().ToList();

            var filteredList = from s in List_SP
                               where s.MaSanPham.ToLower().Contains(tuKhoa_SP) ||
                                     s.TenSanPham.ToLower().Contains(tuKhoa_SP)

                               select new
                               {
                                   s.MaSanPham,
                                   s.TenSanPham,
                                   s.GiaBan,
                                   s.SoLuongBanRaTuan,
                                   s.SoLuongBanRaThang,
                                   s.SoLuongBanRaNam,
                                   s.TongGiaBanRaTuan,
                                   s.TongGiaBanRaThang,
                                   s.TongGiaBanRaNam
                               };

            if (filteredList.Any())
            {
                var resultList = filteredList.ToList();
                dtgvThongKeSanPham.DataSource = resultList;

                DinhDangDataGridView();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hienThi_ThongKe_SanPham();
            }
        }
        private void DinhDangDataGridView()
        {
            if (dtgvThongKeSanPham.Columns.Contains("GiaBan"))
            {
                dtgvThongKeSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["GiaBan"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
            if (dtgvThongKeSanPham.Columns.Contains("SoLuongBanRaTuanPham"))
            {
                dtgvThongKeSanPham.Columns["SoLuongBanRaTuanPham"].DefaultCellStyle.Format = "N0";
            }
        }

        private void So_Luong_Ban_Ra()
        {
            //COMBOBOX PHẦN SỐ LƯỢNG BÁN RA

            cbb_Chon_Soluong_Ban_Ra.Items.AddRange(new object[] {
             "", "Số Lượng Bán Ra Tuần", "Số Lượng Bán Ra Tháng", "Số Lượng Bán Ra Năm",
             "Tổng Giá Bán Ra Tuần", "Tổng Giá Bán Ra Tháng", "Tổng Giá Bán Ra Năm"
            });

            cbb_Chon_Soluong_Ban_Ra.SelectedIndex = 0;
            cbb_Chon_Soluong_Ban_Ra.SelectedIndexChanged += cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged;

            cbb_Chon_Soluong_Ban_Ra.SelectedIndexChanged -= cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged;
            cbb_Chon_Soluong_Ban_Ra.SelectedIndexChanged += cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged;
            cbb_Chon_Soluong_Ban_Ra.Items.Clear();

            cbb_Chon_Soluong_Ban_Ra.Items.Add("");

            cbb_Chon_Soluong_Ban_Ra.Items.Add("Số Lượng Bán Ra Tuần");
            cbb_Chon_Soluong_Ban_Ra.Items.Add("Số Lượng Bán Ra Tháng");
            cbb_Chon_Soluong_Ban_Ra.Items.Add("Số Lượng Bán Ra Năm");
            cbb_Chon_Soluong_Ban_Ra.Items.Add("Tổng Giá Bán Ra Tuần");
            cbb_Chon_Soluong_Ban_Ra.Items.Add("Tổng Giá Bán Ra Tháng");
            cbb_Chon_Soluong_Ban_Ra.Items.Add("Tổng Giá Bán Ra Năm");
        }

        private void cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_Chon_Soluong_Ban_Ra.SelectedItem == null) return;

            string selectedValue = cbb_Chon_Soluong_Ban_Ra.SelectedItem.ToString();

            Dictionary<string, string> columnMapping = new Dictionary<string, string>
            {
            { "Số Lượng Bán Ra Tuần", "SoLuongBanRaTuan" },
            { "Số Lượng Bán Ra Tháng", "SoLuongBanRaThang" },
            { "Số Lượng Bán Ra Năm", "SoLuongBanRaNam" },
            { "Tổng Giá Bán Ra Tuần", "TongGiaBanRaTuan" },
            { "Tổng Giá Bán Ra Tháng", "TongGiaBanRaThang" },
            { "Tổng Giá Bán Ra Năm", "TongGiaBanRaNam" }
            };

            // Ẩn tất cả các cột
            foreach (var columnName in columnMapping.Values)
            {
                if (dtgvThongKeSanPham.Columns.Contains(columnName))
                {
                    dtgvThongKeSanPham.Columns[columnName].Visible = false;
                }
            }

            // Hiện cột tương ứng với lựa chọn
            if (columnMapping.ContainsKey(selectedValue) && dtgvThongKeSanPham.Columns.Contains(columnMapping[selectedValue]))
            {
                dtgvThongKeSanPham.Columns[columnMapping[selectedValue]].Visible = true;
            }
        }


        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Sản phẩm---*/
        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            Them_San_Pham();
        }

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {
            Sua_San_Pham();
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            Xoa_San_Pham();
        }

        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
        {
            TimKiem_SanPham();
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
            TimKiemKhachHang();
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
            Them_Nguyen_Lieu();
        }

        private void btnSuaNguyenLieu_Click(object sender, EventArgs e)
        {
            Sua_Nguyen_Lieu();
        }

        private void btnXoaNguyenLieu_Click(object sender, EventArgs e)
        {
            Xoa_Nguyen_Lieu();
        }

        private void btnTimKiemNguyenLieu_Click(object sender, EventArgs e)
        {
            TimKiem_NguyenLieu();
        }


        /*---Chức năng Thêm, sửa, xóa, tìm kiếm Vai trò---*/
        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {
            ThemVaiTro();
        }

        private void btnsuaVaiTro_Click(object sender, EventArgs e)
        {
            SuaVaiTro();
        }

        private void btnxoaVaiTro_Click(object sender, EventArgs e)
        {
            XoaVaiTro();
        }

        private void btntimkiemVaiTro_Click(object sender, EventArgs e)
        {
            TimKiemVaiTro();
        }

        /*---Đẩy data lên textbox---*/

        private void dtgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dtgvSanPham.Columns["AnhSanPham"].Index && e.RowIndex >= 0)
            {
                if (dtgvSanPham.Columns.Contains("HinhAnh"))
                {
                    var cellValue_HD = dtgvSanPham.Rows[e.RowIndex].Cells["HinhAnh"].Value;
                    if (cellValue_HD != null && cellValue_HD != DBNull.Value)
                    {
                        try
                        {
                            byte[] imageData = ((System.Data.Linq.Binary)cellValue_HD).ToArray();
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                Image image = Image.FromStream(ms);
                                e.Value = image;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể hiển thị hình ảnh: " + ex.Message);
                        }
                    }
                    else
                    {
                        e.Value = null;
                    }
                }
                else
                {
                    e.Value = null;
                }
            }
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

        private void dtgvThongTinNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {

                    DataGridViewRow selectedRow = dtgvThongTinNhanVien.Rows[e.RowIndex];


                    DisplayNhanVienDetails(selectedRow);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Đã xảy ra lỗi khi hiển thị thông tin: " + ex.Message);
                }
            }
        }

        private void dtgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvSanPham.Rows[e.RowIndex];

                txtMaSanPham.Text = row.Cells["MaSanPham"].Value.ToString();
                txtTenSanPham.Text = row.Cells["TenSanPham"].Value.ToString();

                decimal giaBan, giaNhap;

                NumberFormatInfo nfi = new NumberFormatInfo
                {
                    NumberGroupSeparator = ".",
                    NumberDecimalSeparator = ",",
                    NumberGroupSizes = new int[] { 3 }
                };

                if (decimal.TryParse(row.Cells["GiaBan"].Value.ToString(), out giaBan))
                {
                    txtGiaBan.Text = giaBan.ToString("N0", nfi);
                }
                else
                {
                    txtGiaBan.Text = "0";
                }

                if (decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), out giaNhap))
                {
                    txtGiaNhap.Text = giaNhap.ToString("N0", nfi);
                }
                else
                {
                    txtGiaNhap.Text = "0";
                }

                var cellValue = row.Cells["HinhAnh"].Value;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    byte[] DataImg = ((System.Data.Linq.Binary)cellValue).ToArray();
                    using (var ms = new MemoryStream(DataImg))
                    {
                        var image = Image.FromStream(ms);
                        pic_AnhSanPham.Image = image;
                    }
                }
                else
                {
                    pic_AnhSanPham.Image = null;
                }
            }
        }

        private void dtgvThongTinKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dtgvThongTinNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvThongTinNguyenLieu.Rows.Count)
            {
                DataGridViewRow row = dtgvThongTinNguyenLieu.Rows[e.RowIndex];

                if (row.Cells["MaNguyenLieu"] != null && row.Cells["TenNguyenLieu"] != null
                    && row.Cells["GiaNhap"] != null && row.Cells["NhaSanXuat"] != null
                    && row.Cells["ThanhPhan"] != null && row.Cells["SoLuongNhap"] != null
                    && row.Cells["NgayNhap"] != null && row.Cells["NgayHetHan"] != null)
                {
                    txtMaNguyenLieu.Text = row.Cells["MaNguyenLieu"].Value != DBNull.Value ? row.Cells["MaNguyenLieu"].Value.ToString() : string.Empty;
                    txtTenNguyenLieu.Text = row.Cells["TenNguyenLieu"].Value != DBNull.Value ? row.Cells["TenNguyenLieu"].Value.ToString() : string.Empty;

                    decimal giaNhapNL;
                    NumberFormatInfo nfi = new NumberFormatInfo
                    {
                        NumberGroupSeparator = ".",
                        NumberDecimalSeparator = ",",
                        NumberGroupSizes = new int[] { 3 }
                    };

                    if (decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), out giaNhapNL))
                    {
                        txtGiaNhapNguyenLieu.Text = giaNhapNL.ToString("N0", nfi);
                    }
                    else
                    {
                        txtGiaNhapNguyenLieu.Text = "0";
                    }

                    txtNhaSanXuat.Text = row.Cells["NhaSanXuat"].Value != DBNull.Value ? row.Cells["NhaSanXuat"].Value.ToString() : string.Empty;
                    txtThanhPhan.Text = row.Cells["ThanhPhan"].Value != DBNull.Value ? row.Cells["ThanhPhan"].Value.ToString() : string.Empty;

                    string soLuongText = row.Cells["SoLuongNhap"].Value != DBNull.Value ? row.Cells["SoLuongNhap"].Value.ToString() : string.Empty;
                    if (int.TryParse(soLuongText, out int soLuong))
                    {
                        txtSoLuongNguyenLieu.Text = soLuong.ToString();
                    }
                    else
                    {
                        txtSoLuongNguyenLieu.Text = string.Empty;
                    }

                    DateTime ngayNhap, ngayHetHan;
                    string ngayNhapText = row.Cells["NgayNhap"].Value != DBNull.Value ? row.Cells["NgayNhap"].Value.ToString() : string.Empty;
                    string ngayHetHanText = row.Cells["NgayHetHan"].Value != DBNull.Value ? row.Cells["NgayHetHan"].Value.ToString() : string.Empty;

                    if (DateTime.TryParse(ngayNhapText, out ngayNhap))
                    {
                        dttpNgayNhapNguyenLieu.Value = ngayNhap;
                    }
                    else
                    {
                        dttpNgayNhapNguyenLieu.Value = DateTime.Now;
                    }

                    if (DateTime.TryParse(ngayHetHanText, out ngayHetHan))
                    {
                        dttpNgayHethanNguyenLieu.Value = ngayHetHan;
                    }
                    else
                    {
                        dttpNgayHethanNguyenLieu.Value = DateTime.Now;
                    }
                }
                else
                {
                    MessageBox.Show("Dữ liệu không đầy đủ hoặc cột không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtgvVaiTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DataGridViewRow row = dtgvVaiTro.Rows[e.RowIndex];
            string maVaiTro = row.Cells["MaVaiTro"].Value.ToString();
            string tenVaiTro = row.Cells["TenVaiTro"].Value.ToString();

            txtMaVaiTro.Text = maVaiTro;
            cbbvaitro.Text = tenVaiTro;
        }

        /*---THỐNG KÊ---*/

        /*---Thống kê Nguyên liệu---*/
        private void btnTimKiemThongKeNguyenLieu_Click(object sender, EventArgs e)
        {
            Tim_Kiem_Thong_Ke_Nguyen_Lieu();
        }
        private void Hien_Thi_Thong_Ke_Nguyen_Lieu()
        {
            try
            {
                using (var context = new LionQuanLyQuanCaPheDataContext())
                {
                    var thongKeList = from nl in context.NguyenLieus
                                      select new
                                      {
                                          nl.MaNguyenLieu,
                                          nl.TenNguyenLieu,
                                          nl.NgayHetHan,
                                          nl.SoLuongNhap,
                                          nl.GiaNhap
                                      };

                    var resultList_NK = thongKeList.ToList();

                    var nfi = new System.Globalization.NumberFormatInfo
                    {
                        NumberGroupSeparator = ".",
                        NumberDecimalSeparator = ","
                    };

                    var formattedList_NK = resultList_NK.Select(nl => new
                    {
                        nl.MaNguyenLieu,
                        nl.TenNguyenLieu,
                        nl.NgayHetHan,
                        nl.SoLuongNhap,
                        GiaNhap = ((decimal)nl.GiaNhap).ToString("N0", nfi)
                    }).ToList();

                    dtgvThongKeNguyenLieu.DataSource = formattedList_NK;

                    dtgvThongKeNguyenLieu.Columns["GiaNhap"].DefaultCellStyle.FormatProvider = nfi;
                    dtgvThongKeNguyenLieu.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Tim_Kiem_Thong_Ke_Nguyen_Lieu()
        {

            string NguyennLieu = txtTimKiemThongKeNguyenLieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtTimKiemThongKeNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_NL = txtTimKiemThongKeNguyenLieu.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(tuKhoa_NL))
            {
                var list_nguyenlieu = new LionQuanLyQuanCaPheDataContext();

                var List = from nl in list_nguyenlieu.NguyenLieus
                           where nl.MaNguyenLieu.ToLower().Contains(tuKhoa_NL) ||
                         nl.ThanhPhan.ToString().Contains(tuKhoa_NL) ||
                                 nl.NhaSanXuat.ToString().Contains(tuKhoa_NL) ||
                                 nl.TenNguyenLieu.ToString().Contains(tuKhoa_NL) ||
                                 nl.SoLuongNhap.ToString().Contains(tuKhoa_NL)
                           select new
                           {
                               nl.MaNguyenLieu,
                               nl.ThanhPhan,
                               nl.NhaSanXuat,
                               nl.TenNguyenLieu,
                               nl.SoLuongNhap,
                               nl.NgayNhap,
                               nl.NgayHetHan,

                           };

                if (List.Any())
                {
                    dtgvThongKeNguyenLieu.DataSource = List.ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Nguyên Liệu phù hợp", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Thong_Ke_Nguyen_Lieu();
                }
            }
            else
            {
                Hien_Thi_Nguyen_Lieu();
            }

        }

        private void txtTimKiemThongKeNguyenLieu_Click(object sender, EventArgs e)
        {
            Hien_Thi_Thong_Ke_Nguyen_Lieu();
        }

        /*---Thống kê Sản phẩm---*/
        private void btnTimKiemThongKeSanPham_Click(object sender, EventArgs e)
        {
            TimKiem_ThongKe_SanPham();
        }


        private void txtTimKiemThongKeSanPham_Click(object sender, EventArgs e)
        {
            hienThi_ThongKe_SanPham();
        }
        private void dtgvThongKeSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvThongKeSanPham == null) return;

            if (e.ColumnIndex == dtgvThongKeSanPham.Columns["AnhSanPham"]?.Index && e.RowIndex >= 0)
            {
                if (dtgvThongKeSanPham.Columns.Contains("HinhAnh"))
                {
                    var cellValue_HD = dtgvThongKeSanPham.Rows[e.RowIndex]?.Cells["HinhAnh"]?.Value;
                    if (cellValue_HD != null && cellValue_HD != DBNull.Value)
                    {
                        try
                        {
                            byte[] imageData = ((System.Data.Linq.Binary)cellValue_HD).ToArray();
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                Image image = Image.FromStream(ms);
                                e.Value = image;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể hiển thị hình ảnh: " + ex.Message);
                        }
                    }
                    else
                    {
                        e.Value = null;
                    }
                }
                else
                {
                    e.Value = null;
                }
            }
        }

        /*---Thống kê Khách hàng---*/
        private void HienThiThongKeKhachHang()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var thongKeKhach = QLKH.ThongKeKhachHang().ToList();
            dtgvThongKeKhachHang.DataSource = thongKeKhach;
        }

        private void btnTimKiemThongKeKhachHang_Click(object sender, EventArgs e)
        {
            TimKiemThongKeKhachHang();
            txtTimKiemThongKeKhachHang.Clear();
        }

        private void txtTimKiemThongKeKhachHang_Click(object sender, EventArgs e)
        {
            HienThiThongKeKhachHang();
        }

        private void TimKiemThongKeKhachHang()
        {
            string KhachHang = txtTimKiemThongKeKhachHang.Text.Trim();
            if (string.IsNullOrWhiteSpace(KhachHang))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_KH = KhachHang.ToLower();

            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var List_KH = QLBH.ThongKeKhachHang().ToList();

            var filteredList = from kh in List_KH
                               where kh.MaKhachHang.ToLower().Contains(tuKhoa_KH) ||
                                     kh.TenKhachHang.ToLower().Contains(tuKhoa_KH) ||
                                     kh.SDT.ToString().Contains(tuKhoa_KH) ||
                                     kh.SoLuongHoaDonThang.ToString().Contains(tuKhoa_KH) ||
                                     kh.SoLuongHoaDonNam.ToString().Contains(tuKhoa_KH) ||
                                     kh.TongGiaTriThang.ToString().Contains(tuKhoa_KH) ||
                                     kh.TongGiaTriNam.ToString().Contains(tuKhoa_KH)
                               select new
                               {
                                   kh.MaKhachHang,
                                   kh.TenKhachHang,
                                   kh.SDT,
                                   kh.SoLuongHoaDonThang,
                                   kh.SoLuongHoaDonNam,
                                   kh.TongGiaTriThang,
                                   kh.TongGiaTriNam
                               };

            if (filteredList.Any())
            {
                var resultList = filteredList.ToList();
                dtgvThongKeKhachHang.DataSource = resultList;


            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvThongKeKhachHang.DataSource = null;
            }
        }
        private void cbb_Chon_Soluong_Ban_Ra_KhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cbb_Chon_Soluong_Ban_Ra_KhachHang.SelectedItem.ToString();

            Dictionary<string, string> columnMapping = new Dictionary<string, string>
            {
             { "Số Lượng Hóa Đơn Tháng", "SoLuongHoaDonThang" },
             { "Số Lượng Hóa Đơn Năm", "SoLuongHoaDonNam" },
             {"Tổng Giá Trị Tháng", "TongGiaTriThang" },
             { "Tổng Giá Trị Năm", "TongGiaTriNam" }
            };

            foreach (var columnName in columnMapping.Values)
            {
                if (dtgvThongKeKhachHang.Columns.Contains(columnName))
                {
                    dtgvThongKeKhachHang.Columns[columnName].Visible = false;
                }
            }

            if (columnMapping.ContainsKey(selectedValue) && dtgvThongKeKhachHang.Columns.Contains(columnMapping[selectedValue]))
            {
                dtgvThongKeKhachHang.Columns[columnMapping[selectedValue]].Visible = true;
            }
        }

        /*---Thống kê Nhân viên---*/
        private void HienThiThongKeNhanVien()
        {
            var QLNV = new LionQuanLyQuanCaPheDataContext();
            var thongKeNhanVien = QLNV.ThongKeNhanVien().ToList();
            dtgvThongKeNhanVien.DataSource = thongKeNhanVien;
        }
        private void TimKiemThongKeNhanVien()
        {
            string nhanVien = txtTimKiemThongKeNhanVien.Text.Trim();
            if (string.IsNullOrWhiteSpace(nhanVien))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_NV = nhanVien.ToLower();

            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var List_NV = QLBH.ThongKeNhanVien().ToList();

            var filteredList = from nv in List_NV
                               where nv.MaNhanVien.ToLower().Contains(tuKhoa_NV) ||
                               nv.TenNhanVien.ToLower().Contains(tuKhoa_NV) ||
                                     nv.SoLuongHoaDonTuan.ToString().Contains(tuKhoa_NV) ||
                                     nv.SoLuongHoaDonThang.ToString().Contains(tuKhoa_NV) ||
                                     nv.SoLuongHoaDonNam.ToString().Contains(tuKhoa_NV)
                               select new
                               {
                                   nv.MaNhanVien,
                                   nv.TenNhanVien,
                                   nv.SoLuongHoaDonTuan,
                                   nv.SoLuongHoaDonThang,
                                   nv.SoLuongHoaDonNam

                               };

            if (filteredList.Any())
            {
                var resultList = filteredList.ToList();
                dtgvThongKeNhanVien.DataSource = resultList;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvThongKeNhanVien.DataSource = null;
            }
        }

        private void cbb_Chon_Soluong_Ban_Ra_NhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cbb_Chon_Soluong_Ban_Ra_NhanVien.SelectedItem.ToString();

            Dictionary<string, string> columnMapping = new Dictionary<string, string>
     {
         { "Số Lượng Hóa Đơn Tuần", "SoLuongHoaDonTuan" },
         { "Số Lượng Hóa Đơn Tháng", "SoLuongHoaDonThang" },
         { "Số Lượng Hóa Đơn Năm", "SoLuongHoaDonNam" },

     };

            foreach (var columnName in columnMapping.Values)
            {
                if (dtgvThongKeNhanVien.Columns.Contains(columnName))
                {
                    dtgvThongKeNhanVien.Columns[columnName].Visible = false;
                }
            }

            if (columnMapping.ContainsKey(selectedValue) && dtgvThongKeNhanVien.Columns.Contains(columnMapping[selectedValue]))
            {
                dtgvThongKeNhanVien.Columns[columnMapping[selectedValue]].Visible = true;
            }
        }

        private void btnTimKiemThongKeNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemThongKeNhanVien();
            txtTimKiemThongKeNhanVien.Clear();
        }

        private void txtTimKiemThongKeNhanVien_Click(object sender, EventArgs e)
        {
            HienThiThongKeNhanVien();
        }

        /*---Thống kê Hóa đơn---*/
        private void HienThiThongKeHoaDon()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var thongKeHoaDon = QLKH.ThongKeHoaDon().ToList();
            dtgvThongKeHoaDon.DataSource = thongKeHoaDon;
        }
        private void TimKiemThongKeHoaDon()
        {
            string hoaDon = txtTimKiemThongKeHoaDon.Text.Trim();
            if (string.IsNullOrWhiteSpace(hoaDon))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_HD = hoaDon.ToLower();

            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var List_HD = QLBH.ThongKeHoaDon().ToList();

            var filteredList = from hd in List_HD
                               where hd.MaHoaDon.ToLower().Contains(tuKhoa_HD) ||
                                     hd.NgayXuatHoaDon.ToString().Contains(tuKhoa_HD) ||
                                     hd.SoLuongSanPham.ToString().Contains(tuKhoa_HD) ||
                                     hd.SoLuongMon.ToString().Contains(tuKhoa_HD) ||
                                     hd.TongHoaDon.ToString().Contains(tuKhoa_HD)
                               select new
                               {
                                   hd.MaHoaDon,
                                   hd.NgayXuatHoaDon,
                                   hd.SoLuongSanPham,
                                   hd.SoLuongMon,
                                   hd.TongHoaDon
                               };

            if (filteredList.Any())
            {
                var resultList = filteredList.ToList();
                dtgvThongKeHoaDon.DataSource = resultList;
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvThongKeHoaDon.DataSource = null;
            }
        }

        private void btnTimKiemThongKeHoaDon_Click(object sender, EventArgs e)
        {
            TimKiemThongKeHoaDon();
            txtTimKiemThongKeHoaDon.Clear();
        }



        private void txtTimKiemThongKeHoaDon_Click(object sender, EventArgs e)
        {
            HienThiThongKeHoaDon();
        }



        /*---Code Orrder---*/
        // ORDER
        private List<OrderItem> orderList = new List<OrderItem>();

        public class OrderItem
        {
            public string ProductID { get; set; } // Sử dụng string thay vì int
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public DateTime DateTime { get; set; }
        }

        private void hienThiOrder(string keyword = "")
        {
            var QLOD = new LionQuanLyQuanCaPheDataContext();
            var sanPham = from sp in QLOD.SanPhams
                          where sp.TenSanPham.Contains(keyword)
                          select sp;

            flowLayoutPanelMenu.Controls.Clear();

            foreach (var spo in sanPham)
            {
                Panel panelSanPham = new Panel
                {
                    Width = 100,
                    Height = 180,
                    Margin = new Padding(5)
                };

                Button btnSanPham = new Button
                {
                    Tag = spo,
                    Width = 100,
                    Height = 140,
                    BackgroundImageLayout = ImageLayout.Stretch
                };

                if (spo.HinhAnh != null)
                {
                    try
                    {
                        using (var ms = new MemoryStream(spo.HinhAnh.ToArray()))
                        {
                            btnSanPham.BackgroundImage = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image for product {spo.TenSanPham}: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"Product {spo.TenSanPham} does not have an image.");
                }

                btnSanPham.Click += (s, e) => AddProductToOrder((SanPham)btnSanPham.Tag);

                Label lblSanPham = new Label
                {
                    Text = spo.TenSanPham,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom,
                    AutoSize = false,
                    Height = 40
                };

                panelSanPham.Controls.Add(btnSanPham);
                panelSanPham.Controls.Add(lblSanPham);

                flowLayoutPanelMenu.Controls.Add(panelSanPham);
            }
        }

        private void AddProductToOrder(SanPham sanPham)
        {
            var orderItem = orderList.FirstOrDefault(o => o.ProductID == sanPham.MaSanPham);
            if (orderItem != null)
            {
                orderItem.Quantity++;
            }
            else
            {
                orderList.Add(new OrderItem
                {
                    ProductID = sanPham.MaSanPham,
                    ProductName = sanPham.TenSanPham,
                    Quantity = 1,
                    Price = sanPham.GiaBan ?? 0 // Lấy giá bán từ sản phẩm, mặc định là 0 nếu giá bán null
                });
            }

            UpdateOrderUI();
        }


        private void txtTienKhachDua_TextChanged(object sender, EventArgs e)
        {
            // Đọc số tiền cần thanh toán và số tiền khách đưa từ các TextBox
            decimal tienCanThanhToan;
            decimal tienKhachDua;

            // Kiểm tra và chuyển đổi giá trị tiền cần thanh toán
            if (!decimal.TryParse(txtCanThanhToan.Text, out tienCanThanhToan))
            {
                txtTienThua.Text = "0";
                return;
            }

            // Kiểm tra và chuyển đổi giá trị số tiền khách đưa
            if (!decimal.TryParse(txtTienKhachDua.Text, out tienKhachDua))
            {
                txtTienThua.Text = "0";
                return;
            }

            // Tính toán số tiền thừa
            decimal tienThua = tienKhachDua - tienCanThanhToan;

            // Cập nhật TextBox tiền thừa
            if (tienThua < 0)
            {
                txtTienThua.Text = "0"; // Hoặc bạn có thể báo lỗi nếu số tiền khách đưa không đủ
            }
            else
            {
                txtTienThua.Text = String.Format("{0:N0}", tienThua);
            }
        }

        private void UpdateTienThua()
        {
            if (decimal.TryParse(txtTienKhachDua.Text, out decimal tienKhachDua) && decimal.TryParse(txtCanThanhToan.Text.Replace(",", ""), out decimal tienThanhToan))
            {
                decimal tienThua = tienKhachDua - tienThanhToan;
                txtTienThua.Text = String.Format("{0:N0}", tienThua);
            }
            else
            {
                txtTienThua.Text = "0";
            }
        }

        private void cbbGiamGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOrderUI();
        }

        private void ChanKiTuOrder()
        {
            txtTienKhachDua.KeyPress += new KeyPressEventHandler(ChanVanBan_KiTuDacBiet_KeyPress);
            txtTienKhachDua.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
        }

        private void UpdateOrderUI()
        {
            // Xóa các điều khiển hiện có trong flowLayoutPanelOrder
            flowLayoutPanelOrder.Controls.Clear();

            // Tính tổng số tiền
            decimal tongTien = orderList.Sum(o => o.Price * o.Quantity);

            // Tính toán giảm giá
            decimal discountPercentage = 0;
            if (cbbGiamGia.SelectedItem != null)
            {
                string selectedDiscount = cbbGiamGia.SelectedItem.ToString().Replace("%", "").Trim();
                if (decimal.TryParse(selectedDiscount, out discountPercentage))
                {
                    discountPercentage = discountPercentage / 100; // Chuyển đổi phần trăm thành số thập phân
                }
            }

            // Tính tổng số tiền sau khi giảm giá
            decimal discountAmount = tongTien * discountPercentage;
            decimal thanhToan = tongTien - discountAmount;

            // Hiển thị tổng số tiền và số tiền cần thanh toán
            txtTongTien.Text = String.Format("{0:N0}", tongTien);
            txtCanThanhToan.Text = String.Format("{0:N0}", thanhToan);

            // Thêm các mục đơn hàng vào giao diện người dùng
            foreach (var item in orderList)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanelOrder.Width - 25,
                    Height = 60,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblProductName = new Label
                {
                    Text = item.ProductName,
                    Width = 150,
                    Height = 30,
                    Location = new Point(10, 15),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };

                TextBox txtProductPrice = new TextBox
                {
                    Text = String.Format("{0:N0}", item.Price),
                    Width = 80,
                    Height = 30,
                    ReadOnly = true,
                    Location = new Point(170, 15),
                    TextAlign = HorizontalAlignment.Right
                };

                Button btnMinus = new Button
                {
                    Text = "-",
                    Width = 25,
                    Height = 25,
                    Location = new Point(260, 15),
                    BackColor = Color.Gray
                };

                TextBox txtQuantity = new TextBox
                {
                    Text = item.Quantity.ToString(),
                    Width = 40,
                    Height = 30,
                    TextAlign = HorizontalAlignment.Center,
                    Location = new Point(290, 15)
                };

                txtQuantity.KeyPress += (s, e) =>
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                };
                txtQuantity.TextChanged += (s, e) =>
                {
                    if (int.TryParse(txtQuantity.Text, out int quantity))
                    {
                        if (quantity < 0)
                        {
                            txtQuantity.Text = "0";
                        }
                        else
                        {
                            item.Quantity = quantity;
                            tongTien = orderList.Sum(o => o.Price * o.Quantity);
                            txtTongTien.Text = String.Format("{0:N0}", tongTien);
                            txtCanThanhToan.Text = String.Format("{0:N0}", tongTien - (tongTien * discountPercentage));
                            UpdateTienThua();
                        }
                    }
                };

                Button btnPlus = new Button
                {
                    Text = "+",
                    Width = 25,
                    Height = 25,
                    Location = new Point(335, 15),
                    BackColor = Color.Gray
                };

                Button btnRemove = new Button
                {
                    Text = "X",
                    Width = 25,
                    Height = 25,
                    Location = new Point(370, 15),
                    ForeColor = Color.Red
                };

                btnPlus.Click += (s, e) =>
                {
                    item.Quantity++;
                    txtQuantity.Text = item.Quantity.ToString();
                    tongTien = orderList.Sum(o => o.Price * o.Quantity);
                    txtTongTien.Text = String.Format("{0:N0}", tongTien);
                    txtCanThanhToan.Text = String.Format("{0:N0}", tongTien - (tongTien * discountPercentage));
                    UpdateTienThua();
                };
                btnMinus.Click += (s, e) =>
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        txtQuantity.Text = item.Quantity.ToString();
                        tongTien = orderList.Sum(o => o.Price * o.Quantity);
                        txtTongTien.Text = String.Format("{0:N0}", tongTien);
                        txtCanThanhToan.Text = String.Format("{0:N0}", tongTien - (tongTien * discountPercentage));
                        UpdateTienThua();
                    }
                };
                btnRemove.Click += (s, e) =>
                {
                    orderList.Remove(item);
                    UpdateOrderUI();
                };

                panel.Controls.Add(lblProductName);
                panel.Controls.Add(txtProductPrice);
                panel.Controls.Add(btnMinus);
                panel.Controls.Add(txtQuantity);
                panel.Controls.Add(btnPlus);
                panel.Controls.Add(btnRemove);

                flowLayoutPanelOrder.Controls.Add(panel);
            }

            // Đảm bảo tổng số tiền và số tiền cần thanh toán được hiển thị chính xác
            txtTongTien.Text = String.Format("{0:N0}", tongTien);
            txtCanThanhToan.Text = String.Format("{0:N0}", thanhToan);
            UpdateTienThua(); // Cập nhật số tiền thừa
        }

        private void LamMoiOther()
        {
            txtMaKhachHangOrder.Clear();
            txtTongTien.Clear();
            txtTienKhachDua.Clear();
            txtTienThua.Clear();
            txtCanThanhToan.Clear();
            txtMaKhachHangOrder.Focus();
        }

        private void txtTimKiemMenu_Click(object sender, EventArgs e)
        {
            hienThiOrder();
            txtTimKiemMenu.Clear();
        }

        private void btnTimKiemMenu_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemMenu.Text;
            hienThiOrder(keyword);
        }


        private void buttonHuyOrder()
        {
            flowLayoutPanelOrder.Controls.Clear();
            orderList.Clear();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            buttonHuyOrder();
            LamMoiOther();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thanh toán không?", "Xác nhận thanh toán", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                // Kiểm tra số tiền khách đưa
                if (!TienKhachDua())
                {
                    return;
                }

                // Thực hiện thanh toán hóa đơn
                bool thanhToanThanhCong = ThanhToanHoaDon();
                if (thanhToanThanhCong)
                {
                    // Xuất hóa đơn chỉ khi thanh toán thành công
                    XuatHoaDonThanhToan();
                    LamMoiOther();
                    buttonHuyOrder();
                    MessageBox.Show("Hóa đơn đã được tạo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thanh toán không thành công. Hóa đơn sẽ không được xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                // Xử lý khi người dùng chọn "No"
                MessageBox.Show("Thanh toán đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /*---Bắt lỗi Ko bỏ trống trường---*/
        private bool TienKhachDua()
        {
            // Kiểm tra nếu ô số tiền khách đưa để trống hoặc không hợp lệ
            if (string.IsNullOrEmpty(txtTienKhachDua.Text) || !decimal.TryParse(txtTienKhachDua.Text, out decimal soTienKhachDua))
            {
                MessageBox.Show("Số tiền khách đưa không được để trống và phải là số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Kiểm tra nếu số tiền khách đưa nhỏ hơn số tiền cần thanh toán
            if (soTienKhachDua < decimal.Parse(txtCanThanhToan.Text))
            {
                MessageBox.Show("Số tiền khách đưa không được nhỏ hơn số tiền cần thanh toán.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void XuatHoaDonThanhToan()
        {
            try
            {
                // Tạo một tài liệu PDF mới
                var document = new PdfDocument();
                document.Info.Title = "Hóa Đơn";

                // Tạo một trang mới
                var page = document.AddPage();
                page.Width = XUnit.FromMillimeter(210);
                page.Height = XUnit.FromMillimeter(297);
                var gfx = XGraphics.FromPdfPage(page);
                string maKhachHang = txtMaKhachHangOrder.Text;

                // Định dạng font
                var fontRegular = new XFont("Arial", 12, XFontStyle.Regular);
                var fontBold = new XFont("Arial", 14, XFontStyle.Bold);
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontBoldSmall = new XFont("Arial", 12, XFontStyle.Bold); // Font đậm cho giá tiền
                var fontBoldLabel = new XFont("Arial", 12, XFontStyle.Bold); // Font đậm cho tiêu đề thông tin bổ sung

                // Định dạng màu sắc
                var lineColor = XColors.Black; // Màu đen cho đường kẽ dưới
                var linePen = new XPen(lineColor, 1); // Đường kẽ dưới với độ dày 1

                // Vẽ logo
                var logoPath = @"C:\Users\HP\Downloads\Ảnh chụp màn hình 2024-08-04 022516.png"; // Đường dẫn tới logo
                var logo = XImage.FromFile(logoPath);
                var logoWidth = 100; // Điều chỉnh kích thước logo
                var logoHeight = (int)(logo.PixelHeight * (logoWidth / (double)logo.PixelWidth));

                // Vẽ logo ở giữa trang
                var logoX = (page.Width - logoWidth) / 2; // Căn giữa theo trục X
                gfx.DrawImage(logo, logoX, 20, logoWidth, logoHeight);

                // Vẽ tiêu đề và thông tin quán
                var offsetY = 20 + logoHeight + 10; // Khoảng cách từ logo đến thông tin quán

                gfx.DrawString("CAFE LION", fontTitle, XBrushes.Black,
                    new XRect(0, offsetY, page.Width, 20),
                    XStringFormats.Center);

                gfx.DrawString("Toà nhà FPT Polytechnic, Đ. Số 22, Thường Thạnh, Cái Răng, Cần Thơ, Việt Nam", fontRegular, XBrushes.Black,
                    new XRect(0, offsetY + 20, page.Width, 20),
                    XStringFormats.Center);

                gfx.DrawString("HÓA ĐƠN THANH TOÁN", fontBold, XBrushes.Black,
                    new XRect(0, offsetY + 60, page.Width, 20),
                    XStringFormats.Center);

                // Vẽ ngày, giờ vào, giờ ra
                var infoYPosition = offsetY + 90; // Căn chỉnh ngày, giờ vào, giờ ra
                var infoWidth = page.Width - 100;
                var infoHeight = 20;

                // Ngày
                gfx.DrawString($"Ngày: {DateTime.Now:dd/MM/yyyy}", fontRegular, XBrushes.Black,
                    new XRect(50, infoYPosition, infoWidth, infoHeight),
                    XStringFormats.TopLeft);

                // In lúc
                gfx.DrawString($"In lúc: {DateTime.Now:HH:mm}", fontRegular, XBrushes.Black,
                    new XRect(50, infoYPosition + infoHeight, infoWidth, infoHeight),
                    XStringFormats.TopLeft);

                // Giờ vào
                gfx.DrawString($"Giờ vào: {DateTime.Now:HH:mm}", fontRegular, XBrushes.Black,
                    new XRect(50, infoYPosition + 2 * infoHeight, infoWidth, infoHeight),
                    XStringFormats.TopLeft);

                // Giờ ra
                gfx.DrawString($"Giờ xuất hóa đơn: {DateTime.Now:HH:mm}", fontRegular, XBrushes.Black,
                    new XRect(50, infoYPosition + 3 * infoHeight, infoWidth, infoHeight),
                    XStringFormats.TopLeft);

                // Tạo bảng mặt hàng
                var tableTop = infoYPosition + 4 * infoHeight + 10; // Điều chỉnh vị trí tiêu đề bảng xuống một chút
                var rowHeight = 20;
                var columnWidths = new[] { 200, 50, 100, 100 }; // Căn chỉnh cột theo kích thước mong muốn

                // Vẽ tiêu đề bảng
                gfx.DrawString("Sản Phẩm", fontBold, XBrushes.Black,
                    new XRect(50, tableTop, columnWidths[0], rowHeight),
                    XStringFormats.Center);

                gfx.DrawString("Số lượng", fontBold, XBrushes.Black,
                    new XRect(50 + columnWidths[0], tableTop, columnWidths[1], rowHeight),
                    XStringFormats.Center);

                gfx.DrawString("Giá bán", fontBold, XBrushes.Black,
                    new XRect(50 + columnWidths[0] + columnWidths[1], tableTop, columnWidths[2], rowHeight),
                    XStringFormats.Center);

                gfx.DrawString("Tổng tiền", fontBold, XBrushes.Black,
                    new XRect(50 + columnWidths[0] + columnWidths[1] + columnWidths[2], tableTop, columnWidths[3], rowHeight),
                    XStringFormats.Center);

                // Vẽ đường kẽ dưới tiêu đề
                gfx.DrawLine(linePen,
                    new XPoint(50, tableTop + rowHeight),
                    new XPoint(50 + columnWidths.Sum(), tableTop + rowHeight));

                // Vẽ nội dung bảng
                var yPosition = tableTop + rowHeight + 5; // Thêm khoảng cách dưới tiêu đề

                decimal totalAmount = 0;
                foreach (var item in orderList)
                {
                    gfx.DrawString(item.ProductName, fontRegular, XBrushes.Black,
                        new XRect(50, yPosition, columnWidths[0], rowHeight),
                        XStringFormats.Center);

                    gfx.DrawString(item.Quantity.ToString(), fontRegular, XBrushes.Black,
                        new XRect(50 + columnWidths[0], yPosition, columnWidths[1], rowHeight),
                        XStringFormats.Center);

                    gfx.DrawString(string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C0}", item.Price), fontRegular, XBrushes.Black,
                        new XRect(50 + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], rowHeight),
                        XStringFormats.Center);

                    var totalPrice = item.Price * item.Quantity;
                    gfx.DrawString(string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C0}", totalPrice), fontRegular, XBrushes.Black,
                        new XRect(50 + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], rowHeight),
                        XStringFormats.Center);

                    totalAmount += totalPrice;
                    yPosition += rowHeight;
                }

                // Tính toán giảm giá
                decimal discountPercentage = 0;
                if (cbbGiamGia.SelectedItem != null)
                {
                    string selectedDiscount = cbbGiamGia.SelectedItem.ToString().Replace("%", "").Trim();
                    if (decimal.TryParse(selectedDiscount, out discountPercentage))
                    {
                        discountPercentage = discountPercentage / 100; // Chuyển đổi phần trăm thành số thập phân
                    }
                }

                // Tính tổng số tiền sau khi giảm giá
                decimal discountAmount = totalAmount * discountPercentage;
                decimal thanhToan = totalAmount - discountAmount;

                // Thêm thông tin giảm giá vào hóa đơn
                var additionalInfoTop = yPosition + rowHeight + 20; // Điều chỉnh khoảng cách giữa bảng và thông tin bổ sung
                var infoLabelWidth = 150; // Chiều rộng vùng tiêu đề
                var infoValueWidth = page.Width - 50 - infoLabelWidth; // Chiều rộng vùng giá trị

                gfx.DrawString("Tổng tiền:", fontBoldLabel, XBrushes.Black,
                    new XRect(50, additionalInfoTop, infoLabelWidth, 20),
                    XStringFormats.TopLeft);

                gfx.DrawString(string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C0}", totalAmount), fontBoldSmall, XBrushes.Black,
                    new XRect(page.Width - 50 - infoValueWidth, additionalInfoTop, infoValueWidth, 20),
                    XStringFormats.TopRight);

                gfx.DrawString("Giảm giá:", fontBoldLabel, XBrushes.Black,
                    new XRect(50, additionalInfoTop + 20, infoLabelWidth, 20),
                    XStringFormats.TopLeft);

                gfx.DrawString(string.Format(new System.Globalization.CultureInfo("vi-VN"), "-{0:C0}", discountAmount), fontBoldSmall, XBrushes.Black,
                    new XRect(page.Width - 50 - infoValueWidth, additionalInfoTop + 20, infoValueWidth, 20),
                    XStringFormats.TopRight);

                gfx.DrawString("Tiền thanh toán:", fontBoldLabel, XBrushes.Black,
                    new XRect(50, additionalInfoTop + 40, infoLabelWidth, 20),
                    XStringFormats.TopLeft);

                gfx.DrawString(string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C0}", thanhToan), fontBoldSmall, XBrushes.Black,
                    new XRect(page.Width - 50 - infoValueWidth, additionalInfoTop + 40, infoValueWidth, 20),
                    XStringFormats.TopRight);

                gfx.DrawString("Tiền khách đưa:", fontBoldLabel, XBrushes.Black,
                    new XRect(50, additionalInfoTop + 60, infoLabelWidth, 20),
                    XStringFormats.TopLeft);

                gfx.DrawString(txtTienKhachDua.Text, fontBoldSmall, XBrushes.Black,
                    new XRect(page.Width - 50 - infoValueWidth, additionalInfoTop + 60, infoValueWidth, 20),
                    XStringFormats.TopRight);

                gfx.DrawString("Tiền thừa:", fontBoldLabel, XBrushes.Black,
                    new XRect(50, additionalInfoTop + 80, infoLabelWidth, 20),
                    XStringFormats.TopLeft);

                gfx.DrawString(txtTienThua.Text, fontBoldSmall, XBrushes.Black,
                    new XRect(page.Width - 50 - infoValueWidth, additionalInfoTop + 80, infoValueWidth, 20),
                    XStringFormats.TopRight);

                // Vẽ đường kẻ phía trên lời cảm ơn
                var thankYouLineTop = additionalInfoTop + 100;
                gfx.DrawLine(linePen,
                    new XPoint(50, thankYouLineTop),
                    new XPoint(page.Width - 50, thankYouLineTop));

                // Vẽ lời cảm ơn
                var thankYouYPosition = thankYouLineTop + 10; // Vị trí lời cảm ơn
                gfx.DrawString("Cảm ơn Quý khách. Hẹn gặp lại.", fontBold, XBrushes.Black,
                    new XRect(0, thankYouYPosition, page.Width, 20),
                    XStringFormats.Center);

                // Lưu tài liệu PDF
                var filePath = @"C:\Users\HP\Downloads\DuAn1Lion (1)\HoaDonThanhToan.pdf"; // Đường dẫn lưu hóa đơn
                document.Save(filePath);





                // Mở tài liệu sau khi tạo
                Process.Start("explorer.exe", filePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tạo hóa đơn: {ex.Message}");
            }

        }

        public bool ThanhToanHoaDon()
        {
            // Không cần kiểm tra trạng thái hủy ở đây nữa, vì đã được xử lý ở trên

            string maNhanVienDangNhap = maNhanVien; // Mã nhân viên đăng nhập hiện tại
            string maKhachHang = txtMaKhachHangOrder.Text; // Nhập từ giao diện người dùng
            decimal tongHoaDon = orderList.Sum(o => o.Price * o.Quantity); // Tổng hóa đơn từ orderList
            int soLuongMon = orderList.Sum(o => o.Quantity); // Tổng số lượng món từ orderList
            int giamGia = int.Parse(cbbGiamGia.SelectedItem.ToString().Replace("%", "").Trim()); // Giảm giá
            string ghiChu = txtGhiChu.Text; // Ghi chú từ giao diện người dùng

            // Tạo danh sách chi tiết hóa đơn từ orderList
            List<HoaDonChiTiet> chiTietHoaDon = orderList.Select(o => new HoaDonChiTiet
            {
                MaSanPham = o.ProductID,
                SoLuongTungMon = o.Quantity
            }).ToList();

            ThanhToan thanhToan = new ThanhToan(maNhanVienDangNhap);
            return thanhToan.ThucHienThanhToan(maKhachHang, tongHoaDon, soLuongMon, giamGia, ghiChu, chiTietHoaDon);

        }

        public class ThanhToan
        {
            private LionQuanLyQuanCaPheDataContext db;
            private string maNhanVien;

            public ThanhToan(string maNhanVienDangNhap)
            {
                db = new LionQuanLyQuanCaPheDataContext(); // Khởi tạo đối tượng db tại đây
                maNhanVien = maNhanVienDangNhap;
            }

            private string GenerateMaHoaDon()
            {
                // Lấy mã hóa đơn cao nhất hiện tại từ cơ sở dữ liệu
                var maxMaHoaDon = db.HoaDons.OrderByDescending(h => h.MaHoaDon).Select(h => h.MaHoaDon).FirstOrDefault();

                if (maxMaHoaDon == null)
                {
                    return "HD001";
                }

                // Tách phần số ra từ mã hóa đơn
                var numberPart = int.Parse(maxMaHoaDon.Substring(2));

                // Tạo mã hóa đơn mới với phần số tăng lên 1
                return "HD" + (numberPart + 1).ToString("D3");
            }

            public bool ThucHienThanhToan(string maKhachHang, decimal tongHoaDon, int soLuongMon, int giamGia, string ghiChu, List<HoaDonChiTiet> chiTietHoaDon)
            {
                DateTime ngayXuatHoaDon = DateTime.Now;
                string maHoaDon = GenerateMaHoaDon();

                if (string.IsNullOrEmpty(maKhachHang))
                {
                    maKhachHang = "KHONGMA";
                }
                else
                {
                    // Kiểm tra mã khách hàng có tồn tại trong cơ sở dữ liệu hay không
                    var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKhachHang == maKhachHang);
                    if (khachHang == null)
                    {
                        MessageBox.Show("Mã khách hàng không tồn tại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                try
                {
                    // Lưu hóa đơn vào cơ sở dữ liệu
                    SaveHoaDonToDatabase(maHoaDon, maNhanVien, maKhachHang, ngayXuatHoaDon, tongHoaDon, soLuongMon, giamGia, ghiChu, chiTietHoaDon);
                    return true;
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ (ghi log, thông báo lỗi, v.v.)
                    Console.WriteLine("Lỗi: " + ex.Message);
                    MessageBox.Show("Đã xảy ra lỗi khi thực hiện thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            private void SaveHoaDonToDatabase(string maHoaDon, string maNhanVien, string maKhachHang, DateTime ngayXuatHoaDon, decimal tongHoaDon, int soLuongMon, int giamGia, string ghiChu, List<HoaDonChiTiet> chiTietHoaDon)
            {
                try
                {
                    Console.WriteLine("Bắt đầu lưu hóa đơn vào cơ sở dữ liệu.");

                    // Tạo đối tượng hóa đơn mới
                    HoaDon hoaDon = new HoaDon
                    {
                        MaHoaDon = maHoaDon,
                        MaNhanVien = maNhanVien,
                        MaKhachHang = maKhachHang,
                        NgayXuatHoaDon = ngayXuatHoaDon,
                        TongHoaDon = tongHoaDon,
                        SoLuongMon = soLuongMon,
                        GiamGia = giamGia,
                        GhiChu = ghiChu
                    };

                    // Thêm hóa đơn vào cơ sở dữ liệu
                    db.HoaDons.InsertOnSubmit(hoaDon);

                    // Thêm chi tiết hóa đơn vào cơ sở dữ liệu
                    foreach (var item in chiTietHoaDon)
                    {
                        item.MaHoaDon = maHoaDon;
                        db.HoaDonChiTiets.InsertOnSubmit(item);
                    }

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SubmitChanges();

                    Console.WriteLine("Lưu hóa đơn thành công.");

                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ (ghi log, thông báo lỗi, v.v.)
                    Console.WriteLine("Lỗi khi lưu hóa đơn: " + ex.Message);
                    MessageBox.Show("Đã xảy ra lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw; // Ném ngoại lệ để gọi phương thức biết rằng có lỗi
                }
            }
        }


        private void FormChucNangQuanLy_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
            this.Hide();
        }


    }
}



