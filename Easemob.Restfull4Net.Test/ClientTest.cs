using System;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //通过配置文件实例化
            var serverConfig = Client.ServerConfigs;
            var defaultSyncReqeust = Client.DefaultSyncRequest;
            var syncReqeusts = Client.ServerConfigs;
            Assert.IsTrue(serverConfig.Count>0);
            Assert.IsTrue(defaultSyncReqeust!=null);
            Assert.IsTrue(syncReqeusts.Count>0);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //自定义实例化
            var syncRequest = new SyncRequest(new ServerConfig()
            {
                OrgName = "dotnetsdk",
                AppName = "app1",
                ClientId = "YXA6N9H0kFu6EeaqLQ1pVvXmDw",
                ClientSecret = "YXA6Ald8ijVmKwAtpH5CAMrnxbD6ad0",
                IsDebug = true,
            });

            Assert.IsNotNull(syncRequest.GetToken());
        }
    }
}
