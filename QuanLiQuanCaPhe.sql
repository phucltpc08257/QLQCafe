CREATE DATABASE QuanLiQuanCaPhe;
go
use QuanLiQuanCaPhe
go

CREATE TABLE NhanVien (
    MaNhanVien VARCHAR(8) PRIMARY KEY,
    TenNhanVien NVARCHAR(100),
    SDT VARCHAR(10),
    Email VARCHAR(120),
    DiaChi NVARCHAR(250),
    MaVaiTro VARCHAR(8) FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    NgaySinh DATE,
    GioiTinh NVARCHAR(3),
    MatKhau VARCHAR(100),
    NgayBatDauLamViec DATE
);

CREATE TABLE VaiTro (
    MaVaiTro VARCHAR(8) PRIMARY KEY,
    TenVaiTro NVARCHAR(100)
);

CREATE TABLE KhachHang (
    MaKhachHang VARCHAR(10) PRIMARY KEY,
	MaNhanVien VARCHAR(8) FOREIGN KEY(MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    TenKhachHang NVARCHAR(100),
    DiaChi NVARCHAR(250),
    SDT VARCHAR(10),
    NgaySinh DATE,
    Email VARCHAR(120)
);

CREATE TABLE NguyenLieu (
    MaNguyenLieu VARCHAR(8) PRIMARY KEY,
	MaNhanVien VARCHAR(8) FOREIGN KEY(MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    TenNguyenLieu NVARCHAR(150),
    SoLuongNhap INT,
    GiaNhap DECIMAL(10, 3),
    ThanhPhan NVARCHAR(350),
    NhaSanXuat NVARCHAR(125),
    NgayNhap DATE,
	NgayHetHan DATE
);

CREATE TABLE SanPham (
    MaSanPham VARCHAR(8) PRIMARY KEY,
	MaNhanVien VARCHAR(8) FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    TenSanPham NVARCHAR(100),
    GiaBan DECIMAL(10, 3),
	GiaNhap DECIMAL(10, 3),
	HinhAnh IMAGE
);

CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(8) PRIMARY KEY,
    MaNhanVien VARCHAR(8) FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    MaKhachHang VARCHAR(10) FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
	NgayXuatHoaDon DATE,
    TongHoaDon DECIMAL(10, 3),
	SoLuongMon INT,
	GiamGia INT,
	GhiChu NVARCHAR(150)
);

CREATE TABLE HoaDonChiTiet(
	MaHoaDon VARCHAR(8),
    MaSanPham VARCHAR(8),
    SoLuongTungMon INT,
	PRIMARY KEY (MaHoaDon, MaSanPham),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

/*
INSERT CÁC BẢNG
*/
-- Insert data into VaiTro table
INSERT INTO VaiTro (MaVaiTro, TenVaiTro)
VALUES 
('VT001', N'Admin'),
('VT002', N'Quản lý'),
('VT003', N'Nhân viên bán hàng')

-- Insert data into NhanVien table
INSERT INTO NhanVien (MaNhanVien, TenNhanVien, SDT, Email, DiaChi, MaVaiTro, NgaySinh, GioiTinh, MatKhau, NgayBatDauLamViec)
VALUES 
('NV001', N'Lý Trọng Phúc', '0865830945', 'phucltpc08257@gmail.com', N'123 Đường 20, Phường Hưng Phú, Quận Cái Răng', 'VT001', '2005-12-12', N'Nam', 'password123', '2024-07-22')

/*Bảng PROCEDURE*/
USE QuanLiQuanCaPhe;
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SanPham_DaAn')
BEGIN
    ALTER TABLE SanPham
    DROP CONSTRAINT FK_SanPham_DaAn;
END
-- Thống kê nhân viên: Mã NV, Tên NV, số lượng hóa đơn bán ra theo tuần, tháng và năm
CREATE PROCEDURE ThongKeNhanVien
AS
BEGIN
    SELECT 
        NV.MaNhanVien, 
        NV.TenNhanVien, 
        COUNT(CASE WHEN DATEDIFF(week, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN 1 END) AS SoLuongHoaDonTuan,
        COUNT(CASE WHEN DATEDIFF(month, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN 1 END) AS SoLuongHoaDonThang,
        COUNT(CASE WHEN DATEDIFF(year, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN 1 END) AS SoLuongHoaDonNam
    FROM NhanVien NV
    LEFT JOIN HoaDon HD ON NV.MaNhanVien = HD.MaNhanVien
    GROUP BY NV.MaNhanVien, NV.TenNhanVien;
END;
GO

-- Thống kê sản phẩm: Mã SP, Tên SP, Giá bán ra, tổng số lượng SP bán ra theo tuần, tháng, năm, tổng giá bán được
CREATE PROCEDURE ThongKeSanPham
AS
BEGIN
    SELECT 
        SP.MaSanPham, 
        SP.TenSanPham, 
        SP.GiaBan, 
        SUM(CASE WHEN DATEDIFF(week, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon ELSE 0 END) AS SoLuongBanRaTuan,
        SUM(CASE WHEN DATEDIFF(month, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon ELSE 0 END) AS SoLuongBanRaThang,
        SUM(CASE WHEN DATEDIFF(year, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon ELSE 0 END) AS SoLuongBanRaNam,
        SUM(CASE WHEN DATEDIFF(week, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon * SP.GiaBan ELSE 0 END) AS TongGiaBanRaTuan,
        SUM(CASE WHEN DATEDIFF(month, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon * SP.GiaBan ELSE 0 END) AS TongGiaBanRaThang,
        SUM(CASE WHEN DATEDIFF(year, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HDC.SoLuongTungMon * SP.GiaBan ELSE 0 END) AS TongGiaBanRaNam
    FROM SanPham SP
    LEFT JOIN HoaDonChiTiet HDC ON SP.MaSanPham = HDC.MaSanPham
    LEFT JOIN HoaDon HD ON HDC.MaHoaDon = HD.MaHoaDon
    GROUP BY SP.MaSanPham, SP.TenSanPham, SP.GiaBan;
END;
GO

-- Thống kê khách hàng: Mã KH, Tên KH, SDT khách hàng, tổng số hóa đơn đã mua theo tháng và năm, tổng giá trị đã mua
CREATE PROCEDURE ThongKeKhachHang
AS
BEGIN
    SELECT 
        KH.MaKhachHang, 
        KH.TenKhachHang, 
        KH.SDT, 
        COUNT(CASE WHEN DATEDIFF(month, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN 1 END) AS SoLuongHoaDonThang,
        COUNT(CASE WHEN DATEDIFF(year, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN 1 END) AS SoLuongHoaDonNam,
        SUM(CASE WHEN DATEDIFF(month, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HD.TongHoaDon ELSE 0 END) AS TongGiaTriThang,
        SUM(CASE WHEN DATEDIFF(year, HD.NgayXuatHoaDon, GETDATE()) = 0 THEN HD.TongHoaDon ELSE 0 END) AS TongGiaTriNam
    FROM KhachHang KH
    LEFT JOIN HoaDon HD ON KH.MaKhachHang = HD.MaKhachHang
    GROUP BY KH.MaKhachHang, KH.TenKhachHang, KH.SDT;
END;
GO

-- Thống kê hóa đơn: mã HD, ngày xuất HD, số lượng sản phẩm bán ra và số lượng món trong hóa đơn đó, tổng tiền hóa đơn
CREATE PROCEDURE ThongKeHoaDon
AS
BEGIN
    SELECT 
        HD.MaHoaDon, 
        HD.NgayXuatHoaDon, 
        SUM(HDC.SoLuongTungMon) AS SoLuongSanPham,
        HD.SoLuongMon,
        HD.TongHoaDon
    FROM HoaDon HD
    LEFT JOIN HoaDonChiTiet HDC ON HD.MaHoaDon = HDC.MaHoaDon
    GROUP BY HD.MaHoaDon, HD.NgayXuatHoaDon, HD.SoLuongMon, HD.TongHoaDon;
END;
GO

-- Thống kê nguyên liệu: mã NL, Tên NL, ngày hết hạn, số lượng nhập, giá nhập
CREATE PROCEDURE ThongKeNguyenLieu
AS
BEGIN
    SELECT 
        NL.MaNguyenLieu, 
        NL.TenNguyenLieu, 
        NL.NgayHetHan, 
        NL.SoLuongNhap, 
        NL.GiaNhap
    FROM NguyenLieu NL;
END;
GO

DELETE FROM SanPham
WHERE MaSanPham = 'SP008';

SELECT * FROM NhanVien
SELECT * FROM VaiTro
SELECT * FROM SanPham
SELECT * FROM NguyenLieu

DELETE FROM NguyenLieu
WHERE MaNguyenLieu = 'NL002';
