using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dropecho {

  class DirectorySettingsElement : VisualElement, INotifyValueChanged<AnimationImporterDirectorySettings> {
    AnimationImporterDirectorySettings _value;

    public AnimationImporterDirectorySettings value {
      get {
        return _value;
      }
      set {
        _value = value;
        SendChangeEvent(value);
        Rebind();
      }
    }

    private void SendChangeEvent(AnimationImporterDirectorySettings value = null) {
      using (var evt = ChangeEvent<AnimationImporterDirectorySettings>.GetPooled(value, _value)) {
        evt.target = this;
        SendEvent(evt);
      }
    }

    public void SetValueWithoutNotify(AnimationImporterDirectorySettings newValue) => _value = newValue;

    void Rebind() {
      this.Clear();
      this.style.width = new StyleLength(Length.Percent(100));
      this.style.paddingLeft = 5;
      this.style.paddingRight = 5;
      var animsInFolder = new string[] { };


      var reImportAnimsButton = new Button(() => {
        foreach (var guid in animsInFolder) {
          var assetPath = AssetDatabase.GUIDToAssetPath(guid);
          Debug.Log("importing: " + assetPath);

          var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
          AnimationImporter.importModel(modelImporter, _value);
          AnimationImporter.importAnimationClips(modelImporter, _value);
        }

        AssetDatabase.Refresh();
      }) { text = "Reimport Animations", style = { height = 32, marginLeft = 15, marginRight = 15, marginBottom = 10 } };

      this.Add(reImportAnimsButton);

      if (!System.String.IsNullOrWhiteSpace(_value.basePath)) {
        animsInFolder = AssetDatabase.FindAssets("t:animation", new[] { _value.basePath });
      }
      var countEl = new Label("Animation Assets in Folder: " + animsInFolder.Length.ToString()) { style = { paddingLeft = 4, paddingTop = 3 } };

      var basePathEl = new CustomFolderPicker("Base Path") { value = _value?.basePath };
      basePathEl.RegisterValueChangedCallback<string>(evt => {
        _value.basePath = evt.newValue;
        if (!System.String.IsNullOrWhiteSpace(_value.basePath)) {
          animsInFolder = AssetDatabase.FindAssets("t:animation", new[] { _value.basePath });
        }
        countEl.text = " Animation Assets in Folder: " + animsInFolder.Length.ToString();
        SendChangeEvent();
      });
      this.Add(basePathEl);
      this.Add(countEl);

      #region Model Import Settings
      this.Add(new Label("Model Import Settings") {
        style = {
            marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
          unityFontStyleAndWeight = FontStyle.Bold
        }
      });

      var isReadable = new Toggle(ObjectNames.NicifyVariableName("Read/Write")) { value = value.isReadable };
      isReadable.RegisterValueChangedCallback<bool>(evt => {
        _value.isReadable = evt.newValue;
        SendChangeEvent();
      });
      this.Add(isReadable);

      var importLights = new Toggle(ObjectNames.NicifyVariableName("importLights")) { value = value.importLights };
      importLights.RegisterValueChangedCallback<bool>(evt => {
        _value.importLights = evt.newValue;
        SendChangeEvent();
      });
      this.Add(importLights);

      var importCameras = new Toggle(ObjectNames.NicifyVariableName("importCameras")) { value = value.importCameras };
      importCameras.RegisterValueChangedCallback<bool>(evt => {
        _value.importCameras = evt.newValue;
        SendChangeEvent();
      });
      this.Add(importCameras);

      var importBlendShapes = new Toggle(ObjectNames.NicifyVariableName("importBlendShapes")) { value = value.importBlendShapes };
      importBlendShapes.RegisterValueChangedCallback<bool>(evt => {
        _value.importBlendShapes = evt.newValue;
        SendChangeEvent();
      });
      this.Add(importBlendShapes);

      var importVisibility = new Toggle(ObjectNames.NicifyVariableName("importVisibility")) { value = value.importVisibility };
      importVisibility.RegisterValueChangedCallback<bool>(evt => {
        _value.importVisibility = evt.newValue;
        SendChangeEvent();
      });
      this.Add(importVisibility);

      var importNormals = new EnumField(ObjectNames.NicifyVariableName("importNormals"), ModelImporterNormals.Import) { value = value.importNormals };
      importNormals.RegisterValueChangedCallback<Enum>(evt => {
        _value.importNormals = (ModelImporterNormals)evt.newValue;
        SendChangeEvent();
      });
      this.Add(importNormals);
      #endregion

      #region Rig Import Settings
      this.Add(new Label("Rig Import Settings") {
        style = {
               marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
              unityFontStyleAndWeight = FontStyle.Bold
            }
      });

      var baseAvatar = new ObjectField(ObjectNames.NicifyVariableName("baseAvatar")) { value = value.baseAvatar, objectType = typeof(Avatar) };
      baseAvatar.RegisterValueChangedCallback<UnityEngine.Object>(evt => {
        _value.baseAvatar = evt.newValue as Avatar;
        SendChangeEvent();
      });
      this.Add(baseAvatar);
      #endregion

      #region Animation Import Settings
      this.Add(new Label("Animation Import Settings") {
        style = {
             marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
            unityFontStyleAndWeight = FontStyle.Bold
          }
      });
      var renameAnimationClipsToMatchAssetName = new Toggle(ObjectNames.NicifyVariableName("renameAnimationClipsToMatchAssetName")) { value = value.renameAnimationClipsToMatchAssetName };
      renameAnimationClipsToMatchAssetName.RegisterValueChangedCallback<bool>(evt => {
        _value.renameAnimationClipsToMatchAssetName = evt.newValue;
        SendChangeEvent();
      });
      this.Add(renameAnimationClipsToMatchAssetName);
      var replaceSpacesWithUnderscores = new Toggle(ObjectNames.NicifyVariableName("replaceSpacesWithUnderscores")) { value = value.replaceSpacesWithUnderscores };
      replaceSpacesWithUnderscores.RegisterValueChangedCallback<bool>(evt => {
        _value.replaceSpacesWithUnderscores = evt.newValue;
        SendChangeEvent();
      });
      this.Add(replaceSpacesWithUnderscores);
      #endregion


    }


    //   var animsInFolder = new string[]{};

    //   var countEl = new Label("Animation Assets in Folder: " + animsInFolder.Length.ToString());

    //   basePathEl.RegisterValueChangedCallback<string>(evt => {
    //     if (!System.String.IsNullOrWhiteSpace(evt.newValue)) {
    //       animsInFolder = AssetDatabase.FindAssets("t:animation", new[] { evt.newValue });
    //     }
    //     countEl.text = " Animation Assets in Folder: " + animsInFolder.Length.ToString();
    //   });

    //   var reImportAnimsButton = new Button(() => {
    //     // foreach (var guid in animsInFolder) {
    //     //   var assetPath = AssetDatabase.GUIDToAssetPath(guid);
    //     //   Debug.Log("importing: " + assetPath);

    //     //   var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
    //     //   MixamoAnimationImporter.importModel(modelImporter, settings.targetObject as MixamoAnimationImporterSettings);
    //     //   MixamoAnimationImporter.importAnimationClips(modelImporter, settings.targetObject as MixamoAnimationImporterSettings);
    //     // }

    //     // AssetDatabase.Refresh();
    //   }) { text = "Reimport Animations" };

    //   this.Add(title);
    //   this.Add(basePathEl);
    //   this.Add(countEl);
    //   this.Add(settingsEl);
    //   this.Add(reImportAnimsButton);
    // }
  }
}
