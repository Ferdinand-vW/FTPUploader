using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace FTPuploader
{
    public class AccountDataGrid
    {
        DataGridView accountdata;
        DataGridViewCheckBoxColumn checkbox;
        DataGridViewTextBoxColumn name;
        DataGridViewTextBoxColumn type;
        DataGridViewTextBoxColumn username;
        DataGridViewTextBoxColumn password;
        DataGridViewTextBoxColumn server;
        DataGridViewTextBoxColumn port;

        DataGridViewCell checkboxcell;
        DataGridViewCell namecell;
        DataGridViewCell typecell;
        DataGridViewCell usernamecell;
        DataGridViewCell passwordcell;
        DataGridViewCell hostcell;
        DataGridViewCell portcell;

        Button delete;
        Button update;
        Button connect;

        string path = @"Data Source=Resources\Accounts.sdf;";

        public AccountDataGrid(Button[] buttons, Size client)
        {
            accountdata = new DataGridView();



            delete = buttons[0];
            update = buttons[1];
            connect = buttons[2];

            accountdata.Location = new Point(0, 150);
            accountdata.Size = new Size(client.Width, 180);
            accountdata.BackgroundColor = Color.White;
            accountdata.AllowUserToAddRows = false;
            accountdata.AllowUserToDeleteRows = false;
            accountdata.AllowUserToOrderColumns = false;
            accountdata.AllowUserToResizeRows = false;
            accountdata.MultiSelect = false;
            //accountdata.DefaultCellStyle.SelectionBackColor = accountdata.DefaultCellStyle.BackColor;
            //accountdata.DefaultCellStyle.SelectionForeColor = accountdata.DefaultCellStyle.ForeColor;
            accountdata.CellValueChanged += new DataGridViewCellEventHandler(accountdata_CellValueChanged);
            accountdata.CellContentClick += new DataGridViewCellEventHandler(accountdata_CellContentClick);
            accountdata.CellClick+=new DataGridViewCellEventHandler(accountdata_CellClick);

        }


        void CreateDataGrid()
        {

            checkbox = new DataGridViewCheckBoxColumn();
            name = new DataGridViewTextBoxColumn();
            type = new DataGridViewTextBoxColumn();
            username = new DataGridViewTextBoxColumn();
            password = new DataGridViewTextBoxColumn();
            server = new DataGridViewTextBoxColumn();
            port = new DataGridViewTextBoxColumn();


            accountdata.SelectionMode = DataGridViewSelectionMode.CellSelect;
            accountdata.RowHeadersVisible = false;


            checkbox.Name = "CheckBox";
            checkbox.HeaderText = "F";
            checkbox.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(checkbox);

            name.Name = "Name";
            name.HeaderText = "Name";
            name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            accountdata.Columns.Add(name);

            type.Name = "Type Account";
            type.HeaderText = "Account";
            //type.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(type);

            username.Name = "Username";
            username.HeaderText = "Username";
            //username.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(username);

            password.Name = "Password";
            password.HeaderText = "Password";
            //password.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(password);

            server.Name = "Server Adress";
            server.HeaderText = "Host";
            //server.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(server);

            port.Name = "Port";
            port.HeaderText = "Port";
            port.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            accountdata.Columns.Add(port);


            for (int i = 0; i < accountdata.Columns.Count; i++)
            {
                if (i > 0)
                {
                    accountdata.Columns[i].ReadOnly = true;
                }
                accountdata.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            accountdata.AutoResizeColumns();

        }

        void FillDataGrid()
        {
            
            List<string>[] data = new List<string>[6];

            try
            {
                SqlCeConnection scon = new SqlCeConnection(path);

                scon.Open();
                SqlCeCommand scom = new SqlCeCommand("SELECT * FROM Accounts", scon);
                SqlCeDataReader reader = scom.ExecuteReader();

                while (reader.Read())
                {
                    DataGridViewRow rows = new DataGridViewRow();

                    checkboxcell = new DataGridViewCheckBoxCell();
                    namecell = new DataGridViewTextBoxCell();
                    typecell = new DataGridViewTextBoxCell();
                    usernamecell = new DataGridViewTextBoxCell();
                    passwordcell = new DataGridViewTextBoxCell();
                    hostcell = new DataGridViewTextBoxCell();
                    portcell = new DataGridViewTextBoxCell();


                    checkboxcell.Value = false;
                    namecell.Value = reader.GetString(0);
                    typecell.Value = reader.GetString(1);
                    usernamecell.Value = reader.GetString(2);
                    passwordcell.Value = reader.GetString(3);
                    hostcell.Value = reader.GetString(4);
                    portcell.Value = reader.GetString(5);

                    rows.Cells.Add(checkboxcell);
                    rows.Cells.Add(namecell);
                    rows.Cells.Add(typecell);
                    rows.Cells.Add(usernamecell);
                    rows.Cells.Add(passwordcell);
                    rows.Cells.Add(hostcell);
                    rows.Cells.Add(portcell);

                    accountdata.Rows.Add(rows);
                }
                scon.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        void accountdata_CellValueChanged(object o, DataGridViewCellEventArgs dgvcea)
        {
            if ((Boolean)accountdata.Rows[dgvcea.RowIndex].Cells[0].Value == true)
            {
                for (int i = 0; i < accountdata.Rows.Count; i++)
                {
                    if (i != dgvcea.RowIndex)
                    {
                        accountdata.Rows[i].Cells[0].Value = false;
                    }
                }

                delete.Enabled = true;
                connect.Enabled = true;
            }

            if ((Boolean)accountdata.Rows[dgvcea.RowIndex].Cells[0].Value == false)
            {
                delete.Enabled = false;
                connect.Enabled = false;
            }


        }

        public void CreateNewRow(string[] data)
        {
            DataGridViewRow newaccount = new DataGridViewRow();

            checkboxcell = new DataGridViewCheckBoxCell();
            namecell = new DataGridViewTextBoxCell();
            typecell = new DataGridViewTextBoxCell();
            usernamecell = new DataGridViewTextBoxCell();
            passwordcell = new DataGridViewTextBoxCell();
            hostcell = new DataGridViewTextBoxCell();
            portcell = new DataGridViewTextBoxCell();

            checkboxcell.Value = false;
            namecell.Value = data[0];
            typecell.Value = data[1];
            usernamecell.Value = data[2];
            passwordcell.Value = data[3];
            hostcell.Value = data[4];
            portcell.Value = data[5];

            newaccount.Cells.AddRange(checkboxcell, namecell, typecell, usernamecell, passwordcell, hostcell, portcell);
            accountdata.Rows.Add(newaccount);
            accountdata.Invalidate();

            SqlCeConnection scon = new SqlCeConnection(path);
            scon.Open();
            SqlCeCommand scom = new SqlCeCommand("insert into Accounts values(@name,@type,@username,@password,@host,@port)", scon);
            scom.Parameters.AddWithValue("@name", data[0]);
            scom.Parameters.AddWithValue("@type", data[1]);
            scom.Parameters.AddWithValue("@username", data[2]);
            scom.Parameters.AddWithValue("@password", data[3]);
            scom.Parameters.AddWithValue("@host", data[4]);
            scom.Parameters.AddWithValue("@port", data[5]);
            scom.ExecuteNonQuery();
            scon.Close();
        }

        void accountdata_CellContentClick(object o, EventArgs ea)
        {


            accountdata.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        void accountdata_CellClick(object o, EventArgs e)
        {

            if (update.Enabled == false)
            {
                for (int i = 0; i < accountdata.Rows.Count; i++)
                {
                    if ((Boolean)accountdata.Rows[i].Cells[0].Value == true)
                    {
                        if (accountdata.CurrentCell != accountdata.Rows[i].Cells[0])
                        {
                            update.Enabled = true;
                        }
                    }
                }
            }
            else if (update.Enabled == true)
            {
                bool check = false;
                for (int i = 0; i < accountdata.Rows.Count; i++)
                {
                    if ((Boolean)accountdata.Rows[i].Cells[0].Value == true)
                    {
                        if (accountdata.CurrentCell == accountdata.Rows[i].Cells[0])
                        {
                            update.Enabled = false;
                            check = true;
                            break;
                        }
                        else
                        {
                            update.Enabled = true;
                            check = true;
                        }
                    }
                }

                if (check == false)
                {
                    update.Enabled = false;
                }
            }
        }

        public void DeleteRow()
        {
            string TOdeleteName = "";
            for(int i = 0; i<accountdata.Rows.Count; i++)
            {
                if((Boolean)accountdata.Rows[i].Cells[0].Value == true)
                {
                    TOdeleteName = (string)accountdata.Rows[i].Cells[1].Value;
                    accountdata.Rows.RemoveAt(i);
                    break;
                }
            }

            SqlCeConnection scon = new SqlCeConnection(path);
            scon.Open();
            SqlCeCommand scom = new SqlCeCommand("delete from Accounts where Name = @TOdeleteName", scon);
            scom.Parameters.AddWithValue("@TOdeleteName", TOdeleteName);
            scom.ExecuteNonQuery();
            scon.Close();
            if (accountdata.Rows.Count == 0)
            {
                delete.Enabled = false;
                update.Enabled = false;
                connect.Enabled = false;
            }
        }

        public void Update(string changetext)
        {

            int columnindex = accountdata.CurrentCell.ColumnIndex;
            string columnname = accountdata.Columns[columnindex].Name;
            //(columnname);
            int rowindex = accountdata.CurrentCell.RowIndex;
            string currentrowName = (string)accountdata.Rows[rowindex].Cells[1].Value;
            SqlCeConnection scon = new SqlCeConnection(path);
            scon.Open();

            UpdateDatagridView udgv = new UpdateDatagridView(columnname);
            string command = udgv.returnCommand + "=@value where Name = @rowName";

            SqlCeCommand scom = new SqlCeCommand(command, scon);
            scom.Parameters.AddWithValue("@value", changetext);
            scom.Parameters.AddWithValue("@rowName", currentrowName);
            scom.ExecuteNonQuery();
            accountdata.CurrentCell.Value = changetext;
            scon.Close();
        }

        public DataGridView returngridview
        {
            get
            {
                CreateDataGrid();
                FillDataGrid();
                return accountdata; }
        }

        public DataGridViewRowCollection returnrows
        {
            get { return accountdata.Rows; }
        }
    }
}
