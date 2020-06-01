using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.Events;
using Firebase.Extensions;
using Firebase.Database;

public class MissingWordsFeedback : FeedbackDialog
{
    private DatabaseReference _counterRef;
    private int _count = 0;
    protected override void Awake()
    {
        base.Awake();

        _counterRef = FirebaseDatabase.DefaultInstance
            .GetReference("counter");

        _counterRef.ValueChanged += OnCountUpdated;
    }
    private void OnDestroy()
    {
        _counterRef.ValueChanged -= OnCountUpdated;
    }
    public void IncrementClickCounter()
    {
        _counterRef.SetValueAsync(_count + 1);
        Debug.Log("Count: " + _count);
    }
    private void OnCountUpdated(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot == null || e.Snapshot.Value == null) _count = 0;
        else _count = int.Parse(e.Snapshot.Value.ToString());
    }

    public void OnCountMinus()
    {

    }
    public new void OnCloseClick()
    {
        Close();
    }
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
}
