using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEventDebugger.Utility;

namespace UnityEventDebugger.Editor.SettingsProvider
{
	/// <summary>
	///     Settings provider used to inject UnityEventDebugger settings into the Preferences Window.
	/// </summary>
	public class UnityEventDebuggerSettingsProvider : UnityEditor.SettingsProvider
	{
		#region Static Stuff

		public static bool IsSettingsAvailable()
		{
			return File.Exists(UnityEventDebuggerUtils.UnityEventDebuggerSettingsPath);
		}

		[SettingsProvider]
		public static UnityEditor.SettingsProvider CreateUnityEventDebuggerSettingsProvider()
		{
			if (IsSettingsAvailable())
			{
				var provider = new UnityEventDebuggerSettingsProvider("Preferences/UnityEventDebugger", SettingsScope.User);
				provider.keywords = new[]
				                    {
					                    "UnityEventDebugger",
					                    "Event",
					                    "Events",
					                    "Debugger",
					                    "Unity"
				                    };

				return provider;
			}

			return null;
		}

		#endregion

		#region Private Fields

		private SerializedObject _unityEventDebuggerSettings;
		private SerializedProperty _alwaysShowFoldoutsProperty;
		private SerializedProperty _disableInEditModeProperty;
		private SerializedProperty _showByDefaultProperty;
		private SerializedProperty _labelWidthProperty;
		private SerializedProperty _firstRowColorProperty;
		private SerializedProperty _secondRowColorProperty;
		private SerializedProperty _foldoutGroupColorProperty;

		#endregion

		#region Properties

		public SerializedObject Settings => _unityEventDebuggerSettings;

		#endregion

		#region Constructors

		public UnityEventDebuggerSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
		{
			Load();
		}

		#endregion

		#region Unity methods

		public override void OnGUI(string searchContext)
		{
			EditorGUILayout.PropertyField(_disableInEditModeProperty);
			EditorGUILayout.PropertyField(_alwaysShowFoldoutsProperty);
			EditorGUILayout.PropertyField(_showByDefaultProperty);
			EditorGUILayout.PropertyField(_labelWidthProperty);
			EditorGUILayout.PropertyField(_firstRowColorProperty);
			EditorGUILayout.PropertyField(_secondRowColorProperty);
			EditorGUILayout.PropertyField(_foldoutGroupColorProperty);

			if (_unityEventDebuggerSettings.hasModifiedProperties)
			{
				_unityEventDebuggerSettings.ApplyModifiedProperties();
			}
		}

		#endregion

		#region Public methods

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			Load();
		}

		#endregion

		#region Private methods

		private void Load()
		{
			_unityEventDebuggerSettings = UnityEventDebuggerSettings.GetSerializedSettings();
			FindSerializedProperties();
		}

		private void FindSerializedProperties()
		{
			_disableInEditModeProperty = _unityEventDebuggerSettings.FindProperty("_disableInEditMode");
			_alwaysShowFoldoutsProperty = _unityEventDebuggerSettings.FindProperty("_alwaysShowFoldouts");
			_showByDefaultProperty = _unityEventDebuggerSettings.FindProperty("_showFoldoutsByDefault");
			_labelWidthProperty = _unityEventDebuggerSettings.FindProperty("_labelWidth");
			_firstRowColorProperty = _unityEventDebuggerSettings.FindProperty("_firstRowColor");
			_secondRowColorProperty = _unityEventDebuggerSettings.FindProperty("_secondRowColor");
			_foldoutGroupColorProperty = _unityEventDebuggerSettings.FindProperty("_foldoutGroupColor");
		}

		#endregion
	}
}