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
        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
          
      }

        private void tpSanPham_Click(object sender, EventArgs e)
        {

        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {

        }

        private void grbThongTinSanPham_Enter(object sender, EventArgs e)
        {

        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {

        }

        private void grbChucNangSanPham_Enter(object sender, EventArgs e)
        {

        }

        private void tpKhachHang_Click(object sender, EventArgs e)
        {

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
        }
        private void LoadData()
        {
            HienThiNhanVien();
            HienThioVaiTro();
       
        }
        //hien thi nhan vien
        private void LoadVaiTro()
        {
            try
            {
                var QLNV = new LionQuanLyQuanCaPheDataContext();

                // Lấy danh sách vai trò từ cơ sở dữ liệu
                var vaiTros = QLNV.VaiTros.ToList();

                // Thiết lập DisplayMember và ValueMember của ComboBox
                cbbVaiTroCuaNhanVien.DisplayMember = "TenVaiTro"; // Hiển thị tên vai trò
                cbbVaiTroCuaNhanVien.ValueMember = "MaVaiTro";    // Giá trị của mã vai trò

                // Gán danh sách vai trò làm DataSource cho ComboBox
                cbbVaiTroCuaNhanVien.DataSource = vaiTros;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiNhanVien()
        {
            var QLNV = new LionQuanLyQuanCaPheDataContext();

            var List = from nv in QLNV.NhanViens
                       join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro into nvvt
                       from subvt in nvvt.DefaultIfEmpty()
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
                           VaiTro = subvt == null ? "" : subvt.TenVaiTro
                       };

            dtgvThongTinNhanVien.DataSource = List.ToList();

            // Đổ dữ liệu vào ComboBox cbbVaiTroCuaNhanVien
            var vaiTros = QLNV.VaiTros.Select(vt => vt.TenVaiTro).ToList();
            cbbVaiTroCuaNhanVien.DataSource = vaiTros;
        }


        //hienthivaitro
        private void HienThioVaiTro()
        {
            try
            {
                var QLNV = new LionQuanLyQuanCaPheDataContext();

                var List = from vt in QLNV.VaiTros
                           select new
                           {
                               vt.MaVaiTro,
                               vt.TenVaiTro
                           };

                dtgvVaiTro.DataSource = List.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void grbTimKiem_Enter(object sender, EventArgs e)
        {

        }

        private void txtDiaChiKhachHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtgvThongTinNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dtgvThongTinNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvThongTinNhanVien.Rows[e.RowIndex];

              
                string maNhanVien = row.Cells["MaNhanVien"].Value.ToString();
                string tenNhanVien = row.Cells["TenNhanVien"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();
                string sdt = row.Cells["SDT"].Value.ToString();
                string diaChi = row.Cells["DiaChi"].Value.ToString();
                DateTime ngaySinh = (DateTime)row.Cells["NgaySinh"].Value;
                DateTime ngayBatDau = (DateTime)row.Cells["NgayBatDauLamViec"].Value;
                string gioiTinh = row.Cells["GioiTinh"].Value.ToString();
              
              
                txtMaNhanVien.Text = maNhanVien;
                txtTenNhanVien.Text = tenNhanVien;
                txtEmail.Text = email;
                txtSDTNhanVien.Text = sdt;
                txtDiaChi.Text = diaChi;
                dttpNgaySinhNhanVien.Value = ngaySinh;
                dttpNgayBatDauLamCuaNhanVien.Value = ngayBatDau;
                cbbGioiTinhNhanVien.Text = gioiTinh;
             
              

               
            }
        }
       


        private void btnXoaNhanVien_Click(object sender, EventArgs e)
        {
            XoaNhanVien();
            ClearTextBox();
        }



        private void btnSuaNhanVien_Click(object sender, EventArgs e)
        {
            SuaNhanVien();
            ClearTextBox();
        }

        private void btnThemNhanVien_Click(object sender, EventArgs e)
        {
            ThemNhanVien();
            ClearTextBox();

        }



        private void dtgvThongTinVaiTro_Enter(object sender, EventArgs e)
        {

        }
        private void ThemNhanVien()
        {
            // Kiểm tra các trường bắt buộc phải được nhập đầy đủ
            if (string.IsNullOrEmpty(txtTenNhanVien.Text) ||
                string.IsNullOrEmpty(txtSDTNhanVien.Text) ||
                string.IsNullOrEmpty(txtDiaChi.Text) ||
                string.IsNullOrEmpty(cbbGioiTinhNhanVien.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                dttpNgaySinhNhanVien.Value == null ||
                dttpNgayBatDauLamCuaNhanVien.Value == null ||
                cbbVaiTroCuaNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ. Vui lòng nhập đúng định dạng email (ví dụ: example@gmail.com).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(txtSDTNhanVien.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại 10 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var QLNV = new LionQuanLyQuanCaPheDataContext();
            using (QLNV)
            {
                // Tạo mã nhân viên tự động
                string maNhanVien = GenerateMaNhanVien();

                // Lấy giá trị MaVaiTro từ ComboBox
                string maVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString();

                // Kiểm tra xem mã vai trò đã tồn tại trong bảng VaiTros chưa
                var vaiTro = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == maVaiTro);
                if (vaiTro == null)
                {
                  
                    vaiTro = new VaiTro()
                    {
                        MaVaiTro = maVaiTro,
                        TenVaiTro = cbbVaiTroCuaNhanVien.Text // Thêm tên vai trò vào đây, nếu cần
                    };
                    QLNV.VaiTros.InsertOnSubmit(vaiTro);
                    QLNV.SubmitChanges();
                }

                // Sau khi đảm bảo mã vai trò tồn tại, thêm nhân viên
                NhanVien ThemNV = new NhanVien()
                {
                    MaNhanVien = maNhanVien,
                    TenNhanVien = txtTenNhanVien.Text,
                    Email = txtEmail.Text,
                    SDT = txtSDTNhanVien.Text,
                    DiaChi = txtDiaChi.Text,
                    MaVaiTro = maVaiTro, // Gán mã vai trò từ ComboBox
                    NgaySinh = dttpNgaySinhNhanVien.Value,
                    GioiTinh = cbbGioiTinhNhanVien.Text,
                    NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value
                };

                try
                {
                    QLNV.NhanViens.InsertOnSubmit(ThemNV);
                    QLNV.SubmitChanges();
                    MessageBox.Show("Thêm nhân viên thành công");
                    HienThiNhanVien(); // Cập nhật lại danh sách nhân viên
                    ClearTextBox(); // Xóa trắng các control nhập liệu sau khi thêm thành công
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private string GenerateMaNhanVien()
            {
                string maNhanVien = "";
                using (var dd = new LionQuanLyQuanCaPheDataContext())
                {
                    var latestEmployee = dd.NhanViens.OrderByDescending(nv => nv.MaNhanVien).FirstOrDefault();

                    if (latestEmployee == null)
                    {
                        maNhanVien = "NV001";
                    }
                    else
                    {
                        string currentMaNhanVien = latestEmployee.MaNhanVien;
                        int currentIndex = int.Parse(currentMaNhanVien.Substring(2)) + 1;
                        maNhanVien = "NV" + currentIndex.ToString("D3");
                    }
                }

                return maNhanVien;
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

            private bool IsValidPhoneNumber(string phone)
            {
                return Regex.Match(phone, @"^\d{10}$").Success;
            }

       

        
        



















        private void SuaNhanVien()
        {
            if (string.IsNullOrEmpty(txtTenNhanVien.Text) || string.IsNullOrEmpty(txtDiaChi.Text) ||
                string.IsNullOrEmpty(txtMaNhanVien.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
               
                string.IsNullOrEmpty(txtSDTNhanVien.Text))
            {
                MessageBox.Show("Không được bỏ trống các trường!");
                return;
            }



            try
            {
                var QLNV = new LionQuanLyQuanCaPheDataContext();

                string maNV = txtMaNhanVien.Text;
                var nhanVien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == maNV);
                if (nhanVien == null)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại!");
                    return;
                }

                string manv = txtMaNhanVien.Text;
                var nhanvien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == manv);
                if (nhanVien != null)
                {
                    nhanVien.TenNhanVien = txtTenNhanVien.Text;
                    nhanVien.Email = txtEmail.Text;
                    nhanVien.SDT = txtSDTNhanVien.Text;
                    nhanVien.DiaChi = txtDiaChi.Text;
                    nhanVien.MaVaiTro = cbbVaiTroCuaNhanVien.Text;
                    nhanVien.NgaySinh = dttpNgaySinhNhanVien.Value;
                    nhanVien.GioiTinh = cbbGioiTinhNhanVien.Text;
                    nhanVien.NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value;
                    QLNV.SubmitChanges();
                    HienThiNhanVien();
                    MessageBox.Show("Đã cập nhật thông tin nhân viên thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên có mã số này!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
            }
        }
        private void XoaNhanVien()
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var QLNV = new LionQuanLyQuanCaPheDataContext();
                    string maNV = txtMaNhanVien.Text;
                    var nhanvien = QLNV.NhanViens.FirstOrDefault(k => k.MaNhanVien == maNV);
                    if (nhanvien != null)
                    {
                        QLNV.NhanViens.DeleteOnSubmit(nhanvien);
                        QLNV.SubmitChanges();
                        HienThiNhanVien();
                        MessageBox.Show("Đã xóa nhân viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên có mã số này!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên này: " + ex.Message);
            }
        }
        private void ClearTextBox()
        {
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtTenNhanVien.Text = "";
            txtMaNhanVien.Text = "";
            dttpNgayBatDauLamCuaNhanVien.Text = "";
            dttpNgaySinhNhanVien.Text = "";
            txtMaVaiTro.Text = "";
            cbbGioiTinhNhanVien.Text = "";
            txtSDTNhanVien.Text = "";
            txtMaVaiTro.Text = "";
            txtTenVaiTro.Text = "";


        }
        private void cbbVaiTroCuaNhanVien_DropDown(object sender, EventArgs e)
        {
            var QLNV = new LionQuanLyQuanCaPheDataContext();
            var vaiTros = QLNV.VaiTros.ToList(); 

            cbbVaiTroCuaNhanVien.DisplayMember = "TenVaiTro";
            cbbVaiTroCuaNhanVien.ValueMember = "MaVaiTro"; 

            cbbVaiTroCuaNhanVien.DataSource = vaiTros; // Thiết lập nguồn dữ liệu
        }


        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {
            ThemVaiTro();
            ClearTextBox();
        }

        private void btnsuaVaiTro_Click(object sender, EventArgs e)
        {
            SuaVaiTro();
            ClearTextBox();
        }

        private void btnxoaVaiTro_Click(object sender, EventArgs e)
        {
            XoaVaiTro();
            ClearTextBox();
        }
        private void ThemVaiTro()
        {
            if (string.IsNullOrEmpty(txtTenVaiTro.Text) ||

        string.IsNullOrEmpty(txtTenVaiTro.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                // Tạo mã nhân viên tự động ví dụ NV01, NV02, ...
                string maVaiTro = GenerateMavaitro();

                var dd = new LionQuanLyQuanCaPheDataContext();
                using (dd)
                {
                    VaiTro newEmployee = new VaiTro()
                    {
                        MaVaiTro = txtMaVaiTro.Text,
                        TenVaiTro = txtTenVaiTro.Text

                    };


                    dd.VaiTros.InsertOnSubmit(newEmployee);
                    try
                    {
                        dd.SubmitChanges();
                        MessageBox.Show("Thêm thành công");
                        HienThiNhanVien();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

     
        private void SuaVaiTro()
        {
            if (string.IsNullOrEmpty(txtMaVaiTro.Text) ||
               string.IsNullOrEmpty(txtTenVaiTro.Text))
            {
                MessageBox.Show("Không được bỏ trống các trường!");
                return;
            }



            try
            {
                var QLNV = new LionQuanLyQuanCaPheDataContext();

                string mavt = txtMaVaiTro.Text;
                var vaitro = QLNV.VaiTros.FirstOrDefault(vt => vt.MaVaiTro == mavt);
                if (vaitro != null)
                {

                    vaitro.TenVaiTro = txtTenVaiTro.Text;

                    ;
                    QLNV.SubmitChanges();
                    HienThiNhanVien();
                    MessageBox.Show("Đã cập nhật thông tin vai trò thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vai trò có mã số này!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin vai trò: " + ex.Message);
            }
        }

        private void XoaVaiTro()
        {

            try
            {
                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa vai trò hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var Vaitro = new LionQuanLyQuanCaPheDataContext();
                    string MaVt = txtMaVaiTro.Text;
                    var vaiTRO = Vaitro.VaiTros.FirstOrDefault(k => k.MaVaiTro == MaVt);
                    if (vaiTRO != null)

                        Vaitro.VaiTros.DeleteOnSubmit(vaiTRO);
                    Vaitro.SubmitChanges();
                    HienThiNhanVien();
                    MessageBox.Show("Đã xóa Vai trò thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vai trò có mã số này!");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa vai trò: " + ex.Message);
            }
        }
        private string GenerateMavaitro()
        {
            string maVaiTro = "";
            using (var dd = new LionQuanLyQuanCaPheDataContext())
            {
                // Lấy mã nhân viên cao nhất hiện có
                var latestEmployee = dd.VaiTros.OrderByDescending(vt => vt.MaVaiTro).FirstOrDefault();

                if (latestEmployee == null)
                {
                    // Nếu chưa có nhân viên nào, bắt đầu từ NV01
                    maVaiTro = "VT001";
                }
                else
                {
                    // Lấy số thứ tự từ mã nhân viên hiện tại và tăng lên 1
                    string currentMaVaiTro = latestEmployee.MaVaiTro;
                    int currentIndex = int.Parse(currentMaVaiTro.Substring(2));
                    currentIndex++;

                    // Tạo mã mới
                    maVaiTro = "VT" + currentIndex.ToString("D2"); // Định dạng số với 2 chữ số
                }
            }

            return maVaiTro;
        }




        private void btntimkiemVaiTro_Click(object sender, EventArgs e)
        {
            var QLBH = new LionQuanLyQuanCaPheDataContext();
            using (QLBH)
            {
                string maNhanVienQT = txttimkiemVaiTro.Text; // Lấy giá trị tìm kiếm từ TextBox txttimkiem
                var QuanTri = QLBH.VaiTros.FirstOrDefault(nv => nv.MaVaiTro == maNhanVienQT);

                if (QuanTri != null)
                {
                    // Hiển thị thông tin  tìm được trong các TextBox
                    txtMaVaiTro.Text = QuanTri.MaVaiTro;
                    txtTenVaiTro.Text = QuanTri.TenVaiTro;




                }
                else
                {
                    // Nếu không tìm thấy , xóa dữ liệu từ các TextBox và hiển thị thông báo

                    MessageBox.Show("Không tìm thấy nhân viên với mã này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void dtgvVaiTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo người dùng đã chọn một dòng hợp lệ
            {
                DataGridViewRow row = dtgvVaiTro.Rows[e.RowIndex];

                // Lấy giá trị của các ô trong dòng được chọn từ cột MaVaiTro và TenVaiTro
                string maVaiTro = row.Cells["MaVaiTro"].Value.ToString();
                string tenVaiTro = row.Cells["TenVaiTro"].Value.ToString();

                // Hiển thị thông tin lên các control như TextBox, Label, hoặc ComboBox
                txtMaVaiTro.Text = maVaiTro;
                txtTenVaiTro.Text = tenVaiTro;
            }
        }

    }


}



