using Spine.Unity;
using Superpow;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance { get; private set; }
    [HideInInspector] public bool isShowTut;
    [HideInInspector] public bool isBlockSwipe;
    [HideInInspector] public bool isTutBeehive;
    [HideInInspector] public bool isTutFlag;
    public string contentNext;
    public string contentWordAgain;
    public string contentManipulation;
    public string contentHintFree;
    public string contentShuffle;
    public string contentSelectedHint;
    public string contentSelectedHint2;
    public string contentMultipleHint;
    public string contentBonusBox;
    public string contentUnlockBonusBox;
    public string contentCellStar;
    public string contentSetting;
    public string contentBeehive;
    public string contentHelp;
    public string contentChickenBank;
    public string contentFreeBoosters;
    public string contentCellAds;
    public string contentHoneyHeader;
    public string contentFlag;
    [SerializeField] private GameObject _popText;
    [SerializeField] private GameObject _popHint;
    [SerializeField] private GameObject _popShuffle;
    [SerializeField] private GameObject _popSelectedHint;
    [SerializeField] private GameObject _popSelectedHint2;
    [SerializeField] private GameObject _popMultipleHint;
    [SerializeField] private GameObject _popBonusBox;
    [SerializeField] private GameObject _popCellStar;
    [SerializeField] private GameObject _popSetting;
    [SerializeField] private GameObject _popObjective;
    [SerializeField] private GameObject _popBeehive;
    [SerializeField] private GameObject _popHelp;
    [SerializeField] private GameObject _popChickenBank;
    [SerializeField] private GameObject _popFreeBoosters;
    [SerializeField] private GameObject _popCellAds;
    [SerializeField] private GameObject _popHoneyHeader;
    [SerializeField] private GameObject _popFlag;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private TextMeshProUGUI _textTutorial;
    [SerializeField] private TextMeshProUGUI _textTutorialHint;
    [SerializeField] private TextMeshProUGUI _textTutorialShuffle;
    [SerializeField] private TextMeshProUGUI _textTutorialSelectedHint;
    [SerializeField] private TextMeshProUGUI _textTutorialSelectedHint2;
    [SerializeField] private TextMeshProUGUI _textTutorialMultipleHint;
    [SerializeField] private TextMeshProUGUI _textTutorialBonusBox;
    [SerializeField] private TextMeshProUGUI _textTutorialCellStar;
    [SerializeField] private TextMeshProUGUI _textTutorialSetting;
    [SerializeField] private TextMeshProUGUI _textTutorialBeehive;
    [SerializeField] private TextMeshProUGUI _textTutorialHelp;
    [SerializeField] private TextMeshProUGUI _textTutorialChickenBank;
    [SerializeField] private TextMeshProUGUI _textTutorialFreeBoosters;
    [SerializeField] private TextMeshProUGUI _textTutorialCellAds;
    [SerializeField] private TextMeshProUGUI _textTutorialHoneyHeader;
    [SerializeField] private TextMeshProUGUI _textTutorialFlag;
    [Space]
    [SerializeField] private GameObject _handCellAdsTut;
    [SerializeField] private Image _handConnectTut;
    [SerializeField] private Image _handChickenTut;
    [SerializeField] private Image _handFlagTut;
    [SerializeField] private SpineControl _animBeehiveTut;

    public GameObject _handPanelPopHint;
    public GameObject _handPanelPopShuffle;
    public GameObject _handPanelPopSelectedHint;
    public GameObject _handPanelMultipleHint;
    public GameObject _handPanelPopHelp;
    public GameObject _handPanelPopBonusBox;

    [Header("LEVEL SETUP")]
    public int beehiveLevel = 40;
    public int objectiveLevel = 11;
    public int bonusBoxLevel = 10;
    public int hintLevel = 2;
    public int selectedHintLevel = 23;
    public int multipleHintLevel = 30;
    public int shuffleLevel = 5;
    public int cellStarLevel = 8;
    public int helpLevel = 16;

    private LineWord _lineTarget;
    private string _answerTarget;
    private Vector3 mousePoint;
    private float RADIUS = 1.0f;

    public LineWord LineTarget
    {
        get
        {
            return _lineTarget;
        }
    }

    public string AnswerTarget
    {
        get
        {
            return _answerTarget;
        }
    }

    void Start()
    {
        if (instance == null) instance = this;
    }

    public void ShowPopWordTut(string contentPop, int indexAnser = 0, bool lineNotShown = true, string contentAfter = "", bool hidenTextContent = false, LineWord lineCheck = null)
    {
        if (WordRegion.instance.BtnADS != null)
        {
            WordRegion.instance.BtnADS._btnAds.interactable = false;
        }
        if (BlockScreen.instance != null)
            BlockScreen.instance.Block(true);
        isShowTut = true;
        _overlay.SetActive(true);
        _overlay.GetComponent<Canvas>().sortingOrder = 1;
        _popText.SetActive(true);
        var answerTargetRandom = "";
        var letterOrders = new List<Text>();
        if (lineNotShown)
        {
            foreach (var line in WordRegion.instance.Lines)
            {
                if (!line.isShown)
                {
                    _lineTarget = line;
                    LineTarget.lineTutorialBG.sprite = ThemesControl.instance.CurrTheme.uiData.bgTutorialLine;
                    _answerTarget = line.answers[indexAnser];
                    answerTargetRandom = _answerTarget;
                    line.SetDataLetter(line.answers[indexAnser]);
                    if (!hidenTextContent)
                        _textTutorial.text = contentPop + " <color=green>" + _answerTarget + "</color>";
                    LineTarget.GetComponent<Canvas>().overrideSorting = true;
                    LineTarget.GetComponent<Canvas>().sortingOrder = 1;
                    LineTarget.lineTutorialBG.gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            //foreach (var line in WordRegion.instance.Lines)
            //{
            //    if (line.isShown && line.answers.Count > 1)
            //    {

            _lineTarget = lineCheck != null ? lineCheck : WordRegion.instance.Lines.Find(line => line.isShown && line.answers.Count > 1);
            LineTarget.lineTutorialBG.sprite = ThemesControl.instance.CurrTheme.uiData.bgTutorialLine;
            var otherAnswers = _lineTarget.answers.FindAll(ans => ans != _lineTarget.answer);
            answerTargetRandom = otherAnswers[Random.Range(0, otherAnswers.Count)];
            var index = 0;
            _answerTarget = "";
            foreach (var ans in otherAnswers)
            {
                if (index < otherAnswers.Count - 1)
                    _answerTarget += ans + ", ";
                else
                    _answerTarget += ans;
                index++;
            }
            if (!hidenTextContent)
                _textTutorial.text = contentPop + " <color=green>" + _answerTarget + "</color>" + contentAfter;
            LineTarget.GetComponent<Canvas>().overrideSorting = true;
            LineTarget.GetComponent<Canvas>().sortingOrder = 1;
            LineTarget.lineTutorialBG.gameObject.SetActive(true);
            //break;

            //    }
            //}
        }
        PlayHandConnectWordTut(answerTargetRandom, letterOrders);
    }

    #region Hand Connect Word tutorial
    private void PlayHandConnectWordTut(string answerTargetRandom, List<Text> letterOrders)
    {
        if (Pan.instance != null)
        {
            var count = 0;
            var letters = Pan.instance.LetterTexts;
            for (int i = 0; i < answerTargetRandom.Length; i++)
            {
                var letterText = letters.Find(lt => lt.text == answerTargetRandom[i].ToString());
                letterOrders.Add(letterText);
            }
            MoveHandConnectWord(letterOrders, count);
        }
    }

    private void MoveHandConnectWord(List<Text> letterOrders, int count)
    {
        var tweenControl = TweenControl.GetInstance();
        _handConnectTut.transform.position = letterOrders[count].transform.position;
        _handConnectTut.gameObject.SetActive(true);
        count += 1;
        if (count < letterOrders.Count)
        {
            tweenControl.Move(_handConnectTut.transform, letterOrders[count].transform.position, 0.75f, () =>
            {
                MoveHandConnectWord(letterOrders, count);
            }, EaseType.Linear, ShowLineDraw);
        }
        else
        {
            tweenControl.DelayCall(_handConnectTut.transform, 2f, () =>
            {
                HidenHandConnectWord(true);
                count = 0;
                tweenControl.DelayCall(_handConnectTut.transform, 2f, () =>
                {
                    MoveHandConnectWord(letterOrders, count);
                });
            });
        }
    }

    private void ShowLineDraw()
    {
        if (LineDrawer.instance != null && _handConnectTut.gameObject.activeInHierarchy)
        {
            mousePoint = _handConnectTut.transform.position;
            mousePoint.z = 90;
            int nearest = GetNearestPosition(mousePoint, LineDrawer.instance.letterPositions);

            Vector3 letterPosition = LineDrawer.instance.letterPositions[nearest];
            if (Vector3.Distance(letterPosition, mousePoint) < RADIUS)
            {
                if (LineDrawer.instance.currentIndexes.Count >= 2 && LineDrawer.instance.currentIndexes[LineDrawer.instance.currentIndexes.Count - 2] == nearest)
                {
                    LineDrawer.instance.currentIndexes.RemoveAt(LineDrawer.instance.currentIndexes.Count - 1);
                }
                else if (!LineDrawer.instance.currentIndexes.Contains(nearest))
                {
                    LineDrawer.instance.currentIndexes.Add(nearest);
                }
            }
            BuildPoints();

            if (LineDrawer.instance.points.Count >= 2)
            {
                LineDrawer.instance.positions = iTween.GetSmoothPoints(LineDrawer.instance.points.ToArray(), 8);
                LineDrawer.instance.LineRenderer.positionCount = LineDrawer.instance.positions.Count;
                LineDrawer.instance.LineRenderer.SetPositions(LineDrawer.instance.positions.ToArray());
            }
        }
    }

    private void BuildPoints()
    {
        LineDrawer.instance.points.Clear();
        foreach (var i in LineDrawer.instance.currentIndexes) LineDrawer.instance.points.Add(LineDrawer.instance.letterPositions[i]);

        if (LineDrawer.instance.currentIndexes.Count == 1 || LineDrawer.instance.points.Count >= 1 && Vector3.Distance(mousePoint, LineDrawer.instance.points[LineDrawer.instance.points.Count - 1]) >= RADIUS)
        {
            LineDrawer.instance.points.Add(mousePoint);
        }
    }

    private int GetNearestPosition(Vector3 point, List<Vector3> letters)
    {
        float min = float.MaxValue;
        int index = -1;
        for (int i = 0; i < letters.Count; i++)
        {
            float distant = Vector3.Distance(point, letters[i]);
            if (distant < min)
            {
                min = distant;
                index = i;
            }
        }
        return index;
    }

    public void HidenHandConnectWord(bool killTween = true)
    {
        if (_handConnectTut != null)
        {
            if (killTween)
            {
                TweenControl.GetInstance().KillDelayCall(_handConnectTut.transform);
                TweenControl.GetInstance().KillTweener(_handConnectTut.transform);
                TweenControl.GetInstance().KillTween(_handConnectTut.transform);
            }
            if (_handConnectTut.gameObject.activeInHierarchy)
            {
                LineDrawer.instance.positions.Clear();
                LineDrawer.instance.LineRenderer.positionCount = 0;
                LineDrawer.instance.currentIndexes.Clear();
                LineDrawer.instance.lineParticle.SetActive(false);
                _handConnectTut.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    public void ShowPopHintFreeTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHint.gameObject.SetActive(true);
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHint.SetActive(true);
        _textTutorialHint.text = contentHintFree;
    }

    public void ShowPopShuffleTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnShuffle.gameObject.SetActive(true);
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popShuffle.SetActive(true);
        _textTutorialShuffle.text = contentShuffle;
    }

    public void ShowPopBonusBoxTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnBonusBox.gameObject.SetActive(true);
            WordRegion.instance.btnBonusBox.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popBonusBox.SetActive(true);
        _textTutorialBonusBox.text = contentBonusBox;
    }

    public void ShowPopSelectedHintTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHintTarget.gameObject.SetActive(true);
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSelectedHint.SetActive(true);
        _textTutorialSelectedHint.text = contentSelectedHint;
    }

    public void ShowPopSelectedHint2Tut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.GetComponent<Canvas>().sortingLayerName = "UI2";
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSelectedHint2.SetActive(true);
        _textTutorialSelectedHint2.text = contentSelectedHint2;
    }

    public void ShowPopMultipleTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnMultipleHint.gameObject.SetActive(true);
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popMultipleHint.SetActive(true);
        _textTutorialMultipleHint.text = contentMultipleHint;
    }

    public void ShowPopCellStarTut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popCellStar.SetActive(true);
        _textTutorialCellStar.text = contentCellStar;

        _lineTarget = WordRegion.instance.Lines[WordRegion.instance.Lines.Count - 1];
        var canvas = LineTarget.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        LineTarget.lineTutorialBG.sprite = ThemesControl.instance.CurrTheme.uiData.bgTutorialLine;
        LineTarget.lineTutorialBG.gameObject.SetActive(true);

        foreach (var cellTut in _lineTarget.cells)
        {
            if (!cellTut.isShown)
            {
                cellTut.iconCoin.transform.localScale = Vector3.one;
            }
        }
    }

    public void ShowPopSettingTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnSetting.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSetting.SetActive(true);
        _textTutorialSetting.text = contentSetting;
    }

    public void ShowPopObjectiveTut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI2";
        _popObjective.SetActive(true);
    }

    public void ShowPopBeeTut()
    {
        if(ThemesControl.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _animBeehiveTut.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            _animBeehiveTut.SetSkin(currTheme.animData.skinAnim);
        }
        isShowTut = true;
        isBlockSwipe = true;
        isTutBeehive = true;
        _overlay.SetActive(true);
        _popBeehive.SetActive(true);
        _textTutorialBeehive.text = contentBeehive;
    }

    public void BeeFly()
    {
        MainController.instance.beeController.OnBeeButtonClick();
    }

    public void ShowPopHelpTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHelp.gameObject.SetActive(true);
            WordRegion.instance.btnHelp.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHelp.SetActive(true);
        _textTutorialHelp.text = contentHelp;
    }

    public void ShowPopChickenBankTut()
    {
        if (WinDialog.instance != null)
        {
            WinDialog.instance._chickenBank.SetActive(true);
            var canvas = WinDialog.instance._chickenBank.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI2";
            canvas.sortingOrder = 5;
            _handChickenTut.transform.position = WinDialog.instance._chickenBank.transform.position;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI2";
        _popChickenBank.SetActive(true);
        _textTutorialChickenBank.text = contentChickenBank;
    }

    public void ShowPopFreeBoostersTut()
    {
        if (HomeController.instance != null)
        {
            HomeController.instance.btnFreeBoosters.gameObject.SetActive(true);
            HomeController.instance.FreeBoostersShadow.SetActive(true);
            var canvas = HomeController.instance.btnFreeBoosters.GetComponent<Canvas>();
            canvas.overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI";
        _popFreeBoosters.SetActive(true);
        _textTutorialFreeBoosters.text = contentFreeBoosters;
    }

    public void ShowPopCellAdsTut()
    {
        CPlayerPrefs.SetBool("CELL_ADS_TUTORIAL", true);
        if (WordRegion.instance.BtnADS != null)
        {
            var canvas = WordRegion.instance.BtnADS.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI1";
            _handCellAdsTut.transform.position = WordRegion.instance.BtnADS.transform.position;
        }
        isShowTut = true;
        _overlay.SetActive(true);
        _popCellAds.SetActive(true);
        _textTutorialCellAds.text = contentCellAds;

    }

    public void ShowPopHoneyHeaderTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.bgHoney.gameObject.SetActive(true);
            var canvas = WordRegion.instance.bgHoney.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI1";
        }
        isShowTut = true;
        isTutFlag = true;
        _overlay.SetActive(true);
        _popHoneyHeader.SetActive(true);
        _textTutorialHoneyHeader.text = contentHoneyHeader;

    }

    public void ShowPopFlagTut(FlagItemController flagItem)
    {
        CPlayerPrefs.SetBool("HONEY_TUTORIAL", true);
        isShowTut = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI2";
        canvasOverlay.sortingOrder = 5;
        _popFlag.SetActive(true);
        string unlockWord = flagItem.flagUnlockWord != string.Empty ? flagItem.flagUnlockWord : flagItem.flagName;       
        _textTutorialFlag.text = "Tap on the flag to view information. You have found " + '"' + unlockWord.ToUpper() + '"' + " word to unlock " + flagItem.flagName.ToUpper() + " Flag.";
        _handFlagTut.transform.position = flagItem.transform.position;
    }

    public void HidenPopTut()
    {
        if (isTutBeehive && MainController.instance != null)
        {
            CPlayerPrefs.SetBool("BEE_TUTORIAL", true);
            MainController.instance.beeController.OnBeeButtonClick();
        }
        if (WordRegion.instance != null)
        {
            if (WordRegion.instance.BtnADS != null)
            {
                WordRegion.instance.BtnADS._btnAds.interactable = true;
                var canvas = WordRegion.instance.BtnADS.GetComponent<Canvas>();
                canvas.overrideSorting = false;
                canvas.sortingLayerName = "Default";
            }
            WordRegion.instance.GetComponent<Canvas>().sortingLayerName = "UI1";
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnBonusBox.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnSetting.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHelp.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.bgHoney.GetComponent<Canvas>().overrideSorting = false;
            if (LineTarget != null)
            {
                LineTarget.GetComponent<Canvas>().overrideSorting = false;
                LineTarget.lineTutorialBG.gameObject.SetActive(false);
            }
        }
        if (WinDialog.instance != null)
        {
            WinDialog.instance._chickenBank.GetComponent<Canvas>().overrideSorting = false;
        }
        if (HomeController.instance != null)
        {
            var canvas = HomeController.instance.btnFreeBoosters.GetComponent<Canvas>();
            canvas.overrideSorting = false;
        }
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "Default";
        canvasOverlay.sortingOrder = 3;
        isShowTut = false;
        isBlockSwipe = false;
        isTutBeehive = false;
        isTutFlag = false;

        if (_popText != null) _popText.SetActive(false);
        if (_popHint != null) _popHint.SetActive(false);
        if (_popShuffle != null) _popShuffle.SetActive(false);
        if (_popSelectedHint != null) _popSelectedHint.SetActive(false);
        if (_popSelectedHint2 != null) _popSelectedHint2.SetActive(false);
        if (_popMultipleHint != null) _popMultipleHint.SetActive(false);
        if (_popBonusBox != null) _popBonusBox.SetActive(false);
        if (_popCellStar != null) _popCellStar.SetActive(false);
        if (_popSetting != null) _popSetting.SetActive(false);
        if (_popObjective != null) _popObjective.SetActive(false);
        if (_popBeehive != null) _popBeehive.SetActive(false);
        if (_popHelp != null) _popHelp.SetActive(false);
        if (_popChickenBank != null) _popChickenBank.SetActive(false);
        if (_popFreeBoosters != null) _popFreeBoosters.SetActive(false);
        if (_popCellAds != null) _popCellAds.SetActive(false);
        if (_popHoneyHeader != null) _popHoneyHeader.SetActive(false);
        if (_popFlag != null) _popFlag.SetActive(false);
        if (_overlay != null) _overlay.SetActive(false);
        if (_textTutorial != null) _textTutorial.text = "";


        if (CPlayerPrefs.HasKey("BEE_TUTORIAL") && WordRegion.instance != null)
        {
            var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
            var currlevel = WordRegion.instance.CurLevel;
            if (currlevel >= beehiveLevel)
                Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialComplete);
        }
    }

    public void CheckAndShowTutorial()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = WordRegion.instance.CurLevel;
        if (!CPlayerPrefs.HasKey("LEVEL " + currlevel) && !isShowTut)
        {
            if ((currlevel >= beehiveLevel && !CPlayerPrefs.HasKey("BEE_TUTORIAL")) || (BeeManager.instance.CurrBee > 0 && !CPlayerPrefs.HasKey("BEE_TUTORIAL")))
            {
                BeeManager.instance.CreaditAmountBee(3);
                ShowPopBeeTut();
            }
            else if ((currlevel >= hintLevel && !CPlayerPrefs.HasKey("HINT_TUTORIAL")) || (CurrencyController.GetHintFree() > 0 && !CPlayerPrefs.HasKey("HINT_TUTORIAL")))
            {
                CurrencyController.CreditHintFree(2);
                ShowPopHintFreeTut();
                CPlayerPrefs.SetBool("HINT_TUTORIAL", true);
            }
            else if ((currlevel >= objectiveLevel && !CPlayerPrefs.HasKey("OBJ_TUTORIAL") && ObjectiveManager.instance.Icon.activeInHierarchy) /*|| (Prefs.countLevelDaily >= 10 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL"))*/)
            {
                ShowPopSettingTut();
            }
            else if ((currlevel >= selectedHintLevel && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")) || (CurrencyController.GetSelectedHintFree() > 0 && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")))
            {
                CurrencyController.CreditSelectedHintFree(2);
                ShowPopSelectedHintTut();
            }
            else if ((currlevel >= multipleHintLevel && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL")) || (CurrencyController.GetMultipleHintFree() > 0 && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL")))
            {
                CurrencyController.CreditMultipleHintFree(1);
                ShowPopMultipleTut();
            }
            else if (currlevel >= shuffleLevel && !CPlayerPrefs.HasKey("SHUFFLE_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("SHUFFLE_TUTORIAL", true);
                ShowPopShuffleTut();
            }
            else if (currlevel >= cellStarLevel && !CPlayerPrefs.HasKey("CELL_STAR_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("CELL_STAR_TUTORIAL", true);
                ShowPopCellStarTut();
            }
            else if (currlevel >= helpLevel && !CPlayerPrefs.HasKey("HELP_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("HELP_TUTORIAL", true);
                ShowPopHelpTut();
            }
            //else if (currlevel >= 10 && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD"))
            //{
            //    CPlayerPrefs.SetBool("TUT_EXTRA_WORD", true);
            //    ShowPopBonusBoxTut();
            //}

            CPlayerPrefs.SetBool("LEVEL " + currlevel, true);
        }
    }
}
