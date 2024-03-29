using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dropecho {
  [Serializable]
  class AnimationImporterDirectorySettings {
    [SerializeField] public string basePath;
    [SerializeField] public Avatar baseAvatar;

    // Animation Model Settings
    [SerializeField] public bool isReadable = false;
    [SerializeField] public bool importLights = false;
    [SerializeField] public bool importCameras = false;
    [SerializeField] public bool importBlendShapes = false;
    [SerializeField] public bool importVisibility = false;
    [SerializeField] public ModelImporterNormals importNormals = ModelImporterNormals.Import;

    // Animation rename settings
    [SerializeField] public bool renameAnimationClipsToMatchAssetName = true;
    [SerializeField] public bool replaceSpacesWithUnderscores = false;
  }

  // Create a new type of Settings Asset.
  class AnimationImporterSettings : ScriptableObject {
    public static readonly string SettingsPath = "Assets/Settings/DropechoAnimationImporterSettings.asset";
    public List<AnimationImporterDirectorySettings> directories = new();

    internal static AnimationImporterSettings GetSettings() {
      return AssetDatabase.LoadAssetAtPath<AnimationImporterSettings>(SettingsPath);
    }

    internal static AnimationImporterSettings GetOrCreateSettings() {
      var settings = AssetDatabase.LoadAssetAtPath<AnimationImporterSettings>(SettingsPath);
      return settings == null ? CreateSettings() : settings;
    }

    private static AnimationImporterSettings CreateSettings() {
      if (!AssetDatabase.IsValidFolder("Assets/Settings")) {
        AssetDatabase.CreateFolder("Assets", "Settings");
      }
      AnimationImporterSettings settings = ScriptableObject.CreateInstance<AnimationImporterSettings>();
      AssetDatabase.CreateAsset(settings, SettingsPath);
      AssetDatabase.SaveAssets();
      return settings;
    }

    internal static SerializedObject GetSerializedSettings() {
      return new SerializedObject(GetOrCreateSettings());
    }
  }
}
