// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dev_tuningfork.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Android.PerformanceTuner {

  /// <summary>Holder for reflection information generated from dev_tuningfork.proto</summary>
  public static partial class DevTuningforkReflection {

    #region Descriptor
    /// <summary>File descriptor for dev_tuningfork.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DevTuningforkReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRkZXZfdHVuaW5nZm9yay5wcm90byJqCgpBbm5vdGF0aW9uEiQKDWxvYWRp",
            "bmdfc3RhdGUYASABKA4yDS5Mb2FkaW5nU3RhdGUSFQoFc2NlbmUYAiABKA4y",
            "Bi5TY2VuZRIfCgpkaWZmaWN1bHR5GAMgASgOMgsuRGlmZmljdWx0eSJwCg5G",
            "aWRlbGl0eVBhcmFtcxIaChJ0ZXh0dXJlX3Jlc29sdXRpb24YASABKAUSKAoP",
            "c2hhZG93X3F1YWx0aXR5GAIgASgOMg8uU2hhZG93UXVhbHRpdHkSGAoQcGFy",
            "dGljbGVfZGV0YWlscxgDIAEoAipgCgxMb2FkaW5nU3RhdGUSGAoUTE9BRElO",
            "R1NUQVRFX0lOVkFMSUQQABIcChhMT0FESU5HU1RBVEVfTk9UX0xPQURJTkcQ",
            "ARIYChRMT0FESU5HU1RBVEVfTE9BRElORxACKsABCgVTY2VuZRIRCg1TQ0VO",
            "RV9JTlZBTElEEAASKAokU0NFTkVfQVNTRVRTV09SRFBVWlpMRV9TQ0VORVNM",
            "T0FESU5HEAESJQohU0NFTkVfQVNTRVRTV09SRFBVWlpMRV9TQ0VORVNIT01F",
            "EAISLAooU0NFTkVfQVNTRVRTV09SRFBVWlpMRV9TQ0VORVNTRUxFQ1RXT1JM",
            "RBADEiUKIVNDRU5FX0FTU0VUU1dPUkRQVVpaTEVfU0NFTkVTTUFJThAEKmUK",
            "CkRpZmZpY3VsdHkSFgoSRElGRklDVUxUWV9JTlZBTElEEAASEwoPRElGRklD",
            "VUxUWV9FQVNZEAESFQoRRElGRklDVUxUWV9NRURJVU0QAhITCg9ESUZGSUNV",
            "TFRZX0hBUkQQAyp4Cg5TaGFkb3dRdWFsdGl0eRIaChZTSEFET1dRVUFMVElU",
            "WV9JTlZBTElEEAASFgoSU0hBRE9XUVVBTFRJVFlfTE9XEAESGQoVU0hBRE9X",
            "UVVBTFRJVFlfTUVESVVNEAISFwoTU0hBRE9XUVVBTFRJVFlfSElHSBADQiKq",
            "Ah9Hb29nbGUuQW5kcm9pZC5QZXJmb3JtYW5jZVR1bmVyYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Google.Android.PerformanceTuner.LoadingState), typeof(global::Google.Android.PerformanceTuner.Scene), typeof(global::Google.Android.PerformanceTuner.Difficulty), typeof(global::Google.Android.PerformanceTuner.ShadowQualtity), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Android.PerformanceTuner.Annotation), global::Google.Android.PerformanceTuner.Annotation.Parser, new[]{ "LoadingState", "Scene", "Difficulty" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Android.PerformanceTuner.FidelityParams), global::Google.Android.PerformanceTuner.FidelityParams.Parser, new[]{ "TextureResolution", "ShadowQualtity", "ParticleDetails" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum LoadingState {
    [pbr::OriginalName("LOADINGSTATE_INVALID")] Invalid = 0,
    [pbr::OriginalName("LOADINGSTATE_NOT_LOADING")] NotLoading = 1,
    [pbr::OriginalName("LOADINGSTATE_LOADING")] Loading = 2,
  }

  public enum Scene {
    [pbr::OriginalName("SCENE_INVALID")] Invalid = 0,
    [pbr::OriginalName("SCENE_ASSETSWORDPUZZLE_SCENESLOADING")] AssetswordpuzzleScenesloading = 1,
    [pbr::OriginalName("SCENE_ASSETSWORDPUZZLE_SCENESHOME")] AssetswordpuzzleSceneshome = 2,
    [pbr::OriginalName("SCENE_ASSETSWORDPUZZLE_SCENESSELECTWORLD")] AssetswordpuzzleScenesselectworld = 3,
    [pbr::OriginalName("SCENE_ASSETSWORDPUZZLE_SCENESMAIN")] AssetswordpuzzleScenesmain = 4,
  }

  public enum Difficulty {
    [pbr::OriginalName("DIFFICULTY_INVALID")] Invalid = 0,
    [pbr::OriginalName("DIFFICULTY_EASY")] Easy = 1,
    [pbr::OriginalName("DIFFICULTY_MEDIUM")] Medium = 2,
    [pbr::OriginalName("DIFFICULTY_HARD")] Hard = 3,
  }

  public enum ShadowQualtity {
    [pbr::OriginalName("SHADOWQUALTITY_INVALID")] Invalid = 0,
    [pbr::OriginalName("SHADOWQUALTITY_LOW")] Low = 1,
    [pbr::OriginalName("SHADOWQUALTITY_MEDIUM")] Medium = 2,
    [pbr::OriginalName("SHADOWQUALTITY_HIGH")] High = 3,
  }

  #endregion

  #region Messages
  public sealed partial class Annotation : pb::IMessage<Annotation> {
    private static readonly pb::MessageParser<Annotation> _parser = new pb::MessageParser<Annotation>(() => new Annotation());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Annotation> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Android.PerformanceTuner.DevTuningforkReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Annotation() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Annotation(Annotation other) : this() {
      loadingState_ = other.loadingState_;
      scene_ = other.scene_;
      difficulty_ = other.difficulty_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Annotation Clone() {
      return new Annotation(this);
    }

    /// <summary>Field number for the "loading_state" field.</summary>
    public const int LoadingStateFieldNumber = 1;
    private global::Google.Android.PerformanceTuner.LoadingState loadingState_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Android.PerformanceTuner.LoadingState LoadingState {
      get { return loadingState_; }
      set {
        loadingState_ = value;
      }
    }

    /// <summary>Field number for the "scene" field.</summary>
    public const int SceneFieldNumber = 2;
    private global::Google.Android.PerformanceTuner.Scene scene_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Android.PerformanceTuner.Scene Scene {
      get { return scene_; }
      set {
        scene_ = value;
      }
    }

    /// <summary>Field number for the "difficulty" field.</summary>
    public const int DifficultyFieldNumber = 3;
    private global::Google.Android.PerformanceTuner.Difficulty difficulty_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Android.PerformanceTuner.Difficulty Difficulty {
      get { return difficulty_; }
      set {
        difficulty_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Annotation);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Annotation other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (LoadingState != other.LoadingState) return false;
      if (Scene != other.Scene) return false;
      if (Difficulty != other.Difficulty) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (LoadingState != 0) hash ^= LoadingState.GetHashCode();
      if (Scene != 0) hash ^= Scene.GetHashCode();
      if (Difficulty != 0) hash ^= Difficulty.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (LoadingState != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) LoadingState);
      }
      if (Scene != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Scene);
      }
      if (Difficulty != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Difficulty);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (LoadingState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) LoadingState);
      }
      if (Scene != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Scene);
      }
      if (Difficulty != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Difficulty);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Annotation other) {
      if (other == null) {
        return;
      }
      if (other.LoadingState != 0) {
        LoadingState = other.LoadingState;
      }
      if (other.Scene != 0) {
        Scene = other.Scene;
      }
      if (other.Difficulty != 0) {
        Difficulty = other.Difficulty;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            loadingState_ = (global::Google.Android.PerformanceTuner.LoadingState) input.ReadEnum();
            break;
          }
          case 16: {
            scene_ = (global::Google.Android.PerformanceTuner.Scene) input.ReadEnum();
            break;
          }
          case 24: {
            difficulty_ = (global::Google.Android.PerformanceTuner.Difficulty) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  public sealed partial class FidelityParams : pb::IMessage<FidelityParams> {
    private static readonly pb::MessageParser<FidelityParams> _parser = new pb::MessageParser<FidelityParams>(() => new FidelityParams());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FidelityParams> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Android.PerformanceTuner.DevTuningforkReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FidelityParams() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FidelityParams(FidelityParams other) : this() {
      textureResolution_ = other.textureResolution_;
      shadowQualtity_ = other.shadowQualtity_;
      particleDetails_ = other.particleDetails_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FidelityParams Clone() {
      return new FidelityParams(this);
    }

    /// <summary>Field number for the "texture_resolution" field.</summary>
    public const int TextureResolutionFieldNumber = 1;
    private int textureResolution_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int TextureResolution {
      get { return textureResolution_; }
      set {
        textureResolution_ = value;
      }
    }

    /// <summary>Field number for the "shadow_qualtity" field.</summary>
    public const int ShadowQualtityFieldNumber = 2;
    private global::Google.Android.PerformanceTuner.ShadowQualtity shadowQualtity_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Android.PerformanceTuner.ShadowQualtity ShadowQualtity {
      get { return shadowQualtity_; }
      set {
        shadowQualtity_ = value;
      }
    }

    /// <summary>Field number for the "particle_details" field.</summary>
    public const int ParticleDetailsFieldNumber = 3;
    private float particleDetails_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ParticleDetails {
      get { return particleDetails_; }
      set {
        particleDetails_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FidelityParams);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FidelityParams other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TextureResolution != other.TextureResolution) return false;
      if (ShadowQualtity != other.ShadowQualtity) return false;
      if (ParticleDetails != other.ParticleDetails) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TextureResolution != 0) hash ^= TextureResolution.GetHashCode();
      if (ShadowQualtity != 0) hash ^= ShadowQualtity.GetHashCode();
      if (ParticleDetails != 0F) hash ^= ParticleDetails.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (TextureResolution != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(TextureResolution);
      }
      if (ShadowQualtity != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) ShadowQualtity);
      }
      if (ParticleDetails != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(ParticleDetails);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TextureResolution != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(TextureResolution);
      }
      if (ShadowQualtity != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ShadowQualtity);
      }
      if (ParticleDetails != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FidelityParams other) {
      if (other == null) {
        return;
      }
      if (other.TextureResolution != 0) {
        TextureResolution = other.TextureResolution;
      }
      if (other.ShadowQualtity != 0) {
        ShadowQualtity = other.ShadowQualtity;
      }
      if (other.ParticleDetails != 0F) {
        ParticleDetails = other.ParticleDetails;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            TextureResolution = input.ReadInt32();
            break;
          }
          case 16: {
            shadowQualtity_ = (global::Google.Android.PerformanceTuner.ShadowQualtity) input.ReadEnum();
            break;
          }
          case 29: {
            ParticleDetails = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
