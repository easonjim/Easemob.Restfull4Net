using System;
using System.Diagnostics;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class GetTokenTest
    {
        [TestMethod]
        public void TestGetToken()
        {
            var token = Client.DefaultSyncRequest.GetToken();
            Debug.WriteLine(token);
            Assert.IsFalse(token.IsNullOrEmpty());
        }
    }
}
