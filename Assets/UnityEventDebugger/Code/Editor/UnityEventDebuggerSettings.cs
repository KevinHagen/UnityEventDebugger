using UnityEditor;
using UnityEngine;
using UnityEventDebugger.Utility;

namespace UnityEventDebugger.Editor
{
	/// <summary>
	///     ScriptableObject with settings for the UnityEventDebugger tool.
	/// </summary>
	[CreateAssetMenu(fileName = "UnityEventDebuggerDrawerSettings", menuName = "UnityEventDebugger/Drawer Settings", order = -50)]
	public class UnityEventDebuggerSettings : ScriptableObject
	{
		#region Static Stuff

		private static UnityEventDebuggerSettings GetOrCreateSettings()
		{
			var settings = AssetDatabase.LoadAssetAtPath<UnityEventDebuggerSettings>(UnityEventDebuggerUtils.UnityEventDebuggerSettingsPath);

			if (settings == null)
			{
				settings = CreateInstance<UnityEventDebuggerSettings>();

				AssetDatabase.CreateAsset(settings, UnityEventDebuggerUtils.UnityEventDebuggerSettingsPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			return settings;
		}

		internal static SerializedObject GetSerializedSettings()
		{
			return new SerializedObject(GetOrCreateSettings());
		}

		#endregion

		#region Serialize Fields

		[SerializeField] private float _labelWidth = 50f;
		[SerializeField] private bool _alwaysShowFoldouts = false;
		[SerializeField] private bool _disableInEditMode = false;
		[SerializeField] private bool _showFoldoutsByDefault = true;
		[SerializeField] private bool _saveFoldoutStatesToEditorPrefs = false;
		[SerializeField] private Color _firstRowColor = new Color(90f, 83f, 82f, 1f);
		[SerializeField] private Color _secondRowColor = new Color(90f, 83f, 82f, 0.5f);
		[SerializeField] private Color _foldoutGroupColor = new Color(90f, 83f, 82f, 0.5f);

		#endregion

		#region Properties

		public bool AlwaysShowFoldouts => _alwaysShowFoldouts;
		public bool DisableInEditMode => _disableInEditMode;
		public Color FirstRowColor => _firstRowColor;
		public Color FoldoutGroupColor => _foldoutGroupColor;
		public float LabelWidth => _labelWidth;
		public bool SaveFoldoutStatesToEditorPrefs => _saveFoldoutStatesToEditorPrefs;
		public Color SecondRowColor => _secondRowColor;
		public bool ShowFoldoutsByDefault => _showFoldoutsByDefault;

		#endregion
	}
}