using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Messaging;

public class CrashlyticsInitializer : MonoBehaviour
{
    public static CrashlyticsInitializer instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                // for database
                app.SetEditorDatabaseUrl("https://word-puzzle-896ff.firebaseio.com/");
                // for remote config
                InitializeFirebase();
                // for clound messenger
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
                // Set a flag here for indicating that your project is ready to use Firebase.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    void InitializeFirebase()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server yet, or if we ask for values that the server doesn't have:
        defaults.Add("config_test_string", "default local string");
        defaults.Add("config_test_int", 1);
        defaults.Add("config_test_float", 1.0);
        defaults.Add("config_test_bool", false);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        Debug.Log("Remote config ready!");
    }
    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
    }
}
