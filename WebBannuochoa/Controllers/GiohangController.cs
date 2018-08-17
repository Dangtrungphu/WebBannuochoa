using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBannuochoa.Models;

namespace WebNuocHoa.Controllers
{
    public class GioHangController : Controller
    {
         dbQLNuochoaClasses1DataContext data = new dbQLNuochoaClasses1DataContext();
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo.      
        //Lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Them hang vao gio
        public ActionResult ThemGiohang(int iMaNH, string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sách này tồn tại trong Session["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMaNH == iMaNH);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaNH);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Xay dung trang Gio hang
        public ActionResult GioHang()
        {
            if (Session["TaiKhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Trangchu", "Trangchu");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //Tao Partial view de hien thi thong tin gio hang
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            //gio hàng nhỏ
            List<Giohang> lstGiohang = Laygiohang();
            return PartialView(lstGiohang);
        }
        //Cap nhat Giỏ hàng
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {

            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaNH == iMaSP);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        //Xoa Giohang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaNH == iMaSP);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaNH == iMaSP);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Trangchu", "Trangchu");
            }
            return RedirectToAction("GioHang");
        }
        //Xoa tat ca thong tin trong Gio hang
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Trangchu", "Trangchu");
        }
        //Hien thi View DatHang de cap nhat cac thong tin cho Don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if (Session["TaiKhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["TaiKhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            else
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "LatopSore");
            }

            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);

        }
        //Xay dung chuc nang Dathang
        [HttpPost]
        public ActionResult DatHang(CHITIETDONTHANG id, FormCollection collection)
        {
            try
            {
                double iTongTien = 0;
                List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
                if (lstGiohang != null)
                {
                    iTongTien = lstGiohang.Sum(n => n.dThanhtien);
                }
                //Them Don hang
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                List<Giohang> gh = Laygiohang();
                ddh.MaKH = kh.MaKH;
                ddh.Ngaydat = DateTime.Now;
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
                ddh.Ngaygiao = DateTime.Parse(ngaygiao);
                ddh.Tinhtranggiaohang = false;
                ddh.Dathanhtoan = false;
                data.DONDATHANGs.InsertOnSubmit(ddh);
                data.SubmitChanges();
                //Them chi tiet don hang     
                foreach (var item in gh)
                {
                    CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaNH = item.iMaNH;
                    ctdh.Soluong = item.iSoluong;
                    ctdh.Dongia = (decimal)item.dGiaBan;
                    data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
                }
                ////gửi mail----------------------------------------------------------------------------
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Mail/DonHang.html"));
                content = content.Replace("{{CustomerName}}", kh.HoTen);
                content = content.Replace("{{Phone}}", kh.DienthoaiKH);
                content = content.Replace("{{Email}}", kh.Email);
                content = content.Replace("{{Address}}", kh.DiachiKH);
                content = content.Replace("{{NgayGiao}}", ngaygiao);
                content = content.Replace("{{Total}}", iTongTien.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(kh.Email, "Đơn hàng mới từ Tuấn-Phúc Hùng", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Tuấn-Phúc Hùng", content);
                //------------------------------------------------------------------------------------------
                data.SubmitChanges();
                Session["Giohang"] = null;
                return RedirectToAction("Xacnhandonhang", "Giohang");
            }
            catch
            {
                Session["Giohang"] = null;
            }
            return RedirectToAction("Xacnhandonhang", "Giohang");

        }
        public ActionResult Xacnhandonhang()
        {
            if (Session["TaiKhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            return View();
        }
    }
}