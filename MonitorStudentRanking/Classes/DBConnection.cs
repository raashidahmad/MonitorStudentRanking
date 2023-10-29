namespace MonitorStudentRanking.Classes
{
    internal class DBConnection
    {
        public string GetConnectionString()
        {
            return "Server=.; Database=StudentRanking; User ID=sa;Password=master002; MultipleActiveResultSets=true; TrustServerCertificate=True; Encrypt=False;";
        }
    }
}
