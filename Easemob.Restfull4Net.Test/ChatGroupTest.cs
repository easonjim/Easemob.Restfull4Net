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
    public class ChatGroupTest
    {
        private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名

        #region 获取 APP 中所有的群组

        [TestMethod]
        public void ChatGroupGetTest()
        {
            var allChatGroupResponse = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupGet();
            Assert.IsTrue(allChatGroupResponse.StatusCode == HttpStatusCode.OK);
        }
        
        #endregion

        #region 分页获取 APP 下的群组

        [TestMethod]
        public void ChatGroupGetByLimitTest()
        {
            var allChatGroupResponse = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupGetByLimit(1);
            Assert.IsTrue(allChatGroupResponse.StatusCode == HttpStatusCode.OK);
            if (allChatGroupResponse.StatusCode==HttpStatusCode.OK && !allChatGroupResponse.cursor.IsNullOrEmpty())
            {
                var allChatGroupResponse1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupGetByLimit(1,allChatGroupResponse.cursor);
                Assert.IsTrue(allChatGroupResponse1.StatusCode == HttpStatusCode.OK);
            }
        }
        
        #endregion

        #region 获取群组详情

        [TestMethod]
        public void ChatGroupDetailsTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
            if (create.StatusCode==HttpStatusCode.OK)
            {
                //获取详情
                var allChatGroupResponse = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupDetails(new[] { create.data.groupid});
                Assert.IsTrue(allChatGroupResponse.StatusCode == HttpStatusCode.OK);
            }
        }
        
        #endregion

        #region 创建一个群组
        
        //[TestMethod]
        public ChatGroupResponseCreate ChatGroupCreateTest()
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
                //创建群组
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupCreate(new CreateChatGroupRequest()
                {
                    approval = false,
                    desc = "test" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                    groupname = "testcreate" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
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

        #region 修改群组信息

        [TestMethod]
        public void ChatGroupUpdateTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //更新群组
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupUpdate(create.data.groupid, new UpdateChatGroupRequest()
                {
                    groupname = "testupdate",
                    maxusers = 200,
                    description = "update"
                });
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 删除群组

        [TestMethod]
        public void ChatGroupDeleteTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //删除群组
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupDelete(create.data.groupid);
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 获取群组所有成员

        [TestMethod]
        public void ChatGroupMemberAllTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //获取群组成员
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAll(create.data.groupid);
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 添加群组成员[单个]

        [TestMethod]
        public void ChatGroupMemberAddTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAdd(create.data.groupid, user.entities[0].username);
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 添加群组成员[批量]

        [TestMethod]
        public void ChatGroupMemberAddBatchTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAddBatch(create.data.groupid, new[] {user.entities[0].username, user2.entities[0].username});
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 删除群组成员[单个]

        [TestMethod]
        public void ChatGroupMemberDeleteTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAdd(create.data.groupid, user.entities[0].username);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //群组删人
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberDelete(create.data.groupid, user.entities[0].username);
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 删除群组成员[批量]

        [TestMethod]
        public void ChatGroupMemberDeleteBatchTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAddBatch(create.data.groupid, new[] {user.entities[0].username, user2.entities[0].username});
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //群组删人
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberDeleteBatch(create.data.groupid,new string[]{ user.entities[0].username,user2.entities[0].username});
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 获取一个用户参与的所有群组

        [TestMethod]
        public void ChatGroupUserTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAdd(create.data.groupid, user.entities[0].username);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //获取用户参与的群组
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupUser(user.entities[0].username);
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 转让群组

        [TestMethod]
        public void ChatGroupChangeTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupMemberAddBatch(create.data.groupid, new[] {user.entities[0].username, user2.entities[0].username});
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //群组转让
                        var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupChange(create.data.groupid,user2.entities[0].username);
                        Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                    }
                }
            }
        }

        #endregion

        #region 查询群组黑名单

        [TestMethod]
        public void ChatGroupBlockTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
            if (create.StatusCode == HttpStatusCode.OK)
            {
                //查询群组黑名单
                var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlock(create.data.groupid);
                Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            }
        }

        #endregion

        #region 添加用户至群组黑名单[单个]

        [TestMethod]
        public void ChatGroupBlockAddTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //添加用户至群组黑名单
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAdd(create.data.groupid, user.entities[0].username);
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 添加用户至群组黑名单[批量]

        [TestMethod]
        public void ChatGroupBlockAddBatchTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //添加用户至群组黑名单
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAddBatch(create.data.groupid, new[] {user.entities[0].username, user2.entities[0].username});
                    Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        #endregion

        #region 从群组黑名单移除用户[单个]

        [TestMethod]
        public void ChatGroupBlockDeleteTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAdd(create.data.groupid, user.entities[0].username);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //添加用户至群组黑名单
                        var response2 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAdd(create.data.groupid, user.entities[0].username);
                        Assert.IsTrue(response2.StatusCode == HttpStatusCode.OK);
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            //从群组黑名单移除用户
                            var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockDelete(create.data.groupid, user.entities[0].username);
                            Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                        }
                    }
                }
            }
        }

        #endregion

        #region 从群组黑名单移除用户[批量]

        [TestMethod]
        public void ChatGroupBlockDeleteBatchTest()
        {
            //先创建一个群组
            var create = ChatGroupCreateTest();
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
                    //群组加人
                    var response = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAddBatch(create.data.groupid, new[] {user.entities[0].username, user2.entities[0].username});
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //添加用户至群组黑名单
                        var response2 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockAddBatch(create.data.groupid, new[] { user.entities[0].username, user2.entities[0].username });
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            //从群组黑名单移除用户
                            var response1 = Easemob.Restfull4Net.Client.DefaultSyncRequest.ChatGroupBlockDeleteBatch(create.data.groupid, new string[] {user.entities[0].username, user2.entities[0].username});
                            Assert.IsTrue(response1.StatusCode == HttpStatusCode.OK);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
