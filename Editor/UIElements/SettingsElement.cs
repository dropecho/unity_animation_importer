using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dropecho {
  class SettingsElement : VisualElement {
    private readonly AnimationImporterSettings _value;
    private ListView _list;
    private VisualElement _rightPanel;

    public SettingsElement(AnimationImporterSettings value) {
      _value = value;

      // rootElement.Add(new Label("hello"));
      this.style.height = new StyleLength(Length.Percent(100));
      var box = new Box() {
        style = {
          flexDirection = FlexDirection.Row,
          height = new StyleLength(Length.Percent(100)),
          borderTopColor = new Color(0.1f,0.1f,0.1f,1f),
          borderTopWidth = 1
        }
      };
      var leftpanel = new VisualElement() {
        style = {
          paddingTop=5,
        flexDirection = FlexDirection.Column,
        minWidth = 250, borderRightColor = new Color(0.1f,0.1f,0.1f,1f), borderRightWidth = 1 }
      };
      _rightPanel = new VisualElement() { style = { paddingTop = 5, width = new StyleLength(Length.Percent(100)) } };

      box.Add(leftpanel);
      box.Add(_rightPanel);


      _list = new ListView() {
        itemsSource = _value.directories,
        makeItem = makeDirectoryElement,
        bindItem = bindDirectoryElement,
        selectionType = SelectionType.Single
      };
      _list.onSelectionChange += selectedDirectoryChanged;


      leftpanel.Add(_list);

      var header = new VisualElement() { style = { flexDirection = FlexDirection.Row, justifyContent = Justify.SpaceBetween, alignItems = Align.Center } };

      header.Add(new Label("Animation Importer Settings") { style = { fontSize = 18, unityFontStyleAndWeight = FontStyle.Bold, paddingLeft = 5 } });
      header.Add(new Button(() => {
        _value.directories.Add(new AnimationImporterDirectorySettings());
        _list.RefreshItems();
        _list.selectedIndex = _list.itemsSource.Count - 1;
        EditorUtility.SetDirty(_value);
      }) { text = "Create New Directory Settings", style = { height = 28 } });
      this.Add(header);
      this.Add(box);
    }

    private void selectedDirectoryChanged(IEnumerable<object> obj) {
      _rightPanel.Clear();
      var directorySettings = obj.FirstOrDefault() as AnimationImporterDirectorySettings;
      if (directorySettings == null) {
        return;
      }
      var el = new DirectorySettingsElement() { value = directorySettings };
      el.RegisterValueChangedCallback(evt => {
        _list.RefreshItems();
        EditorUtility.SetDirty(_value);
      });
      _rightPanel.Add(el);
    }

    private void bindDirectoryElement(VisualElement element, int index) {
      var label = element.Q<Label>();
      var button = element.Q<Button>();
      var value = _value.directories[index];

      label.text = string.IsNullOrWhiteSpace(value?.basePath) ? "__NEW DIRECTORY__" : value?.basePath?.Replace("Assets/", "");
      element.Remove(button);
      element.Add(new Button(() => {
        if (_list.selectedIndex == index) {
          if (_list.selectedIndex > 0) {
            _list.selectedIndex -= 1;
          } else {
            _list.selectedIndex = -1;
          }
        }
        _value.directories.RemoveAt(index);
        _list.RefreshItems();
        EditorUtility.SetDirty(_value);
      }) { text = "Delete" });
    }

    private VisualElement makeDirectoryElement() {
      var container = new VisualElement() {
        style = {
        flexDirection = FlexDirection.Row, alignItems = Align.Center, justifyContent = Justify.SpaceBetween ,
        paddingLeft = 5, paddingRight = 5

      }
      };
      container.Add(new Label("Label"));
      container.Add(new Button() { text = "Delete" });

      return container;
    }
  }
}