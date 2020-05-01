using UnityEngine;
using UnityEngine.UI;

namespace UnityEventDebugger.Examples
{
	/// <summary>
	///     Simple example for an event subscriber. This uses a UnityEngine.UI.Button to show how the different callbacks can
	///     be inspected
	///     with the tool.
	/// </summary>
	public class EventSubscriberExample : MonoBehaviour
	{
		#region Serialize Fields

		[SerializeField] private Button _button = null;
		[SerializeField] private Slider _slider = null;
		[SerializeField] private Toggle _toggle = null;

		#endregion

		#region Unity methods

		private void Awake()
		{
			if (_button != null)
			{
				_button.onClick.AddListener(DoSomething);
				_button.onClick.AddListener(DoSomethingElse);
			}

			if (_slider != null)
			{
				_slider.onValueChanged.AddListener(DoSomethingFloat);
				_slider.onValueChanged.AddListener(DoSomethingElseFloat);
			}

			if (_toggle != null)
			{
				_toggle.onValueChanged.AddListener(DoSomethingBool);
				_toggle.onValueChanged.AddListener(DoSomethingElseBool);
			}
		}

		#endregion

		#region Private methods

		private void DoSomethingElseBool(bool arg0)
		{
			Debug.Log($"Do something else value passed = {arg0}!");
		}

		private void DoSomethingBool(bool arg0)
		{
			Debug.Log($"Do something on click value passed = {arg0}!");
		}

		private void DoSomethingElseFloat(float arg0)
		{
			Debug.Log($"Do something else value passed = {arg0}!");
		}

		private void DoSomethingFloat(float arg0)
		{
			Debug.Log($"Do something on click value passed = {arg0}!");
		}

		private void DoSomethingElse()
		{
			Debug.Log("Do something else on click!");
		}

		private void DoSomething()
		{
			Debug.Log("Do something on click!");
		}

		#endregion
	}
}