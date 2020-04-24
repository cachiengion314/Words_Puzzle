using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour
{
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

    private AnimatorStateInfo info;
    private bool isShowing;

    protected virtual void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        onDialogCompleteClosed += OnDialogCompleteClosed;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        if (enableEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
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

                });
            }
            else
            {
                anim.SetTrigger("show");
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
        Sound.instance.Play(Sound.Others.PopupClose);
        if (isShowing == false) return;
        isShowing = false;
        if (anim != null && IsIdle() && hidingAnimation != null)
        {
            if (scaleDialog)
            {
                if (anim.GetComponent<CanvasGroup>() != null)
                {
                    var canvasGroup = anim.GetComponent<CanvasGroup>();
                    if (canvasGroup.alpha == 1)
                        TweenControl.GetInstance().FadeAnfa(canvasGroup, 0, 0.3f);
                }
                TweenControl.GetInstance().ScaleFromOne(anim.gameObject, 0.3f, () =>
                {
                    DoClose();
                });
            }
            else
            {
                anim.SetTrigger("hide");
                Timer.Schedule(this, hidingAnimation.length, DoClose);
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
        Destroy(gameObject);
        if (onDialogCompleteClosed != null) onDialogCompleteClosed();
        if (showDialogReward)
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.FreeStars, DialogShow.STACK);
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
}
