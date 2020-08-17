using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;
using System;
using Superpow;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

public class NotificationController : MonoBehaviour
{
    private const double NOTI_DELAY_1 = 1 * 24 * 3600;
    private const double NOTI_DELAY_2 = 2 * 24 * 3600;
    private const double NOTI_DELAY_3 = 4 * 24 * 3600;
    private const double NOTI_DELAY_4 = 7 * 24 * 3600;
    private const double NOTI_DELAY_5 = 10 * 24 * 3600;
    private const double NOTI_DELAY_6 = 15 * 24 * 3600;
    private const double NOTI_DELAY_7 = 30 * 24 * 3600;

    private const string DAILYTIME_MORNING = "10:00:00"; // 10:00:00 +3 more hours 
    private const string DAILYTIME_AFTERNOON = "18:00:00"; // 18:00:00 +3 more hours

    private string DAILYTIME_CUSTOM = "17:00:00";

    private int MAX_CURRENT_LOOP = 7; // 7
    private readonly double dayToSecondsValue = 24 * 3600; // 24 * 3600
    private readonly float hourToSecondsValue = 3600; // 3600

    public static NotificationController instance;
    public int maxFreeBoostersProgress;
    public int currentFreeBoostersProgress;
    public double currentChickenBank;
    public int maxChickenBank;
    public string unFinishWord = "Unfinish_Word";
    private string findWordMessage = "Find the words built from the letters ";

    private bool isNotificationOn;
    public bool IsNotificationOn
    {
        get
        {
            int intToBool = PlayerPrefs.GetInt("Is_Notification_On");
            isNotificationOn = true;
            if (intToBool == -1) { isNotificationOn = false; }
            return isNotificationOn;
        }
        set
        {
            isNotificationOn = value;
            int intTobool = -1;
            if (isNotificationOn) { intTobool = 1; CancelAndPushManyNotification(); }
            PlayerPrefs.SetInt("Is_Notification_On", intTobool);
        }
    }

    private bool ingame = false;

