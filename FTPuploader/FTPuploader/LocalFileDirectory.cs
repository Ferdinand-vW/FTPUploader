using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;

namespace FTPuploader
{
    class LocalFileDirectory:FileDirectory
    {
        Form1 f1;
        bool start = false;

        public LocalFileDirectory(FTPConnection ftp, Form1 f1): base(ftp, f1)
        {
            start = true;
            ListDirectory();
            this.f1 = f1;
            currentfolderpath += treebox.Nodes[0].Text;
            CurrentFolder = treebox.Nodes[0];

        }

        void ListDirectory()
        {
            treebox.Nodes.Clear();
            DirectoryInfo di = new DirectoryInfo("C:\\");
                        treebox.BeforeExpand += new TreeViewCancelEventHandler(treebox_BeforeExpand);
            treebox.AfterCollapse += new TreeViewEventHandler(treebox_AfterCollapse);
            treebox.AfterExpand += new TreeViewEventHandler(treebox_AfterExpand);
            treebox.Nodes.Add(Fill(di));
            //treebox.Nodes[0].Expand();
            //LoadLastUsedNode(basenode);
        }


        TreeNode Fill(DirectoryInfo di)
        {
            TreeNode newNode = new TreeNode(di.Name);
            newNode.Name = di.FullName;
            newNode.ImageIndex = 0;
            newNode.SelectedImageIndex = 0;
            try
            {
                foreach (DirectoryInfo dirc in di.GetDirectories())
                {
                    newNode.Nodes.Add(Fill(dirc));
                }

                foreach (FileInfo fi in di.GetFiles())
                {
                    TreeNode tn = new TreeNode(fi.Name);
                    tn.ImageIndex = 1;
                    tn.SelectedImageIndex = 1;
                    newNode.Nodes.Add(tn);
                }


            }
            catch (UnauthorizedAccessException)
            {
                newNode.Nodes.Add(new TreeNode("Acces Denied"));
            }
            return newNode;
        }

        protected override void treebox_NodeMouseDoubleClick(object o, TreeNodeMouseClickEventArgs mea)
        {
            TreeViewHitTestInfo info = treebox.HitTest(mea.Location);
            if(info.Location == TreeViewHitTestLocations.Label)
            {
                if (ftp != null)
                {
                    ftp.UpLoad(mea.Node.Text, mea.Node.FullPath);
                    f1.UpdateServer(mea.Node.Text);
                }
                //
            }
        }

        void treebox_BeforeExpand(object o, TreeViewCancelEventArgs tvea)
        {
            CurrentFolder = tvea.Node;
            if (CurrentFolder != treebox.Nodes[0])
            {
                currentfolderpath = CurrentFolder.Name;
                f1.CurrentFolder.Text = currentfolderpath;
            }
            
        }

        void treebox_AfterExpand(object o, TreeViewEventArgs tvea)
        {
            UpdateSettingsNode(tvea.Node.Name);
        }

        String getLastNode()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.Settings);
            XmlNode node = xml.SelectSingleNode("Root/LastNode");
            Console.WriteLine(node.InnerText);
            return node.LastChild.InnerText;
        }
        void UpdateSettingsNode(string NodeName)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.Settings);

            XmlNode node = xml.SelectSingleNode("Root/LastNode");
            XmlNode newnode = xml.CreateNode(XmlNodeType.Element, "Path", null);
            node.AppendChild(newnode);

            if (NodeName == "")
            {
                NodeName = "C:\\";
            }
            newnode.InnerText = NodeName;
            
            xml.Save(@"Resources\Settings.xml");
        }

        void treebox_AfterCollapse(object o, TreeViewEventArgs tvea)
        {
            if (tvea.Node.Parent != treebox.Nodes[0])
            {
                CurrentFolder = tvea.Node.Parent;   
            }
            else
            {
                CurrentFolder = treebox.Nodes[0];
                CurrentFolder.Name = treebox.Nodes[0].Text;
                //Console.WriteLine(currentfolderpath);
            }
            currentfolderpath = CurrentFolder.Name;
            f1.CurrentFolder.Text = currentfolderpath;

            UpdateSettingsNode(tvea.Node.Parent.Name);
        }


        void LoadLastUsedNode(TreeNode tn)
        {
            if (start == true)
            {
                string LastUsedNode = getLastNode();
                TreeNode lastUsedTreeNode;
                lastUsedTreeNode = FindTreeNode(tn, LastUsedNode);
                Console.WriteLine(LastUsedNode);
                //Console.WriteLine(lastUsedTreeNode.Name);
                if (LastUsedNode == "")
                {
                    treebox.Nodes[0].Expand();
                    Console.WriteLine("does something");
                }
                else
                {
                    List<TreeNode> ParentNodes = new List<TreeNode>();
                    getParentNodes(lastUsedTreeNode, ParentNodes);
                    Console.WriteLine(lastUsedTreeNode);
                    foreach (TreeNode treenode in ParentNodes)
                    {
                        treenode.Expand();
                    }
                    lastUsedTreeNode.Expand();
                }
                
                //Console.WriteLine(lastUsedTreeNode.Name);
            }
        }

        List<TreeNode> getParentNodes(TreeNode tn, List<TreeNode> ParentNodes)
        {
            if (tn.Parent != null)
            {
                ParentNodes.Add(tn.Parent);
                getParentNodes(tn.Parent, ParentNodes);
            }

            return ParentNodes;
        }

        TreeNode FindTreeNode(TreeNode node, string text)
        {
            
            foreach (TreeNode tn in node.Nodes)
            {
                Console.WriteLine(node);
                if (tn.Name == text)
                {
                    Console.WriteLine("found" + tn);
                    return tn;
                }
                else
                {
                    FindTreeNode(tn, text);
                }
            }
            
            return new TreeNode();
        }

    }
}
