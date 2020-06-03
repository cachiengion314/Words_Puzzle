using System.Collections;
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
        MissingWordsFeedback._missingWordsRef = FirebaseDatabase.DefaultInstance
        .GetReference("Missing and Irrelevant Words");

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
        Debug.Log(emailBody);
    }
    public void EndEditinputFieldYourEmailCallback(string arg)
    {
        email = arg;
        Debug.Log(email);
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
        string key = MissingWordsFeedback._missingWordsRef.Push().Key;

        MissingWordsFeedback.childUpdates["/Email/" + key] = email;
        MissingWordsFeedback._missingWordsRef.UpdateChildrenAsync(MissingWordsFeedback.childUpdates);

        Close();
    }
    public void CloseBtt()
    {
        Close();
    }
}
