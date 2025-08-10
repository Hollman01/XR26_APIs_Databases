using UnityEngine;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System;

namespace Databases
{
    /// <summary>
    /// Handles all database operations for the game
    /// </summary>
    public class GameDataManager : MonoBehaviour
    {
        [Header("Database Configuration")]
        [SerializeField] private string databaseName = "GameData.db";

        private SQLiteConnection _database;
        private string _databasePath;

        public static GameDataManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDatabase(); // TODO: Ensure database is initialized at startup
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Creates the database file and ensures the required tables exist
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                // Database path
                _databasePath = Path.Combine(Application.persistentDataPath, databaseName);

                // Create SQLite connection
                _database = new SQLiteConnection(_databasePath);

                // TODO: Students will create table for high scores
                _database.CreateTable<HighScore>();

                Debug.Log($"Database initialized at: {_databasePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize database: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a high score to the database
        /// </summary>
        public void AddHighScore(string playerName, int score, string levelName = "Default")
        {
            try
            {
                var highScore = new HighScore(playerName, score, levelName);
                _database.Insert(highScore);
                Debug.Log($"High score added: {playerName} - {score} points");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add high score: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns top high scores from all levels
        /// </summary>
        public List<HighScore> GetTopHighScores(int limit = 10)
        {
            try
            {
                return _database.Table<HighScore>()
                    .OrderByDescending(h => h.Score)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }

        /// <summary>
        /// Returns top high scores for a specific level
        /// </summary>
        public List<HighScore> GetHighScoresForLevel(string levelName, int limit = 10)
        {
            try
            {
                return _database.Table<HighScore>()
                    .Where(h => h.LevelName == levelName)
                    .OrderByDescending(h => h.Score)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get level high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }

        /// <summary>
        /// Returns the total count of high scores in the database
        /// </summary>
        public int GetHighScoreCount()
        {
            try
            {
                return _database.Table<HighScore>().Count();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high score count: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Deletes all high scores from the database
        /// </summary>
        public void ClearAllHighScores()
        {
            try
            {
                _database.DeleteAll<HighScore>();
                Debug.Log("All high scores cleared");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to clear high scores: {ex.Message}");
            }
        }

        private void OnApplicationQuit()
        {
            _database?.Close();
        }
    }
}
