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
            txtMaNhanVien.ReadOnly = true;
            txtMaNhanVien.TabStop = false;
            //txtMaNhanVien.Text = FormDangNhap.Lay_Ma_Nhan_Vien;
        }
        public FormChucNangQuanLy()
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
            string maNhanVien = FormDangNhap.MaNhanVienHienTai;
            string randomPassword = RandomPassword(8);
            this.Load += new System.EventHandler(this.FormChucNangQuanLy_Load);
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
            cbbVaiTroCuaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbVaiTroCuaNhanVien.DropDown += new EventHandler(cbbVaiTroCuaNhanVien_DropDown);
            LoadVaiTro();
            txtGiaBan.Text = ".000";
            txtGiaNhap.Text = ".000";

            txtGiaBan.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);
            txtGiaNhap.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);

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

            txtGiaNhapNguyenLieu.Text = "0.000";
            txtGiaNhapNguyenLieu.KeyPress += new KeyPressEventHandler(TxtGia_KeyPress);

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

            // Kiểm tra giá nhập và giá bán không vượt quá giới hạn
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


        private string giaBanMacDinh = ".000";
        private string giaNhapMacDinh = ".000";

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tclFormChucNang.SelectedTab == tpSanPham) // Thay thế tabPage1 bằng tên tab chứa TextBox
            {
                txtGiaBan.Text = giaBanMacDinh;
                txtGiaNhap.Text = giaNhapMacDinh;
            }
        }

        public class TabState
        {
            public string GiaBan { get; set; }
            public string GiaNhap { get; set; }
        }

        private TabState currentState = new TabState();

       

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
                LamMoi_SP();
                Hien_Thi_Nguyen_Lieu();
                LamMoi_NguyenLieu();
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
            // Kiểm tra các trường nhập liệu
            if (string.IsNullOrEmpty(txtTenNguyenLieu.Text) ||
                string.IsNullOrEmpty(txtSoLuongNguyenLieu.Text) ||
                string.IsNullOrEmpty(txtNhaSanXuat.Text))
            {
                MessageBox.Show("Vui Lòng Điền Đầy Đủ Dữ Liệu!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra tên nguyên liệu
            Regex regexName = new Regex(@"^[\p{L}\s]+$");
            if (!regexName.IsMatch(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Lỗi! Vui Lòng Chỉ Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra nhà sản xuất
            Regex regexManufacturer = new Regex(@"^[\p{L}\s]+$");
            if (!regexManufacturer.IsMatch(txtNhaSanXuat.Text))
            {
                MessageBox.Show("Lỗi! Nhà Sản Xuất Chỉ Được Nhập Chữ Cái (Có Dấu) Và Khoảng Trắng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra giá nhập
            string giaNhapText = txtGiaNhapNguyenLieu.Text.EndsWith(".000") && txtGiaNhapNguyenLieu.Text.Length >= 4
                ? txtGiaNhapNguyenLieu.Text.Substring(0, txtGiaNhapNguyenLieu.Text.Length - 4) : txtGiaNhapNguyenLieu.Text;

            if (!decimal.TryParse(giaNhapText.Replace(".", "").Replace(",", ""), out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Lỗi! Giá Nhập Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            giaNhap *= 1000;

            // Kiểm tra số lượng nhập
            if (!int.TryParse(txtSoLuongNguyenLieu.Text, out int soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Lỗi! Số Lượng Nhập Phải Là Số Và Lớn Hơn 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra ngày nhập và ngày hết hạn
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

            // Kiểm tra mã nhân viên
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

            // Thêm nguyên liệu vào cơ sở dữ liệu
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

        private void txtGiaNhapNguyenLieu_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            // Lấy giá trị mà không có .000
            string textWithoutSuffix = textBox.Text;

            // Nếu có .000, loại bỏ nó để xử lý
            if (textWithoutSuffix.EndsWith(".000") && textWithoutSuffix.Length > 4)
            {
                textWithoutSuffix = textWithoutSuffix.Substring(0, textWithoutSuffix.Length - 4);
            }

            // Loại bỏ dấu phân cách hàng nghìn
            textWithoutSuffix = textWithoutSuffix.Replace(".", "").Replace(",", "");

            // Định dạng lại giá trị với phân cách hàng nghìn và thêm .000
            if (decimal.TryParse(textWithoutSuffix, out decimal value))
            {
                NumberFormatInfo nfi = new NumberFormatInfo
                {
                    NumberGroupSeparator = ".",
                    NumberDecimalSeparator = ",",
                    NumberGroupSizes = new int[] { 3 }
                };

                textBox.Text = value.ToString("N0", nfi) + ".000";
                textBox.SelectionStart = textBox.Text.Length - 4;
            }
            else
            {
                // Nếu giá trị không hợp lệ, hiển thị lại .000
                textBox.Text = "0.000";
                textBox.SelectionStart = 0;
            }
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
                if (dtpNgayHetHan.Value < dtpNgayNhapNguyenLieu.Value)
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
    }
}



