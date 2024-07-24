using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public FormChucNangQuanLy(string VaiTro)
        {
            InitializeComponent();
            UserRole = VaiTro;
            SetupUI();

        }

        private void SetupUI()
        {
            if (UserRole == "admin")
            {

            }
            else if (UserRole == "Quản lý")
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
            else if (UserRole == "Nhân viên bán hàng")
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

        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();

        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            hienThiKhachHang();
            HienThiNhanVien();


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
            timKiemNhanVien();
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


        private void timKiemNhanVien()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string maNhanVien = txtTimKiemNhanVien.Text.Trim(); // Lấy mã nhân viên từ textbox tìm kiếm

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
        private void txtMaSanPham_TextChanged(object sender, EventArgs e)
        {

        }

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

        //  THEM KHÁCH HÀNG
        private int maKh = 01;
        private void themKhachHang()
        {
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

                    TenKhachHang = txtTenKhachHang.Text,
                    DiaChi = txtDiaChiKhachHang.Text,
                    SDT = txtSDTKhachHang.Text,
                    NgaySinh = dttpNgaySinhKhachHang.Value,
                    Email = txtEmailKhachHang.Text


                };
                QLKH.KhachHangs.InsertOnSubmit(Themkh);
                Themkh.MaKhachHang = "KH" + maKh.ToString("D3");
                maKh +=1;

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




    }


}



