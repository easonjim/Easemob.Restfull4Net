using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class UserTest
    {private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名

        #region 用户体系

        #region IM 用户管理

        [TestMethod]
        public void UserCreateTest1()
        {
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void UserCreateTest2()
        {
            //批量创建
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName),
                    password = "123456",
                    username = string.Concat("Test", this._userName),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetUserTest1()
        {
            //获取单个用户
            //先创建一个临时用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            var user1 = Client.DefaultSyncRequest.UserGet(user.entities[0].username);
            Assert.IsTrue(user1.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetUserTest2()
        {
            //获取单个用户
            var user = Client.DefaultSyncRequest.UserGetByLimit(2, "");
            Assert.IsTrue(user.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public void DeleteUserTest1()
        {
            //删除单个用户
            //先创建一个临时用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            //再进行删除
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var delete = Client.DefaultSyncRequest.UserDelete(user.entities[0].username);
                Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void DeleteUserTest2()
        {
            //删除单个用户
            //先创建一个临时用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            //再进行删除
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var delete = Client.DefaultSyncRequest.UserDeleteByLimit(1);
                Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void ResetPassword()
        {
            //重置密码
            //先创建一个临时用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);

            if (user.StatusCode == HttpStatusCode.OK)
            {
                var delete = Client.DefaultSyncRequest.UserRestPassword(new UserPasswordRestRequest() { newpassword = "654321", username = user.entities[0].username });
                Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void ResetNickName()
        {
            //修改用户昵称
            //先创建一个临时用户
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);

            if (user.StatusCode == HttpStatusCode.OK)
            {
                var delete = Client.DefaultSyncRequest.UserRestNickName(new UserNickNameRestRequest() { nickname = "654321", username = user.entities[0].username });
                Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            }
        }

        #endregion

        #region 好友与黑名单

        [TestMethod]
        public void UserFriendAddTest()
        {
            //给 IM 用户添加好友
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserFriendAdd(new UserFriendAddRequest() { friend_username = user.entities[1].username, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void UserFriendDeleteTest()
        {
            //解除 IM 用户的好友关系
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserFriendAdd(new UserFriendAddRequest() { friend_username = user.entities[1].username, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
                if (add.StatusCode == HttpStatusCode.OK)
                {
                    var delete = Client.DefaultSyncRequest.UserFriendDelete(new UserFriendDeleteRequest() { friend_username = user.entities[1].username, owner_username = user.entities[0].username });
                    Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
                }
            }
        }

        [TestMethod]
        public void GetFriendTest1()
        {
            //查看好友
            //给 IM 用户添加好友
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserFriendAdd(new UserFriendAddRequest() { friend_username = user.entities[1].username, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
                if (add.StatusCode == HttpStatusCode.OK)
                {
                    var user1 = Client.DefaultSyncRequest.UserFriendGet(add.entities[0].username);
                    Assert.IsTrue(user1.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        [TestMethod]
        public void GetBlockTest1()
        {
            //查看黑名单
            //给 IM 用户添加好友
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserBlockAdd(new UserBlockAddRequest() { usernames = new string[] { user.entities[1].username }, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
                if (add.StatusCode == HttpStatusCode.OK)
                {
                    var user1 = Client.DefaultSyncRequest.UserBlockGet(user.entities[0].username);
                    Assert.IsTrue(user1.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        [TestMethod]
        public void AddBlockTest1()
        {
            //添加黑名单
            //给 IM 用户添加好友
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserBlockAdd(new UserBlockAddRequest() { usernames = new string[] { user.entities[1].username }, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void DeleteBlockTest1()
        {
            //添加黑名单
            //给 IM 用户添加好友
            var user = Client.DefaultSyncRequest.UserCreate(new List<UserCreateReqeust>()
            {
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"1"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"1"),
                },
                new UserCreateReqeust()
                {
                    nickname = string.Concat("Test", this._userName,"2"),
                    password = "123456",
                    username = string.Concat("Test", this._userName,"2"),
                }
            });
            Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
            if (user.StatusCode == HttpStatusCode.OK)
            {
                var add = Client.DefaultSyncRequest.UserBlockAdd(new UserBlockAddRequest() { usernames = new string[] { user.entities[1].username }, owner_username = user.entities[0].username });
                Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
                if (add.StatusCode == HttpStatusCode.OK)
                {
                    var delete = Client.DefaultSyncRequest.UserBlockDelete(new UserBlockDeleteRequest() { blocked_username = user.entities[1].username, owner_username = user.entities[0].username });
                    Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
                }
            }
        }
        #endregion

        #region 在线与离线

        [TestMethod]
        public void GetUserStatusTest1()
        {
            //查看用户在线状态
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
                var status = Client.DefaultSyncRequest.UserStatusGet(user.entities[0].username);
                Assert.AreEqual(status.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void GetUserOfflineMsgCountTest1()
        {
            //查看用户在线状态
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
                //发送消息
                var send = Client.DefaultSyncRequest.MsgSend<MsgText>(new MsgRequest<MsgText>()
                {
                    target_type = "users",
                    from = "admin",
                    msg = new MsgText()
                    {
                        msg = "hello"
                    },
                    target = new string[] { user.entities[0].username }
                });
                Assert.AreEqual(send.StatusCode, HttpStatusCode.OK);
                Thread.Sleep(1000);//休息1秒
                //拿取消息
                var status = Client.DefaultSyncRequest.UserOfflineMsgCountGet(user.entities[0].username);
                Assert.IsTrue(status.data[user.entities[0].username].SafeInt(0)>0);
            }
        }

        [TestMethod]
        public void GetUserOfflineMsgStatustTest1()
        {
            //查询某条离线消息状态
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            //发送消息
            var send = Client.DefaultSyncRequest.MsgSend<MsgText>(new MsgRequest<MsgText>()
            {
                target_type = "users",
                from = "admin",
                msg = new MsgText()
                {
                    msg = "hello"
                },
                target = new string[] { user.entities[0].username }
            });
            Assert.AreEqual(send.StatusCode, HttpStatusCode.OK);
            Thread.Sleep(1000);//休息1秒
            //获取聊天记录
            var chatMsg = Client.DefaultSyncRequest.ChatMsgExport("select * where timestamp>1403164734226");
            Assert.IsTrue(chatMsg.StatusCode == HttpStatusCode.OK);
            if (chatMsg.StatusCode == HttpStatusCode.OK)
            {
                //获取离线消息状态
                var status = Client.DefaultSyncRequest.UserOfflineMsgStatust(new UserOfflineMsgStatusGetRequest()
                {
                    msg_id = chatMsg.entities[0].msg_id,
                    username = user.entities[0].username
                });
                Assert.AreEqual(status.StatusCode, HttpStatusCode.OK);
            }
            

        }
        #endregion

        #region 账号禁用与解禁

        [TestMethod]
        public void SetUserDeactivateTest1()
        {
            //用户账号禁用
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });

            var status = Client.DefaultSyncRequest.UserSetDeactivate(user.entities[0].username);
            Assert.AreEqual(status.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        public void SetUserActivateTest1()
        {
            //用户账号解禁
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });

            var status = Client.DefaultSyncRequest.UserSetActivate(user.entities[0].username);
            Assert.AreEqual(status.StatusCode, HttpStatusCode.OK);

        }

        #endregion

        #region 强制用户下线

        [TestMethod]
        public void SetUserDisconnectTest1()
        {
            //强制用户下线
            //单个创建
            var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });

            var status = Client.DefaultSyncRequest.UserSetDeactivate(user.entities[0].username);
            Assert.AreEqual(status.StatusCode, HttpStatusCode.OK);

        }

        #endregion
        
        #endregion
    }
}
