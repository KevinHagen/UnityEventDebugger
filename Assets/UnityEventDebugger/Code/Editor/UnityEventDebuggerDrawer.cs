using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEventDebugger.Editor.SettingsProvider;

namespace UnityEventDebugger.Editor
{
	/// <summary>
	///     Custom Property Drawer inheriting UnityEventDrawer. Adds a foldout below the UnityEvent that inspects the callbacks
	///     subscribed to the given event.
	/// </summary>
	[CustomPropertyDrawer(typeof(UnityEventBase), true)]
	public class UnityEventDebuggerDrawer : UnityEventDrawer
	{
		#region Private Fields

		private UnityEventDebuggerSettings _settings;
		private bool _isShowing;

		#endregion

		#region Unity methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);

			if (_settings == null)
			{
				UnityEventDebuggerSettingsProvider provider = (UnityEventDebuggerSettingsProvider) UnityEventDebuggerSettingsProvider.CreateUnityEventDebuggerSettingsProvider();
				_settings = (UnityEventDebuggerSettings) provider.Settings.targetObject;
				_isShowing = _settings.ShowFoldoutsByDefault;
			}

			if (_settings.DisableInEditMode && !Application.isPlaying)
			{
				return;
			}

			List<UnityEventMethodContextHolder> unityEventMethodContextHolders = UnityEventHelper.GetCallbacksOnObjectForEvent(property.serializedObject.targetObject, property.name);

			DrawPropertyGUILayout(unityEventMethodContextHolders);
		}

		#endregion

		#region Private methods

		private void DrawPropertyGUILayout(List<UnityEventMethodContextHolder> unityEventMethodContextHolders)
		{
			EditorGUILayout.Space();
			bool shouldShow = _isShowing || _settings.AlwaysShowFoldouts;
			_isShowing = EditorGUILayout.BeginFoldoutHeaderGroup(shouldShow, "Subscribed Callbacks", GetFoldoutStyle(unityEventMethodContextHolders.Count));

			if (_isShowing)
			{
				float originalLabelWidth = EditorGUIUtility.labelWidth;

				Texture2D[] backgroundTextures = CreateBackgroundTextures();

				for (int index = 0; index < unityEventMethodContextHolders.Count; index++)
				{
					UnityEventMethodContextHolder unityEventMethodContextHolder = unityEventMethodContextHolders[index];
					EditorGUIUtility.labelWidth = _settings.LabelWidth;

					GUIStyle style = new GUIStyle();
					style.normal.background = backgroundTextures[index % 2 == 0 ? 0 : 1];
					EditorGUILayout.BeginHorizontal(style);

					EditorGUI.BeginDisabledGroup(true);

					EditorGUILayout.TextField("Callback", unityEventMethodContextHolder.CallbackName);
					EditorGUILayout.ObjectField("Context", unityEventMethodContextHolder.Context, typeof(Object), true);

					EditorGUI.EndDisabledGroup();

					EditorGUILayout.EndHorizontal();
				}

				EditorGUIUtility.labelWidth = originalLabelWidth;
			}

			EditorGUILayout.EndFoldoutHeaderGroup();
			EditorGUILayout.Space();
		}

		private GUIStyle GetFoldoutStyle(int foldoutRows)
		{
			GUIStyle foldoutStyle = new GUIStyle("Foldout");

			Texture2D normalBackground = new Texture2D(1, 1);
			normalBackground.SetPixel(0, 0, _settings.FoldoutGroupColor);
			normalBackground.Apply();
			foldoutStyle.normal.background = normalBackground;

			RectOffset offset = foldoutStyle.overflow;
			if (_isShowing && (foldoutRows > 0))
			{
				offset.top = (int) (0.5f * EditorGUIUtility.singleLineHeight);
				offset.bottom = (int) (foldoutRows * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight);
			}

			foldoutStyle.overflow = offset;

			return foldoutStyle;
		}

		private Texture2D[] CreateBackgroundTextures()
		{
			Texture2D[] backgroundTextures =
			{
				new Texture2D(1, 1),
				new Texture2D(1, 1)
			};

			backgroundTextures[0].SetPixel(0, 0, _settings.FirstRowColor);
			backgroundTextures[0].Apply();

			backgroundTextures[1].SetPixel(0, 0, _settings.SecondRowColor);
			backgroundTextures[1].Apply();

			return backgroundTextures;
		}

		#endregion
	}
}