    private void Awake()
    {
        instance = this;

        maxFreeBoostersProgress = PlayerPrefs.GetInt("Max_FreeBooster_Progress"); // DailyGiftsDialog
        //Debug.Log(maxFreeBoostersProgress);

        currentFreeBoostersProgress = CPlayerPrefs.GetInt("PROGRESS", 0);
        //Debug.Log(currentFreeBoostersProgress);

        currentChickenBank = FacebookController.instance.user.currBank; // ChickenBankController
        //Debug.Log(currentChickenBank);

        maxChickenBank = ConfigController.instance.config.gameParameters.maxBank;
        //Debug.Log(maxChickenBank);
    }
    private void Start()
    {
        //InGame();
        InGamePushNotification();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // OutGame();
            OutGamePushNotification();
        }
        else
        {
            // InGame();
            InGamePushNotification();
        }
    }
    private void OnApplicationQuit()
    {
        //  OutGame();
        OutGamePushNotification();
    }
    private TimeSpan RunCodeAtSpecificTime(string DAILYTIME)
    {
        string[] timeParts = DAILYTIME.Split(new char[1] { ':' });

        DateTime dateNow = DateTime.Now;
        DateTime specificTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                   int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
        TimeSpan intervalTime;
        if (specificTime > dateNow)
            intervalTime = specificTime - dateNow;
        else
        {
            specificTime = specificTime.AddDays(1);
            intervalTime = specificTime - dateNow;
        }
        return intervalTime;
    }
    private TimeSpan NotificationCall(double TIME)
    {
        TimeSpan specificTime = TimeSpan.FromSeconds(TIME);
        Task.Delay(specificTime).ContinueWith((x) => { });
        return specificTime;
    }
    private void PushManyNotifications(string DAILYTIME)
    {
        if (!IsNotificationOn) return;

        double tempSeconds = RunCodeAtSpecificTime(DAILYTIME).TotalSeconds;
        float randomNum = UnityEngine.Random.Range(0f, 3f * hourToSecondsValue);
        tempSeconds += (double)randomNum;

        for (int i = 0; i < MAX_CURRENT_LOOP; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0) PushChickenBankNotification(tempSeconds + (i * dayToSecondsValue));
            else if (randomIndex == 1) PushFindWordsNotification(tempSeconds + (i * dayToSecondsValue));
            else if (randomIndex == 2) PushFreeBoostersNotification(tempSeconds + (i * dayToSecondsValue));
        }
    }
    private string RandomDailyTimePick(string DAILY_TIME1, string DAILY_TIME2)
    {
        string dailyTime = DAILY_TIME1;
        if (UnityEngine.Random.Range(0, 2) == 1) dailyTime = DAILY_TIME2;
        return dailyTime;
    }
    private void CancelAndPushManyNotification()
    {
        NotificationManager.CancelAll();
        string DAILY_TIME = RandomDailyTimePick(DAILYTIME_MORNING, DAILYTIME_AFTERNOON);
        PushManyNotifications(DAILY_TIME);
    }
    private void OutGamePushNotification()
    {
        ingame = false;

        CancelAndPushManyNotification();
    }
    private void InGamePushNotification()
    {
        if (ingame) return;
        ingame = true;

        CancelAndPushManyNotification();
    }
    private string CheckUnfinishLine()
    {
        string unFinishWordDefault = "A, B, C";

        if (PlayerPrefs.GetString(unFinishWord) != null)
        {
            unFinishWordDefault = PlayerPrefs.GetString(unFinishWord);
            if (unFinishWordDefault == null || unFinishWordDefault == string.Empty)
            {
                if (FacebookController.instance.user.answerProgress[0] != null || FacebookController.instance.user.answerProgress[0] != string.Empty)
                {
                    unFinishWordDefault = FacebookController.instance.user.answerProgress[0];
                }
                else
                {
                    unFinishWordDefault = "A, B, C";
                }
            }
            string[] stringArr = unFinishWordDefault.Split();
            List<string> stringList = new List<string>();
            stringList.AddRange(stringArr);
            stringList.Sort();
            unFinishWordDefault = string.Empty;
            if (stringList.Count > 1)
            {
                for (int i = 0; i < stringList.Count; i++)
                {
                    if (i < stringList.Count - 1)
                    {
                        unFinishWordDefault += stringList[i] + ", ";
                    }
                    else if (i == stringList.Count - 1)
                    {
                        unFinishWordDefault += stringList[i];
                    }
                }
            }
        }

        return unFinishWordDefault;
    }
    private void PushChickenBankNotification(double delay)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(currentChickenBank < maxChickenBank) { PushFindWordsNotification(delay); return; }

        string message = "Chicken Bank is full. Crack to open it!";
        NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(delay), "Word Puzzle Connect", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
    }
    private void PushFindWordsNotification(double delay)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string message = findWordMessage + CheckUnfinishLine();
        NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(delay), "Word Puzzle Connect", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
    }
    private void PushFreeBoostersNotification(double delay)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (currentFreeBoostersProgress < maxFreeBoostersProgress || maxFreeBoostersProgress == 0 ) { PushFindWordsNotification(delay); return; }

        string message = "Free Boosters are ready! Let's discover what they are!";
        NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(delay), "Word Puzzle Connect", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
    }

    /////////////////////////////////////////////////////////////////// Old function
    private void InGame()
    {
        if (ingame) return;
        ingame = true;

        if (!PlayerPrefs.HasKey("out_game_time")) return;

        double restTimeSecs = CUtils.GetCurrentTime() - CUtils.GetActionTime("out_game");
        double restTime = restTimeSecs / 3600;

        if (PlayerPrefs.HasKey("level_progress"))
        {
            if (Prefs.isNoti1Enabled && restTime > NOTI_DELAY_1)
            {
                UnlockLetter();
            }

            if (Prefs.isNoti2Enabled && restTime > NOTI_DELAY_2)
            {
                UnlockLetter();
            }
        }

        int num = 0;

        if (restTime > NOTI_DELAY_3)
        {
            GiveFreeRuby(Prefs.noti3Ruby);
            num += Prefs.noti3Ruby;
        }

        if (restTime > NOTI_DELAY_4)
        {
            GiveFreeRuby(Prefs.noti4Ruby);
            num += Prefs.noti4Ruby;
        }

        if (restTime > NOTI_DELAY_5)
        {
            GiveFreeRuby(Prefs.noti5Ruby);
            num += Prefs.noti5Ruby;
        }

        if (restTime > NOTI_DELAY_6)
        {
            GiveFreeRuby(Prefs.noti6Ruby);
            num += Prefs.noti6Ruby;
        }

        if (restTime > NOTI_DELAY_7)
        {
            GiveFreeRuby(Prefs.noti7Ruby);
            num += Prefs.noti7Ruby;
        }

        if (num > 0)
        {
            Toast.instance.ShowMessage(string.Format("You got {0} free rubies", num));
        }

        Prefs.isNoti1Enabled = false;
        Prefs.isNoti2Enabled = false;
        //NotificationManager.CancelAll();
    }

    private void OutGame()
    {
        ingame = false;

        if (PlayerPrefs.HasKey("level_progress"))
        {
            char? letter = GetUnlockLetter(0);
            if (letter != null)
            {
                PushUnlockLetter((char)letter, NOTI_DELAY_1);
                Prefs.isNoti1Enabled = true;
            }

            letter = GetUnlockLetter(1);
            if (letter != null)
            {
                PushUnlockLetter((char)letter, NOTI_DELAY_2);
                Prefs.isNoti2Enabled = true;
            }
        }

        int num = PushFreeRuby(NOTI_DELAY_3);
        Prefs.noti3Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_4);
        Prefs.noti4Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_5);
        Prefs.noti5Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_6);
        Prefs.noti6Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_7);
        Prefs.noti7Ruby = num;

        CUtils.SetActionTime("out_game");
    }

    private TwoValues<int, int> GetUnlockLetterIndex(string[] progress, int numPass = 0)
    {
        int numEmpty = 0;
        int tmpNumPass = numPass;
        TwoValues<int, int> t = null;

        for (int i = 0; i < progress.Length; i++)
        {
            for (int j = 0; j < progress[i].Length; j++)
            {
                if (progress[i][j] == '0')
                {
                    if (t == null)
                    {
                        if (tmpNumPass == 0)
                            t = new TwoValues<int, int>(i, j);
                        else
                            tmpNumPass--;
                    }

                    numEmpty++;
                }
            }
        }

        if (numEmpty - numPass >= 2)
            return t;
        else
            return null;
    }

    private char? GetUnlockLetter(int numPass)
    {
        string[] progress = Prefs.levelProgress;
        GameLevel gameLevel = Utils.Load(Prefs.unlockedWorld, Prefs.unlockedSubWorld, Prefs.unlockedLevel);
        var answers = CUtils.BuildListFromString<string>(gameLevel.answers);

        var index = GetUnlockLetterIndex(progress, numPass);
        if (index != null && index.Item1 < answers.Count)
        {
            if (index.Item2 < answers[index.Item1].Length)
            {
                char letter = answers[index.Item1][index.Item2];
                letter = char.ToUpper(letter);
                return letter;
            }
            else return null;
        }
        return null;
    }

    private void UnlockLetter()
    {
        string[] progress = Prefs.levelProgress;
        var index = GetUnlockLetterIndex(progress);

        if (index == null) return;
        if (index.Item1 >= progress.Length) return;

        StringBuilder sb = new StringBuilder(progress[index.Item1]);
        sb[index.Item2] = '1';
        progress[index.Item1] = sb.ToString();

        Prefs.levelProgress = progress;
    }

    private void GiveFreeRuby(int num)
    {
        CurrencyController.CreditBalance(num);
    }

    private void PushUnlockLetter(char letter, double delay)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string message = "We unlocked a letter " + letter + " for you. Open the game and continue :)";
        NotificationManager.SendWithAppIcon(TimeSpan.FromHours(delay), "Unlock letter", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
    }

    private int PushFreeRuby(double delay)
    {
        int num = UnityEngine.Random.Range(5, 8);
#if UNITY_ANDROID && !UNITY_EDITOR
        string message = "You got " + num + " free rubies. Let's play and challenge yourself";
        NotificationManager.SendWithAppIcon(TimeSpan.FromHours(delay), "Ruby gift", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
        return num;
    }
}
