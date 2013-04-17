using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FTPuploader
{
    public partial class Form1 : Form
    {

        MenuStrip menustrip;
        ContextMenu cmform;
        ToolStripMenuItem toolbutton1;
        ToolStripMenuItem toolbutton2;
        ToolStripMenuItem toolbutton3;

        ToolStripMenuItem Sconnect;
        ToolStripMenuItem SAccounts;
        ToolStripMenuItem Sfiles;

        ToolStripMenuItem Pclose;
        ToolStripMenuItem Pabout;

        ToolStripMenuItem Lview;
        ToolStripMenuItem Lexport;

        TreeView serverdirectory;
        TextBox serverduplicate;
        TreeView localdirectory;

        TextBox serveroutput;
        TextBox transferoutput;
        TextBox serveraccountdetails;


        FTPConnection ftp;
        FileDirectory local;
        FileDirectory sfd;
        public Label CurrentFolder;

        public Form1()
        {

            this.ClientSize = new Size(700, 500);
            this.Name = "FTPuploader";
            menustrip = new MenuStrip();
            toolbutton1 = new ToolStripMenuItem();
            toolbutton2 = new ToolStripMenuItem();
            toolbutton3 = new ToolStripMenuItem();

            Sconnect = new ToolStripMenuItem();
            SAccounts = new ToolStripMenuItem();
            Sfiles = new ToolStripMenuItem();

            Pclose = new ToolStripMenuItem();
            Pabout = new ToolStripMenuItem();

            Lview = new ToolStripMenuItem();
            Lexport = new ToolStripMenuItem();

            menustrip.Size = new Size(300, 30);

            menustrip.Items.Add(toolbutton1);
            menustrip.Items.Add(toolbutton2);
            menustrip.Items.Add(toolbutton3);

            toolbutton1.Text = "Servers";
            toolbutton2.Text = "Program";
            toolbutton3.Text = "Logs";

            Sconnect.Text = "Connect";
            SAccounts.Text = "Accounts";
            Sfiles.Text = "Files";

            Pclose.Text = "Close";
            Pabout.Text = "About";

            Lview.Text = "View Logs";
            Lexport.Text = "Export";

            Sconnect.Click +=new EventHandler(Sconnect_Click);
            SAccounts.Click +=new EventHandler(SAccounts_Click);
            Sfiles.Click +=new EventHandler(Sfiles_Click);

            Pclose.Click +=new EventHandler(Pclose_Click);
            Pabout.Click +=new EventHandler(Pabout_Click);

            Lview.Click +=new EventHandler(Lview_Click);
            Lexport.Click +=new EventHandler(Lexport_Click);

            toolbutton1.DropDownItems.Add(Sconnect);
            toolbutton1.DropDownItems.Add(SAccounts);
            toolbutton1.DropDownItems.Add(Sfiles);

            toolbutton2.DropDownItems.Add(Pclose);
            toolbutton2.DropDownItems.Add(Pabout);

            toolbutton3.DropDownItems.Add(Lview);
            toolbutton3.DropDownItems.Add(Lexport);


            serveraccountdetails = new TextBox();
            serveroutput = new TextBox();

            serveraccountdetails.Size = new Size(350, 100);
            serveraccountdetails.Location = new Point(0, 35);
            serveraccountdetails.Multiline = true;
            serveraccountdetails.ReadOnly = true;
            serveraccountdetails.BackColor = Color.White;

            serveroutput.Size = new Size(350, 100);
            serveroutput.Location = new Point(350, 35);
            serveroutput.Multiline = true;
            serveroutput.ReadOnly = true;
            serveroutput.BackColor = Color.White;

            localdirectory = new TreeView();
            local = new LocalFileDirectory(ftp, this);
            serverduplicate = new TextBox();



            localdirectory = local.returnTreeView;
            localdirectory.Location = new Point(0, 150);
            localdirectory.Size = new Size(350, 200);

            CurrentFolder = new Label();
            CurrentFolder.AutoSize = true;
            CurrentFolder.Location = new Point(0, 135);
            CurrentFolder.Text = local.CurrentFolderPath;

            serverduplicate.Location = new Point(350, 150);
            serverduplicate.Size = new Size(350, 200);
            serverduplicate.BackColor = Color.Gray;
            serverduplicate.Multiline = true;

            cmform = new ContextMenu();

            transferoutput = new TextBox();

            this.Controls.Add(CurrentFolder);
            this.Controls.Add(serveroutput);
            this.Controls.Add(serveraccountdetails);
            this.Controls.Add(localdirectory);
            this.Controls.Add(serverduplicate);
            this.Controls.Add(menustrip);
            InitializeComponent();
        }

        

        void Sconnect_Click(object o, EventArgs e)
        {
        }

        void SAccounts_Click(object o, EventArgs e)
        {
            Accountform accountform = new Accountform(this);
            accountform.Show();
        }

        void Sfiles_Click(object o, EventArgs e)
        {
        }

        void Pclose_Click(object o, EventArgs e)
        {
        }

        void Pabout_Click(object o, EventArgs e)
        {
        }

        void Lview_Click(object o,  EventArgs e)
        {
        }

        void Lexport_Click(object o, EventArgs r)
        {
        }

        public void CreateConnection(string [] connectiondata)
        {
            ftp = new FTPConnection(connectiondata);
            sfd = new ServerFileDirectory(ftp, this);
            local.CreateConnection = ftp;

            serverdirectory = sfd.returnTreeView;
            serverdirectory.Size = new Size(350, 200);
            serverdirectory.Location = new Point(350, 150);
            this.Controls.Add(serverdirectory);
            serverduplicate.Hide();
        }

        public void UpdateServer(string FileName)
        {
            sfd.returnTreeView.Nodes[0].Nodes.Add(FileName);
            sfd.returnTreeView.Nodes[0].Nodes[sfd.returnTreeView.Nodes[0].Nodes.Count - 1].ImageIndex = 1;
            sfd.returnTreeView.Nodes[0].Nodes[sfd.returnTreeView.Nodes[0].Nodes.Count - 1].SelectedImageIndex = 1;
        }

        public String DownloadFromServer()
        {
            return CurrentFolder.Text;
        }

        public void UpdateLocal(string downloadedFile)
        {
            TreeNode node = FindTreeNode(local.returnTreeView.Nodes[0], CurrentFolder.Text);
            Console.WriteLine(node + "located folder node");
            node.Nodes.Add(downloadedFile);
            if (Path.HasExtension(downloadedFile))
            {
                node.Nodes[node.Nodes.Count - 1].SelectedImageIndex = 1;
                node.Nodes[node.Nodes.Count - 1].ImageIndex = 1;
            }
            else
            {
                node.Nodes[node.Nodes.Count - 1].SelectedImageIndex = 0;
                node.Nodes[node.Nodes.Count - 1].ImageIndex = 0;
            }
        }

        TreeNode FindTreeNode(TreeNode node, string text)
        {
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Name == text)
                {
                    return tn;
                }
                else
                {
                    TreeNode treenode = FindTreeNode(tn, text);
                    if (treenode != null)
                    {
                        return treenode;
                    }
                }
            }
            return (TreeNode)null;
        }
        
    }
}
