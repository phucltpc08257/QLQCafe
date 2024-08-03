using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DuAn1Lion
{

    public partial class FormChucNangQuanLy : Form
    {
        private string connectionString = "Server=ADMIN-LUAN-PC08;Database=QuanLiQuanCaPhe;User Id=sa;Password=123456;";

        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
            dtgvThongTinNhanVien.CellFormatting += dtgvThongTinNhanVien_CellFormatting;
            dtgvThongTinNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvThongTinNhanVien.MultiSelect = false;
          
            AnMaNhanVien();
            AnMaVaiTro();
            Console.WriteLine(RandomMatKhau());
        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {



        }
        //ẨN TEXTBOX MÃ NHAN VIEN KHÔNG ĐƯỢC NHẬP
        private void angioitinhvaitro()
        {

        }
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

        private void LoadData()
        {

            cbbVaiTroCuaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGioiTinhNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;

            HienThiNhanVien();
            HienThioVaiTro();
            HienThiThongKeNhanVien();
            LoadGroupBoxData();
        }

        private void tclFormChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tclFormChucNang.SelectedTab == tpNhanVien)
            {
                HienThiNhanVien();

            }
            else if (tclFormChucNang.SelectedTab == tpVaiTro)
            {
                HienThioVaiTro();

            }
        }

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

                var predefinedRoles = new List<string> {" " ,"Admin", "Quản lý", "Nhân viên bán hàng " }; 
                cbbvaitro.DataSource = predefinedRoles;
            }
        }

        private void dtgvThongTinNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvThongTinNhanVien.Columns[e.ColumnIndex].Name == "MatKhau")
            {
                if (e.Value != null)
                {
                    e.Value = "********";
                }
            }
        }

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

        private void btnTimKiemthongtinNhanVien_Click_1(object sender, EventArgs e)
        {
            TimKiemNhanVien();

        }



        private void txtTimKiemThongTinVaiTro_TextChanged(object sender, EventArgs e)
        {
            TimKiemVaiTro();
        }
        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
        {
            // Handle search product button click
        }

        private void TcThongKeNhanVien_Click(object sender, EventArgs e)
        {
            // Handle statistical tab click
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Handle search button click
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

        private void ClearTextBox()
        {
            txtMaVaiTro.Clear();
            cbbGioiTinhNhanVien.SelectedIndex = -1;
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtEmail.Clear();
            txtSDTNhanVien.Clear();
            txtDiaChi.Clear();
        
            dttpNgaySinhNhanVien.Value = DateTime.Now;

            dttpNgayBatDauLamCuaNhanVien.Value = DateTime.Now;
        }

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


        private void btnResetNhanVien_Click(object sender, EventArgs e)
        {
            ClearTextBox();
        }

        private void btnResetVaiTro_Click(object sender, EventArgs e)
        {
            txtMaVaiTro.Clear();
            cbbvaitro.SelectedIndex = -1;
        }

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
                    cbbGioiTinhNhanVien.SelectedIndex = -1;
                }
            }
        }

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


        private void btnResetNhanVien_Click_1(object sender, EventArgs e)
        {
            ClearTextBox();
        }

        private void btnResetVaiTro_Click_1(object sender, EventArgs e)
        {
            txtMaVaiTro.Clear();
            cbbvaitro.SelectedIndex = -1;
        }

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
                        // Kiểm tra trùng lặp tên vai trò
                        string tenVaiTroMoi = cbbvaitro.Text;
                        var vaiTroTrungLap = QLNV.VaiTros.FirstOrDefault(vt => vt.TenVaiTro == tenVaiTroMoi && vt.MaVaiTro != maVT);

                        if (vaiTroTrungLap != null)
                        {
                            MessageBox.Show("Tên vai trò đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Dừng quá trình cập nhật
                        }

                        // Cập nhật thông tin vai trò
                        vaiTro.TenVaiTro = tenVaiTroMoi;

                        try
                        {
                            QLNV.SubmitChanges();
                            MessageBox.Show("Cập nhật thông tin vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThioVaiTro(); // Cập nhật giao diện hoặc danh sách vai trò
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
                            // Danh sách các vai trò không thể bị xóa
                            var vaiTroKhongXoa = new HashSet<string> { "Admin", "Quản lý", "Nhân viên bán hàng " };

                            if (vaiTroKhongXoa.Contains(vaiTro.TenVaiTro))
                            {
                                MessageBox.Show("Vai trò này không thể bị xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Dừng quá trình xóa
                            }

                            try
                            {
                                QLNV.VaiTros.DeleteOnSubmit(vaiTro);
                                QLNV.SubmitChanges();
                                MessageBox.Show("Xóa vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                HienThioVaiTro (); // Cập nhật giao diện hoặc danh sách vai trò
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

        private void TimKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                // Lấy từ khóa tìm kiếm từ textbox và loại bỏ khoảng trắng thừa
                string keyword = txtTimKiemThongTinNhanVien.Text.Trim();

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


        private void HienThiThongKeNhanVien()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("ThongKeNhanVien", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtgvThongKeNhanVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void TimKiemThongKeNhanVien()
        {
            string tuKhoa_NV = txtThongKeNhanVien.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa_NV))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên hoặc tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var QLBH = new LionQuanLyQuanCaPheDataContext(); // Đảm bảo đã khởi tạo đối tượng DataContext
                var List_NV = QLBH.ThongKeNhanVien().ToList(); // Gọi procedure ThongKeNhanVien để lấy danh sách nhân viên

                var filteredList = from nv in List_NV
                                   where nv.MaNhanVien.ToLower().Contains(tuKhoa_NV) ||
                                         nv.TenNhanVien.ToLower().Contains(tuKhoa_NV)
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

                    DinhDangDataGridViewNhanVien();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgvThongKeNhanVien.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DinhDangDataGridViewNhanVien()
        {
            // Dinh dạng DataGridView tương tự như phần DinhDangDataGridView của sản phẩm
            // Có thể thêm định dạng cho các cột như MaNhanVien, TenNhanVien, SoLuongHoaDonTuan, ...

            // Ví dụ:
            if (dtgvThongKeNhanVien.Columns.Contains("SoLuongHoaDonTuan"))
            {
                dtgvThongKeNhanVien.Columns["SoLuongHoaDonTuan"].DefaultCellStyle.Format = "N0";
            }
        }




        private DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

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

                // Lấy 8 ký tự đầu tiên của chuỗi hexa và thay thế bằng dấu '*'
                string md5Hash = sb.ToString();
                string maskedHash = new string('*', 8); // Dự kiến là dấu '*'

                return maskedHash;
            }
        }

        private void btnThongKeNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemThongKeNhanVien();

        }

        private void LoadGroupBoxData()
        {
            txtMaVaiTro.Clear();
            cbbGioiTinhNhanVien.SelectedIndex = -1;

            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtEmail.Clear();
            txtSDTNhanVien.Clear();
            txtDiaChi.Clear();

            dttpNgaySinhNhanVien.Value = DateTime.Now;

            dttpNgayBatDauLamCuaNhanVien.Value = DateTime.Now;
        }
        private void grbTimKiemNhanVien_Enter(object sender, EventArgs e)
        {
            LoadGroupBoxData();
            grbTimKiemNhanVien.Refresh();
        }

        private void picout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hệ thống đang Load.......", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Xác nhận Thoát hệ hệ thống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show(" Cúc lẹ......", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            FormDangNhap form = new FormDangNhap();

            form.ShowDialog();
            form = null;
            this.Show();
            this.Close();
        }

        private void grbTimKiem_Enter(object sender, EventArgs e)
        {

        }

        private void cbbVaiTroCuaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
    }
}
