using UnityEngine;
using System.Collections.Generic;

namespace Superpow
{
    public class Utils
    {
        public static int GetNumLevels(int world, int subWorld)
        {

            // Indicate the number of levels in specific sub-worlds.
            int[,] numLevels =
            {
                { 7, 7, 7, 7, 7 }, // For world 0
                { 7, 7, 7, 7, 7 }, // For world 1
                { 7, 7, 7, 7, 7 }, // For world 2
                { 7, 7, 7, 7, 7 }, // For world 3
                { 7, 7, 7, 7, 7 }, // For world 4
                { 7, 7, 7, 7, 7 }, // For world 5
                { 7, 7, 7, 7, 7 }, // Not used yet
                { 7, 7, 7, 7, 7 }, // Not used yet
                { 7, 7, 7, 7, 7 }, // Not used yet
                { 7, 7, 7, 7, 7 },  // Not used yet
            };

            return numLevels[world, subWorld];
        }

        public static int GetLeaderboardScore()
        {
            int levelInSub = Prefs.unlockedWorld == 0 && Prefs.unlockedSubWorld == 0 ? 12 : 18;
            int score = (Prefs.unlockedWorld * 5 + Prefs.unlockedSubWorld) * levelInSub + Prefs.unlockedLevel;

            if (levelInSub == 18) score -= 6;
            return score;
        }

        public static GameLevel Load(int world, int subWorld, int level)
        {
            return Resources.Load<GameLevel>("World_" + world + "/SubWorld_" + subWorld + "/Level_" + level);
        }
    }
}