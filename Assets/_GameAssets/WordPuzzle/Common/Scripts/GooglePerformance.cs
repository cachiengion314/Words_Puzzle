using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Android.PerformanceTuner;
using UnityEngine.Scripting;
using Google.Protobuf.Reflection;

public class GooglePerformance : MonoBehaviour
{
    AndroidPerformanceTuner<FidelityParams, Annotation> tuner =
            new AndroidPerformanceTuner<FidelityParams, Annotation>();

    private void Awake()
    {
        tuner.EnableLocalEndpoint();
    }

    void Start()
    {
        tuner.SetCurrentAnnotation(new Annotation
        {
            Scene = Scene.AssetsGameassetswordpuzzleScenesmain,
            LoadingState = LoadingState.Loading,
            Gamedifficulty = Difficulty.Medium
        });
        tuner.SetFidelityParameters(new FidelityParams
        {
            ParticleDetails = 1.0f,
            ShadowQualtity = ShadowQualtity.High,
            TextureResolution = 2048,
        });
        ErrorCode startErrorCode = tuner.Start();

        tuner.onReceiveUploadLog += request =>
        {
            tuner.SetCurrentAnnotation(new Annotation
            {
                Scene = Scene.AssetsGameassetswordpuzzleScenesmain,
                LoadingState = LoadingState.NotLoading,
                Gamedifficulty = Difficulty.Medium
            });
        };
    }

    // Don't call this method.
    [Preserve]
    void ExampleOfForceReflectionInitializationForProtobuf()
    {
        FileDescriptor.ForceReflectionInitialization<LoadingState>();
        FileDescriptor.ForceReflectionInitialization<Scene>();
        FileDescriptor.ForceReflectionInitialization<ShadowQualtity>();
        FileDescriptor.ForceReflectionInitialization<Difficulty>();
        // Add FileDescriptor.ForceReflectionInitialization<T> for each generated enum.
        // You can find the list of enums in DevTuningfork.cs -> enum section
        // or in the list of enums in Google -> Android Performance Tuner.
    }
}
