using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1Lion
{
    public partial class FormChucNangQuanLy : Form
    {

        private void LoadData()
        {
            HienThiNhanVien();
            hienThiSan_Pham();
            Hien_Thi_Nguyen_Lieu();
            hienThi_ThongKe_SanPham();
            Hien_Thi_Thong_Ke_Nguyen_Lieu();
            Hien_Thi_Gia_Text_Box();
            //txtMaNhanVien.Text = FormDangNhap.Lay_Ma_Nhan_Vien;
        }
        private void Load_An_Text_Box()
        {
            txtMaNguyenLieu.ReadOnly = true;
            txtMaNguyenLieu.TabStop = false;
            txtMaNhanVien.ReadOnly = true;
            txtMaNhanVien.TabStop = false;
        }
        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
            Load_An_Text_Box();
            string maNhanVien = FormDangNhap.MaNhanVienHienTai;
            string randomPassword = RandomPassword(8);
            this.Load += new System.EventHandler(this.FormChucNangQuanLy_Load);
            //dtgvThongKeNguyenLieu.EditingControlShowing += dtgvThongKeNguyenLieu_EditingControlShowing;
            Hien_Thi_Gia_Text_Box();
        }

        private void tpSanPham_Click(object sender, EventArgs e)
        {

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
            txtTimKiem_ThongKeSanPham.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
            txtTimKiem_ThongKeNguyenLieu.KeyPress += new KeyPressEventHandler(Chan_KiTuDacBiet_KeyPress);
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
        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            Hien_Thi_Gia_Text_Box();
            hienThi_ThongKe_SanPham();
            cbbVaiTroCuaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbVaiTroCuaNhanVien.DropDown += new EventHandler(cbbVaiTroCuaNhanVien_DropDown);
            LoadVaiTro();
            So_Luong_Ban_Ra();
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
        private void grbThongTinSanPham_Enter(object sender, EventArgs e)
        {

        }
        private bool IsMaNhanVienValid(string maNhanVien)
        {
            using (var sp = new LionQuanLyQuanCaPheDataContext())
            {
                return sp.NhanViens.Any(nv => nv.MaNhanVien == maNhanVien);
            }
        }

        //Thêm, Sửa, Xóa, Tìm Kiếm, Thống Kê, DTGV Sản Phẩm
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
                MessageBox.Show("Lỗi! Tên Sản Phẩm Chỉ Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (giaNhap > 70)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Quá 70", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (giaBan > 100)
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
        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            Them_San_Pham();
        }


        //Hàm Sửa Sản Phẩm
        private void ChanSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }



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

            if (donGiaNhap > 70)
            {
                MessageBox.Show("Lỗi! Giá Nhập Không Được Quá 70", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (donGiaBan > 100)
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




        //Hàm Xóa Sản Phẩm
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

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {
            Sua_San_Pham();
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            Xoa_San_Pham();
        }

        //Hàm Tìm Kiếm Sản Phẩm
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
                       join nv in list_Tim_Kiem_Sp.NhanViens on sp.MaNhanVien equals nv.MaNhanVien
                       where sp.MaSanPham.ToLower().Contains(tuKhoa_SP) ||
                             sp.TenSanPham.ToLower().Contains(tuKhoa_SP) ||
                             sp.GiaBan.ToString().Contains(tuKhoa_SP)
                       select new
                       {
                           sp.MaSanPham,
                           sp.TenSanPham,
                           sp.GiaNhap,
                           sp.GiaBan,
                           nv.TenNhanVien,
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
                if (dtgvSanPham.Columns.Contains("AnhSanPham"))
                {
                    dtgvSanPham.Columns.Remove("AnhSanPham");
                }
                hienThiSan_Pham();

            }
        }
        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
        {
            TimKiem_SanPham();
        }
        private void hienThiSan_Pham()
        {
            var list_SP = new LionQuanLyQuanCaPheDataContext();

            var List_SP = from Sp in list_SP.SanPhams
                          join nv in list_SP.NhanViens on Sp.MaNhanVien equals nv.MaNhanVien
                          select new
                          {
                              Sp.MaSanPham,
                              Sp.TenSanPham,
                              Sp.GiaNhap,
                              Sp.GiaBan,
                              Sp.HinhAnh,
                              nv.TenNhanVien
                          };

            var resultList = List_SP.ToList();
            dtgvSanPham.DataSource = resultList;

            // Định dạng cột Giá bán và Giá nhập
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

            // Thêm cột Ảnh sản phẩm nếu chưa có
            if (!dtgvSanPham.Columns.Contains("AnhSanPham"))
            {
                DataGridViewImageColumn Column = new DataGridViewImageColumn();
                Column.Name = "AnhSanPham";
                Column.HeaderText = "Ảnh Sản Phẩm";
                Column.Width = 100;
                Column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dtgvSanPham.Columns.Add(Column);
            }

            // Sắp xếp thứ tự hiển thị cột
            dtgvSanPham.Columns["MaSanPham"].DisplayIndex = 0;
            dtgvSanPham.Columns["TenSanPham"].DisplayIndex = 1;
            dtgvSanPham.Columns["GiaNhap"].DisplayIndex = 2;
            dtgvSanPham.Columns["GiaBan"].DisplayIndex = 3;
            dtgvSanPham.Columns["AnhSanPham"].DisplayIndex = 4;
            dtgvSanPham.Columns["TenNhanVien"].DisplayIndex = 5;

            // Hiển thị ảnh sản phẩm
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

            // Ẩn cột "HinhAnh" gốc
            if (dtgvSanPham.Columns["HinhAnh"] != null)
            {
                dtgvSanPham.Columns["HinhAnh"].Visible = false;
            }
        }

        private string giaBanMacDinh = ".000";

        private string giaNhapMacDinh = ".000";

        private string giaNhapMacDinh_NL = ".000";
        private void dtgvSanPham_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dtgvThongTinNguyenLieu_SelectionChanged(object sender, EventArgs e)
        {

        }
        public class TabState
        {
            public string GiaBan { get; set; }
            public string GiaNhap { get; set; }
            public string GiaNhapNL { get; set; }
        }

        private TabState currentState = new TabState();


        /*
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
        */

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
                if (e.KeyChar == (char)Keys.Back && textBox.SelectionStart >= dotIndex + 4)
                {
                    e.Handled = true;
                }
            }
        }

        private void TxtGia_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

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
            else
            {
                textBox.Text = ".000";
                textBox.SelectionStart = 0;
            }
        }

        private void TxtGia_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text == ".000")
            {
                textBox.Text = ".000";
            }
        }

        private void dtgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvSanPham.Rows[e.RowIndex];

                //txtMaSanPham.Text = row.Cells["MaSanPham"].Value.ToString();
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





        private void dtgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dtgvSanPham.Columns.Contains("AnhSanPham") &&
                    e.ColumnIndex == dtgvSanPham.Columns["AnhSanPham"].Index)
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
                                e.FormattingApplied = true;
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
            }
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


        private string imagePath = "";
        private void Chon_Anh_San_Pham()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                pic_AnhSanPham.Image = Image.FromFile(imagePath);
            }

        }
        private void btnAnhSanPham_Click(object sender, EventArgs e)
        {
            Chon_Anh_San_Pham();
        }
        /*
        private void LoadImage(string ma_san_pham)
        {
            var QLBH = new LionQuanLyQuanCaPheDataContext();

            var item = QLBH.SanPhams.FirstOrDefault(s => s.MaSanPham == ma_san_pham);

            if (item != null)
            {
                byte[] imageData = item.HinhAnh.ToArray();

                if (imageData != null && imageData.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pic_AnhSanPham.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Không thể tải hình ảnh: " + ex.Message);
                    }
                }
            }
            else
            {
                pic_AnhSanPham.Image = null;
            }
        }
        */

        //Hàm Khai Báo DTGV Thống Kê Sản Phẩm
        private void ThongKe_SanPham_DataBindingComplete()
        {
            foreach (DataGridViewRow row in dtgvSanPham.Rows)
            {
                var cellValue = row.Cells["HinhAnh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    byte[] DataImg = ((System.Data.Linq.Binary)cellValue).ToArray();
                    using (var ms = new MemoryStream(DataImg))
                    {
                        var image = Image.FromStream(ms);
                        row.Cells["HinhAnh"].Value = image;
                    }
                }
                else
                {
                    row.Cells["AnhSanPham"].Value = null;
                }
            }
        }
        private void dtgvThongKeSanPham_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //ThongKe_SanPham_DataBindingComplete();
        }

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
            if (dtgvThongKeSanPham.Columns.Contains("TongGiaBanRaTuan"))
            {
                dtgvThongKeSanPham.Columns["TongGiaBanRaTuan"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["TongGiaBanRaTuan"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
            if (dtgvThongKeSanPham.Columns.Contains("TongGiaBanRaThang"))
            {
                dtgvThongKeSanPham.Columns["TongGiaBanRaThang"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["TongGiaBanRaThang"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }if (dtgvThongKeSanPham.Columns.Contains("TongGiaBanRaNam"))
            {
                dtgvThongKeSanPham.Columns["TongGiaBanRaNam"].DefaultCellStyle.Format = "N0";
                dtgvThongKeSanPham.Columns["TongGiaBanRaNam"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            }
        }

        private void TimKiem_ThongKe_SanPham()
        {
            string SanPham = txtTimKiem_ThongKeSanPham.Text.Trim();
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
        private void btnTimKiem_ThongKeSanPham_Click(object sender, EventArgs e)
        {
            TimKiem_ThongKe_SanPham();
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
        private void LamMoi_SP()
        {
            txtTenSanPham.Clear();
            txtGiaBan.Clear();
            txtGiaNhap.Clear();
            pic_AnhSanPham.Image = null;
        }
        private void LamMoi_NV()
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtSDTNhanVien.Clear();
            txtDiaChi.Clear();
            cbbGioiTinhNhanVien.SelectedIndex = -1;
            cbbVaiTroCuaNhanVien.SelectedIndex = -1;
            txtEmail.Clear();
            dtNgaySinhNhanVien.Value = DateTime.Now;
            dtNgayBatDauLamCuaNhanVien.Value = DateTime.Now;
            txtTenVaiTro.Clear();
        }

        private void grbChucNangSanPham_Enter(object sender, EventArgs e)
        {

        }

        private void tpKhachHang_Click(object sender, EventArgs e)
        {

        }

        private void tclFormChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentState.GiaBan = txtGiaBan.Text;
            currentState.GiaNhap = txtGiaNhap.Text;
            currentState.GiaNhapNL = txtGiaNhapNguyenLieu.Text;

            if (tclFormChucNang.SelectedTab == tpSanPham)
            {
                hienThiSan_Pham();
                if (tclFormChucNang.SelectedTab == tpSanPham)
                {
                    txtGiaBan.Text = giaBanMacDinh;
                    txtGiaNhap.Text = giaNhapMacDinh;
                }
                else
                {
                    txtGiaBan.Text = currentState.GiaBan;
                    txtGiaNhap.Text = currentState.GiaNhap;
                }

            }
            if (tclFormChucNang.SelectedTab == tpNhanVien)
            {
                LamMoi_SP();
                LamMoi_NV();
                HienThiNhanVien();
                LamMoi_NguyenLieu();
            }
            if (tclFormChucNang.SelectedTab == tpKhachHang)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();
                LamMoi_NV();

            }

            if (tclFormChucNang.SelectedTab == tpNguyenLieu)
            {
                Hien_Thi_Nguyen_Lieu();

                if (tclFormChucNang.SelectedTab == tpNguyenLieu)
                {
                    txtGiaNhapNguyenLieu.Text = giaNhapMacDinh_NL;
                }
                else
                {
                    txtGiaNhapNguyenLieu.Text = currentState.GiaNhapNL;
                }
                LamMoi_SP();
                //LamMoi_NguyenLieu();
                LamMoi_NV();

            }
            if (tclFormChucNang.SelectedTab == tpOrder)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();
                LamMoi_NV();

            }
            if (tclFormChucNang.SelectedTab == tpThongKe)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();
                LamMoi_NV();

            }
            if (tclFormChucNang.SelectedTab == tpVaiTro)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();
                LamMoi_NV();

            }
        }

        //HIỂN THỊ NHÂN VIÊN
        private void HienThiNhanVien()
        {
            var QLNV = new LionQuanLyQuanCaPheDataContext();

            var List = from nv in QLNV.NhanViens
                       join vt in QLNV.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                       select new
                       {
                           nv.MaNhanVien,
                           nv.TenNhanVien,
                           nv.SDT,
                           nv.Email,
                           nv.DiaChi,
                           nv.MaVaiTro,
                           TenVaiTro = vt.TenVaiTro,
                           MatKhau = new string('*', nv.MatKhau.Length),
                           nv.NgaySinh,
                           nv.GioiTinh,
                           nv.NgayBatDauLamViec
                       };

            dtgvThongTinNhanVien.DataSource = List.ToList();
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


        private int CalculateDropDownWidth(ComboBox comboBox)
        {
            int maxWidth = 0;
            foreach (var item in comboBox.Items)
            {
                int itemWidth = TextRenderer.MeasureText(item.ToString(), comboBox.Font).Width;
                if (itemWidth > maxWidth)
                {
                    maxWidth = itemWidth;
                }
            }
            return maxWidth;
        }
        private void dtgvThongTinNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvThongTinNhanVien.Rows.Count)
            {
                DataGridViewRow row = dtgvThongTinNhanVien.Rows[e.RowIndex];

                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value != DBNull.Value ? row.Cells["MaNhanVien"].Value.ToString() : string.Empty;
                txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value != DBNull.Value ? row.Cells["TenNhanVien"].Value.ToString() : string.Empty;
                txtSDTNhanVien.Text = row.Cells["SDT"].Value != DBNull.Value ? row.Cells["SDT"].Value.ToString() : string.Empty;
                txtEmail.Text = row.Cells["Email"].Value != DBNull.Value ? row.Cells["Email"].Value.ToString() : string.Empty;
                txtDiaChi.Text = row.Cells["DiaChi"].Value != DBNull.Value ? row.Cells["DiaChi"].Value.ToString() : string.Empty;
                txtMaVaiTro.Text = row.Cells["MaVaiTro"].Value != DBNull.Value ? row.Cells["MaVaiTro"].Value.ToString() : string.Empty;
                dtNgaySinhNhanVien.Text = row.Cells["NgaySinh"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["NgaySinh"].Value).ToString("yyyy-MM-dd") : string.Empty;
                cbbGioiTinhNhanVien.Text = row.Cells["GioiTinh"].Value != DBNull.Value ? row.Cells["GioiTinh"].Value.ToString() : string.Empty;
                dtNgayBatDauLamCuaNhanVien.Text = row.Cells["NgayBatDauLamViec"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["NgayBatDauLamViec"].Value).ToString("yyyy-MM-dd") : string.Empty;

                string tenVaiTro = row.Cells["TenVaiTro"].Value != DBNull.Value ? row.Cells["TenVaiTro"].Value.ToString() : string.Empty;
                cbbVaiTroCuaNhanVien.SelectedIndex = cbbVaiTroCuaNhanVien.FindStringExact(tenVaiTro);
            }
            else
            {
                MessageBox.Show("Dữ liệu không đầy đủ hoặc cột không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Thêm, Sửa, Xóa Nhân Viên
        /*
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
                    int currentIndex = int.Parse(currentMaNhanVien.Substring(2));
                    currentIndex++;

                    maNhanVien = "NV" + currentIndex.ToString("D3"); 
                }
            }

            return maNhanVien;
        }
        */
        private string RandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder password = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                password.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return password.ToString();
        }
        private void dtgvThongTinVaiTro_Enter(object sender, EventArgs e)
        {

        }
        private void LoadVaiTro()
        {
            var QLNV = new LionQuanLyQuanCaPheDataContext();

            var vaiTroList = from vt in QLNV.VaiTros
                             select new { vt.MaVaiTro, vt.TenVaiTro };

            cbbVaiTroCuaNhanVien.DataSource = vaiTroList.ToList();
            cbbVaiTroCuaNhanVien.DisplayMember = "TenVaiTro";
            cbbVaiTroCuaNhanVien.ValueMember = "MaVaiTro";
        }


        private int maNV = 002;
        private void ThemNhanVien()
        {
            if (string.IsNullOrEmpty(txtSDTNhanVien.Text) ||
                string.IsNullOrEmpty(txtDiaChi.Text) || string.IsNullOrEmpty(cbbGioiTinhNhanVien.Text) ||
                string.IsNullOrEmpty(dtNgaySinhNhanVien.Text) ||
                string.IsNullOrEmpty(dtNgayBatDauLamCuaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Lỗi!, Vui Lòng Nhập Địa Chỉ Email Hợp Lệ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(txtTenNhanVien.Text, "^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưƯỨỪỂẾỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừửữựỷỹ\\s]+$"))
            {
                MessageBox.Show("Lỗi!, Tên Nhân Viên Chỉ Được Nhập Chữ Cái Và Dấu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(txtSDTNhanVien.Text, "^[0-9]+$"))
            {
                MessageBox.Show("Lỗi!, Vui Lòng Nhập Số Điện Thoại Chỉ Chứa Số", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var QLNV = new LionQuanLyQuanCaPheDataContext();
            using (QLNV)
            {
                try
                {
                    string randomPassword = RandomPassword(8);
                    var isEmailExist = QLNV.NhanViens.Any(u => u.Email == txtEmail.Text);
                    string maVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString();
                    if (isEmailExist)
                    {
                        MessageBox.Show("Email đã tồn tại! Vui lòng nhập email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    NhanVien ThemNV = new NhanVien()
                    {
                        TenNhanVien = txtTenNhanVien.Text,
                        Email = txtEmail.Text,
                        SDT = txtSDTNhanVien.Text,
                        DiaChi = txtDiaChi.Text,
                        MaVaiTro = maVaiTro,
                        NgaySinh = dtNgaySinhNhanVien.Value,
                        GioiTinh = cbbGioiTinhNhanVien.Text,
                        NgayBatDauLamViec = dtNgayBatDauLamCuaNhanVien.Value,
                        MatKhau = randomPassword
                    };

                    QLNV.NhanViens.InsertOnSubmit(ThemNV);
                    ThemNV.MaNhanVien = "NV" + maNV.ToString("D3");
                    maNV++;
                    QLNV.SubmitChanges();
                    MessageBox.Show("Thêm Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiNhanVien();
                    LamMoi_NV();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm thông tin nhân viên: " + ex.Message);
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


            string maVaiTro = cbbVaiTroCuaNhanVien.SelectedValue.ToString();
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var vaiTro = QLNV.VaiTros.FirstOrDefault(v => v.MaVaiTro == maVaiTro);
                if (vaiTro == null)
                {
                    MessageBox.Show("Mã vai trò không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var nhanVien = QLNV.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == txtMaNhanVien.Text);
                    if (nhanVien != null)
                    {
                        nhanVien.TenNhanVien = txtTenNhanVien.Text;
                        nhanVien.SDT = txtSDTNhanVien.Text;
                        nhanVien.Email = txtEmail.Text;
                        nhanVien.DiaChi = txtDiaChi.Text;
                        nhanVien.MaVaiTro = maVaiTro;
                        nhanVien.NgaySinh = dtNgaySinhNhanVien.Value;
                        nhanVien.GioiTinh = cbbGioiTinhNhanVien.Text;
                        nhanVien.NgayBatDauLamViec = dtNgayBatDauLamCuaNhanVien.Value;

                        QLNV.SubmitChanges();
                        MessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiNhanVien();
                    }
                    else
                    {
                        MessageBox.Show("Nhân viên không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin nhân viên: " + ex.Message);
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

        //HÀM TÌM KIẾM NHÂN VIÊN
        private void TimKiem_NhanVien()
        {
            string tuKhoa_SP = txtTimKiemNhanVien.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa_SP))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var list_Tim_Kiem_Nv = new LionQuanLyQuanCaPheDataContext())
            {
                var List = from nv in list_Tim_Kiem_Nv.NhanViens
                           join vt in list_Tim_Kiem_Nv.VaiTros on nv.MaVaiTro equals vt.MaVaiTro
                           where nv.MaNhanVien.ToLower().Contains(tuKhoa_SP) ||
                                 nv.TenNhanVien.ToLower().Contains(tuKhoa_SP) ||
                                 nv.SDT.ToString().Contains(tuKhoa_SP) ||
                                 nv.DiaChi.ToString().Contains(tuKhoa_SP) ||
                                 nv.GioiTinh.ToString().Contains(tuKhoa_SP) ||
                                 nv.Email.ToString().Contains(tuKhoa_SP) ||
                                 nv.NgaySinh.ToString().Contains(tuKhoa_SP) ||
                                 nv.NgayBatDauLamViec.ToString().Contains(tuKhoa_SP) ||
                                 vt.TenVaiTro.ToLower().Contains(tuKhoa_SP)
                           select new
                           {
                               nv.MaNhanVien,
                               nv.TenNhanVien,
                               nv.SDT,
                               nv.DiaChi,
                               nv.GioiTinh,
                               nv.Email,
                               nv.NgaySinh,
                               nv.NgayBatDauLamViec,
                               nv.MaVaiTro,
                               TenVaiTro = vt.TenVaiTro,
                           };

                if (List.Any())
                {
                    dtgvThongTinNhanVien.DataSource = List.ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgvThongTinNhanVien.DataSource = null;
                }
            }
        }


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

        private void tpVaiTro_Click(object sender, EventArgs e)
        {

        }



        private void tpThongKeSanPham_Click(object sender, EventArgs e)
        {

        }
        //HÀM THÊM NGUYÊN LIỆU
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

            DateTime ngayNhap = dtpNgayNhapNguyenLieu.Value;
            DateTime ngayHetHan = dtpNgayHetHan.Value;

            if (ngayHetHan < ngayNhap || ngayHetHan == ngayNhap)
            {
                MessageBox.Show("Ngày Hết Hạn Phải Sau, Không Cùng Ngày Nhập!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            /*
            else if (ngayHetHan == ngayNhap)
            {
                MessageBox.Show("Ngày Hết Hạn Không Được Cùng Ngày Nhập!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            */
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
                    Hien_Thi_Gia_Text_Box();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //HÀM HIỂN THỊ NGUYÊN LIỆU
        //HẰM CHẶN NHẬP KÍ TỰ ĐẶC BIỆT
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

        private void LamMoi_NguyenLieu()
        {
            txtMaNguyenLieu.Clear();
            txtGiaNhapNguyenLieu.Clear();
            txtTenNguyenLieu.Clear();
            txtThanhPhan.Clear();
            txtNhaSanXuat.Clear();
            txtSoLuongNguyenLieu.Clear();

            dtpNgayNhapNguyenLieu.Value = DateTime.Now;
            dtpNgayHetHan.Value = DateTime.Now;
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
        private void btnThemNguyenLieu_Click(object sender, EventArgs e)
        {
            Them_Nguyen_Lieu();
        }
        //XỬ LÍ TRƯỜNG HỢP GÁN MẶC ĐỊNH .000 FORM NGUYÊN LIỆU
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
                        dtpNgayNhapNguyenLieu.Value = ngayNhap;
                    }
                    else
                    {
                        dtpNgayNhapNguyenLieu.Value = DateTime.Now;
                    }

                    if (DateTime.TryParse(ngayHetHanText, out ngayHetHan))
                    {
                        dtpNgayHetHan.Value = ngayHetHan;
                    }
                    else
                    {
                        dtpNgayHetHan.Value = DateTime.Now;
                    }
                }
                else
                {
                    MessageBox.Show("Dữ liệu không đầy đủ hoặc cột không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        //HÀM SỬA NGUYÊN LIỆU
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
                if (dtpNgayHetHan.Value <= dtpNgayNhapNguyenLieu.Value)
                {
                    MessageBox.Show("Ngày hết hạn không được trước ngày nhập hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                SuaNguyenLieu.ThanhPhan = txtThanhPhan.Text;
                SuaNguyenLieu.GiaNhap = donGiaNhap;
                SuaNguyenLieu.NhaSanXuat = txtNhaSanXuat.Text;
                SuaNguyenLieu.TenNguyenLieu = txtTenNguyenLieu.Text;
                SuaNguyenLieu.SoLuongNhap = soLuongNhap;
                SuaNguyenLieu.NgayNhap = dtpNgayNhapNguyenLieu.Value;
                SuaNguyenLieu.NgayHetHan = dtpNgayHetHan.Value;

                try
                {
                    sua_Nl.SubmitChanges();
                    MessageBox.Show("Cập nhật thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hien_Thi_Nguyen_Lieu();
                    LamMoi_NguyenLieu();
                    Hien_Thi_Gia_Text_Box();

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



        //Hàm Xóa NGUYÊN LIỆU
        private void Xoa_Nguyen_Lieu()
        {
            if (dtgvThongTinNguyenLieu.SelectedRows.Count > 0)
            {
                DialogResult dl = MessageBox.Show("Bạn có chắc chắn muốn xóa Nguyên Liệu đã chọn không?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dtgvThongTinNguyenLieu.SelectedRows)
                    {
                        // Kiểm tra tên cột có chính xác không
                        if (row.Cells["TenNguyenLieu"] != null)
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
                                        MessageBox.Show("Xóa Nguyên Liệu Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Lỗi khi xóa nguyên liệu: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy nguyên liệu để xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy cột 'TenNguyenLieu' trong DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    Hien_Thi_Nguyen_Lieu();
                    LamMoi_NguyenLieu();
                    Hien_Thi_Gia_Text_Box();

                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //HÀM TÌM KIẾM NGUYÊN LIỆU
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

        //HIỂN THỊ THỐNG KÊ NGUYÊN LIỆU
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

        //TÌM KIẾM THỐNG KÊ NGUYÊN LIỆU
        private void Tim_Kiem_Thong_Ke_Nguyen_Lieu()
        {

            string NguyennLieu = txtTimKiem_ThongKeNguyenLieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtTimKiem_ThongKeNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_NL = txtTimKiem_ThongKeNguyenLieu.Text.Trim().ToLower();

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

        private void btnTimKiem_ThongKeNguyenLieu_Click(object sender, EventArgs e)
        {
            Tim_Kiem_Thong_Ke_Nguyen_Lieu();
        }

        private void btnThem_Nhan_Vien_Click(object sender, EventArgs e)
        {
            ThemNhanVien();
        }

        private void cbbVaiTroCuaNhanVien_DropDown(object sender, EventArgs e)
        {
            cbbVaiTroCuaNhanVien.DropDownWidth = CalculateDropDownWidth(cbbVaiTroCuaNhanVien);
        }

        private void btnSua_Nhan_Vien_Click(object sender, EventArgs e)
        {
            SuaNhanVien();
        }

        private void btn_Xoa_Nhan_Vien_Clicl(object sender, EventArgs e)
        {
            XoaNhanVien();
        }

        private void btnTimKiemNhanVien_Click(object sender, EventArgs e)
        {
            TimKiem_NhanVien();
        }
        private void TenNguyenLieu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !IsVietnameseLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool IsVietnameseLetter(char c)
        {
            string vietnameseLetters = "àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ"
                                      + "ÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴÈÉẸẺẼÊỀẾỆỂỄÌÍỊỈĨÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠÙÚỤỦŨƯỪỨỰỬỮỲÝỴỶỸĐ"
                                      + "âăêôơư";
            return vietnameseLetters.IndexOf(c) >= 0;
        }


        private void dtgvThongKeNguyenLieu_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dtgvThongKeNguyenLieu.CurrentCell.ColumnIndex == dtgvThongKeNguyenLieu.Columns["TenNguyenLieu"].Index)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= new KeyPressEventHandler(TenNguyenLieu_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(TenNguyenLieu_KeyPress);
                }
            }
        }
        //THÊM KHÁCH HÀNG
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
            txtTenKhachHang.Clear();
            txtDiaChiKhachHang.Clear();
            txtSDTKhachHang.Clear();
            txtEmailKhachHang.Clear();
            txtTenKhachHang.Focus();
        }
        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            ThemKhachHang();
        }
        private void btnSuaKhachHang_Click(object sender, EventArgs e)
        {
            SuaKhachHang();
        }
        private void btnXoaKhachHang_Click(object sender, EventArgs e)
        {
            XoaKhachHang();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiemKhachHang();
        }
        private void ThemKhachHang()
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
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) 
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            if (string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                Regex.IsMatch(txtDiaChiKhachHang.Text, @"[^\p{L}\d\s,.-]")) 
            {
                MessageBox.Show("Địa chỉ không được bỏ trống và không được chứa ký tự đặc biệt ngoài dấu câu hợp lệ!");
                return;
            }

            if (string.IsNullOrEmpty(txtSDTKhachHang.Text) ||
                !Regex.IsMatch(txtSDTKhachHang.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không được bỏ trống, không được chứa ký tự đặc biệt và phải có 10 số!");
                return;
            }

            if (string.IsNullOrEmpty(txtEmailKhachHang.Text) ||
                !Regex.IsMatch(txtEmailKhachHang.Text, @"^[a-zA-Z0-9._%+-]+@gmail\.com$") ||
                Regex.IsMatch(txtEmailKhachHang.Text, @"[^\w.@+-]")) 
            {
                MessageBox.Show("Email không được bỏ trống, phải có định dạng @gmail.com và không chứa ký tự đặc biệt ngoài những ký tự được phép!");
                return;
            }

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
        //SỬA KHÁCH HÀNG
        private void SuaKhachHang()
        {
            if (string.IsNullOrEmpty(txtTenKhachHang.Text) || string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                string.IsNullOrEmpty(txtSDTKhachHang.Text) || string.IsNullOrEmpty(dttpNgaySinhKhachHang.Text) ||
                string.IsNullOrEmpty(txtEmailKhachHang.Text))
            {
                MessageBox.Show("Bạn không thể sửa khi để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) ||
                Regex.IsMatch(txtTenKhachHang.Text, @"\d") ||
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) 
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            if (string.IsNullOrEmpty(txtTenKhachHang.Text) ||
                Regex.IsMatch(txtTenKhachHang.Text, @"\d") ||
                Regex.IsMatch(txtTenKhachHang.Text, @"[^\p{L}\s]")) 
            {
                MessageBox.Show("Tên không được bỏ trống, không được chứa số và không được chứa ký tự đặc biệt ngoài các ký tự có dấu!");
                return;
            }

            if (string.IsNullOrEmpty(txtDiaChiKhachHang.Text) ||
                Regex.IsMatch(txtDiaChiKhachHang.Text, @"[^\p{L}\d\s,.-]")) 
            {
                MessageBox.Show("Địa chỉ không được bỏ trống và không được chứa ký tự đặc biệt ngoài dấu câu hợp lệ!");
                return;
            }

            if (string.IsNullOrEmpty(txtSDTKhachHang.Text) ||
                !Regex.IsMatch(txtSDTKhachHang.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không được bỏ trống, không được chứa ký tự đặc biệt và phải có 10 số!");
                return;
            }

            if (string.IsNullOrEmpty(txtEmailKhachHang.Text) ||
                !Regex.IsMatch(txtEmailKhachHang.Text, @"^[a-zA-Z0-9._%+-]+@gmail\.com$") ||
                Regex.IsMatch(txtEmailKhachHang.Text, @"[^\w.@+-]"))
            {
                MessageBox.Show("Email không được bỏ trống, phải có định dạng @gmail.com và không chứa ký tự đặc biệt ngoài những ký tự được phép!");
                return;
            }

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
        //XÓA KHÁCH HÀNG
        private void XoaKhachHang()
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
        //TÌM KIẾM KHÁCH HÀNG
        private void TimKiemKhachHang()
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();

            if (string.IsNullOrEmpty(txtTimKiemKhachHang.Text))
            {
                MessageBox.Show("Bạn chưa nhập khách hàng cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string timKiemValue = txtTimKiemKhachHang.Text.Trim();

            if (Regex.IsMatch(timKiemValue, @"[^\p{L}\d\s\-\,\.\/]"))
            { 
                MessageBox.Show("Giá trị tìm kiếm không được chứa ký tự đặc biệt ngoài các ký tự có dấu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                var timKiem = from kh in QLNV.KhachHangs
                              join nv in QLNV.NhanViens on kh.MaNhanVien equals nv.MaNhanVien into vtGroup
                              from vt in vtGroup.DefaultIfEmpty()
                              where kh.MaKhachHang.Contains(timKiemValue) ||
                                    kh.TenKhachHang.Contains(timKiemValue) ||
                                    kh.DiaChi.Contains(timKiemValue) ||
                                    kh.SDT.Contains(timKiemValue) ||
                                    kh.NgaySinh.ToString().Contains(timKiemValue) || 
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

        //DATAGRIDVIEW THÔNG TIN KHÁCH HÀNG

        private void dtgvThongTinKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var QLKH = new LionQuanLyQuanCaPheDataContext();
            var HTKhachHang = (from kh in QLKH.KhachHangs

                               where kh.MaKhachHang == dtgvThongTinKhachHang.CurrentRow.
                               Cells["MaKhachHang"].Value.ToString()
                               select kh).SingleOrDefault();

            txtTenKhachHang.Text = HTKhachHang.TenKhachHang.ToString();
            txtDiaChiKhachHang.Text = HTKhachHang.DiaChi.ToString();
            txtSDTKhachHang.Text = HTKhachHang.SDT.ToString();
            dttpNgaySinhKhachHang.Text = HTKhachHang.NgaySinh.ToString();
            txtEmailKhachHang.Text = HTKhachHang.Email.ToString();

        }
    }
}



