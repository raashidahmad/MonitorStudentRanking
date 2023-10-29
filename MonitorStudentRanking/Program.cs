using MonitorStudentRanking.Classes;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace MonitorStudentRanking
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //var mapper = new ModelToTableMapper<Student>();
            //mapper.AddMapping(m => m.FullName, "StudentName");
            DBConnection dbConnection = new DBConnection();
            string connectionString = dbConnection.GetConnectionString();
            using (var dep = new SqlTableDependency<StudentAchievements>(connectionString, "StudentAchievements"))
            {
                dep.OnChanged += RecordChanged;
                dep.Start();

                Console.WriteLine("Press a key to exit");
                Console.ReadKey();

                dep.Stop();
            }
        }

        public static void RecordChanged(Object sender, RecordChangedEventArgs<StudentAchievements> args)
        {
            var changedEntity = args.Entity;
            Console.WriteLine(args.ChangeType);

            switch (args.ChangeType)
            {
                case ChangeType.Insert:
                    break;

                case ChangeType.Update:
                    break;
            }
        }
    }
}