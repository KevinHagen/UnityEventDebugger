using System;
using System.Reflection;
using Object = UnityEngine.Object;

namespace UnityEventDebugger
{
	/// <summary>
	///     Stores exactly one callback for an UnityEventBase eventName.
	///     The name of the callback is given like ClassName.MethodName and the Object that subscribed itself to the event is
	///     also stored.
	/// </summary>
	[Serializable]
	public class UnityEventMethodContextHolder
	{
		#region Private Fields

		private string _className;
		private string _methodName;
		private string _eventName;
		private Object _context;

		#endregion

		#region Properties

		public string CallbackName => $"{_className}.{_methodName}";
		public Object Context => _context;
		public string EventName => _eventName;

		#endregion

		#region Constructors

		public UnityEventMethodContextHolder(string eventName, MethodInfo methodInfo, Object context)
		{
			_eventName = eventName;
			_className = methodInfo.DeclaringType.Name;
			_methodName = methodInfo.Name;
			_context = context;
		}

		#endregion
	}
}