using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class ChatFileTest
    {
        #region 上传语音/图片文件

        [TestMethod]
        public void UploadFileTest()
        {
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo("9a4d7c1710652ab80ef2c8e249bd0b55.jpg").FullName,30000);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
        }
        
        #endregion

        #region 下载语音/图片文件

        [TestMethod]
        public void DownloadFileTest()
        {
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo("9a4d7c1710652ab80ef2c8e249bd0b55.jpg").FullName,30000);
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
                            img.Save(string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"),".jpg"), ImageFormat.Jpeg);
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
            //上传语音/图片文件
            var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo("9a4d7c1710652ab80ef2c8e249bd0b55.jpg").FullName,30000);
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
                            img.Save(string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"),"thumbnail",".jpg"), ImageFormat.Jpeg);
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}
