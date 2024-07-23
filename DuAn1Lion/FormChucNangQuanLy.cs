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

            if(tclFormChucNang.SelectedTab == tpVaiTro)
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
        private void HienThiNhanVien()
        {

            var QLNV = new LionQuanLyQuanCaPheDataContext();

            var List = from nv in QLNV.NhanViens
                       from vt in QLNV.VaiTros
                       where nv.MaVaiTro == vt.MaVaiTro
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
                           vt.MaVaiTro

                       };

            dtgvThongTinNhanVien.DataSource = List.ToList();
        }

        //hienthivaitro
        private void HienThioVaiTro()
        {

            var Vt = new LionQuanLyQuanCaPheDataContext();

            var List = from nv in Vt.VaiTros
                       from vt in Vt.VaiTros
                       where nv.MaVaiTro == vt.MaVaiTro
                       select new
                       {
                           nv.MaVaiTro,
                           nv.TenVaiTro
                       };

            dtgvVaiTro.DataSource = List.ToList();
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
            ThemNhanVieṇ();
            ClearTextBox();

        }

        private string GenerateMaNhanVien()
        {
            string maNhanVien = "";
            using (var dd = new LionQuanLyQuanCaPheDataContext())
            {
                // Lấy mã nhân viên cao nhất hiện có
                var latestEmployee = dd.NhanViens.OrderByDescending(nv => nv.MaNhanVien).FirstOrDefault();

                if (latestEmployee == null)
                {
                    // Nếu chưa có nhân viên nào, bắt đầu từ NV01
                    maNhanVien = "NV001";
                }
                else
                {
                    // Lấy số thứ tự từ mã nhân viên hiện tại và tăng lên 1
                    string currentMaNhanVien = latestEmployee.MaNhanVien;
                    int currentIndex = int.Parse(currentMaNhanVien.Substring(2));
                    currentIndex++;

                    // Tạo mã mới
                    maNhanVien = "NV" + currentIndex.ToString("D3"); // Định dạng số với 2 chữ số
                }
            }

            return maNhanVien;
        }

        private void dtgvThongTinVaiTro_Enter(object sender, EventArgs e)
        {

        }

        private int maNV = 002;
        private void ThemNhanVieṇ()
        {
            if (string.IsNullOrEmpty(txtTenNhanVien.Text) || string.IsNullOrEmpty(txtSDTNhanVien.Text) ||
         string.IsNullOrEmpty(txtDiaChi.Text) || string.IsNullOrEmpty(cbbGioiTinhNhanVien.Text) ||
         string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(dttpNgaySinhNhanVien.Text) || string.IsNullOrEmpty(dttpNgayBatDauLamCuaNhanVien.Text
      ))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                var QLNV = new LionQuanLyQuanCaPheDataContext();
                using (QLNV)
                {
                    NhanVien ThemNV = new NhanVien()
                    {

                    TenNhanVien = txtTenNhanVien.Text,
                    Email = txtEmail.Text,
                    SDT = txtSDTNhanVien.Text,
                    DiaChi = txtDiaChi.Text,
                    MaVaiTro = cbbVaiTroCuaNhanVien.Text,
                    NgaySinh = dttpNgaySinhNhanVien.Value,
                    GioiTinh = cbbGioiTinhNhanVien.Text,
                    NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value,
                };

                    try
                    {
                        QLNV.NhanViens.InsertOnSubmit(ThemNV);
                        ThemNV.MaNhanVien = "NV" + maNV.ToString("D3");
                        maNV++;
                        QLNV.SubmitChanges();
                        MessageBox.Show("Thêm thành công");
                        HienThiNhanVien();
                        ClearTextBox();

                    }
                    catch ( Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm  thông tin nhân viên: " + ex.Message);
                    }
                }
            }


        }
        private void SuaNhanVien()
        {
            if (string.IsNullOrEmpty(txtTenNhanVien.Text) || string.IsNullOrEmpty(txtDiaChi.Text) ||
                string.IsNullOrEmpty(txtMaNhanVien.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(cbbVaiTroCuaNhanVien.Text) ||
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

     



        private void LoadcbbVaiTroCuaNhanVien()
        {
            try
            {
                using (var context = new LionQuanLyQuanCaPheDataContext())
                {
                    // Select all MaVaiTro from VaiTros table
                    var vaiTros = context.VaiTros.Select(vt => vt.MaVaiTro).ToList();

                    // Bind the list to the ComboBox
                    cbbVaiTroCuaNhanVien.DataSource = vaiTros;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }


}



