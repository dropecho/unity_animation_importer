using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Dropecho {
  public class AnimationDirectionContext {
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/0 Deg", false, 10)]
    static void ChangeAnimationDirectionTo0(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, Vector3.forward);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/90 Deg", false, 10)]
    static void ChangeAnimationDirectionTo90(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, Vector3.right);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/-90 Deg", false, 10)]
    static void ChangeAnimationDirectionToNeg90(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, Vector3.left);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/45 Deg", false, 10)]
    static void ChangeAnimationDirectionTo45(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, (Vector3.forward + Vector3.right).normalized);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/-45 Deg", false, 10)]
    static void ChangeAnimationDirectionToNeg45(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, (Vector3.forward + Vector3.left).normalized);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/135 Deg", false, 10)]
    static void ChangeAnimationDirectionTo135(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, (Vector3.back + Vector3.right).normalized);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/-135 Deg", false, 10)]
    static void ChangeAnimationDirectionToNeg135(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, (Vector3.back + Vector3.left).normalized);
    [UnityEditor.MenuItem("CONTEXT/AnimationClip/Direction/180 Deg", false, 10)]
    static void ChangeAnimationDirectionTo180(UnityEditor.MenuCommand menuCommand) => ChangeAnimationDirection(menuCommand, (Vector3.back).normalized);

    static void ChangeAnimationDirection(UnityEditor.MenuCommand menuCommand, Vector3 changeTo) {
      // do stuff.
      var clip = menuCommand.context as AnimationClip;
      var path = AssetDatabase.GetAssetPath(clip);
      var modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;

      var reloaded = ResetAndReload(clip);
      var dir = Vector3.ProjectOnPlane(reloaded.averageSpeed, Vector3.up);
      dir.y = 0;
      modelImporter.clipAnimations = SetRotation(clip, modelImporter, Vector3.SignedAngle(changeTo, dir, Vector3.up));
      modelImporter.SaveAndReimport();
    }

    static ModelImporterClipAnimation[] SetRotation(AnimationClip clip, ModelImporter modelImporter, float rot) {
      ModelImporterClipAnimation[] clipAnimations;
      if (modelImporter.clipAnimations.Length <= 0) {
        clipAnimations = modelImporter.defaultClipAnimations;
      } else {
        clipAnimations = modelImporter.clipAnimations;
      }

      for (var i = 0; i < clipAnimations.Length; i++) {
        var cur = clipAnimations[i];
        if ("__preview__" + cur.takeName == clip.name) {
          cur.rotationOffset = rot;
        }
      }

      return clipAnimations;
    }

    static AnimationClip ResetAndReload(AnimationClip clip) {
      var path = AssetDatabase.GetAssetPath(clip);
      var modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;

      modelImporter.clipAnimations = SetRotation(clip, modelImporter, 0);
      modelImporter.SaveAndReimport();
      return AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
    }


    // // Validate the menu item defined by the function above.
    // // The menu item will be disabled if this function returns false.
    // [UnityEditor.MenuItem("GameObject/Dropecho/Damage Dealer", true)]
    // static bool ValidateCreateDamageDealerGameObject() {
    //   // Return false if no transform is selected.
    //   return UnityEditor.Selection.activeTransform != null;
    // }
  }
}