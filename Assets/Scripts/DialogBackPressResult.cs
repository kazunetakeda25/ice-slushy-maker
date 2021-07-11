using UnityEngine;

public class DialogBackPressResult : MonoBehaviour
{
	public void SetResult(bool result)
	{
		GameObject.Find("DialogSavePictureManager").GetComponent<DialogSavePictureManager>().SetDialogResult(result);
	}

	public void SetState(bool state)
	{
		GameObject.Find("DialogSavePictureManager").GetComponent<DialogSavePictureManager>().SetDialogState(state);
	}
}
