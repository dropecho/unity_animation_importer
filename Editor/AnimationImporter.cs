using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dropecho {
  public class AnimationImporter : AssetPostprocessor {
    void OnPreprocessModel() {
      var importerSettings = AnimationImporterSettings.GetSettings();

      if (importerSettings == null || !assetImporter.importSettingsMissing) {
        return;
      }

      foreach (var dir in importerSettings.directories) {
        if (string.IsNullOrWhiteSpace(dir.basePath) || !assetPath.Contains(dir.basePath)) {
          continue;
        }
        importModel(assetImporter as ModelImporter, dir);
      }
    }

    internal static void importModel(ModelImporter importer, AnimationImporterDirectorySettings importerSettings) {
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
      var importerSettings = AnimationImporterSettings.GetSettings();

      if (importerSettings == null || !assetImporter.importSettingsMissing) {
        return;
      }

      foreach (var dir in importerSettings.directories) {
        if (string.IsNullOrWhiteSpace(dir.basePath) || !assetPath.Contains(dir.basePath)) {
          continue;
        }
        importAnimationClips(assetImporter as ModelImporter, dir);
      }
    }

    internal static void importAnimationClips(ModelImporter modelImporter, AnimationImporterDirectorySettings settings) {
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
      }

      //Assign modiffied clip names back to modelImporter
      modelImporter.clipAnimations = clipAnimations;

      // Save
      modelImporter.SaveAndReimport();
    }
  }
}