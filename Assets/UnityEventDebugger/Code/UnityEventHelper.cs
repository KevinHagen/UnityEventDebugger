using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;
using UnityEventDebugger.Utility;
using Object = UnityEngine.Object;

namespace UnityEventDebugger
{
	public static class UnityEventHelper
	{
		#region Static Stuff

		/// <summary>
		///     Receives all subscribed callbacks to a UnityEventBase for a Member eventName on the given UnityEngine.Object using
		///     Reflections.
		/// </summary>
		/// <param name="theObject">The object with the UnityEvent-Member</param>
		/// <param name="eventName">The member name of the event in the class</param>
		/// <returns>A list of <see cref="UnityEventMethodContextHolder" />, containing all available callbacks</returns>
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
				// Sometimes the event is not private (as in Button - m_onClick) but public (as in Toggle - onValueChanged)
				memberInfos = theObject.GetType().GetMember(eventName, BindingFlags.Public | BindingFlags.Instance);
				// if it is still empty, there is no event.
				if (memberInfos.Length == 0)
				{
					return null;
				}
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
				FieldInfo delegateFieldInfo;
				int tries = 5;
				do
				{
					delegateFieldInfo = callbackType.GetField("Delegate", BindingFlags.Instance | BindingFlags.NonPublic);
					tries--;
					if (delegateFieldInfo == null)
					{
						callbackType = callbackType.BaseType;
					}
				} while ((delegateFieldInfo == null) || (tries > 0));

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

		#endregion
	}
}