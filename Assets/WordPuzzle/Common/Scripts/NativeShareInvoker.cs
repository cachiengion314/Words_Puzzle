using System.Collections;
using System.IO;
using UnityEngine;
using Facebook.Unity;
using System;
using System.Collections.Generic;
using Superpow;

public class NativeShareInvoker : MonoBehaviour
{
    public static NativeShareInvoker instance;
    public GameData gameData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //   FB.Init(OnInit);
    }
    private void OnInit()
    {
        FB.LogInWithPublishPermissions(new List<string>() { "publish_to_groups" });
    }
    private void DoSomeThing()
    {
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);


        byte[] encodedScreenShot = screenShot.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", encodedScreenShot, "ScreenShot.png");
        wwwForm.AddField("message", "Write description here");

        FB.API("me/photos", HttpMethod.POST, ShareScreenShotCallback, wwwForm);
    }

    private void ShareScreenShotCallback(IGraphResult result)
    {
        Debug.Log("Shared");
    }
    public void TakeScreenShotAndShareDelay(string androidPackageName = null)
    {
        StartCoroutine(TakeScreenShotAndShare(androidPackageName));
    }
    private IEnumerator TakeScreenShotAndShare(string androidPackageName = null)
    {
        yield return new WaitForSeconds(.8f);
        StartCoroutine(ScreenShotCoroutine(androidPackageName));
    }
    private IEnumerator ScreenShotCoroutine(string androidPackageName = null)
    {
        yield return new WaitForEndOfFrame();
        string path = Application.persistentDataPath + "/" + "MyScreenShot.png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        screenImage.Apply();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }
        //Convert to png(Expensive)
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }
        //Create new thread then save image to file
        //    new System.Threading.Thread(() =>
        //    {
        //        System.Threading.Thread.Sleep(100);
        //       File.WriteAllBytes(path, imageBytes);
        //   }).Start();

        string filePath = Path.Combine(Application.temporaryCachePath, "WordPuzzle-Level-" + AudienceNetworkBanner.instance.currlevel + ".png");
        File.WriteAllBytes(filePath, imageBytes);
        Destroy(screenImage);

        if (androidPackageName != null)
        {
            new NativeShare().AddFile(filePath).SetSubject("LOOKING FOR HELP! Nearly made it!")
                .SetText("Hey guys, please help me complete this level. I nearly break the record!")
         .SetTarget(androidPackageName).Share();
        }
        else
        {
            new NativeShare().AddFile(filePath).SetSubject("LOOKING FOR HELP! Nearly made it!")
                .SetText("Hey guys, please help me complete this level. I nearly break the record!").Share();
        }
    }
    private void NativeShareGalleryMethod(byte[] imageBytes)
    {
        // NativeGallery.SaveImageToGallery(imageBytes, "WPScreenShot", "ScreenshotName.png", null);

    }
}
