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

        public static async void RecordChanged(Object sender, RecordChangedEventArgs<StudentAchievements> args)
        {
            var changedEntity = args.Entity;
            Console.WriteLine(args.ChangeType);
            PostLatestSummary postSummary = new PostLatestSummary();
            StudentAchievements achievement = new StudentAchievements()
            {
                StudentId = changedEntity.StudentId,
                AchievementId = changedEntity.AchievementId,
                AchievementCounter = changedEntity.AchievementCounter
            };

            switch (args.ChangeType)
            {
                case ChangeType.Insert:
                    int result = await postSummary.SendLatestSummary(achievement);
                    Console.WriteLine($"Data Pushed: {result}");
                    break;
            }

        }
    }
}