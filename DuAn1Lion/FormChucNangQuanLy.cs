using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormChucNangQuanLy : Form
    {

        private string UserRole;

        public FormChucNangQuanLy(string vaiTro)
        {

            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            // UserRole = VaiTro;
            SetupUI();

        }

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



        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
          
            HienThiNhanVien();
            HienThioVaiTro();
            hienThiKhachHang();
            hienThiSanPham();
            HienThiNhanVien();
            HienThiThongKeKhachHang();
            HienThiThongKeHoaDon();
            
            anMaKH();
            SetupUI();

        }
        //ẨN TEXTBOX MÃ KHÁCH HÀNG KHÔNG ĐƯỢC NHẬP
        private void anMaKH()
        {
            txtMaKhachHang.ReadOnly = true;
            txtMaKhachHang.TabStop = false;
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
                HienThiThongKeKhachHang();
                HienThiThongKeHoaDon();
            }

            if (tclFormChucNang.SelectedTab == tpOrder)
            {
                flowLayoutPanelOrder.Controls.Clear();
                hienThiOrder();
            }
            if (tclFormChucNang.SelectedTab == tpSanPham)
            {
                hienThiSanPham();
            }
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

        private void btnTimKiemNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemNhanVien();
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



        private int maNV = 001;
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
                        MaVaiTro = cbbVaiTroCuaNhanVien.Text.Trim(),
                        NgaySinh = dttpNgaySinhNhanVien.Value,
                        GioiTinh = cbbGioiTinhNhanVien.Text,
                        NgayBatDauLamViec = dttpNgayBatDauLamCuaNhanVien.Value,
                        MatKhau = randomMatKhau()
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
                    catch (Exception ex)
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
                               TenVaiTro = vt.TenVaiTro // Include the name of the role
                           };

                dtgvThongTinNhanVien.DataSource = list.ToList();
            }
        }



        private string randomMatKhau()
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













        //PHAN CUA QUOC ANH


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

        //TIM KIEM THONG TIN THONG KE
        private void btnTimKiemThongKeKhachHang_Click(object sender, EventArgs e)
        {
            TimKiemThongKeKhachHang();
        }

        private void btnTìmKiemThongKeHoaDon_Click(object sender, EventArgs e)
        {
            TimKiemThongKeHoaDon();
        }





        //LAM MỚI KHÁCH HÀNG
        private void lamMoiKhachHang()
        {

            txtMaKhachHang.Clear();
            txtTenKhachHang.Clear();
            txtDiaChiKhachHang.Clear();
            txtSDTKhachHang.Clear();
            txtEmailKhachHang.Clear();
            txtTenKhachHang.Focus();
        }

        // HIEN THI KHACH HANG
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

        private int maKh = 1;

        // LÀM MỚI MÃ NHÂN VIÊN HIỆN TẠI
        string maNhanVien = FormDangNhap.MaNhanVienHienTai;

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

            // Kiểm tra tên khách hàng (chấp nhận dấu)
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
         

            if (ngaySinh > ngayHienTai || ngaySinh == ngayHienTai )
            {
                MessageBox.Show("Ngày sinh không được lớn hơn hoặc bàng ngày hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Themkh.MaKhachHang = "KH" + maKh.ToString("D3");
                maKh++;

                QLKH.KhachHangs.InsertOnSubmit(Themkh);
                QLKH.SubmitChanges();

                MessageBox.Show("Thêm thành công");

                hienThiKhachHang();
                lamMoiKhachHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }





        //  SUA KHÁCH HÀNG
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


        //  XOA KHÁCH HÀNG
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

                dtgvThongTinKhachHang.DataSource = timKiem.ToList();
            }
        }



        // HIEN THI LEN TEXTBOX KHACH HANG
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

        // HIEN THI THONG KE KHACH HANG
        private void HienThiThongKeKhachHang()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var thongKeKhach = QLKH.ThongKeKhachHang().ToList();
            dtgvThongKeKhachHang.DataSource = thongKeKhach;
        }



        //TIM KIEM THONG KE KHACH HANG
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


        // HIEN THI THONG KE HOA DON
        private void HienThiThongKeHoaDon()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var thongKeHoaDon = QLKH.ThongKeHoaDon().ToList();
            dtgvThongKeHoaDon.DataSource = thongKeHoaDon;
        }

        //TIM KIEM THONG KE HOA DON 
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

        private void FormChucNangQuanLy_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
            this.Hide();
        }

     
        //  Sản phẩm


        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            themSanPham();
        }

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {

        }

        private void dtgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string imagePath = "";
        private void btnAnhSanPham_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                pic_AnhSanPham.Image = Image.FromFile(imagePath);
            }
        }

        // HIEN THI SẢN PHẨM
        private void hienThiSanPham()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();

            var list = from sp in QLKH.SanPhams
                       where sp.MaSanPham == sp.MaSanPham
                       select new
                       {
                           sp.MaSanPham,
                           sp.TenSanPham,
                           sp.GiaBan,
                           sp.GiaNhap,
                           sp.HinhAnh
                       };



            dtgvSanPham.DataSource = list.ToList();

        }
        //  THEM SẢN PHẨM
        private int maSP = 01;
        private void themSanPham()
        {


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

            try
            {
                var QLKH = new LionQuanLyQuanCaPheDataContext();
                SanPham Themsp = new SanPham()
                {


                    TenSanPham = txtTenSanPham.Text,
                    GiaBan = int.Parse(txtGiaBan.Text),
                    GiaNhap = int.Parse(txtGiaNhap.Text),
                    HinhAnh = imgData != null ? new System.Data.Linq.Binary(imgData) : null



                };
                QLKH.SanPhams.InsertOnSubmit(Themsp);
                Themsp.MaSanPham = "SP" + maSP.ToString("D3");
                maSP ++;

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


        // ORDER
        private List<OrderItem> orderList = new List<OrderItem>();

        public class OrderItem
        {
            public string ProductID { get; set; } // Sử dụng string thay vì int
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
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
            // Sử dụng giá trị thực của MaSanPham để kiểm tra sự tồn tại của sản phẩm trong danh sách đơn hàng
            var orderItem = orderList.FirstOrDefault(o => o.ProductID == sanPham.MaSanPham);
            if (orderItem != null)
            {
                // Sản phẩm đã tồn tại, tăng số lượng
                orderItem.Quantity++;
            }
            else
            {
                // Sản phẩm chưa tồn tại, thêm sản phẩm mới vào danh sách đơn hàng
                orderList.Add(new OrderItem
                {
                    ProductID = sanPham.MaSanPham,
                    ProductName = sanPham.TenSanPham,
                    Quantity = 1, // Đặt số lượng ban đầu là 1
                    Price = sanPham.GiaBan.Value
                });
            }

            // Cập nhật giao diện đơn hàng
            UpdateOrderUI();
        }


        private void UpdateOrderUI()
        {
            // Xóa các điều khiển cũ trước khi thêm điều khiển mới
            flowLayoutPanelOrder.Controls.Clear();

            foreach (var item in orderList)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanelOrder.Width - 25,
                    Height = 60, // Giảm chiều cao của panel để điều khiển gọn hơn
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblProductName = new Label
                {
                    Text = item.ProductName,
                    Width = 150, // Giảm chiều rộng của Label để gần các điều khiển khác
                    Height = 30,
                    Location = new Point(10, 15), // Gần hơn với các điều khiển khác
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };

                TextBox txtProductPrice = new TextBox
                {
                    Text = String.Format("{0:N0}", item.Price),
                    Width = 80,
                    Height = 30,
                    ReadOnly = true,
                    Location = new Point(170, 15), // Gần Label tên sản phẩm hơn
                    TextAlign = HorizontalAlignment.Right
                };

                Button btnMinus = new Button
                {
                    Text = "-",
                    Width = 25,
                    Height = 25,
                    Location = new Point(260, 15), // Điều chỉnh để gần các điều khiển khác
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

                // Sự kiện để chỉ cho phép nhập số và không cho phép nhập số nhỏ hơn 0
                txtQuantity.KeyPress += (s, e) =>
                {
                    // Chỉ cho phép nhập số và điều khiển xóa, backspace
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

                        }
                    }
                };

                Button btnPlus = new Button
                {
                    Text = "+",
                    Width = 25,
                    Height = 25,
                    Location = new Point(335, 15), // Gần hơn với các điều khiển khác
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

                };
                btnMinus.Click += (s, e) =>
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        txtQuantity.Text = item.Quantity.ToString();

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
        }

        private void btnTimKiemMenu_Click_1(object sender, EventArgs e)
        {
            string keyword = txtTimKiemMenu.Text;
            hienThiOrder(keyword);
        }

        private void txtTimKiemMenu_Click_1(object sender, EventArgs e)
        {
            hienThiOrder();
        }

        private void buttonHuyOrder()
        {
            flowLayoutPanelOrder.Controls.Clear();
            orderList.Clear();
        }

        //Button hủy ORDER

    

        private void btnHuy_Click(object sender, EventArgs e)
        {
            buttonHuyOrder();
        }
    }


}



