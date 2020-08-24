using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using System;
using Superpow;
using UnityEngine.UI;

public class MissingWordsFeedback : Dialog
{
    public TMP_InputField inputfield;
    private TMP_InputField.SubmitEvent submitEvent;
    private string missingWord;

    public static DatabaseReference _dataWordsRef;
    public static Dictionary<string, object> childUpdates = new Dictionary<string, object>();

    public int currlevel;

    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _boardMissingWord;
    [SerializeField] private Image _btnSend;
    [SerializeField] private TextMeshProUGUI _txtSend;
    [SerializeField] private TextMeshProUGUI _txtTitle;

    // test data
    private int _count = 0;
    protected override void Awake()
    {
        base.Awake();
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;
        submitEvent = new TMP_InputField.SubmitEvent();
        submitEvent.AddListener(typingCall);
        inputfield.onEndEdit = submitEvent;

        // Firebase setup
        _dataWordsRef = FirebaseDatabase.DefaultInstance
            .GetReference("feedbacks");

        // _dataWordsRef.ValueChanged += OnCountUpdated;

    }

    protected override void Start()
    {
        base.Start();
        CheckTheme();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _boardMissingWord.sprite = currTheme.uiData.missingWordData.boardMissingWord;
            _btnSend.sprite = currTheme.uiData.missingWordData.btnSend;

            _boardMissingWord.SetNativeSize();
            _btnSend.SetNativeSize();

            _txtSend.color = currTheme.uiData.missingWordData.colorTextBtn;
            _txtTitle.color = currTheme.fontData.colorContentDialog;
            inputfield.textComponent.color = inputfield.placeholder.color = currTheme.uiData.missingWordData.colorTextFiel;
        }
    }

    private void OnDestroy()
    {
        _dataWordsRef.ValueChanged -= OnCountUpdated;
    }
    public void SelectCall(string arg0)
    {
        if (inputfield)
            inputfield.text = null;
    }
    public void typingCall(string arg0Words)
    {
        missingWord = arg0Words;
    }
    public void OnSendWords()
    {
        if (missingWord == null || missingWord.Length == 0 ) { Close(); return; }
       
        Dictionary<string, object> infoDic = new Dictionary<string, object>
        {
            ["type"] = "missing",
            ["results"] = missingWord,
            ["date"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["status"] = "open",
            ["level"] = currlevel
        };
        // push information
        string key = _dataWordsRef.Push().Key;
        childUpdates["/" + key] = infoDic;    
        _dataWordsRef.UpdateChildrenAsync(childUpdates);

        Close();
    }
    public void OnCloseClick()
    {
        Close();
    }
    // Test Firebase Database Method
    public void IncrementClickCounter()
    {
        _dataWordsRef.RunTransaction(data =>
        {
            data.Value = _count + 1;
            return TransactionResult.Success(data);
        }).ContinueWith(task =>
        {
            if (task.Exception != null)
                Debug.Log(task.Exception.ToString());
        });
    }
    private void OnCountUpdated(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot == null || e.Snapshot.Value == null)
        {
            _count = 0;
        }
        else
        {
            _count = int.Parse(e.Snapshot.Value.ToString());
        }
        // We check for an error, then set the value of our _count according to what the database value returns. 
        // The return value is in the Snapshot variable.
        // Note that we check for e.Snapshot == null - this is important because if there isn't any data at the path, 
        // e.Snapshot will be null - this lets us set a default.
    }
}
