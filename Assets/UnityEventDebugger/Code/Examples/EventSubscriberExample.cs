using UnityEngine;
using UnityEngine.UI;

namespace UnityEventDebugger.Examples
{
	public class EventSubscriberExample : MonoBehaviour
	{
		[SerializeField] private Button _button;

		private void Awake()
		{
			_button.onClick.AddListener(DoSomething);
			_button.onClick.AddListener(DoSomethingElse);
		}

		private void DoSomethingElse()
		{
			Debug.Log("Do something else!");
		}

		private void DoSomething()
		{
			Debug.Log("Do something!");
		}
	}
}