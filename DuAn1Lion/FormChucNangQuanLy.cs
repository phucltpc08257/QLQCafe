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


        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {



        }

        private void LoadData()
        {
            HienThiNhanVien();
            HienThioVaiTro();
            HienThiThongKeNhanVien();
            txtMaNhanVien.ReadOnly = true;
            txtMaNhanVien.TabStop = false;
            txtMaVaiTro.ReadOnly = true;
            txtMaVaiTro.TabStop = false;

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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvThongTinNhanVien.Rows[e.RowIndex];
                DisplayNhanVienDetails(selectedRow);


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

        private bool ValidateVaiTroInput()
        {
            if (string.IsNullOrWhiteSpace(cbbvaitro.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private string RandomMatKhau()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnThongKeNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemThongKeNhanVien();

        }

        private void LoadGroupBoxData()
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

        private void grbTimKiemNhanVien_Enter(object sender, EventArgs e)
        {
            LoadGroupBoxData();
            grbTimKiemNhanVien.Refresh();
        }
    }
}
