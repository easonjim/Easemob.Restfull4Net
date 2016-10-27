using System;
using System.Net;
using System.Threading;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Entity.Response;
using Easemob.Restfull4Net.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class ChatRoomTest
    {
        private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名

        #region 获取 APP 中所有的聊天室

        [TestMethod]
        public void ChatRoomGetTest()
        {
            var allChatRoomResponse = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomGet();
            Assert.IsTrue(allChatRoomResponse.StatusCode == HttpStatusCode.OK);
        }
        
        #endregion

        #region 获取聊天室详情

        [TestMethod]
        public void ChatRoomDetailsTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode==HttpStatusCode.OK)
            {
                //获取详情
                var allChatRoomResponse = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomDetails(create.data.id);
                Assert.IsTrue(allChatRoomResponse.StatusCode == HttpStatusCode.OK);
            }
        }
        
        #endregion

        #region 创建一个聊天室
        
        //[TestMethod]
        public ChatRoomResponseCreate ChatRoomCreateTest()
        {
            //先创建用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            var user2 = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            if (user.StatusCode == HttpStatusCode.OK && user2.StatusCode == HttpStatusCode.OK)
            {
                //创建聊天室
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomCreate(new CreateChatRoomRequest()
                {
                    description = "test" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                    name = "testcreate" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                    maxusers = 100,
                    members = new string[]{user2.entities[0].username},
                    owner = user.entities[0].username,
                });
                //Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                return response;
            }
            return null;
        }
        
        #endregion

        #region 修改聊天室信息

        [TestMethod]
        public void ChatRoomUpdateTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //更新聊天室
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomUpdate(create.data.id, new UpdateChatRoomRequest()
                {
                    name = "testupdate",
                    maxusers = 200,
                    description = "update"
                });
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 删除聊天室

        [TestMethod]
        public void ChatRoomDeleteTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //删除聊天室
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomDelete(create.data.id);
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 添加聊天室成员[单个]

        [TestMethod]
        public void ChatRoomMemberAddTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //创建用户
                var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                if (user.StatusCode==HttpStatusCode.OK)
                {
                    //聊天室加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberAdd(create.data.id, user.entities[0].username);
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 添加聊天室成员[批量]

        [TestMethod]
        public void ChatRoomMemberAddBatchTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //创建用户
                var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                var user2 = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                if (user.StatusCode == HttpStatusCode.OK&&user2.StatusCode == HttpStatusCode.OK)
                {
                    //聊天室加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberAddBatch(create.data.id, new[] {user.entities[0].username, user2.entities[0].username});
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 删除聊天室成员[单个]

        [TestMethod]
        public void ChatRoomMemberDeleteTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //创建用户
                var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                if (user.StatusCode == HttpStatusCode.OK)
                {
                    //聊天室加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberAdd(create.data.id, user.entities[0].username);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //聊天室删人
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberDelete(create.data.id, user.entities[0].username);
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 删除聊天室成员[批量]

        [TestMethod]
        public void ChatRoomMemberDeleteBatchTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //创建用户
                var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                var user2 = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                if (user.StatusCode == HttpStatusCode.OK&&user2.StatusCode == HttpStatusCode.OK)
                {
                    //聊天室加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberAddBatch(create.data.id, new[] {user.entities[0].username, user2.entities[0].username});
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //聊天室删人
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberDeleteBatch(create.data.id,new string[]{ user.entities[0].username,user2.entities[0].username});
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 获取一个用户参与的所有聊天室

        [TestMethod]
        public void ChatRoomUserTest()
        {
            //先创建一个聊天室
            var create = ChatRoomCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //创建用户
                var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                });
                if (user.StatusCode == HttpStatusCode.OK)
                {
                    //聊天室加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomMemberAdd(create.data.id, user.entities[0].username);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //获取用户参与的聊天室
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatRoomUser(user.entities[0].username);
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion
    }
}
