using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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

        public FormChucNangQuanLy(string VaiTro)
        {
            InitializeComponent();
            UserRole = VaiTro;
            SetupUI();

            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();

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
        //Thêm, Sửa, Xóa, Tìm Kiếm, Thống Kê, DTGV Sản Phẩm
        private int Ma_Nhan_Vien = 001;
        private string GetNewMaSanPham(LionQuanLyQuanCaPheDataContext sp_Quan_Tri)
        {
            var existingMaHangs = sp_Quan_Tri.SanPhams.Select(sp => sp.MaSanPham).ToList();

        public FormChucNangQuanLy()
            int newMaHang = 1;
            while (existingMaHangs.Contains("SP" + newMaHang.ToString("D3")))
            {
                newMaHang++;
            }

            return "SP" + newMaHang.ToString("D3");
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            InitializeComponent();
            tclFormChucNang.SelectedIndexChanged += tclFormChucNang_SelectedIndexChanged;
            LoadData();
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

            if (pic_AnhSanPham.Image == null)
            {
                MessageBox.Show("Vui lòng chọn ảnh cho sản phẩm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] imgData = null;
            using (MemoryStream ms = new MemoryStream())
            {
                pic_AnhSanPham.Image.Save(ms, pic_AnhSanPham.Image.RawFormat);
                imgData = ms.ToArray();
            }

            using (var sp = new LionQuanLyQuanCaPheDataContext())
            {
                string auto_Ma_Sp = GetNewMaSanPham(sp);
                SanPham ThemSp = new SanPham
                {
                    MaSanPham = auto_Ma_Sp,
                    MaNhanVien = txtMaNhanVien.Text,
                    TenSanPham = txtTenSanPham.Text,
                    GiaBan = giaBan,
                    GiaNhap = giaNhap,
                    HinhAnh = imgData != null ? new System.Data.Linq.Binary(imgData) : null
                };

                sp.SanPhams.InsertOnSubmit(ThemSp);
                ThemSp.MaNhanVien = "NV" + Ma_Nhan_Vien.ToString("D3");
                Ma_Nhan_Vien++;

                try
                {
                    sp.SubmitChanges();
                    MessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hienThiSan_Pham();
                    LamMoi_SP();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnSuaSanPham_Click(object sender, EventArgs e)
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

        private void btnXoaSanPham_Click(object sender, EventArgs e)
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
                            var XoaSanPham = Sp.SanPhams.SingleOrDefault(sp => sp.TenSanPham == tenSP);

                            if (XoaSanPham != null)
                            {
                                if (XoaSanPham.HinhAnh != null)
                                {
                                }

                                Sp.SanPhams.DeleteOnSubmit(XoaSanPham);
                                Sp.SubmitChanges();
                            }
                        }
                    }
                    MessageBox.Show("Xóa Thành Công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hienThiSan_Pham();
                    LamMoi_SP();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiemSanPham_Click(object sender, EventArgs e)
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
                           sp.GiaBan,
                           sp.GiaNhap,
                           sp.HinhAnh
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
        private void hienThiSan_Pham()
        {

            var list_SP = new LionQuanLyQuanCaPheDataContext();

            var List_SP = from Sp in list_SP.SanPhams
                          select new
                          {
                              Sp.MaSanPham,
                              Sp.TenSanPham,
                              Sp.GiaBan,
                              Sp.GiaNhap,
                              Sp.MaNhanVien,
                              Sp.HinhAnh,
                          };

            var resultList = List_SP.ToList();

            dtgvSanPham.DataSource = resultList;

            /*
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
            */
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

                // Lấy giá bán và giá nhập từ ô dữ liệu
                decimal giaBan, giaNhap;

                if (decimal.TryParse(row.Cells["GiaBan"].Value.ToString(), out giaBan))
                {
                    txtGiaBan.Text = giaBan.ToString("0"); // Hiển thị số nguyên bình thường, không có dấu phân cách
                }
                else
                {
                    txtGiaBan.Text = string.Empty; // Xử lý trường hợp không phải là số hợp lệ
                }

                if (decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), out giaNhap))
                {
                    txtGiaNhap.Text = giaNhap.ToString("0"); // Hiển thị số nguyên bình thường, không có dấu phân cách
                }
                else
                {
                    txtGiaNhap.Text = string.Empty; // Xử lý trường hợp không phải là số hợp lệ
                }

                var cellValue = row.Cells["AnhSanPham"].Value;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    var image = (Image)cellValue;
                    pic_AnhSanPham.Image = image;
                }
                else
                {
                    pic_AnhSanPham.Image = null;
                }

                if (e.RowIndex >= 0 && e.RowIndex < dtgvSanPham.Rows.Count)
                {
                    string ma_san_pham = row.Cells["MaSanPham"].Value.ToString();
                    LoadImage(ma_san_pham);
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
            if (e.ColumnIndex == dtgvThongKeSanPham.Columns["AnhSanPhamImage_TK"].Index && e.RowIndex >= 0)
            {
                if (dtgvThongKeSanPham.Columns.Contains("AnhSanPham"))
                {
                    var cellValue_HD = dtgvThongKeSanPham.Rows[e.RowIndex].Cells["AnhSanPham"].Value;
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

        private void FormChucNangQuanLy_Load(object sender, EventArgs e)
        {
            hienThiKhachHang();
            HienThiNhanVien();
            load();
            SetupUI();
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

        private void load()
        private void dtgvThongKeSanPham_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            txtMaKhachHang.ReadOnly = true;
            txtMaKhachHang.TabStop = false;
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

        private void hienThi_ThongKe_SanPham()
        {

            var list_TK_SP = new LionQuanLyQuanCaPheDataContext();

            var List_TK_SP = from Sp in list_TK_SP.SanPhams
                             select new
                             {
                                 Sp.MaSanPham,
                                 Sp.TenSanPham,
                                 Sp.GiaBan,
                                 Sp.GiaNhap,
                                 Sp.HinhAnh,
                             };

            var resultList_NK = List_TK_SP.ToList();

            dtgvThongKeSanPham.DataSource = resultList_NK;


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

            if (!dtgvThongKeSanPham.Columns.Contains("AnhSanPham_TK"))
            {
                DataGridViewImageColumn Column_NK = new DataGridViewImageColumn();
                Column_NK.Name = "AnhSanPham_TK";
                Column_NK.HeaderText = "Ảnh Sản Phẩm";
                Column_NK.Width = 100;
                Column_NK.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dtgvThongKeSanPham.Columns.Add(Column_NK);
            }

            foreach (DataGridViewRow row_NK in dtgvThongKeSanPham.Rows)
            {
                var cellValue_NK = row_NK.Cells["HinhAnh"].Value;
                if (cellValue_NK != null && cellValue_NK != DBNull.Value)
                {
                    byte[] DataImg_NK = ((System.Data.Linq.Binary)cellValue_NK).ToArray();
                    using (var ms_NK = new MemoryStream(DataImg_NK))
                    {
                        var image_NK = Image.FromStream(ms_NK);
                        row_NK.Cells["AnhSanPham_TK"].Value = image_NK;
                    }
                }
                else
                {
                    row_NK.Cells["AnhSanPham_TK"].Value = null;
                }
            }

            if (dtgvThongKeSanPham.Columns["HinhAnh"] != null)
            {
                dtgvThongKeSanPham.Columns["HinhAnh"].Visible = false;
            }
        }
        /*
                private void hienThi_Thong_Ke_San_Pham_NhapKho()
                {
                    try
                    {
                        using (var context = new LionQuanLyQuanCaPheDataContext())
                        {
                            var ThongKe_SanPhamList = from sp in context.SanPhams
                                              select new
                                              {
                                                  sp.MaSanPham,
                                                  sp.TenSanPham,
                                                  sp.GiaBan,
                                                  sp.GiaNhap,
                                                  sp.HinhAnh,
                                              };

                            var resultList_NK = ThongKe_SanPhamList.ToList();

                            var formattedList_NK = resultList_NK.Select(sp => new
                            {
                                sp.MaSanPham,
                                sp.TenSanPham,
                                sp.GiaBan,
                                txtGiaBan = sp.GiaBan.ToString("N0").Replace(",", "."),
                                sp.GiaNhap,
                                txtGiaNhap = sp.GiaNhap.ToString("N0").Replace(",", "."),
                                sp.HinhAnh,
                            }).ToList();

                            dtgvThongKeSanPham.DataSource = formattedList_NK;

                            if (!dtgvThongKeSanPham.Columns.Contains("AnhSanPhamImage_TK"))
                            {
                                DataGridViewImageColumn Column_NK = new DataGridViewImageColumn();
                                Column_NK.Name = "AnhSanPhamImage_TK";
                                Column_NK.HeaderText = "Ảnh Sản Phẩm";
                                Column_NK.Width = 100;
                                Column_NK.ImageLayout = DataGridViewImageCellLayout.Zoom;
                                dtgvThongKeSanPham.Columns.Add(Column_NK);
                            }

                            foreach (DataGridViewRow row_NK in dtgvThongKeSanPham.Rows)
                            {
                                var cellValue_NK = row_NK.Cells["AnhSanPham"].Value;
                                if (cellValue_NK != null && cellValue_NK != DBNull.Value)
                                {
                                    byte[] DataImg_NK = ((System.Data.Linq.Binary)cellValue_NK).ToArray();
                                    using (var ms_NK = new MemoryStream(DataImg_NK))
                                    {
                                        var image_NK = Image.FromStream(ms_NK);
                                        row_NK.Cells["AnhSanPhamImage_TK"].Value = image_NK;
                                    }
                                }
                                else
                                {
                                    row_NK.Cells["AnhSanPhamImage_TK"].Value = null;
                                }
                            }

                            if (dtgvThongKeSanPham.Columns["AnhSanPham"] != null)
                            {
                                dtgvThongKeSanPham.Columns["AnhSanPham"].Visible = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }*/
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

            if (tclFormChucNang.SelectedTab == tpVaiTro)
            {
                HienThioVaiTro();
            }

            if (tclFormChucNang.SelectedTab == tpKhachHang)
            {
                hienThiKhachHang();
                lamMoiKhachHang();
            }
        private void tpKhachHang_Click(object sender, EventArgs e)
        {

        }

        private void tclFormChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            HienThiNhanVien();
            hienThiSan_Pham();
            txtMaSanPham.ReadOnly = true;
            txtMaSanPham.TabStop = false;
            //txtMaNhanVien.Text = FormDangNhap.Lay_Ma_Nhan_Vien;
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


            dtgvVaiTro.DataSource = List.ToList();
        }



        private void grbTimKiem_Enter(object sender, EventArgs e)
        {

        }

        private void txtDiaChiKhachHang_TextChanged(object sender, EventArgs e)
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

        private void btnTimKiemNhanVien_Click(object sender, EventArgs e)
        {
            TimKiemNhanVien();
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
        }

        private void btnsuaVaiTro_Click(object sender, EventArgs e)
        {
        }

        private void btnxoaVaiTro_Click(object sender, EventArgs e)
        {

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

        }
        private void ThemVaiTro()
        {

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


        }






        private void LoadcbbVaiTroCuaNhanVien()
        {

        }



        private void btntimkiemVaiTro_Click(object sender, EventArgs e)
        {


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
            TimKiemKhachHang();
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
        }

        private void tpVaiTro_Click(object sender, EventArgs e)
        {

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


        private void TimKiemKhachHang()
        {
            using (var QLNV = new LionQuanLyQuanCaPheDataContext())
            {
                string timKiemValue = txtTimKiemKhachHang.Text.Trim(); // Lấy giá trị tìm kiếm từ textbox

                // Query lấy thông tin nhân viên từ database dựa vào mã nhân viên hoặc tên nhân viên nhập vào
                var timKiem = from kh in QLNV.KhachHangs
                              join nv in QLNV.NhanViens on kh.MaNhanVien equals nv.MaNhanVien into vtGroup
                              from vt in vtGroup.DefaultIfEmpty()
                              where kh.MaKhachHang.Contains(timKiemValue) || kh.TenKhachHang.Contains(timKiemValue)
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
}



