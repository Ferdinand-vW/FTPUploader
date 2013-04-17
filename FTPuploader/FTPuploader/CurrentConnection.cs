using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPuploader
{
    class CurrentConnection
    {
        FTPConnection ftp;

        public void UpdateConnection(string connectionstring)
        {
            ftp.Connectionstring = connectionstring;
        }

        public void NewConnection(FTPConnection ftp_)
        {
            ftp = ftp_;
        }

        public FTPConnection ReturnFTP
        {
            get { return ftp; }
        }
    }
}
