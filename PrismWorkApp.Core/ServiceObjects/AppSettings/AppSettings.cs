
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.Core
{
   public partial class AppSettings: SmallBindableBase
    {
        private string _projectBDConnectionString = @"Data Source ={0}; Initial Catalog = master; Database = {1}; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
        public string ProjectBDConnectionString
        {
            get { return string.Format(_projectBDConnectionString,DataSource,Database);}
            set { /*SetProperty(ref _projectBDConnectionString, value);*/ }
        }
        private string _dataSource= @"M-RUK-04\TEW_SQLEXPRESS_5";
        public string DataSource
        {
            get { return _dataSource; }
            set { SetProperty(ref _dataSource, value); }
        }
        private string _database = "work_bd";
        public string Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

    }
}
