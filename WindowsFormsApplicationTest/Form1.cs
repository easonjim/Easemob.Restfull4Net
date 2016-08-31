using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Easemob.Restfull4Net;
using Easemob.Restfull4Net.Entity.Request;

namespace WindowsFormsApplicationTest
{
    public partial class Form1 : Form
    {
        private string _userName
        {
            get
            {
                Thread.Sleep(100);
                return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            }
        } //用户名

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //单个创建
            var syncRequest = Client.DefaultSyncRequest;

            var user = syncRequest.UserCreate(new UserCreateReqeust()
            {
                nickname = string.Concat("Test", this._userName),
                password = "123456",
                username = string.Concat("Test", this._userName),
            });
            MessageBox.Show((user.StatusCode==HttpStatusCode.OK).ToString());
        }
    }
}
