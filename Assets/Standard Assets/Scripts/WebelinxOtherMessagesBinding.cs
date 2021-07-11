using System.Runtime.InteropServices;
using UnityEngine;

public class WebelinxOtherMessagesBinding : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void _sendMessageOther(string appSignature);

	public static void sendMessage(string appSignature)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_sendMessageOther(appSignature);
		}
	}
}
