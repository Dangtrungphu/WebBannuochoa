using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBannuochoa.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebBannuochoa.Controllers
{
    public class AdminController : Controller
    {
        dbQLNuochoaClasses1DataContext db = new dbQLNuochoaClasses1DataContext();
        // GET: Admin
        
        public ActionResult Nuochoa(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //var nuochoa = db.NUOCHOAs.ToList().OrderBy(n => n.MaNH).ToPagedList(pageNumber, pageSize);
            return View(db.NUOCHOAs.ToList().OrderBy(n => n.MaNH).ToPagedList(pageNumber, pageSize));
        }
 
        [HttpGet]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["Username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult themNH()
        {

            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaH = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenH), "MaH", "TenH");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themNH(NUOCHOA nh, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai",nh.MaLoai);
            ViewBag.MaH = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenH), "MaH", "TenH",nh.MaH);

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    nh.Anhbia = fileName;
                    db.NUOCHOAs.InsertOnSubmit(nh);
                    db.SubmitChanges();
                }
                return RedirectToAction("Nuochoa");//trả về trang muốn đến sau khi thưc hiện xong
            }
        }
        public ActionResult chitietnh(int id)
        {
            NUOCHOA nh = db.NUOCHOAs.SingleOrDefault(n => n.MaNH == id);
            ViewBag.MaNH = nh.MaNH;
            if(nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nh);
        }
        [HttpGet]
        public ActionResult xoanh(int id)
        {
            NUOCHOA nh = db.NUOCHOAs.SingleOrDefault(n => n.MaNH == id);
            ViewBag.MaNH = nh.MaNH;
            if(nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nh);
        }
        [HttpPost,ActionName("xoanh")]
        public ActionResult xacnhanxoa(int id)
        {
            NUOCHOA nh = db.NUOCHOAs.SingleOrDefault(n => n.MaNH == id);
            ViewBag.MaNH = nh.MaNH;
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NUOCHOAs.DeleteOnSubmit(nh);
            db.SubmitChanges();
            return RedirectToAction("Nuochoa");
        }
        [HttpGet]
        public ActionResult suanh(int id)
        {
            NUOCHOA nh = db.NUOCHOAs.SingleOrDefault(n => n.MaNH == id);
            ViewBag.MaNH = nh.MaNH;
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai",nh.MaLoai);
            ViewBag.MaH = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenH), "MaH", "TenH",nh.MaH);
            return View(nh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suanh(NUOCHOA nh, HttpPostedFileBase fileupload, FormCollection fc)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", nh.MaLoai);
            ViewBag.MaH = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenH), "MaH", "TenH", nh.MaH);

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);

                    }
                    var nuoc = db.NUOCHOAs.SingleOrDefault(n => n.MaNH == nh.MaNH);
                    nuoc.Anhbia = fileName;
                    UpdateModel(nuoc);
                    db.SubmitChanges();

                }
                return RedirectToAction("Nuochoa");//trả về trang muốn đến sau khi thưc hiện xong
            }
        }
        ///////////
        public ActionResult LoaiNuochoa(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //var nuochoa = db.NUOCHOAs.ToList().OrderBy(n => n.MaNH).ToPagedList(pageNumber, pageSize);
            return View(db.Loais.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult themloai()
        {
            return View();
        }
        [HttpPost]
        public ActionResult themloai(Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Loais.InsertOnSubmit(loai);
                db.SubmitChanges();
            }
            return RedirectToAction("LoaiNuochoa");
        }

        [HttpGet]
        public ActionResult sualoai(int id)
        {
            Loai hang = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult sualoai(Loai loai)
        {

            if (ModelState.IsValid)
            {
              //  var L=db.Loais.SingleOrDefault(n=>n.MaLoai==loai.MaLoai)
                UpdateModel(loai);
                db.SubmitChanges();
            }
            return RedirectToAction("LoaiNuochoa");
        }
        [HttpGet]
        public ActionResult xoaloai(int id)
        {
            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            return View(loai);
        }
        [HttpPost, ActionName("xoaloai")]
        public ActionResult XnxoaHang(int id)
        {
            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            try
            {
                if (loai == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else
                {
                    db.Loais.DeleteOnSubmit(loai);
                    db.SubmitChanges();
                    ViewBag.Thongbao1 = "Đã Xóa Thành Công";
                }
            }
            catch
            {
                ViewBag.Thongbao1 = "Đã có nước hoa của loại này trong danh sách";
            }

            return RedirectToAction("LoaiNuochoa");
        }
        //////////////
        public ActionResult Hangsx(/*int? page*/)
        {
            //int pageNumber = (page ?? 1);
            //int pageSize = 5;
            //var nuochoa = db.NUOCHOAs.ToList().OrderBy(n => n.MaNH).ToPagedList(pageNumber, pageSize);
            return View(db.HANGs.ToList()/*.OrderBy(n => n.MaH).ToPagedList(pageNumber, pageSize)*/);
        }
        [HttpGet]
        public ActionResult themhang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult themhang(HANG hang)
        {
            if (ModelState.IsValid)
            {
                db.HANGs.InsertOnSubmit(hang);
                db.SubmitChanges();
            }
            return RedirectToAction("HangSX");
        }
        public ActionResult chitiethang(int id)
        {
            HANG hang = db.HANGs.Single(n => n.MaH == id);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        }

        [HttpGet]
        public ActionResult xoahang(int id)
        {
            HANG loai = db.HANGs.SingleOrDefault(n => n.MaH == id);
            return View(loai);
        }
        [HttpPost, ActionName("xoahang")]
        public ActionResult Xnxoahang(int id)
        {
            HANG loai = db.HANGs.SingleOrDefault(n => n.MaH == id);
            try
            {
                if (loai == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else
                {
                    db.HANGs.DeleteOnSubmit(loai);
                    db.SubmitChanges();
                    ViewBag.Thongbao1 = "Đã Xóa Thành Công";
                }
            }
            catch
            {
                ViewBag.Thongbao1 = "Đã có nước hoa của hãng này trong danh sách";
            }

            return RedirectToAction("HangSX");
        }
        [HttpGet]
        public ActionResult suahang(int id)
        {
            HANG hang = db.HANGs.SingleOrDefault(n => n.MaH == id);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suahang(HANG hang)
        {

            if (ModelState.IsValid)
            {
                UpdateModel(hang);
                db.SubmitChanges();
            }
            return RedirectToAction("HangSX");
        }
    }
}