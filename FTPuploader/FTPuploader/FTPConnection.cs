using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FTPuploader
{
    class FTPConnection
    {
        string[] serverdata_;
        string connection;
        List<string> directorydata = new List<string>();



        public FTPConnection(string[] serverdata)
        {
            serverdata_ = serverdata;
            connection = "ftp://" + serverdata_[4];
        }

        public FTPConnection()
        {
        }

        public List<string> ListServerDirectory()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connection);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(serverdata_[2], serverdata_[3]);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responsestream = response.GetResponseStream();
            using(StreamReader sr = new StreamReader(responsestream))
            {
                string line = sr.ReadLine();

                while (line != null)
                {
                    directorydata.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            response.Close();
            return directorydata;
        }

        public void UpLoad(string FileName, string SourceFile)
        {
            string filelocation = connection + "/" + FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filelocation);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(serverdata_[2], serverdata_[3]);
            byte[] b = File.ReadAllBytes(SourceFile);
            request.ContentLength = b.Length;

            using (Stream s = request.GetRequestStream())
            {
                s.Write(b, 0, b.Length);
            }

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            response.Close();

        }

        public void Delete(string FileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connection + "/" + FileName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(serverdata_[2], serverdata_[3]);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
        }

        public void Download(string FileName, string saveloc)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connection + "/" + FileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(serverdata_[2], serverdata_[3]);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream readresponse = response.GetResponseStream();
            FileStream fs = new FileStream(saveloc +"/"+ FileName, FileMode.Create);

            int length= 2048;
            Byte[] buffer = new Byte[length];

            int BufferSize = readresponse.Read(buffer, 0, length);
            while (BufferSize > 0)
            {
                fs.Write(buffer, 0, BufferSize);
                BufferSize = readresponse.Read(buffer, 0, length);
            }

            readresponse.Close();
            fs.Close();
            Console.WriteLine(response.StatusDescription);
            Console.WriteLine(saveloc);

        }



        public String Connectionstring
        {
            get { return connection; }
            set { connection = value; }
        }

        public String HostFTPstring
        {
            get { return "ftp://" + serverdata_[4]; }
        }
    }
}
