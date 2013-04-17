using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FTPuploader
{
    public partial class newAccountForm : Form
    {
        Label accountnamelabel;
        Label accounttypelabel;
        Label accountusernamelabel;
        Label accountpasswordlabel;
        Label accounthostlabel;
        Label accountportlabel;

        TextBox accountname;
        ComboBox accounttype;
        TextBox accountusername;
        TextBox accountpassword;
        TextBox accounthost;
        TextBox accountport;

        Button AddAccount;
        Button CancelAccount;

        bool addaccount = false;

        AccountDataGrid adg_;

        string[] accountdata = new string[6];
        public newAccountForm(AccountDataGrid adg)
        {

            this.ClientSize = new Size(150, 300);

            adg_ = adg;

            accountnamelabel = new Label();
            accounttypelabel = new Label();
            accountusernamelabel = new Label();
            accountpasswordlabel = new Label();
            accounthostlabel = new Label();
            accountportlabel = new Label();

            accountname = new TextBox();
            accounttype = new ComboBox();
            accountusername = new TextBox();
            accountpassword = new TextBox();
            accounthost = new TextBox();
            accountport = new TextBox();

            AddAccount = new Button();
            CancelAccount = new Button();

            accountnamelabel.Text = "Name:";
            accountnamelabel.Location = new Point(20, 12);
            accountnamelabel.AutoSize = true;

            accounttypelabel.Text = "Account Type:";
            accounttypelabel.Location = new Point(20, 55);
            accounttypelabel.AutoSize = true;

            accountusernamelabel.Text = "Username:";
            accountusernamelabel.Location = new Point(20, 95);
            accountusernamelabel.AutoSize = true;

            accountpasswordlabel.Text = "Password:";
            accountpasswordlabel.Location = new Point(20, 135);
            accountpasswordlabel.AutoSize = true;

            accounthostlabel.Text = "Host:";
            accounthostlabel.Location = new Point(20, 175);
            accounthostlabel.AutoSize = true;

            accountportlabel.Text = "Port:";
            accountportlabel.Location = new Point(20, 215);
            accountportlabel.AutoSize = true;

            accountname.Size = new Size(110, 20);
            accountname.Location = new Point(20, 30);

            accounttype.Size = new Size(110, 20);
            accounttype.Location = new Point(20, 70);
            accounttype.Items.Add("Account");
            accounttype.Items.Add("Anonymous");
            accounttype.SelectedItem = accounttype.Items[0];
            accounttype.DropDownStyle = ComboBoxStyle.DropDownList;
            accounttype.FlatStyle = FlatStyle.Flat;
            accounttype.BackColor = Color.White;

            accountusername.Size = new Size(110, 20);
            accountusername.Location = new Point(20, 110);
            accountusername.BackColor = Color.White;

            accountpassword.Size = new Size(110, 20);
            accountpassword.Location = new Point(20, 150);
            accountpassword.BackColor = Color.White;

            accounthost.Size = new Size(110, 20);
            accounthost.Location = new Point(20, 190);

            accountport.Size = new Size(110, 20);
            accountport.Location = new Point(20, 230);

            AddAccount.Text = "Add";
            AddAccount.Size = new Size(70, 20);
            AddAccount.Location = new Point(5, 270);

            CancelAccount.Text = "Cancel";
            CancelAccount.Size = new Size(70, 20);
            CancelAccount.Location = new Point(75, 270);

            this.Controls.Add(accountnamelabel);
            this.Controls.Add(accounttypelabel);
            this.Controls.Add(accountusernamelabel);
            this.Controls.Add(accountpasswordlabel);
            this.Controls.Add(accounthostlabel);
            this.Controls.Add(accountportlabel);

            this.Controls.Add(accountname);
            this.Controls.Add(accounttype);
            this.Controls.Add(accountusername);
            this.Controls.Add(accountpassword);
            this.Controls.Add(accounthost);
            this.Controls.Add(accountport);

            this.Controls.Add(AddAccount);
            this.Controls.Add(CancelAccount);

            accounttype.SelectedIndexChanged +=new EventHandler(accounttype_SelectedIndexChanged);
            AddAccount.Click +=new EventHandler(AddAccount_Click);
            CancelAccount.Click +=new EventHandler(CancelAccount_Click);
            InitializeComponent();
        }

        void accounttype_SelectedIndexChanged(object o, EventArgs ea)
        {
            if ((string)accounttype.SelectedItem == "Account")
            {
                accountusername.Enabled = true;
                accountpassword.Enabled = true;
            }
            else if ((string)accounttype.SelectedItem == "Anonymous")
            {
                accountusername.Enabled = false;
                accountpassword.Enabled = false;
            }
        }

        void AddAccount_Click(object o, EventArgs ea)
        {
            accountdata[0] = accountname.Text;
            accountdata[1] = accounttype.Text;
            accountdata[2] = accountusername.Text;
            accountdata[3] = accountpassword.Text;
            accountdata[4] = accounthost.Text;
            accountdata[5] = accountport.Text;
            addaccount = true;

            adg_.CreateNewRow(accountdata);
            this.Close();
        }

        void CancelAccount_Click(object o, EventArgs ea)
        {
            addaccount = false;
            this.Close();
        }

        public Boolean DoAddAccount
        {
            get { return addaccount; }
        }

        public String[] NewAccountData
        {
            get { return accountdata; }
        }
    }
}
