using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using Easemob.Restfull4Net.Common;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Utility.HttpUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easemob.Restfull4Net.Test
{
    [TestClass]
    public class LogTraceHelper
    {
        [TestMethod]
        public void UseTheedWriterTest()
        {
            try
            {
                ThreadStart num = new ThreadStart(PrintNum);
                Thread ConstrolNum = new Thread(num);
                ThreadStart str = new ThreadStart(PrintStr);
                Thread ConstrolStr = new Thread(str);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                ConstrolNum.Start();
                ConstrolStr.Start();
                while (true)
                {
                    if (ConstrolNum.ThreadState == System.Threading.ThreadState.Stopped && ConstrolStr.ThreadState == System.Threading.ThreadState.Stopped)
                    {
                        watch.Stop();
                        Helper.LogTraceHelper.SendLog(string.Concat("UseTheedWriter", watch.Elapsed.TotalMilliseconds), null, null, null, null, "UseTheedWriterTest", true);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + ex.StackTrace);                
            }
            
        }

        private static void PrintNum()
        {
            for (int i = 1; i < 1000; i++)
            {
                try
                {
                    Helper.LogTraceHelper.SendLog(string.Concat("PrintNum", i.ToString()), null, null, null, null, "UseTheedWriterTest", true);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message + ex.StackTrace);
                }
            }
        }
        private static void PrintStr()
        {
            for (int i = 1; i < 1000; i++)
            {
                try
                {
                    Helper.LogTraceHelper.SendLog(string.Concat("PrintStr", i.ToString()), null, null, null, null, "UseTheedWriterTest", true);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
