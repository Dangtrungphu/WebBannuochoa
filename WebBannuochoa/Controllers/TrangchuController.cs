using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBannuochoa.Models;
using PagedList;
using PagedList.Mvc;

namespace WebNuocHoa.Controllers
{
    public class TrangchuController : Controller
    {
        dbQLNuochoaClasses1DataContext data = new dbQLNuochoaClasses1DataContext();

        private List<NUOCHOA> LayHetSP()
        {
            return data.NUOCHOAs.OrderByDescending(a => a.Ngaycapnhat).ToList();
        }
        // GET: Trangchu

        public ActionResult Trangchu(FormCollection fc, int ? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 6;

            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            //Nút tìm kiếm---------------------------------------
            string tk = fc["Search"];
            if (tk != null && tk != "")
            {
                var fullnh = (from n in data.NUOCHOAs where n.TenNH.ToUpper().Contains(tk.ToUpper()) select n).ToList();
                return View(fullnh.ToPagedList(pageNum,pageSize));
            }
            //-----------------------------------------------
            //hiển thị tất cả sản phẩm của cửa hàng
            else
            {
                var fullnh = (from n in data.NUOCHOAs select n).ToList();
                return View(fullnh.ToPagedList(pageNum, pageSize));
            }
            //--------------------------------------------------------
        }
        //action tìm kiếm hiển thị nút tìm kiếm
        public ActionResult Search()
        {
            return PartialView();
        }
        //--------------------------------------------------
        public ActionResult HangSX()
        {
            var hang = from hsx in data.HANGs select hsx;
            return PartialView(hang);
        }

        public ActionResult LoaiNH()
        {
            var loai = from lnh in data.Loais select lnh;
            return PartialView(loai);
        }
        public ActionResult SPTheoHang(int id/*,int ? page*/)
        {
            //int pageSize = 5;
            //int pageNum = (page ?? 1);
            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            var nuochoa = from lt in data.NUOCHOAs where lt.MaH == id select lt;
            return View(nuochoa/*.ToPagedList(pageNum,pageSize)*/);
        }

        public ActionResult SPTheoLoai(int id/*,int ? page*/)
        {
            //int pageSize = 6;
            //int pageNum = (page ?? 1);
            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            var nuochoa = from lt in data.NUOCHOAs where lt.MaLoai == id select lt;
           return View(nuochoa/*.ToPagedList(pageNum, pageSize)*/);
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            var nh = from s in data.NUOCHOAs
                     where s.MaNH == id
                     select s;
            return View(nh.Single());
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {

            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                PQ_Chucvu kh = data.PQ_Chucvus.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("Trangchu", "Trangchu");
        }
        public ActionResult Dangxuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Trangchu");
        }

        //Hiện thị thông tin khách hàng và lihcj sử mua hàng
        public ActionResult ThongtinKH(FormCollection fc)
        {
            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.ht = kh.HoTen;
                ViewBag.dt = kh.DienthoaiKH;
                ViewBag.dc = kh.DiachiKH;
                ViewBag.email = kh.Email;
                ViewBag.ns = kh.Ngaysinh;
                ViewBag.CV = kh.Chucvu;

                int tk = kh.MaKH;
                var fullnh = (from n in data.thongtinKHs where n.MaKH == tk select n).ToList();
                return View(fullnh);
            }
            //--------------------------------------------------------
            else
            {
                ViewBag.thongbao = "Đăng nhập trước khi xem nhé";
                return View();
            }
        }
        public ActionResult Lienhe()
        {
            if (Session["Taikhoan"] != null)
            {
                PQ_Chucvu kh = (PQ_Chucvu)Session["Taikhoan"];
                ViewBag.TenKH = "Chào  " + kh.HoTen;
                ViewBag.MaKH = kh.MaKH;
                ViewBag.CV = kh.Chucvu;
            }
            return View();
        }
        public ActionResult About()
        {
            if (Session["Taikhoan"] != null)
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