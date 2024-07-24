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

        // Event handlers
        private void dtgvThongTinNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvThongTinNhanVien.Rows[e.RowIndex];
                DisplayNhanVienDetails(selectedRow);

                // Kích hoạt nút Sửa và Xóa khi có hàng được chọn
                btnSuaNhanVien.Enabled = true;
                btnXoaNhanVien.Enabled = true;
            }
            else
            {
                // Vô hiệu hóa nút Sửa và Xóa khi không có hàng nào được chọn
                btnSuaNhanVien.Enabled = false;
                btnXoaNhanVien.Enabled = false;
            }
        }

        public partial class YourDataContext : DataContext
        {
            public Table<NhanVien> NhanViens;
            public Table<HoaDon> HoaDons;

            public YourDataContext(string connectionString) : base(connectionString) { }

            [Function(Name = "ThongKeNhanVien")]
            public ISingleResult<ThongKeNhanVienResult> ThongKeNhanVien()
            {
                IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
                return ((ISingleResult<ThongKeNhanVienResult>)(result.ReturnValue));
            }
        }
        [Table(Name = "ThongKeNhanVien")]
        public class ThongKeNhanVienResult
        {
            [Column(Name = "MaNhanVien")]
            public string MaNhanVien { get; set; }

            [Column(Name = "TenNhanVien")]
            public string TenNhanVien { get; set; }

            [Column(Name = "SoLuongHoaDonTuan")]
            public int SoLuongHoaDonTuan { get; set; }

            [Column(Name = "SoLuongHoaDonThang")]
            public int SoLuongHoaDonThang { get; set; }

            [Column(Name = "SoLuongHoaDonNam")]
            public int SoLuongHoaDonNam { get; set; }


        }


        private void dtgvThongTinNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra nếu đang xử lý cột "MatKhau"
            if (dtgvThongTinNhanVien.Columns[e.ColumnIndex].Name == "MatKhau")
            {
                // Kiểm tra giá trị của ô
                if (e.Value != null)
                {
                    e.Value = "********"; // Thay đổi giá trị của ô thành "********"
                }
            }
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

        // Form load and initialization
        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }

        // Data loading and display methods
        private void LoadData()
        {
            HienThiNhanVien();
            HienThioVaiTro();
            txtMaVaiTro.ReadOnly = true;
            txtMaVaiTro.TabStop = false;
            TimKiemThongKeNhanVien();
            string connectionString = @"Data Source=ADMIN-LUAN-PC08;Initial Catalog=QuanLiQuanCaPhe;Integrated Security=True;";

            using (var db = new YourDataContext(connectionString))
            {
                try
                {
                    var result = db.ThongKeNhanVien(); // Thực thi stored procedure

                    dtgvThongKeNhanVien.DataSource = result.ToList(); // Hiển thị kết quả lên DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thực thi stored procedure: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                               nv.GioiTinh,
                               nv.NgaySinh,
                               nv.TenNhanVien,
                               nv.SDT,
                               nv.NgayBatDauLamViec,
                               nv.Email,
                               nv.DiaChi,
                               nv.MatKhau,
                               vt.MaVaiTro,
                               vt.TenVaiTro
                           };

                dtgvThongTinNhanVien.DataSource = list.ToList();

                // Load mã và tên vai trò vào ComboBox
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
            }
        }





        // CRUD operations for NhanVien
        private void ThemNhanVien()
        {
            if (ValidateNhanVienInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string randomPassword = RandomMatKhau(); // Tạo mật khẩu ngẫu nhiên
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
                        MatKhau = randomPassword // Gán mật khẩu ngẫu nhiên vào nhân viên mới
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


        private string RandomMatKhau()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomMatKhau = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                randomMatKhau.Append(chars[random.Next(chars.Length)]);
            }
            return randomMatKhau.ToString();
        }


        private void SuaNhanVien()
        {
            if (ValidateNhanVienInput()) // Kiểm tra thông tin nhập vào là hợp lệ
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string maNV = txtMaNhanVien.Text;
                    var nhanVien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == maNV);

                    if (nhanVien != null)
                    {
                        // Cập nhật thông tin từ các control vào đối tượng nhanVien
                        nhanVien.TenNhanVien = txtTenNhanVien.Text;
                        nhanVien.Email = txtEmail.Text;
                        nhanVien.SDT = txtSDTNhanVien.Text;
                        nhanVien.DiaChi = txtDiaChi.Text;
                        nhanVien.MaVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString(); // Lấy mã vai trò từ combobox
                        nhanVien.NgaySinh = dttpNgaySinhNhanVien.Value;
                        nhanVien.GioiTinh = cbbGioiTinhNhanVien.Text;
                        nhanVien.NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value;

                        try
                        {
                            QLNV.SubmitChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
                            MessageBox.Show("Đã cập nhật thông tin nhân viên thành công!");
                            HienThiNhanVien(); // Hiển thị lại danh sách nhân viên
                            ClearTextBox(); // Xóa các trường nhập liệu
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên có mã số này!");
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
                            MessageBox.Show("Đã xóa nhân viên thành công!");
                            ClearTextBox();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên có mã số này!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên này: " + ex.Message);
            }
        }

        // CRUD operations for VaiTro
        private void TimKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string maNhanVien = txtTimKiemThongTinNhanVien.Text.Trim(); // Lấy mã nhân viên từ textbox tìm kiếm

                // Query lấy thông tin nhân viên từ database dựa vào mã nhân viên nhập vào
                var timKiem = from nv in QLNV.NhanViens
                              join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro into vtGroup
                              from vt in vtGroup.DefaultIfEmpty()
                              where nv.MaNhanVien.Contains(maNhanVien)
                              select new
                              {
                                  nv.MaNhanVien,
                                  nv.GioiTinh,
                                  nv.NgaySinh,
                                  nv.TenNhanVien,
                                  nv.SDT,
                                  nv.NgayBatDauLamViec,
                                  nv.Email,
                                  nv.DiaChi,
                                  nv.MatKhau,
                                  MaVaiTro = vt != null ? vt.MaVaiTro : "", // Lấy mã vai trò nếu tồn tại, ngược lại trả về rỗng
                                  TenVaiTro = vt != null ? vt.TenVaiTro : "" // Lấy tên vai trò nếu tồn tại, ngược lại trả về rỗng
                              };

                dtgvThongTinNhanVien.DataSource = timKiem.ToList(); // Gán kết quả vào DataSource của DataGridView
            }
        }


        private void ThemVaiTro()
        {
            if (ValidateVaiTroInput())
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    VaiTro newVaiTro = new VaiTro()
                    {
                        MaVaiTro = GenerateMaVaiTro(),
                        TenVaiTro = txtTenVaiTro.Text
                    };

                    QLNV.VaiTros.InsertOnSubmit(newVaiTro);
                    try
                    {
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm thành công");
                        HienThioVaiTro();
                        ClearTextBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void SuaVaiTro()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string mavt = txtMaVaiTro.Text;
                var vaitro = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == mavt);
                if (vaitro != null)
                {
                    vaitro.TenVaiTro = txtTenVaiTro.Text;
                    try
                    {
                        QLNV.SubmitChanges();
                        MessageBox.Show("Sửa thông tin vai trò thành công");
                        HienThioVaiTro();
                        ClearTextBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vai trò có mã số này!");
                }
            }
        }

        private void XoaVaiTro()
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (var QLNV = new LionQuanLyQuanCaPheDataContext())
                {
                    string ma = txtMaVaiTro.Text;
                    var xoa = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == ma);
                    if (xoa != null)
                    {
                        QLNV.VaiTros.DeleteOnSubmit(xoa);
                        QLNV.SubmitChanges();
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThioVaiTro();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy vai trò", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void TimKiemVaiTro()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string maVaiTro = txttimkiemVaiTro.Text.Trim(); // Lấy mã vai trò từ textbox tìm kiếm

                // Query lấy thông tin vai trò từ database dựa vào mã vai trò nhập vào
                var timKiem = from vt in QLNV.VaiTros
                              where vt.MaVaiTro.Contains(maVaiTro)
                              select new
                              {
                                  vt.MaVaiTro,
                                  vt.TenVaiTro
                              };

                dtgvVaiTro.DataSource = timKiem.ToList(); // Gán kết quả vào DataSource của DataGridView
            }
        }




        // Utility methods
        private string GenerateMaNhanVien()
        {
            using (var context = new LionQuanLyQuanCaPheDataContext())
            {
                int nextId = 1;
                string newMaNhanVien = $"NV{nextId:D3}";

                // Kiểm tra xem mã nhân viên đã tồn tại chưa
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

                // Kiểm tra xem mã vai trò đã tồn tại chưa
                while (context.VaiTros.Any(vt => vt.MaVaiTro == newMaVaiTro))
                {
                    nextId++;
                    newMaVaiTro = $"VT{nextId:D3}";
                }

                return newMaVaiTro;
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

            // Lấy mã và tên vai trò từ dòng được chọn
            string maVaiTro = selectedRow.Cells["MaVaiTro"].Value.ToString();
            string tenVaiTro = selectedRow.Cells["TenVaiTro"].Value.ToString();

            // Hiển thị vai trò lên combobox cbbVaiTroCuaNhanVien
            var vaiTroList = cbbVaiTroCuaNhanVien.DataSource as List<VaiTro>; // Lấy danh sách vai trò từ combobox
            if (vaiTroList != null)
            {
                // Tìm vai trò trong danh sách
                VaiTro selectedVaiTro = vaiTroList.FirstOrDefault(vt => vt.MaVaiTro == maVaiTro);
                if (selectedVaiTro != null)
                {
                    cbbVaiTroCuaNhanVien.SelectedItem = selectedVaiTro; // Chọn vai trò tương ứng
                }
            }


        }


        private void ClearTextBox()
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtEmail.Clear();
            txtSDTNhanVien.Clear();
            txtDiaChi.Clear();
            cbbVaiTroCuaNhanVien.SelectedIndex = -1; // or .Text = ""
            dttpNgaySinhNhanVien.Value = DateTime.Now; // or your default value
            cbbGioiTinhNhanVien.SelectedIndex = -1; // or .Text = ""
            dttpNgayBatDauLamCuaNhanVien.Value = DateTime.Now; // or your default value
        }

        private bool ValidateNhanVienInput()
        {

            return true;
        }

        private bool ValidateVaiTroInput()
        {
            return true;
        }

        private void dtgvVaiTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1) return;


            DataGridViewRow row = dtgvVaiTro.Rows[e.RowIndex];


            string maVaiTro = row.Cells["MaVaiTro"].Value.ToString();
            string tenVaiTro = row.Cells["TenVaiTro"].Value.ToString();

            txtMaVaiTro.Text = maVaiTro;
            txtTenVaiTro.Text = tenVaiTro;
        }


        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
        {

        }

     

        private void TcThongKeNhanVien_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }


        private void TimKiemThongKeNhanVien()
        {

        }

        private void btnTimKiemthongtinNhanVien_Click_1(object sender, EventArgs e)
        {
            TimKiemNhanVien();
        }
    }




}

