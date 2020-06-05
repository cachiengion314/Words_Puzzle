using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
public class ContactUsDialog : Dialog
{
    private TMP_InputField.SubmitEvent submitBodyEvent;
    private TMP_InputField.SubmitEvent submitEmailEvent;

    public TMP_InputField inputFieldYourEmail;
    public TMP_InputField inputFieldBody;

    private string email;
    private string emailBody;
    protected override void Awake()
    {
        base.Awake();
        // get the reference key in database
        MissingWordsFeedback._dataWordsRef = FirebaseDatabase.DefaultInstance
        .GetReference("feedbacks");

        submitBodyEvent = new TMP_InputField.SubmitEvent();
        submitBodyEvent.AddListener(EndEditInputFieldBodyCallback);
        inputFieldBody.onEndEdit = submitBodyEvent;

        submitEmailEvent = new TMP_InputField.SubmitEvent();
        submitEmailEvent.AddListener(EndEditinputFieldYourEmailCallback);
        inputFieldYourEmail.onEndEdit = submitEmailEvent;
    }
    public void EndEditInputFieldBodyCallback(string arg)
    {
        emailBody = arg;
    }
    public void EndEditinputFieldYourEmailCallback(string arg)
    {
        email = arg;
    }
    public void SelectYourEmailCall(string arg0)
    {
        inputFieldYourEmail.text = null;
    }
    public void SelectBodyCall(string arg0)
    {
        inputFieldBody.text = null;
    }
    public void OnSendEmailFirebase()
    {
        Dictionary<string, object> infoDic = new Dictionary<string, object>
        {
            ["type"] = "contact",
            ["results"] = ToResultDictionary(),
            ["date"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["status"] = "open",
            ["level"] = (GameState.currentLevel + 1)
        };
        if (infoDic["results"] == null) { Close(); return; }

        // Push imformation
        string key = MissingWordsFeedback._dataWordsRef.Push().Key;
        MissingWordsFeedback.childUpdates["/" + key] = infoDic;
        MissingWordsFeedback._dataWordsRef.UpdateChildrenAsync(MissingWordsFeedback.childUpdates);

        Close();
    }
    private Dictionary<string, object> ToResultDictionary()
    {
        Dictionary<string, object> infoDic = new Dictionary<string, object>();
        infoDic["email"] = email;
        infoDic["body"] = emailBody;

        if (email == null && emailBody == null) { return null; }
        else if (email?.Length == 0 && emailBody?.Length == 0) { return null; }
        else if (email == null && emailBody?.Length == 0) { return null; }
        else if (email?.Length == 0 && emailBody == null) { return null; }
        else { return infoDic; }
    }
    public void CloseBtt()
    {
        Close();
    }
}
