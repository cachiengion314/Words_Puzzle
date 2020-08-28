using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppReviewManger : MonoBehaviour
{
    public static InAppReviewManger instance;
    // Create instance of ReviewManager
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _reviewManager = new ReviewManager();
        StartCoroutine(InvokeInAppReview());
    }

    private IEnumerator InvokeInAppReview()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log("Using: " + requestFlowOperation.Error.ToString());
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();
    }
    public void LauchInAppReviewMethod()
    {
        StartCoroutine(LaunchInAppReviewFlow());
    }
    private IEnumerator LaunchInAppReviewFlow()
    {
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
}
