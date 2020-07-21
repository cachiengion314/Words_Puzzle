using System.Collections.Generic;

public class GameState
{
    public static int currentWorld, currentSubWorld, currentLevel;
    public static string currentSubWorldName = "Subworld name";
    public static int unlockedWorld = -1, unlockedSubWord = -1, unlockedLevel = -1;
    public static int countSpell = -1, countLevel = -1, countChapter = -1, countExtra = -1, countBooster = -1, countLevelMisspelling = -1;
    public static int amazingCount = -1, awesomeCount = -1, excelentCount = -1, goodCount = -1, greatCount = -1;
    public static int countSpellDaily = -1, countLevelDaily = -1, countChapterDaily = -1, countExtraDaily = -1, countBoosterDaily = -1, countLevelMisspellingDaily = -1;
    public static int amazingCountDaily = -1, awesomeCountDaily = -1, excelentCountDaily = -1, goodCountDaily = -1, greatCountDaily = -1;
    public static bool isLastLevel;
    public static Stack<Quest> curDailyquests = new Stack<Quest>();
}