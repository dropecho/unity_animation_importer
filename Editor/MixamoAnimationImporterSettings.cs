#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Dropecho {
  // Create a new type of Settings Asset.
  class MixamoAnimationImporterSettings : ScriptableObject {
    public const string SettingsPath = "Assets/Settings/MixamoAnimationImporterSettings.asset";

    [SerializeField] public string basePath;
    [SerializeField] public GameObject baseAvatar;

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

    // Animation Specific Settings
    // [SerializeField] public bool importVisibility = false;

    internal static MixamoAnimationImporterSettings GetSettings() {
        return AssetDatabase.LoadAssetAtPath<MixamoAnimationImporterSettings>(SettingsPath);
    }

    internal static MixamoAnimationImporterSettings GetOrCreateSettings() {
      var settings = AssetDatabase.LoadAssetAtPath<MixamoAnimationImporterSettings>(SettingsPath);
      if (settings == null) {
        if(!AssetDatabase.IsValidFolder("Assets/Settings")) {
          AssetDatabase.CreateFolder("Assets", "Settings");
        }
        settings = ScriptableObject.CreateInstance<MixamoAnimationImporterSettings>();
        AssetDatabase.CreateAsset(settings, SettingsPath);
        AssetDatabase.SaveAssets();
      }
      return settings;
    }

    internal static SerializedObject GetSerializedSettings() {
      return new SerializedObject(GetOrCreateSettings());
    }
  }
}

#endif