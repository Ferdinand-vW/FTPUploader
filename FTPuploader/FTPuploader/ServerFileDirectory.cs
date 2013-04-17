using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FTPuploader
{
    class ServerFileDirectory:FileDirectory
    {

        TreeNode lastNode;
        TreeNode root;
        List<string> Paths_ = new List<string>();

        Form1 F1;

        public ServerFileDirectory(FTPConnection ftp, Form1 f1): base(ftp, f1)
        {
            treebox.AfterExpand += new TreeViewEventHandler(treebox_AfterExpand);
            treebox.KeyDown +=new KeyEventHandler(treebox_KeyDown);
            treebox.NodeMouseClick+=new TreeNodeMouseClickEventHandler(treebox_NodeMouseClick);
            Paths_ = this.ftp.ListServerDirectory();
            root = new TreeNode("Server");
            root.Name = "Server";
            ListDirectory(root);
            F1 = f1;
        }

        void Fill(TreeNode tn)
        {
            if (Paths_.Count >1)
            {
                for (int i = 1; i < Paths_.Count; i++)
                {
                    TreeNode newtreenode = new TreeNode(Paths_[i]);
                    newtreenode.Name = "/" + Paths_[i];
                    if (Path.HasExtension(Paths_[i]) == false)
                    {
                        newtreenode.ImageIndex = 0;
                        newtreenode.Nodes.Add("Loading..");
                        newtreenode.SelectedImageIndex = 0;
                    }
                    else
                    {

                        newtreenode.ImageIndex = 1;
                        newtreenode.SelectedImageIndex = 1;
                    }
                    tn.Nodes.Add(newtreenode);
                }
            }
            else
            {

                TreeNode returnnode = new TreeNode("..");
                returnnode.Name = "/..";
                returnnode.ImageIndex = 0;
                returnnode.SelectedImageIndex = 0;
                tn.Nodes.Add(returnnode);

            }
        }

        void ListDirectory(TreeNode tn)
        {
            treebox.Nodes.Add(tn);
            Fill(tn);
            treebox.Nodes[0].Expand();
            Console.WriteLine("check");
        }

        void treebox_AfterExpand(object o, TreeViewEventArgs tvea)
        {
            if (treebox.Nodes[0] != tvea.Node)
            {
                if (Path.HasExtension(tvea.Node.Name) == false)
                {
                    Paths_.RemoveRange(0, Paths_.Count);
                    string connect = ftp.Connectionstring;
                    TreeNode basenode;
                    if (tvea.Node.Name.Contains(".."))
                    {
                        string sub = treebox.Nodes[0].Text;

                        sub = sub.Substring(root.Text.Length, sub.Length - root.Name.Length - lastNode.Name.Length);
                        Console.WriteLine(ftp.HostFTPstring);
                        connect = ftp.HostFTPstring + sub;
                        Console.WriteLine(connect);
                        basenode = new TreeNode(root.Text +sub);
                        lastNode = new TreeNode(sub);
                        lastNode.Name = sub;
                    }
                    else
                    {
                        connect += tvea.Node.Name;
                        basenode = new TreeNode(treebox.Nodes[0].Name + tvea.Node.Name);
                        lastNode = tvea.Node;
                    }

                    ftp.Connectionstring = connect;
                    Paths_ = ftp.ListServerDirectory();

                    basenode.Name = basenode.Text;
                    treebox.Nodes.Clear();
                    ListDirectory(basenode);
                }
            }

        }

        protected override void treebox_NodeMouseDoubleClick(object o, TreeNodeMouseClickEventArgs mea)
        {
        }

        void treebox_AfterSelect(object sender, TreeViewEventArgs tvea)
        {
        }

        void treebox_KeyDown(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode == Keys.Delete)
            {
                foreach (TreeNode tn in treebox.Nodes[0].Nodes)
                {
                    if (tn.IsSelected == true)
                    {
                        ftp.Delete(tn.Text);
                        tn.Remove();
                        break;
                    }
                }
            }
        }

        void treebox_NodeMouseClick(object o, TreeNodeMouseClickEventArgs mea)
        {
            if (mea.Button == MouseButtons.Right)
            {
                treebox.SelectedNode = mea.Node;
                foreach (TreeNode tn in treebox.Nodes[0].Nodes)
                {
                    if (tn.IsSelected == true)
                    {
                        ContextMenuStrip cm = new ContextMenuStrip();
                        cm.Items.Add("Download");
                        cm.Items.Add("Delete");
                        cm.Items[0].Click+=new EventHandler(ServerFileDirectoryDownload_Click);
                        cm.Items[1].Click+=new EventHandler(ServerFileDirectoryDelete_Click);
                        treebox.ContextMenuStrip = cm;
                    }
                }
            }
        }


        void ServerFileDirectoryDownload_Click(object o, EventArgs ea)
        {
            string savelocation = F1.DownloadFromServer();
            ftp.Download(treebox.SelectedNode.Text, savelocation);
            F1.UpdateLocal(treebox.SelectedNode.Text);
        }

        void ServerFileDirectoryDelete_Click(object o, EventArgs ea)
        {
            ftp.Delete(treebox.SelectedNode.Text);
            treebox.Nodes[0].Nodes.Remove(treebox.SelectedNode);
        }

    }
}
