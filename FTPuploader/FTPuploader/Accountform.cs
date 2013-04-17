using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace FTPuploader
{
    public partial class Accountform : Form
    {
        Button createnew;
        Button cancel;
        Button[] changebuttons = new Button[3];
        AccountDataGrid adg;
        Label accountsdisplay;
        TextBox update_textbox;
        Form1 F1 = new Form1();

        string [] data = new string[6];

        public Accountform(Form1 f1)
        {
            this.ClientSize = new Size(500, 400);

            F1 = f1;
            changebuttons[0] = new Button();
            changebuttons[1] = new Button();
            changebuttons[2] = new Button();
            createnew = new Button();
            cancel = new Button();
            accountsdisplay = new Label();
            update_textbox = new TextBox();


             adg = new AccountDataGrid(changebuttons, this.ClientSize);

            changebuttons[0].Text = "Delete";
            changebuttons[0].AutoSize = true;
            changebuttons[0].Location = new Point(10, 20);
            changebuttons[0].Enabled = false;

            changebuttons[1].Text = "Update";
            changebuttons[1].AutoSize = true;
            changebuttons[1].Location = new Point(10, 120);
            changebuttons[1].Enabled = false;

            createnew.Text = "Create";
            createnew.AutoSize = true;
            createnew.Location = new Point(380, 20);

            cancel.Text = "Cancel";
            cancel.AutoSize = true;
            cancel.Location = new Point(250, this.ClientSize.Height - cancel.Height);

            changebuttons[2].Text = "Connect";
            changebuttons[2].AutoSize = true;
            changebuttons[2].Location = new Point(170, this.ClientSize.Height - changebuttons[2].Height);
            changebuttons[2].Enabled = false;

            accountsdisplay.Text = "Server Accounts:";
            accountsdisplay.Location = new Point(this.ClientSize.Width / 2 - 50, 130);
            accountsdisplay.AutoSize = true;

            update_textbox.Size = new Size(70, changebuttons[1].Size.Height);
            update_textbox.Location = new Point(90, 122);

            changebuttons[0].Click +=new EventHandler(delete_Click);
            changebuttons[1].Click +=new EventHandler(update_Click);
            createnew.Click +=new EventHandler(createnew_Click);
            cancel.Click+=new EventHandler(cancel_Click);
            changebuttons[2].Click+=new EventHandler(connect_Click);

            this.Controls.Add(adg.returngridview);
            this.Controls.Add(changebuttons[0]);
            this.Controls.Add(changebuttons[1]);
            this.Controls.Add(createnew);
            this.Controls.Add(cancel);
            this.Controls.Add(changebuttons[2]);
            this.Controls.Add(accountsdisplay);
            this.Controls.Add(update_textbox);
            InitializeComponent();
        }

        void delete_Click(object o, EventArgs e)
        {
            adg.DeleteRow();
        }

        void update_Click(object o, EventArgs e)
        {
            adg.Update(update_textbox.Text);
        }

        void createnew_Click(object o, EventArgs e)
        {
            newAccountForm naf = new newAccountForm(adg);
            naf.Show();

        }

        void cancel_Click(object o, EventArgs e)
        {
            this.Close();
        }

        void connect_Click(object o, EventArgs e)
        {
            for(int i = 0; i<adg.returnrows.Count; i++)
            {
                if((Boolean)adg.returnrows[i].Cells[0].Value == true)
                {
                    data[0] = (string)adg.returnrows[i].Cells[1].Value;
                    data[1] = (string)adg.returnrows[i].Cells[2].Value;
                    data[2] = (string)adg.returnrows[i].Cells[3].Value;
                    data[3] = (string)adg.returnrows[i].Cells[4].Value;
                    data[4] = (string)adg.returnrows[i].Cells[5].Value;
                    data[5] = (string)adg.returnrows[i].Cells[6].Value;
                }
            }

            F1.CreateConnection(data);
            this.Close();
        }

        
    }
}
