using System;
using System.Reflection;

namespace UnityEventDebugger.Utility
{
	/// <summary>
	///     Class with some Reflection Extension methods
	/// </summary>
	public static class ReflectionExtensions
	{
		#region Static Stuff

		/// <summary>
		///     Switches on memberInfo.MemberType to return the correct value
		/// </summary>
		/// <param name="memberInfo">The MemberInfo</param>
		/// <param name="outer">The object from which to retrieve the value</param>
		/// <returns>The retrieved value</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static object GetValue(this MemberInfo memberInfo, object outer)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Field:
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					return fieldInfo.GetValue(outer);
				case MemberTypes.Property:
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					return propertyInfo.GetValue(outer, new object[0]);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion
	}
}