using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace FTPuploader
{
    abstract class FileDirectory
    {
        protected TreeView treebox;
        ImageList myimagelist = new ImageList();
        Bitmap bmp = new Bitmap(Properties.Resources.small_folder);
        Bitmap bmp1 = new Bitmap(Properties.Resources.file_icon);

        protected string currentfolderpath = "";
        protected TreeNode CurrentFolder;

        protected FTPConnection ftp;

        public FileDirectory(FTPConnection ftp, Form1 f1)
        {
            this.ftp = ftp;
            treebox = new TreeView();
            //treebox.BeforeExpand +=new TreeViewCancelEventHandler(treebox_BeforeExpand);
            treebox.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treebox_NodeMouseDoubleClick);

            myimagelist.Images.Add(bmp);
            myimagelist.Images.Add(bmp1);
            treebox.ImageList = myimagelist;
        }


        /*void treebox_BeforeExpand(object sender, TreeViewCancelEventArgs tvcea)
        {
            if (tvcea.Node.Nodes[0].Text == "*")
            {
                tvcea.Node.Nodes.Clear();
                Fill(tvcea.Node);
            }
        }*/

        protected abstract void treebox_NodeMouseDoubleClick(object o, TreeNodeMouseClickEventArgs mea);

        public TreeView returnTreeView
        {
            get
            {
                return treebox; }
        }

        public FTPConnection CreateConnection
        {
            get { return ftp; }
            set { ftp = value; }
        }

        public String CurrentFolderPath
        {
            get { return currentfolderpath; }
        }
    }
}
