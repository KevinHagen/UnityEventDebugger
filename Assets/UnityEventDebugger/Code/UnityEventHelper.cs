using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnityEventDebugger
{
	[Serializable]
	public class UnityEventMethodContextHolder
	{
		public Object Context => _context;
		public string CallbackName => $"{_className}.{_methodName}";
		public string EventName => _eventName;

		private string _className;
		private string _methodName;
		private string _eventName;
		private Object _context;
		
		public UnityEventMethodContextHolder(string eventName, MethodInfo methodInfo, Object context)
		{
			_eventName = eventName;
			_className = methodInfo.DeclaringType.Name;
			_methodName = methodInfo.Name;
			_context = context;
		}
	}
	
	public static class UnityEventHelper
	{
		public static List<UnityEventMethodContextHolder> GetCallbacksOnObjectForEvent(Object theObject, string eventName)
		{
			// Prepare our return value
			List<UnityEventMethodContextHolder> retList = new List<UnityEventMethodContextHolder>();
		
			// Each UnityEvent (onClick, onValueChanged, ...) is of type UnityEventBase. It contains methods and to receive all callbacks subscribed to it,
			// which we can trigger with reflection.
			Type unityEventBaseType = typeof(UnityEventBase);
			
			// MemberInfo of the event called eventName on the selectable
			MemberInfo[] memberInfos = theObject.GetType().GetMember(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
			// If the selectable in question does not have an item named eventName, return here
			if (memberInfos.Length == 0)
			{
				return null;
			}
			MemberInfo selectableEventMemberInfo = memberInfos[0];
			
			// PrepareInvoke is a method called on UnityEventBase that collects all BaseInvokableCall objects (runtime, persistent, ...) subscribed to the UnityEvent
			// It returns List<BaseInvokableCall>, where one BaseInvokableCall is one invoked delegate. 
			// Casting it to IList interface allows us to iterate over all callbacks subscribed to the UnityEvent in question
			MethodInfo unityEventBasePrepareInvokMethodInfo = unityEventBaseType.GetMethod("PrepareInvoke", BindingFlags.NonPublic | BindingFlags.Instance);
			IList callbacks = unityEventBasePrepareInvokMethodInfo.Invoke(selectableEventMemberInfo.GetValue(theObject), null) as IList;

			// Iterate over the BaseInvokableCalls
			foreach (object callback in callbacks)
			{
				// type of the callback used (e.g. InvokableCall or InvokableCall<T0>)
				Type callbackType = callback.GetType();
				
				// Each class inheriting BaseInvokableCall implements a field "Delegate" which is a delegate of type UnityAction, UnityAction<T0>, UnityAction<T0, T1>, ...
				FieldInfo delegateFieldInfo = callbackType.GetField("Delegate", BindingFlags.Instance | BindingFlags.NonPublic);
				// TODO handle already subscribed objects
				// the MultiCastDelegate object from the current BaseInvokableCall 
				MulticastDelegate eventDelegate = (MulticastDelegate) delegateFieldInfo.GetValue(callback);
				
				// C# Delegates have a method "GetInvocationList()" which returns Delegate[] where one element is one callback.
				// Retrieving the methodInfo for this method and invoking it with passing the MultiCastDelegate object from the InvokableCall class returns
				// the array of methods which will be invoked upon invoking the event.
				MethodInfo delegateToStringMethodInfo = delegateFieldInfo.FieldType.GetMethod("GetInvocationList", BindingFlags.Public | BindingFlags.Instance);
				Delegate[] delegatesForCurrentCallback = delegateToStringMethodInfo.Invoke(eventDelegate, null) as Delegate[];

				// Loop over all delegates and store all required information in a new object for our return list
				foreach (Delegate theDelegate in delegatesForCurrentCallback)
				{
					Object target = (Object) theDelegate.Target;
					retList.Add(new UnityEventMethodContextHolder(eventName, theDelegate.Method, target));
				}
			}
			
			return retList;
		}
	}
	
	public static class ReflectionExtensions
	{
		public static object GetValue(this MemberInfo memberInfo, object outer)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Field:
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					return fieldInfo.GetValue(outer);
				case MemberTypes.Property:
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					return propertyInfo.GetValue(outer,new object[0]);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
