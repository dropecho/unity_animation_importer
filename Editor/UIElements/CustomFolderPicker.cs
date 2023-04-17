using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace Dropecho
{
    class CustomFolderPicker : VisualElement, INotifyValueChanged<string> {
    private TextField _textField;

    public string value {
      get { return _textField.value; }
      set { _textField.value = value; }
    }

    public void SetValueWithoutNotify(string newValue) {
      _textField.SetValueWithoutNotify(newValue);
    }

    public CustomFolderPicker(string label = "") : base() {
      _textField = new TextField(label);
      this.Add(_textField);
      var openDirPickerButton = new Button(OpenFolderPicker) { text = "..." };
      this.Add(openDirPickerButton);

      _textField.style.flexGrow = 1;
      openDirPickerButton.style.flexGrow = 0;
      this.style.flexDirection = FlexDirection.Row;
    }

    void OpenFolderPicker() {
      var path = EditorUtility.OpenFolderPanel("", string.IsNullOrWhiteSpace(value) ? "Assets" : value, "");
      if (!string.IsNullOrWhiteSpace(path)) {
        value = Path.GetRelativePath(Directory.GetCurrentDirectory(), path).Replace("\\", "/");
      }
      this.Blur();
    }
  }
}



// rootElement.Add(new Label("hello"));
// var box = new Box() { style = { flexDirection = FlexDirection.Row } };
// var leftpanel = new VisualElement() { style = { flexDirection = FlexDirection.Column, minWidth = 250 } };
// var rightPanel = new VisualElement();

// box.Add(leftpanel);
// box.Add(rightPanel);

// leftpanel.Add(new Label("test"));
// rightPanel.Add(new Label("test"));
// // rightPanel.Add(new IMGUIContainer(() => {
// //   Editor.CreateEditor(settings.names[0]).DrawDefaultInspector();
// // }));
// rootElement.Add(box);

// var title = new Label(" Mixamo Animation Importer Settings") {
//   style ={
//     fontSize = 18,
//     marginLeft = 5f,marginRight = 5f,marginTop = 5f,marginBottom = 5f,
//     unityFontStyleAndWeight = FontStyle.Bold
//   }
// };
// title.AddToClassList("title");

// var basePathEl = new CustomFolderPicker("Base Path").FluentBindProperty(settings.FindProperty("basePath"));
// var settingsEl = new VisualElement();

// settingsEl.Add(new Label("Model Import Settings") {
//   style = {
//       marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
//     unityFontStyleAndWeight = FontStyle.Bold
//   }
// });
// settingsEl.Add(new PropertyField(settings.FindProperty("isReadable")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("importLights")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("importCameras")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("importBlendShapes")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("importVisibility")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("importNormals")).FluentBind(settings));

// settingsEl.Add(new Label("Rig Import Settings") {
//   style = {
//      marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
//     unityFontStyleAndWeight = FontStyle.Bold
//   }
// });
// settingsEl.Add(new PropertyField(settings.FindProperty("baseAvatar")).FluentBind(settings));

// settingsEl.Add(new Label("Animation Import Settings") {
//   style = {
//      marginLeft = 2.5f, marginRight = 5, marginBottom = 1f, paddingTop = 5, paddingBottom = 1f,
//     unityFontStyleAndWeight = FontStyle.Bold
//   }
// });
// settingsEl.Add(new PropertyField(settings.FindProperty("renameAnimationClipsToMatchAssetName")).FluentBind(settings));
// settingsEl.Add(new PropertyField(settings.FindProperty("replaceSpacesWithUnderscores")).FluentBind(settings));


// var countEl = new Label("Animation Assets in Folder: " + animsInFolder.Length.ToString());

// basePathEl.RegisterValueChangedCallback<string>(evt => {
//   if (!System.String.IsNullOrWhiteSpace(evt.newValue)) {
//     animsInFolder = AssetDatabase.FindAssets("t:animation", new[] { evt.newValue });
//   }
//   countEl.text = " Animation Assets in Folder: " + animsInFolder.Length.ToString();
// });

// var reImportAnimsButton = new Button(() => {
//   foreach (var guid in animsInFolder) {
//     var assetPath = AssetDatabase.GUIDToAssetPath(guid);
//     Debug.Log("importing: " + assetPath);

//     var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
//     MixamoAnimationImporter.importModel(modelImporter, settings.targetObject as MixamoAnimationImporterSettings);
//     MixamoAnimationImporter.importAnimationClips(modelImporter, settings.targetObject as MixamoAnimationImporterSettings);
//   }

//   AssetDatabase.Refresh();
// }) { text = "Reimport Animations" };

// rootElement.Add(title);
// rootElement.Add(basePathEl);
// rootElement.Add(countEl);
// rootElement.Add(settingsEl);
// rootElement.Add(reImportAnimsButton);
