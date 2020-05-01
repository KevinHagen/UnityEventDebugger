using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEventDebugger.Editor
{
	[CustomPropertyDrawer(typeof(UnityEventBase), true)]
	public class UnityEventDebuggerDrawer : UnityEventDrawer
	{
		// private bool _showDebugGroup;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);

			List<UnityEventMethodContextHolder> unityEventMethodContextHolders = UnityEventHelper.GetCallbacksOnObjectForEvent(property.serializedObject.targetObject, property.name);

			// _showDebugGroup = EditorGUILayout.BeginFadeGroup(_showDebugGroup ? 1 : 0);

			foreach (UnityEventMethodContextHolder unityEventMethodContextHolder in unityEventMethodContextHolders)
			{
				EditorGUILayout.BeginHorizontal();

				GUI.enabled = false;
				EditorGUILayout.TextField("Callback", unityEventMethodContextHolder.CallbackName);
				GUI.enabled = true;
				EditorGUILayout.ObjectField("Context", unityEventMethodContextHolder.Context, typeof(Object), true);

				EditorGUILayout.EndHorizontal();
			}

			// EditorGUILayout.EndFadeGroup();
		}
	}
}