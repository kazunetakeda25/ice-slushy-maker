using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogSavePictureManager : MonoBehaviour
{
	public bool dialogState;

	public bool dialogResult;

	public GameObject savePictureDialog;

	public RawImage dialogContent;

	public Text dialogTitle;

	private Texture2D _Texture;

	private string _DirectoryName;

	private string _PictureName;

	public RawImage savedImg;

	private void __Awake()
	{
		savePictureDialog = GameObject.Find("Canvas/PopUps/PopUpDialogSavePicture");
		dialogContent = GameObject.Find("Canvas/PopUps/PopUpDialogSavePicture/AnimationHolder/Body/ContentHolder/TextBG/RawImage").GetComponent<RawImage>();
		dialogTitle = GameObject.Find("Canvas/PopUps/PopUpDialogSavePicture/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>();
		base.transform.name = "DialogSavePictureManager";
	}

	public void SetDialogResult(bool result)
	{
		dialogResult = result;
	}

	public void SetDialogState(bool state)
	{
		dialogState = state;
	}

	public void ShowDialog(string title, Texture2D content)
	{
		if (savedImg != null)
		{
			savedImg.enabled = true;
			savedImg.texture = content;
			savedImg.transform.parent.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			Vector2 sizeDelta = savedImg.rectTransform.sizeDelta;
			float y = sizeDelta.x / (float)content.width * (float)content.height;
			savedImg.rectTransform.sizeDelta = new Vector2(sizeDelta.x, y);
		}
		dialogTitle.text = title;
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(savePictureDialog);
		dialogState = true;
	}

	public void SavePicture(Texture2D texture, string directoryName, string pictureName)
	{
		_Texture = texture;
		_DirectoryName = directoryName;
		_PictureName = pictureName;
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					androidJavaObject.Call("checkPermissionAndRun");
				}
			}
		}
		catch
		{
			UnityEngine.Debug.Log("Error Contacting Android save image!");
		}
	}

	public void SaveImage()
	{
		Texture2D texture = _Texture;
		string directoryName = _DirectoryName;
		string pictureName = _PictureName;
		if (savedImg != null)
		{
			savedImg.enabled = true;
			savedImg.texture = texture;
			savedImg.transform.parent.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			Vector2 sizeDelta = savedImg.rectTransform.sizeDelta;
			float y = sizeDelta.x / (float)texture.width * (float)texture.height;
			savedImg.rectTransform.sizeDelta = new Vector2(sizeDelta.x, y);
		}
		if (!Directory.Exists(Application.persistentDataPath + "/" + directoryName))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
		}
		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png", bytes);
		string text = Share.ReturnGalleryFolder();
		if (text != string.Empty)
		{
			try
			{
				if (!Directory.Exists(text + directoryName))
				{
					Directory.CreateDirectory(text + directoryName);
				}
				string text2 = text;
				text = text2 + directoryName + "/" + pictureName + ".png";
				string sourceFileName = Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png";
				File.Copy(sourceFileName, text, overwrite: false);
				GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogTitleText("PICTURE SAVED");
				GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogCustomMessageText(" ");
				UnityEngine.Debug.Log("NAIL picture saved");
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("NAIL PATH 2 " + ex.Message);
				if (savedImg != null)
				{
					savedImg.enabled = false;
					savedImg.transform.parent.GetComponent<Image>().color = Color.white;
				}
				GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogTitleText("PICTURE NOT SAVED");
				GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogCustomMessageText(" ");
			}
		}
		else
		{
			Invoke("WaitAndShowFailure", 2f);
		}
		Share.RefreshGalleryFolder(text);
	}

	private void WaitAndShowFailure()
	{
		if (savedImg != null)
		{
			savedImg.enabled = false;
			savedImg.transform.parent.GetComponent<Image>().color = Color.white;
		}
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogTitleText("PICTURE NOT SAVED");
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogCustomMessageText("Galery folder could not be found.");
	}

	public Texture2D LoadTextureFromFile(string directory, string imageName, int width, int height)
	{
		string path = Application.persistentDataPath + "/" + directory + "/" + imageName + ".png";
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.filterMode = FilterMode.Bilinear;
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			texture2D.LoadImage(data);
			texture2D.Apply();
		}
		return texture2D;
	}

	public void LoadTextureFromFile(string directory, string imageName, Texture2D target)
	{
		string path = Application.persistentDataPath + "/" + directory + "/" + imageName + ".png";
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			target.LoadImage(data);
			target.Apply();
		}
	}

	public void ClosePopUpSave()
	{
		StartCoroutine(WaitAndHide());
	}

	private IEnumerator WaitAndHide()
	{
		yield return new WaitForSeconds(1f);
		if (savedImg != null)
		{
			savedImg.enabled = false;
			savedImg.transform.parent.GetComponent<Image>().color = Color.white;
		}
	}
}
