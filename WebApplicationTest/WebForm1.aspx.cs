using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Easemob.Restfull4Net;
using Easemob.Restfull4Net.Entity.Request;

namespace WebApplicationTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //单个创建
            var syncRequest = Client.DefaultSyncRequest;

            var user = syncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            Response.Write((user.StatusCode == HttpStatusCode.OK).ToString());
        }
    }
}