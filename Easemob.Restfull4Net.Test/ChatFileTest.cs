using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Easemob.Restfull4Net.Common;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Utility.HttpUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class ChatFileTest
    {
        #region 下载临时文件
        
        private string DownloadImage()
        {
            string imgUrl = "http://www.baidu.com/img/baidu_sylogo1.gif";
            string fileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".gif");
            using (var input = new MemoryStream())
            {
                Get.FillDownload(imgUrl, input, true, null, null, ContentType.DEFAULT);
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(input))
                {
                    using (var output = new MemoryStream())
                    {
                        img.Save(fileName, ImageFormat.Gif);
                    }
                }
            }
            return fileName;
        }

        #endregion

        #region 上传语音/图片文件

        [TestMethod]
        public void UploadFileTest()
        {
            string fileName = DownloadImage();
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo(fileName).FullName, 30000);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
        }
        
        #endregion

        #region 下载语音/图片文件

        [TestMethod]
        public void DownloadFileTest()
        {
            string fileName = DownloadImage();
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo(fileName).FullName, 30000);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
            if (result.StatusCode==HttpStatusCode.OK)
            {
                //下载语音/图片文件
                using (var input = new MemoryStream())
                {
                    var result1 = Client.DefaultSyncRequest.ChatFileDownload(result.entities[0].share_secret,result.entities[0].uuid,input, 30000);
                    Assert.AreEqual(result1.StatusCode, HttpStatusCode.OK);
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(input))
                    {
                        using (var output = new MemoryStream())
                        {
                            img.Save(string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"),".gif"), ImageFormat.Gif);
                        }
                    }
                }
            }
        }
        
        #endregion

        #region 下载缩略图

        [TestMethod]
        public void DownloadThumbnailTest()
        {
            string fileName = DownloadImage();
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo(fileName).FullName, 30000);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
            if (result.StatusCode==HttpStatusCode.OK)
            {
                //下载缩略图
                using (var input = new MemoryStream())
                {
                    var result1 = Client.DefaultSyncRequest.ChatFileDownloadThumbnail(result.entities[0].share_secret,result.entities[0].uuid,input, 30000);
                    Assert.AreEqual(result1.StatusCode, HttpStatusCode.OK);
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(input))
                    {
                        using (var output = new MemoryStream())
                        {
                            img.Save(string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"),"thumbnail",".gif"), ImageFormat.Gif);
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}
