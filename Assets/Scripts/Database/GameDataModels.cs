using System;
using SQLite;

namespace Databases
{
    /// TODO: Students will add SQLite attributes
    [Table("HighScores")] // Added table name for SQLite
    public class HighScore
    {
        // TODO: Students will add the correct SQLite attributes
        [PrimaryKey, AutoIncrement] // Primary key with auto increment
        public int Id { get; set; }
        
        // TODO: Add [Indexed] attribute for frequently queried fields
        [Indexed] // Indexed for faster searches
        public string PlayerName { get; set; }
        
        public int Score { get; set; }
        
        // TODO: Add [Indexed] attribute here too
        [Indexed] // Indexed for level-specific queries
        public string LevelName { get; set; }
        
        public DateTime AchievedAt { get; set; }
        
        public float CompletionTime { get; set; }
        
        // Constructor for easy creation
        public HighScore()
        {
            AchievedAt = DateTime.UtcNow;
        }
        
        public HighScore(string playerName, int score, string levelName, float completionTime = 0f)
        {
            PlayerName = playerName;
            Score = score;
            LevelName = levelName;
            CompletionTime = completionTime;
            AchievedAt = DateTime.UtcNow;
        }
        
        public override string ToString()
        {
            return $"{PlayerName}: {Score} points on {LevelName} ({CompletionTime:F2}s)";
        }
    }
}
