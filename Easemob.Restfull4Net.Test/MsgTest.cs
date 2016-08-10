using System;
using System.Net;
using System.Threading;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class MsgTest
    {
        private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名

        #region 发送消息

        [TestMethod]
        public void SendTextMsgTest()
        {
            //发送文本消息
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode==HttpStatusCode.OK)
            {
                Thread.Sleep(1000);//刚创建的用户要休息1秒再发送
                var send = Client.DefaultSyncRequest.MsgSend<MsgText>(new MsgRequest<MsgText>()
                {
                   target_type="users",
                   from="admin",
                   msg = new MsgText()
                   {
                       msg = "hello"
                   },
                   target = new string[] { user.entities[0].username}
                });
                Assert.AreEqual(send.StatusCode,HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void SendImgMsgTest()
        {
            //发送图片消息
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                //上传语音/图片文件
                var result = Client.DefaultSyncRequest.ChatFileUpload(FileHelper.GetFileInfo("9a4d7c1710652ab80ef2c8e249bd0b55.jpg").FullName, 30000);
                Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Thread.Sleep(1000);//刚创建的用户要休息1秒再发送
                    var send = Client.DefaultSyncRequest.MsgSend<MsgImg>(new MsgRequest<MsgImg>()
                    {
                        target_type = "users",
                        from = "admin",
                        msg = new MsgImg()
                        {
                            filename = "9a4d7c1710652ab80ef2c8e249bd0b55.jpg",
                            secret = result.entities[0].share_secret,
                            size = new MsgImgSize() {height = 100, width = 100},
                            url = string.Concat(result.uri, "/", result.entities[0].uuid)
                        },
                        target = new string[] {user.entities[0].username}
                    });
                    Assert.AreEqual(send.StatusCode, HttpStatusCode.OK);
                }
            }
        }

        #endregion
    }
}
