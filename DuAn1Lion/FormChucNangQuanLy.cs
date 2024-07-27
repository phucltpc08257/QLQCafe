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
            txtMaSanPham.ReadOnly = true;
            txtMaSanPham.TabStop = false;
            txtMaNguyenLieu.ReadOnly = true;
            txtMaNguyenLieu.TabStop = false;
            //txtMaNhanVien.Text = FormDangNhap.Lay_Ma_Nhan_Vien;
        }
        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
            string maNhanVien = FormDangNhap.MaNhanVienHienTai;


        }

        private void tpSanPham_Click(object sender, EventArgs e)
        {

        }

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            cbb_Chon_Soluong_Ban_Ra.Items.AddRange(new object[] {
            "", "Số Lượng Bán Ra Tuần", "Số Lượng Bán Ra Tháng", "Số Lượng Bán Ra Năm",
            "Tổng Giá Bán Ra Tuần", "Tổng Giá Bán Ra Tháng", "Tổng Giá Bán Ra Năm"
            });
            cbb_Chon_Soluong_Ban_Ra.SelectedIndex = 0;
            cbb_Chon_Soluong_Ban_Ra.SelectedIndexChanged += cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged;

            hienThi_ThongKe_SanPham();
        }
        private void cbb_Chon_Soluong_Ban_Ra_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            foreach (var columnName in columnMapping.Values)
            {
                if (dtgvThongKeSanPham.Columns.Contains(columnName))
                {
                    dtgvThongKeSanPham.Columns[columnName].Visible = false;
                }
            }

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

        private void btnThemSanPham_Click(object sender, EventArgs e)
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //Hàm Sửa Sản Phẩm
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

            // Nhân giá trị với 1000
            donGiaBan *= 1000;
            donGiaNhap *= 1000;

            var sua_Sp = new LionQuanLyQuanCaPheDataContext();

            string idSanPham = dtgvSanPham.CurrentRow.Cells["MaSanPham"].Value.ToString();
            var SuaSanPham = sua_Sp.SanPhams.FirstOrDefault(s => s.MaSanPham == idSanPham);

            if (SuaSanPham != null)
            {
                SuaSanPham.TenSanPham = txtTenSanPham.Text;
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
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvSanPham.DataSource = null;
                if (dtgvSanPham.Columns.Contains("AnhSanPham"))
                {
                    dtgvSanPham.Columns.Remove("AnhSanPham");
                }
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
                    txtGiaBan.Text = string.Empty;
                }

                if (decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), out giaNhap))
                {
                    txtGiaNhap.Text = giaNhap.ToString("N0", nfi);
                }
                else
                {
                    txtGiaNhap.Text = string.Empty;
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
        //Hàm Khai Báo DTGV Thống Kê Sản Phẩm
        private void dtgvThongKe_SanPham_DataBindingComplete()
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
            dtgvThongKe_SanPham_DataBindingComplete();
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
                                     s.TenSanPham.ToLower().Contains(tuKhoa_SP) ||
                                     s.GiaBan.ToString().Contains(tuKhoa_SP) ||
                                     s.SoLuongBanRaTuan.ToString().Contains(tuKhoa_SP) ||
                                     s.SoLuongBanRaThang.ToString().Contains(tuKhoa_SP) ||
                                     s.SoLuongBanRaNam.ToString().Contains(tuKhoa_SP) ||
                                     s.TongGiaBanRaTuan.ToString().Contains(tuKhoa_SP) ||
                                     s.TongGiaBanRaThang.ToString().Contains(tuKhoa_SP) ||
                                     s.TongGiaBanRaNam.ToString().Contains(tuKhoa_SP)



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
                dtgvThongKeSanPham.DataSource = null;
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
            txtMaSanPham.Clear();
            txtGiaBan.Clear();
            txtGiaNhap.Clear();
            pic_AnhSanPham.Image = null;
        }
        private void grbChucNangSanPham_Enter(object sender, EventArgs e)
        {

        }

        private void tpKhachHang_Click(object sender, EventArgs e)
        {

        }

        private void tclFormChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tclFormChucNang.SelectedTab == tpSanPham)
            {
                hienThiSan_Pham();
            }
            if (tclFormChucNang.SelectedTab == tpNhanVien)
            {
                LamMoi_SP();
                HienThiNhanVien();
                LamMoi_NguyenLieu();
            }
            if (tclFormChucNang.SelectedTab == tpKhachHang)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();

            }
            if (tclFormChucNang.SelectedTab == tpNguyenLieu)
            {
                LamMoi_SP();
                Hien_Thi_Nguyen_Lieu();
                LamMoi_NguyenLieu();

            }
            if (tclFormChucNang.SelectedTab == tpOrder)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();

            }
            if (tclFormChucNang.SelectedTab == tpThongKe)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();

            }
            if (tclFormChucNang.SelectedTab == tpVaiTro)
            {
                LamMoi_SP();
                LamMoi_NguyenLieu();

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
        //Thêm, Sửa, Xóa Nhân Viên
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
        private void ClearTextBox()
        {



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
        private void ThemVaiTro()
        {

        }
        private void SuaVaiTro()
        {

        }

        private void XoaVaiTro()
        {


        }






        private void LoadcbbVaiTroCuaNhanVien()
        {

        }



        private void btntimkiemVaiTro_Click(object sender, EventArgs e)
        {


        }

        private void tpVaiTro_Click(object sender, EventArgs e)
        {

        }

        private void btnThemNhanVien_Click_1(object sender, EventArgs e)
        {

        }

        private void tpThongKeSanPham_Click(object sender, EventArgs e)
        {

        }
        //HÀM tHÊM NGUYÊN LIỆU
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

            Regex regex = new Regex(@"^[\p{L}\s]+$");
            if (!regex.IsMatch(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Lỗi! Vui Lòng Chỉ Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtGiaNhapNguyenLieu.Text, out decimal giaNhap) || giaNhap <= 0)
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
            if (ngayNhap == null || ngayHetHan == null)
            {
                MessageBox.Show("Vui Lòng Chọn Ngày Nhập và Ngày Hết Hạn!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ngayHetHan <= ngayNhap)
            {
                MessageBox.Show("Ngày Hết Hạn Phải Sau Ngày Nhập!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        //HÀM HIỂN THỊ NGUYÊN LIỆU
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
                              Nl.ThanhPhan,
                              Nl.GiaNhap,
                              Nl.NhaSanXuat,
                              Nl.TenNguyenLieu,
                              Nl.SoLuongNhap,
                              Nl.NgayNhap,
                              Nl.NgayHetHan
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

                    decimal giaNhap;
                    NumberFormatInfo nfi = new NumberFormatInfo
                    {
                        NumberGroupSeparator = ".",
                        NumberDecimalSeparator = ",",
                        NumberGroupSizes = new int[] { 3 }
                    };

                    string giaNhapText = row.Cells["GiaNhap"].Value != DBNull.Value ? row.Cells["GiaNhap"].Value.ToString() : string.Empty;
                    if (decimal.TryParse(giaNhapText, out giaNhap))
                    {
                        txtGiaNhapNguyenLieu.Text = giaNhap.ToString("N0", nfi);
                    }
                    else
                    {
                        txtGiaNhapNguyenLieu.Text = string.Empty;
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

            // Kiểm tra đơn giá nhập
            if (!decimal.TryParse(txtGiaNhapNguyenLieu.Text, out donGiaNhap) || donGiaNhap <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            donGiaNhap *= 1000;

            // Kiểm tra số lượng nhập
            if (!int.TryParse(txtSoLuongNguyenLieu.Text, out int soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập phải là số nguyên dương", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra dòng hiện tại trong DataGridView
            if (dtgvThongTinNguyenLieu.CurrentRow == null)
            {
                MessageBox.Show("Không có dòng nào được chọn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy mã nguyên liệu từ dòng hiện tại
            string idNguyenLieu = dtgvThongTinNguyenLieu.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            var sua_Nl = new LionQuanLyQuanCaPheDataContext();
            var SuaNguyenLieu = sua_Nl.NguyenLieus.FirstOrDefault(s => s.MaNguyenLieu == idNguyenLieu);

            if (SuaNguyenLieu != null)
            {
                // Kiểm tra ngày hết hạn không được trước ngày nhập
                if (dtpNgayHetHan.Value < dtpNgayNhapNguyenLieu.Value)
                {
                    MessageBox.Show("Ngày hết hạn không được trước ngày nhập hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật thông tin nguyên liệu
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



        //Hàm Xóa Sản Phẩm
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

        //HÀM TÌM KIẾM NGUYÊN LIỆU
        private void TimKiem_NguyenLieu()
        {
            string NguyennLieu = txtTimKiemNguyenLieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtTimKiemNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập mã để tìm kiếm!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tuKhoa_NL = txtTimKiemNguyenLieu.Text.Trim().ToLower();

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
                    dtgvThongTinNguyenLieu.DataSource = List.ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Nguyên Liệu phù hợp", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgvThongTinNguyenLieu.DataSource = null;
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
                                          nl.ThanhPhan,
                                          nl.NhaSanXuat,
                                          nl.SoLuongNhap,
                                          nl.NgayNhap,
                                          nl.NgayHetHan,

                                      };

                    var resultList_NK = thongKeList.ToList();

                    var formattedList_NK = resultList_NK.Select(nl => new
                    {
                        nl.MaNguyenLieu,
                        nl.TenNguyenLieu,
                        nl.ThanhPhan,
                        nl.NhaSanXuat,
                        nl.SoLuongNhap,
                        nl.NgayNhap,
                        nl.NgayHetHan,
                    }).ToList();

                    dtgvThongKeNguyenLieu.DataSource = formattedList_NK;

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
                    dtgvThongKeNguyenLieu.DataSource = null;
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
    }
}



