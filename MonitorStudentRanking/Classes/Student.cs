namespace MonitorStudentRanking.Classes
{
    internal class Student
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = "None";
    }

    internal class StudentAchievements
    {
        public int StudentId { get; set; }
        public int AchievementId { get; set; }
        public int AchievementCounter { get; set; }
    }
}
