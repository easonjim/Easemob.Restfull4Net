using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class ChatMsgTest
    {
        #region 聊天记录

        [TestMethod]
        public void ExportChatMsgTest1()
        {
            //获取聊天记录
            var user = Client.DefaultSyncRequest.ChatMsgExport("select * where timestamp>1403164734226");
            Assert.IsTrue(user.StatusCode == HttpStatusCode.OK);
        }
        
        #endregion
    }
}
