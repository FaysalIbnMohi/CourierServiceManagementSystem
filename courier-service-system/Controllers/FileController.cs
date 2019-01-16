using JOBS.Entities;
using JOBS.Framework.Utils;
using JOBS.Web.Ui.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOBS.Web.Ui.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public string _error = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveProfileCropImage()
        {
            string resumeId = _Util.Facade.CreateCVFacade.GetResumeId(User.Identity.Name);
            Image orgImg = null;
            int x = 0, y = 0, w = 0, h = 0;
            string coordinate = "";
            string ImageUrl = Request["ImageUrl"];
            if (string.IsNullOrWhiteSpace(ImageUrl))
            {
                return Json(new { isUploaded = false, message = "please try again" }, "text/html");
            }
            string ImagePath = Server.MapPath("~" + ImageUrl);


            #region Getting Image from drive 
            try
            {
                orgImg = Image.FromFile(ImagePath);
            }
            catch (Exception)
            {
                return Json(new { isUploaded = false, message = "please try again" }, "text/html");
            }
            #endregion
            var size = new FileInfo(ImagePath).Length;
            if ((size > (7 * 1024 * 1024)) || (orgImg.Height > 5000 || orgImg.Width > 5000))
            {
                orgImg.Dispose();
                return Json(new { isUploaded = false, message = "size limit exceeded" }, "text/html");
            }

            string name = Request["UserFullName"].ToString();
            string UserName = Request["UserName"].ToString();
            string extention = Path.GetExtension(ImagePath); //Path.GetExtension(httpPostedFileBase.FileName);
            name = name.Replace(' ', '-');
            string fileName = "Profile-" + name + "-{0}-" + DateTime.Now.Ticks + "{1}";
            bool isUploaded = false;
            string profileFolderName = AppConfig.ProfileImageFolder;
            string savethumbPath = string.Concat("~/", profileFolderName, resumeId, "/", string.Format(fileName, "thumb", extention));
            //string savefullPath = string.Concat("~/", profileFolderName, "/", string.Format(fileName, "full", extention));
            string returnPath = ""; 
            int resizeHeight = Convert.ToInt32(Request["hdnheight"].ToString());
            int resizeWidth = Convert.ToInt32(Request["hdnwidth"].ToString());
            if (Request["x"] != "" && Request["y"] != "" && Request["w"] != "" && Request["h"] != "")
            {
                x = Convert.ToInt32(Request["x"].ToString());
                y = Convert.ToInt32(Request["y"].ToString());
                w = Convert.ToInt32(Request["w"].ToString());
                h = Convert.ToInt32(Request["h"].ToString());

                double ratiow = (double)orgImg.Width / resizeWidth;
                double ratioh = (double)orgImg.Height / resizeHeight;

                coordinate = Math.Round(x * ratiow, 2) + "," + Math.Round(y * ratiow, 2) + "," + Math.Round(w * ratiow, 2) + "," + Math.Round(h * ratiow, 2);
            }

            
            string _ERRORMESSAGE = string.Empty;
            
                try
                {
                    string propertyFolderPath = Server.MapPath("~/" + profileFolderName);
                    if (Request["x"] != "" && Request["y"] != "" && Request["w"] != "" && Request["h"] != "")
                    {
                        //Image orgImg = Image.FromStream(httpPostedFileBase.InputStream, true, true);

                        Bitmap orgImgresize = ImageHelper.GetImageResize(resizeWidth, resizeHeight, orgImg);

                        Rectangle CropArea = new Rectangle(x, y, w, h);
                        Bitmap bitMap = new Bitmap(CropArea.Width, CropArea.Height);
                        using (Graphics g = Graphics.FromImage(bitMap))
                        {
                            g.DrawImage(orgImgresize, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
                        }
                        if (FileHelper.CreateFolderIfNeeded(propertyFolderPath))
                        {

                            var qualityEncoder = Encoder.Quality;
                            var quality = (long)90;
                            var ratio = new EncoderParameter(qualityEncoder, quality);
                            var codecParams = new EncoderParameters(1);
                            codecParams.Param[0] = ratio;
                            var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        //orgImg.Save(Server.MapPath(savefullPath), GetEncoder(ImageFormat.Jpeg), codecParams);
                            PersonalDetail _PersonalDetail = _Util.Facade.CreateCVFacade.GetPersonalDetailByResumeId(resumeId);
                            returnPath = savethumbPath.TrimStart('~');
                        _PersonalDetail.ThumbProfilePicture = returnPath;
                            _Util.Facade.CreateCVFacade.UpdatePersonalDetails(_PersonalDetail, out _error);
                            bitMap.Save(Server.MapPath(savethumbPath), GetEncoder(ImageFormat.Jpeg), codecParams);
                            codecParams.Dispose();
                            orgImg.Dispose();
                            bitMap.Dispose();
                           // returnPath = savethumbPath.TrimStart('~');

                            //Update Resumie detail Table with Thumb
                        }
                    }
                    
                    isUploaded = true;
                    //try
                    //{
                    //    if (System.IO.File.Exists(ImagePath))
                    //    {
                    //        System.IO.File.Delete(ImagePath);
                    //    }
                    //}
                    //catch (Exception) { }

                }
                catch (Exception)
                {
                    return Json(new { isUploaded = false, message = "please try again" }, "text/html");
                }

            
            return Json(new { isUploaded, filePath = returnPath }, "text/html"); 
    }
        public JsonResult SaveCompanyProfileCropImage()
        {
            string ComanyId = _Util.Facade.CompanyRegistrationFacade.GetCompanyIdByUsername(User.Identity.Name).ToString();
            Image orgImg = null;
            int x = 0, y = 0, w = 0, h = 0;
            string coordinate = "";
            string ImageUrl = Request["ImageUrl"];
            if (string.IsNullOrWhiteSpace(ImageUrl))
            {
                return Json(new { isUploaded = false, message = "please try again" }, "text/html");
            }
            string ImagePath = Server.MapPath("~" + ImageUrl);


            #region Getting Image from drive 
            try
            {
                orgImg = Image.FromFile(ImagePath);
            }
            catch (Exception)
            {
                return Json(new { isUploaded = false, message = "please try again" }, "text/html");
            }
            #endregion
            var size = new FileInfo(ImagePath).Length;
            if ((size > (7 * 1024 * 1024)) || (orgImg.Height > 5000 || orgImg.Width > 5000))
            {
                orgImg.Dispose();
                return Json(new { isUploaded = false, message = "size limit exceeded" }, "text/html");
            }

            string name = Request["UserFullName"].ToString();
            string UserName = Request["UserName"].ToString();
            string extention = Path.GetExtension(ImagePath); //Path.GetExtension(httpPostedFileBase.FileName);
            string fileName = "Profile-" + name + "-{0}-" + DateTime.Now.Ticks + "{1}";
            bool isUploaded = false;
            string profileFolderName = AppConfig.CompanyProfileImageFolder;
            string savethumbPath = string.Concat("~/", profileFolderName, ComanyId, "/", string.Format(fileName, "thumb", extention));
            //string savefullPath = string.Concat("~/", profileFolderName, "/", string.Format(fileName, "full", extention));
            string returnPath = "";
            int resizeHeight = Convert.ToInt32(Request["hdnheight"].ToString());
            int resizeWidth = Convert.ToInt32(Request["hdnwidth"].ToString());
            if (Request["x"] != "" && Request["y"] != "" && Request["w"] != "" && Request["h"] != "")
            {
                x = Convert.ToInt32(Request["x"].ToString());
                y = Convert.ToInt32(Request["y"].ToString());
                w = Convert.ToInt32(Request["w"].ToString());
                h = Convert.ToInt32(Request["h"].ToString());

                double ratiow = (double)orgImg.Width / resizeWidth;
                double ratioh = (double)orgImg.Height / resizeHeight;

                coordinate = Math.Round(x * ratiow, 2) + "," + Math.Round(y * ratiow, 2) + "," + Math.Round(w * ratiow, 2) + "," + Math.Round(h * ratiow, 2);
            }


            string _ERRORMESSAGE = string.Empty;

            try
            {
                string propertyFolderPath = Server.MapPath("~/" + profileFolderName);
                if (Request["x"] != "" && Request["y"] != "" && Request["w"] != "" && Request["h"] != "")
                {
                    //Image orgImg = Image.FromStream(httpPostedFileBase.InputStream, true, true);

                    Bitmap orgImgresize = ImageHelper.GetImageResize(resizeWidth, resizeHeight, orgImg);

                    Rectangle CropArea = new Rectangle(x, y, w, h);
                    Bitmap bitMap = new Bitmap(CropArea.Width, CropArea.Height);
                    using (Graphics g = Graphics.FromImage(bitMap))
                    {
                        g.DrawImage(orgImgresize, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
                    }
                    if (FileHelper.CreateFolderIfNeeded(propertyFolderPath))
                    {

                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)90;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        //orgImg.Save(Server.MapPath(savefullPath), GetEncoder(ImageFormat.Jpeg), codecParams);
                        //PersonalDetail _PersonalDetail = _Util.Facade.CreateCVFacade.GetPersonalDetailByResumeId(ComanyId);
                        returnPath = savethumbPath.TrimStart('~');
                        CompanyInformation _CompanyInformation = _Util.Facade.CompanyRegistrationFacade.GetCompanyInfoByCompanyId(ComanyId);
                        _CompanyInformation.CompanyLogo = returnPath;
                        _Util.Facade.CompanyRegistrationFacade.UpdateCompany(_CompanyInformation, out _error);
                        bitMap.Save(Server.MapPath(savethumbPath), GetEncoder(ImageFormat.Jpeg), codecParams);
                        codecParams.Dispose();
                        orgImg.Dispose();
                        bitMap.Dispose();
                        // returnPath = savethumbPath.TrimStart('~');

                        //Update Resumie detail Table with Thumb
                    }
                }

                isUploaded = true;
                try
                {
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }
                catch (Exception) { }

            }
            catch (Exception)
            {
                return Json(new { isUploaded = false, message = "please try again" }, "text/html");
            }


            return Json(new { isUploaded, filePath = returnPath }, "text/html");
        }
        public JsonResult UploadProfilePicture()
        {
            HttpPostedFileBase httpPostedFileBase = Request.Files["ImageFile"];
            string resumeId = Request["resumeId"].ToString();
            string returnfilename = "";
            bool result = false;
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string extention = Path.GetExtension(httpPostedFileBase.FileName);
                string fileName = resumeId + "-{0}-" + DateTime.Now.Ticks + "{1}";
                string imagefolder = AppConfig.ProfileImageFolder;

                string profileImageFolderPath = Server.MapPath("~/" + imagefolder + resumeId);
                returnfilename = imagefolder + resumeId + "/" + string.Format(fileName, "full", extention);
                if (FileHelper.CreateFolderIfNeeded(profileImageFolderPath))
                {
                    try
                    {
                        Image orgImg = Bitmap.FromStream(httpPostedFileBase.InputStream);
                        foreach (var prop in orgImg.PropertyItems)
                        {
                            if (prop.Id == 0x0112) //value of EXIF
                            {
                                int orientationValue = orgImg.GetPropertyItem(prop.Id).Value[0];
                                RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                                orgImg.RotateFlip(rotateFlipType);

                                break;
                            }
                        }
                        if (orgImg.Height > 300 || orgImg.Width > 300)
                        {
                            orgImg = ImageHelper.GetImageResize(300 ,300, orgImg);
                        }
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        PersonalDetail _PersonalDetail = _Util.Facade.CreateCVFacade.GetPersonalDetailByResumeId(resumeId);
                        _PersonalDetail.ProfilePicture = "/" + returnfilename;
                        _Util.Facade.CreateCVFacade.UpdatePersonalDetails(_PersonalDetail, out _error);
                        orgImg.Save(Path.Combine(profileImageFolderPath, string.Format(fileName, "full", extention)), GetEncoder(ImageFormat.Jpeg), codecParams);
                        result = true;
                    }
                    catch(Exception ex)
                    {
                        result = false;
                    }


                }
            }

            return Json(new { success= result, filename = returnfilename });
        }
        public JsonResult UploadResume()
        {
            HttpPostedFileBase httpPostedFileBase = Request.Files["myPDF"];
            string resumeId = _Util.Facade.CreateCVFacade.GetResumeId(User.Identity.Name);
            PersonalDetail _PersonalDetail = _Util.Facade.CreateCVFacade.GetPersonalDetailByResumeId(resumeId);
            string returnfilename = "";
            bool result = false;
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string extention = Path.GetExtension(httpPostedFileBase.FileName);
                if (extention == ".pdf" || extention == ".doc" || extention == ".docx")
                {
                    _PersonalDetail.Name = _PersonalDetail.Name.Replace(' ', '-');
                    int count = 0;
                    string fileName = "";
                    if (_Util.Facade.CreateCVFacade.GetAllResumeByResumeId(resumeId).Count != 0)
                    {
                        ResumeFile resume = _Util.Facade.CreateCVFacade.GetAllResumeByResumeId(resumeId).Last();
                        resume.FileName = Path.GetFileName(resume.ResumeLocation);
                        var split = resume.FileName.Split('.');
                        split = split[0].Split('-');
                        //split = split[1];
                        count = Convert.ToInt32(split[3]);
                        fileName = _PersonalDetail.Name + "{0}-" + ++count + "{1}";
                    }
                    else
                    {
                        fileName = _PersonalDetail.Name + "{0}-" + ++count + "{1}";
                    }
                    string resumefolder = AppConfig.UserResumeFolder;

                    string profileImageFolderPath = Server.MapPath("~/" + resumefolder + resumeId);
                    returnfilename = resumefolder + resumeId + "/" + string.Format(fileName, "full", extention);
                    ResumeFile resumeFile = new ResumeFile();
                    resumeFile.ResumeId = new Guid(resumeId);
                    resumeFile.ResumeLocation = "/" + returnfilename;
                    resumeFile.IsActive = true;
                    _Util.Facade.CreateCVFacade.InsertResume(resumeFile, out _error);
                    if (FileHelper.CreateFolderIfNeeded(profileImageFolderPath))
                    {
                        var path = Path.Combine(profileImageFolderPath, string.Format(fileName, "full", extention));
                        httpPostedFileBase.SaveAs(path);
                        result = true;
                    }
                }
                else
                {
                    result = false;
                    return Json(new { success = result, filename = "Invalid" });
                }
            }

            return Json(new { success = result, filename = returnfilename });
        }
        public JsonResult UploadCompanyLogo()
        {
            HttpPostedFileBase httpPostedFileBase = Request.Files["ImageFile"];
            string companyId = Request["companyId"].ToString();
            string returnfilename = "";
            bool result = false;
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string extention = Path.GetExtension(httpPostedFileBase.FileName);
                string fileName = companyId + "-{0}-" + DateTime.Now.Ticks + "{1}";
                string imagefolder = AppConfig.CompanyProfileImageFolder;

                string profileImageFolderPath = Server.MapPath("~/" + imagefolder + companyId);
                returnfilename = imagefolder + companyId + "/" + string.Format(fileName, "full", extention);
                if (FileHelper.CreateFolderIfNeeded(profileImageFolderPath))
                {
                    try
                    {
                        Image orgImg = Bitmap.FromStream(httpPostedFileBase.InputStream);
                        foreach (var prop in orgImg.PropertyItems)
                        {
                            if (prop.Id == 0x0112) //value of EXIF
                            {
                                int orientationValue = orgImg.GetPropertyItem(prop.Id).Value[0];
                                RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                                orgImg.RotateFlip(rotateFlipType);

                                break;
                            }
                        }
                        if (orgImg.Height > 300 || orgImg.Width > 300)
                        {
                            orgImg = ImageHelper.GetImageResize(300, 300, orgImg);
                        }
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        CompanyInformation _CompanyInformation = _Util.Facade.CompanyRegistrationFacade.GetCompanyInfoByCompanyId(companyId);
                        _CompanyInformation.CompanyLogo = "/" + returnfilename;
                        _Util.Facade.CompanyRegistrationFacade.UpdateCompany(_CompanyInformation, out _error);
                        orgImg.Save(Path.Combine(profileImageFolderPath, string.Format(fileName, "full", extention)), GetEncoder(ImageFormat.Jpeg), codecParams);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                    }


                }
            }

            return Json(new { success = result, filename = returnfilename });
        }
        
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }
    }
}