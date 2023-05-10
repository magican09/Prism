




namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class AppSettings : BindableBase
    {
      //private string _projectBDConnectionString = @"Data Source ={0}; Initial Catalog = master; Database = {1};  User Id={2};Password={3}";//Integrated Security = SSPI; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False ;
        private string _projectBDConnectionString = @"Data Source ={0}; Initial Catalog = master;Integrated Security = True; Trusted_Connection=True; MultipleActiveResultSets=True;Database = {1}; ";//
        public string ProjectBDConnectionString
        {
            get { return string.Format(_projectBDConnectionString, DataSource, Database, UserName, Password); }
            set { /*SetProperty(ref _projectBDConnectionString, value);*/ }
        }
        private string _dataSource = @"localhost\TEW_SQLEXPRESS_5";//*/@"M-GEODEZ-01\TEW_SQLEXPRESS";// @"M-RUK-04\TEW_SQLEXPRESS_5";
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
        private string _userName = "sql_adm";
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        private string _password = "123456789";
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

    }
}
