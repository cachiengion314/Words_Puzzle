using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogOverlay : MonoBehaviour
{
    public static DialogOverlay instance;
    private Image overlay;

    public Image Overlay
    {
        get
        {
            return overlay;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        overlay = GetComponent<Image>();
    }

    void Start()
    {
        DialogController.instance.onDialogsOpened += OnDialogOpened;
        DialogController.instance.onDialogsCompleteClosed += OnDialogClosed;
    }

    public void OnClickScreen()
    {
        if (overlay.gameObject.activeInHierarchy && !TutorialController.instance.isShowTut)
        {
            gameObject.GetComponent<Button>().interactable = false;
            WordRegion.instance.OnClickHintTarget();
        }
    }

    public void ShowOverlay(bool show)
    {
        gameObject.GetComponent<Button>().interactable = true;
        overlay.enabled = show;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        overlay.enabled = false;
    }

    private void OnDialogOpened()
    {
        gameObject.GetComponent<Button>().interactable = false;
        if (WordRegion.instance != null)
        {
            WordRegion.instance.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = false;
        }
        overlay.enabled = true;
    }

    private void OnDialogClosed()
    {
        if (DialogController.instance.current == null)
        {
            overlay.enabled = false;
            overlay.color = new Color32(0, 0, 0, 113);
        }
    }

    private void OnDestroy()
    {
        DialogController.instance.onDialogsOpened -= OnDialogOpened;
        DialogController.instance.onDialogsClosed -= OnDialogClosed;
    }
}
