#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dropecho {
  public class MixamoAnimationImporter : AssetPostprocessor {
    void OnPreprocessModel() {
      var importerSettings = MixamoAnimationImporterSettings.GetSettings();

      if (importerSettings == null || string.IsNullOrWhiteSpace(importerSettings.basePath) || !assetPath.Contains(importerSettings.basePath) || !assetImporter.importSettingsMissing) {
        return;
      }

      importModel(assetImporter as ModelImporter, importerSettings);
    }

    internal static void importModel(ModelImporter importer, MixamoAnimationImporterSettings importerSettings) {
      if (importer != null) {
        importer.isReadable = importerSettings.isReadable;
        importer.importLights = importerSettings.importLights;
        importer.importCameras = importerSettings.importCameras;
        importer.importBlendShapes = importerSettings.importBlendShapes;
        importer.importVisibility = importerSettings.importVisibility;
        importer.importNormals = importerSettings.importNormals;

        importer.animationType = ModelImporterAnimationType.Human;

        var baseAvatarPath = AssetDatabase.GetAssetPath(importerSettings.baseAvatar);
        // TODO: Figure out a better way to do this.
        if (importer.assetPath == baseAvatarPath) {
          importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        } else {
          importer.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
          importer.sourceAvatar = AssetDatabase.LoadAssetAtPath<Avatar>(baseAvatarPath);
        }

        importer.SaveAndReimport();
      }
    }

    void OnPreprocessAnimation() {
      var importerSettings = MixamoAnimationImporterSettings.GetSettings();

      if (importerSettings == null || string.IsNullOrWhiteSpace(importerSettings.basePath) || !assetPath.Contains(importerSettings.basePath) || !assetImporter.importSettingsMissing) {
        return;
      }

      importAnimationClips(assetImporter as ModelImporter, importerSettings);
    }

    internal static void importAnimationClips(ModelImporter modelImporter, MixamoAnimationImporterSettings settings) {
      ModelImporterClipAnimation[] clipAnimations;
      if (modelImporter.clipAnimations.Length <= 0) {
        clipAnimations = modelImporter.defaultClipAnimations;
      } else {
        clipAnimations = modelImporter.clipAnimations;
      }
      var assetName = Path.GetFileNameWithoutExtension(modelImporter.assetPath);

      //Modify/Rename animation clips?
      for (int i = 0; i < clipAnimations.Length; i++) {
        var clip = clipAnimations[i];

        if (settings.renameAnimationClipsToMatchAssetName) {
          clip.name = assetName + (i == 0 ? "" : $" (i)");
        }

        if (settings.replaceSpacesWithUnderscores) {
          clip.name = clip.name.Replace(" ", "_");
        }
        Debug.Log("wee:" + clip.loop);
        Debug.Log("wee:" + clip.loopTime);
        // clip.loopTime = true;
      }

      //Assign modiffied clip names back to modelImporter
      modelImporter.clipAnimations = clipAnimations;

      // Save
      modelImporter.SaveAndReimport();
    }
  }
}
#endif