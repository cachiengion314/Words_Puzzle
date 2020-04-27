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
        DialogController.instance.onDialogsClosed += OnDialogClosed;
    }

    public void OnClickScreen()
    {
        if (overlay.gameObject.activeInHierarchy)
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
        overlay.enabled = true;
    }

    private void OnDialogClosed()
    {
        overlay.enabled = false;
    }

    private void OnDestroy()
    {
        DialogController.instance.onDialogsOpened -= OnDialogOpened;
        DialogController.instance.onDialogsClosed -= OnDialogClosed;
    }
}
