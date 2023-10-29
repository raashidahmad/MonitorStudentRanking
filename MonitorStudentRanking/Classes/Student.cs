namespace MonitorStudentRanking.Classes
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = "None";
    }

    public class StudentAchievements
    {
        public int StudentId { get; set; }
        public int AchievementId { get; set; }
        public int AchievementCounter { get; set; }
    }
}
