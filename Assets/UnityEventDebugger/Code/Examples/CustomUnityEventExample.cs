using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEventDebugger.Examples
{
	/// <summary>
	///     A more complex example showing that the tool can inspect any kind of UnityEvent, no matter what/how many custom
	///     arguments.
	/// </summary>
	public class CustomUnityEventExample : MonoBehaviour
	{
		#region Serialize Fields

		[SerializeField] private MyCustomUnityEvent _event;

		#endregion

		#region Unity methods

		private void Awake()
		{
			_event.AddListener(DoSomething);
			_event.AddListener(DoSomething);
			_event.AddListener(DoSomething);
			_event.AddListener(DoSomething);
			_event.AddListener(DoSomething);
			_event.AddListener(DoSomething);
		}

		#endregion

		#region Private methods

		private void DoSomething(int arg0, float arg1, string arg2, bool arg3)
		{
			Debug.Log("Do Something!");
		}

		#endregion

		#region Nested type: MyCustomUnityEvent

		[Serializable]
		public class MyCustomUnityEvent : UnityEvent<int, float, string, bool>
		{
		}

		#endregion
	}
}