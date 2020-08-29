using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Superpow;

public class Dialog : MonoBehaviour
{
    [Header("UI Theme Change")]
    public bool isCustomTheme;
    public Image bgBoard;
    public Image imageTitle;
    public Image btnClose;
    [Header("UI Setup")]
    public Animator anim;
    public AnimationClip hidingAnimation;
    public GameObject title, message;
    public Action<Dialog> onDialogOpened;
    public Action<Dialog> onDialogClosed;
    public Action onDialogCompleteClosed;
    public Action<Dialog> onButtonCloseClicked;
    public DialogType dialogType;
    public bool showDialogReward = false;
    public bool enableAd = true;
    public bool enableEscape = true;
    public bool scaleDialog = false;
    public bool resestAnim = true;
    public Image imgOverlay;
    public bool HidenMainCanvas;

    private Image _thisImage;
    private AnimatorStateInfo info;
    private bool isShowing;
    [HideInInspector] public bool isSound = true;

    protected virtual void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();

    }

    protected virtual void Start()
    {
        _thisImage = gameObject.GetComponent<Image>();
        onDialogCompleteClosed += OnDialogCompleteClosed;
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "UI2";
        DialogCallEventFirebase(dialogType.ToString());
    }

    private void Update()
    {
        if (enableEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    private void CheckTheme()
    {
        var indexTheme = 0;
        if(MainController.instance != null)
            indexTheme = CPlayerPrefs.GetInt("CURR_THEMES", 0);
        var currTheme = ThemesControl.instance.ThemesDatas[indexTheme];
        if (!isCustomTheme)
        {
            if (bgBoard != null)
            {
                bgBoard.sprite = currTheme.uiData.bgBoardDialog;
            }
            if (imageTitle != null)
            {
                imageTitle.sprite = currTheme.uiData.imageTitleDialog;
            }
            if (btnClose != null)
            {
                btnClose.sprite = currTheme.uiData.btnCloseDialog;
                btnClose.SetNativeSize();
            }
        }
    }


    public void SetTitleContent(string content)
    {
        if (title != null) title.SetText(content);
    }

    public void SetMessageContent(string content)
    {
        if (message != null) message.SetText(content);
    }

    public void HidenOverlay()
    {
        if (imgOverlay != null)
        {
            imgOverlay.color = new Color(1, 1, 1, 0);
        }
    }

    public virtual void ShowNoAnim()
    {
        if (gameObject != null)
            gameObject.SetActive(true);
        isShowing = true;

        if (scaleDialog)
        {
            anim.transform.localScale = Vector3.one;
            var canvasGroup = anim.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
                canvasGroup.alpha = 0;
        }
        onDialogOpened(this);

        if (enableAd)
        {
            Timer.Schedule(this, 0.3f, () =>
            {
                CUtils.ShowInterstitialAd();
            });
        }
    }

    private void ShowMainCanvas(bool show)
    {
        if (HidenMainCanvas)
        {
            if (MainController.instance != null)
                MainController.instance.mainCanvas.gameObject.SetActive(show);
            if (HomeController.instance != null)
                HomeController.instance.mainCanvas.gameObject.SetActive(show);
        }
    }

    public virtual void Show()
    {
        if (gameObject != null)
            gameObject.SetActive(true);
        isShowing = true;
        if (anim != null && IsIdle())
        {
            if (scaleDialog)
            {
                anim.enabled = false;
                anim.transform.localScale = Vector3.zero;
                if (anim.GetComponent<CanvasGroup>() != null)
                {
                    var canvasGroup = anim.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 0;
                    TweenControl.GetInstance().FadeAnfa(canvasGroup, 1, 0.5f);
                }
                TweenControl.GetInstance().ScaleFromZero(anim.gameObject, 0.5f, () =>
                {
                    ShowMainCanvas(false);
                });
            }
            else
            {
                anim.SetTrigger("show");
                TweenControl.GetInstance().DelayCall(anim.transform, anim.GetCurrentAnimatorClipInfo(0).Length,()=> {
                    ShowMainCanvas(false);
                });
            }
            onDialogOpened(this);
        }

        if (enableAd)
        {
            Timer.Schedule(this, 0.3f, () =>
            {
                CUtils.ShowInterstitialAd();
            });
        }
    }

    public virtual void Close()
    {
        var _tweenControll = TweenControl.GetInstance();
        if (isShowing == false) return;
        if (isSound) Sound.instance.Play(Sound.Others.PopupClose);
        isShowing = false;
        if (anim != null /*&& IsIdle()*/ && hidingAnimation != null)
        {
            if (scaleDialog)
            {
                if (anim.GetComponent<CanvasGroup>() != null)
                {
                    var canvasGroup = anim.GetComponent<CanvasGroup>();
                    if (canvasGroup.alpha == 1)
                        _tweenControll.FadeAnfa(canvasGroup, 0, 0.3f);
                }
                if (_thisImage != null)
                    _tweenControll.FadeAnfa(_thisImage, 0, 0.3f);
                _tweenControll.FadeAnfa(_thisImage, 0, 0.3f);
                if (DialogOverlay.instance != null && DialogController.instance.dialogs.Count == 1)
                    _tweenControll.FadeAnfa(DialogOverlay.instance.Overlay, 0, 0.3f);
                _tweenControll.ScaleFromOne(anim.gameObject, 0.3f, () =>
                {
                    ShowMainCanvas(true);
                    DoClose();
                });
            }
            else
            {
                anim.SetTrigger("hide");
                //Timer.Schedule(this, hidingAnimation.length, DoClose);
                _tweenControll.KillDelayCall(transform);
                if (_thisImage != null)
                    _tweenControll.FadeAnfa(_thisImage, 0, hidingAnimation.length);
                if (DialogOverlay.instance != null && DialogController.instance.dialogs.Count == 1)
                    _tweenControll.FadeAnfa(DialogOverlay.instance.Overlay, 0, 0.3f);
                _tweenControll.DelayCall(transform, hidingAnimation.length, () =>
                {
                    ShowMainCanvas(true);
                    DoClose();
                });
            }
        }
        else
        {
            DoClose();
        }
        onDialogClosed(this);
    }

    private void DoClose()
    {
        var gameData = Resources.Load<GameData>("GameData");
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = (Prefs.unlockedLevel + numlevels * Prefs.unlockedSubWorld + gameData.words[0].subWords.Count * numlevels * Prefs.unlockedWorld) + 1;
        if (this != null)
            Destroy(gameObject);
        if (onDialogCompleteClosed != null) onDialogCompleteClosed();
        if (showDialogReward && currlevel >= AdsManager.instance.MinLevelToLoadRewardVideo)
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.FreeStars, DialogShow.STACK_DONT_HIDEN);
        }
    }

    public void Hide()
    {
        resestAnim = false;
        gameObject.SetActive(false);
        isShowing = false;
    }

    public void DontHiden()
    {
        resestAnim = false;
        isShowing = false;
    }

    public bool IsIdle()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Idle");
    }

    public bool IsShowing()
    {
        return isShowing;
    }

    public virtual void OnDialogCompleteClosed()
    {
        //if (ExtraWord.instance != null && DialogController.instance.current == null)
        //    ExtraWord.instance.OnClaimed();
        onDialogCompleteClosed -= OnDialogCompleteClosed;
    }

    public void PlayButton()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void DialogCallEventFirebase(string nameDialog)
    {
        var parameters = new Firebase.Analytics.Parameter[]
        {
            new Firebase.Analytics.Parameter("dialog_name", nameDialog)
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("dialog_used", parameters);
    }
}
