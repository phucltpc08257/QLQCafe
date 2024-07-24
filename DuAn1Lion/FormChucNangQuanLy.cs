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

namespace DuAn1Lion
{
    public partial class FormChucNangQuanLy : Form
    {
        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
            dtgvThongTinNhanVien.CellFormatting += dtgvThongTinNhanVien_CellFormatting;
        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }

        private void LoadData()
        {
            HienThiNhanVien();
            HienThioVaiTro();

            txtMaNhanVien.ReadOnly = true;
            txtMaNhanVien.TabStop = false;
            txtMaVaiTro.ReadOnly = true;
            txtMaVaiTro.TabStop = false;
            TimKiemThongKeNhanVien();
          
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

        private void TimKiemThongKeNhanVien()
        {
            // Implement employee statistics search
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
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var HTNhanVien = (from nv in QLKH.NhanViens

                              where nv.MaNhanVien == dtgvThongTinNhanVien.CurrentRow.
                              Cells["MaNhanVien"].Value.ToString()
                              select nv).SingleOrDefault();

            txtMaNhanVien.Text = HTNhanVien.MaNhanVien.ToString();
            txtTenNhanVien.Text = HTNhanVien.TenNhanVien.ToString();
            txtSDTNhanVien.Text = HTNhanVien.SDT.ToString();
            txtDiaChi.Text = HTNhanVien.DiaChi.ToString();
            cbbGioiTinhNhanVien.Text = HTNhanVien.GioiTinh.ToString();
            txtEmail.Text = HTNhanVien.Email.ToString();
            dttpNgaySinhNhanVien.Text = HTNhanVien.NgaySinh.ToString();
            dttpNgayBatDauLamCuaNhanVien.Text = HTNhanVien.NgayBatDauLamViec.ToString();
            cbbVaiTroCuaNhanVien.Text = HTNhanVien.VaiTro.ToString();
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
                        MessageBox.Show("Thêm thành công");
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
                            MessageBox.Show("Cập nhật thông tin nhân viên thành công");
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
                        MessageBox.Show("Không tìm thấy nhân viên để cập nhật");
                    }
                }
            }
        }

        private void XoaNhanVien()
        {
            string maNV = txtMaNhanVien.Text;

            if (!string.IsNullOrEmpty(maNV))
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                    {
                        var nhanVien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == maNV);

                        if (nhanVien != null)
                        {
                            try
                            {
                                QLNV.NhanViens.DeleteOnSubmit(nhanVien);
                                QLNV.SubmitChanges();
                                MessageBox.Show("Xóa nhân viên thành công");
                                HienThiNhanVien();
                                ClearTextBox();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để xóa");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa");
            }
        }
        private bool ValidateNhanVienInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên");
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
                    VaiTro ThemVT = new VaiTro()
                    {
                        MaVaiTro = GenerateMaVaiTro(),
                        TenVaiTro = cbbvaitro.Text
                    };

                    try
                    {
                        QLNV.VaiTros.InsertOnSubmit(ThemVT);
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm vai trò thành công");
                        HienThioVaiTro();
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
                        vaiTro.TenVaiTro = cbbvaitro.Text;

                        try
                        {
                            QLNV.SubmitChanges();
                            MessageBox.Show("Cập nhật thông tin vai trò thành công");
                            HienThioVaiTro();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật vai trò: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy vai trò để cập nhật");
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
                            try
                            {
                                QLNV.VaiTros.DeleteOnSubmit(vaiTro);
                                QLNV.SubmitChanges();
                                MessageBox.Show("Xóa vai trò thành công");
                                HienThioVaiTro();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi xóa vai trò: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy vai trò để xóa");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vai trò để xóa");
            }
        }

        private bool ValidateVaiTroInput()
        {
            if (string.IsNullOrWhiteSpace(cbbvaitro.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò");
                return false;
            }

            return true;
        }

        private void TimKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var list = from nv in QLNV.NhanViens
                           join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                           where nv.MaNhanVien.Contains(txtTimKiemThongTinNhanVien.Text) || nv.TenNhanVien.Contains(txtTimKiemThongTinNhanVien.Text)
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


        private string RandomMatKhau()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
    }
}
