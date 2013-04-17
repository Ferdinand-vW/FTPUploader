using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPuploader
{
    class UpdateDatagridView
    {
        string Column;
        string sqlcommand = "";
        public UpdateDatagridView(string column_)
        {
            Column = column_;
        }

        void ChooseSqlCommand()
        {
            switch (Column)
            {
                case "Name":
                    sqlcommand = "update Accounts set Name";
                    break;
                case "Type Account":
                    sqlcommand = "update Accounts set Type Account";
                    break;
                case "Username":
                    sqlcommand = "update Accounts set Username";
                    break;
                case "Password":
                    sqlcommand = "update Accounts set Password";
                    break;
                case "Server Adress":
                    sqlcommand = "update Accounts set Server Adress";
                    break;
                case "Port":
                    sqlcommand = "update Accounts set Port";
                    break;
            }
        }

        public String returnCommand
        {
            get
            {
                ChooseSqlCommand();
                return sqlcommand;
            }
        }
    }
}
