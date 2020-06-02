using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.Events;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class MissingWordsFeedback : FeedbackDialog
{
    public TMP_InputField inputfield;
    private TMP_InputField.SubmitEvent submitEvent;
    private TMP_InputField.SelectionEvent selectEvent;
    private string missingWord;

    public static DatabaseReference _missingWordsRef;
    public static Dictionary<string, object> childUpdates = new Dictionary<string, object>();


    private int _count = 0;
    protected override void Awake()
    {
        base.Awake();
        submitEvent = new TMP_InputField.SubmitEvent();
        submitEvent.AddListener(typingCall);
        inputfield.onEndEdit = submitEvent;

        // Firebase setup
        _missingWordsRef = FirebaseDatabase.DefaultInstance
            .GetReference("Missing and Irrelevant Words");

        _missingWordsRef.ValueChanged += OnCountUpdated;

    }

    private void OnDestroy()
    {
        _missingWordsRef.ValueChanged -= OnCountUpdated;
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
        string key = _missingWordsRef.Push().Key;

        childUpdates["/Missing words/" + key] = missingWord;
        _missingWordsRef.UpdateChildrenAsync(childUpdates);

        Close();
    }
    public new void OnCloseClick()
    {
        Close();
    }
    // Test Firebase Database Method
    public void IncrementClickCounter()
    {
        _missingWordsRef.RunTransaction(data =>
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

        if (e.Snapshot == null || e.Snapshot.Value == null) { } // missingWordsList = null;
        else { } // missingWordsList = (List<string>) e.Snapshot.Value;

        // We check for an error, then set the value of our _count according to what the database value returns. 
        // The return value is in the Snapshot variable.
        // Note that we check for e.Snapshot == null - this is important because if there isn't any data at the path, 
        // e.Snapshot will be null - this lets us set a default.
    }
}
