using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net.Http.Json;

namespace MonitorStudentRanking.Classes
{
    public class PostLatestSummary
    {
        HttpClient httpClient;
        string connectionString = "";
        List<Achievement> achievementsList;
        List<Student> studentsList;
        string baseUrl = "http://localhost:5283/api/Ranking";
        public PostLatestSummary()
        {
            httpClient = new HttpClient();
            DBConnection dbConnection = new DBConnection();
            connectionString = dbConnection.GetConnectionString();
            achievementsList = new List<Achievement>()
            {
                new Achievement() { AchievementId = 1, AchievementName = "Assignment", Score =  20},
                new Achievement() { AchievementId = 2, AchievementName = "Presentation", Score =  15},
                new Achievement() { AchievementId = 3, AchievementName = "Research Idea", Score =  30},
                new Achievement() { AchievementId = 4, AchievementName = "Sport Activity", Score =  10},
                new Achievement() { AchievementId = 5, AchievementName = "Educational Event", Score =  35},
            };

            studentsList = new List<Student>()
            {
                new Student() { Id = 1, StudentName = "Aslam" },
                new Student() { Id = 2, StudentName = "Muhammad Ahmad" },
                new Student() { Id = 3, StudentName = "Waseem Akram" },
                new Student() { Id = 4, StudentName = "Imran Khan" },
                new Student() { Id = 5, StudentName = "Babar Azam" },
                new Student() { Id = 6, StudentName = "Shahid Afridi" },
                new Student() { Id = 7, StudentName = "Inzimam" },
                new Student() { Id = 8, StudentName = "M Yousuf" },
                new Student() { Id = 9, StudentName = "Saeed Anwar" },
                new Student() { Id = 10, StudentName = "Shoaid Akhtar" }
            };
        }

        public async Task<int> SendLatestSummary(StudentAchievements achievement)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                int studentId = achievement.StudentId;
                int achievementId = achievement.AchievementId;

                string checkStudentAchievement = "SELECT * FROM StudentAchievements WHERE StudentId = " + studentId + " AND AchievementId = " + achievementId;
                command.CommandText = checkStudentAchievement;
                SqlDataReader reader = command.ExecuteReader();
                string query = "";
                int achivementCount = 1;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        achivementCount = reader.GetInt32(2) + 1;
                    }
                    query = "UPDATE StudentAchievements SET AchievementCounter = " + achivementCount + " WHERE StudentId = " + studentId + " AND AchievementId = " + achievementId;
                }
                else
                {
                    query = "INSERT INTO StudentAchievements VALUES(" + studentId + "," + achievementId + "," + achivementCount + ")";
                }
                reader.Close();

                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = query;
                command.ExecuteNonQuery();
                command.Connection.Close();
                var findStudentName = (from student in studentsList
                                       where student.Id.Equals(studentId)
                                       select student.StudentName).FirstOrDefault();
                StudentPointsSummary summary = new StudentPointsSummary()
                {
                    StudentId = studentId,
                    StudentName = findStudentName == null ? "Default User" : findStudentName,
                    Points = achivementCount
                };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(baseUrl, summary);
                response.EnsureSuccessStatusCode();
                return 1;
            }
        }
    }

    public class Achievement
    {
        public int AchievementId { get; set; }
        public string AchievementName { get; set; } = "";
        public int Score { get; set; }
    }


    public class StudentPointsSummary
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        public int Points { get; set; }
    }
}
