using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAF_OpticalFailureDetector.threading
{
    public delegate void ThreadErrorHandler(object sender, ThreadErrorEventArgs e);

    public class ThreadErrorEventArgs : EventArgs
    {
        private string errMsg;
        private Exception ex;
        private bool stoppingThread;

        public ThreadErrorEventArgs(string errMsg, Exception ex, bool stopping)
        {
            this.errMsg = errMsg;
            this.ex = ex;
            this.stoppingThread = stopping;
        }

        public ThreadErrorEventArgs(string errMsg, bool stopping)
        {
            this.errMsg = errMsg;
            this.ex = null;
            this.stoppingThread = stopping;
        }

        public ThreadErrorEventArgs(string errMsg, Exception ex)
        {
            this.errMsg = errMsg;
            this.ex = ex;
            stoppingThread = false;
        }

        public ThreadErrorEventArgs(string errMsg)
        {
            this.errMsg = errMsg;
            this.ex = null;
            stoppingThread = false;
        }

        public string ErrMsg
        {
            get { return errMsg; }
        }

        public Exception Ex
        {
            get { return ex; }
        }

        public bool StoppingThread
        {
            get { return stoppingThread; }
        }

        public void ShowErrorMsgBoxEx()
        {
            string err = errMsg;
            if (ex != null)
            {
                err += " " + ex.ToString();
            }
            MessageBox.Show(err, "Thread Error");
        }

        public void ShowErrorMsgBoxNoEx()
        {
            MessageBox.Show(errMsg, "Thread Error");
        }

        public static void OnThreadError(object sender, ThreadErrorHandler handler, ThreadErrorEventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
