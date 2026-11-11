using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Gierka.Core
{
    public class RankingEntry
    {
        public string UserName { get; set; }
        public int Score { get; set; } // Złotówki
        public DateTime Date { get; set; }
    }

    public static class RankingManager
    {
        private static readonly string RankingFile = "ranking.json";

        public static void AddEntry(string userName, int score)
        {
            var entries = LoadRanking();
            entries.Add(new RankingEntry
            {
                UserName = userName,
                Score = score,
                Date = DateTime.Now
            });

            // Sort by Score descending
            entries = entries.OrderByDescending(e => e.Score).ToList();

            SaveRanking(entries);
        }

        public static List<RankingEntry> LoadRanking()
        {
            if (!File.Exists(RankingFile))
            {
                return new List<RankingEntry>();
            }

            try
            {
                var json = File.ReadAllText(RankingFile);
                return JsonSerializer.Deserialize<List<RankingEntry>>(json) ?? new List<RankingEntry>();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load ranking: {ex.Message}");
                return new List<RankingEntry>();
            }
        }

        private static void SaveRanking(List<RankingEntry> entries)
        {
            try
            {
                var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(RankingFile, json);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to save ranking: {ex.Message}");
            }
        }
    }
}
