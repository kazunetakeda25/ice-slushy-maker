using UnityEngine;

public class Stickers : MonoBehaviour
{
	public GameObject[] StickerPrefabs;

	public Transform StickersHolder;

	public DecorationTransform decorationTransform;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CreateSticker(int stickerIndex)
	{
		if (decorationTransform.ActiveDecoration != null)
		{
			decorationTransform.ActiveDecoration.GetComponent<Decoration>().DeactivateDecoration();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(StickerPrefabs[stickerIndex]);
		gameObject.transform.parent = StickersHolder;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = 100f * UnityEngine.Random.insideUnitCircle;
		decorationTransform.ResetDecorationTransform();
		Decoration component = gameObject.GetComponent<Decoration>();
		component.decorationTransform = decorationTransform;
		decorationTransform.ActiveDecoration = gameObject;
		decorationTransform.ShowDecorationTransformTool();
	}

	public void DeleteAllStickers()
	{
		for (int num = StickersHolder.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(StickersHolder.GetChild(num).gameObject);
		}
		decorationTransform.ActiveDecoration = null;
		decorationTransform.HideDecorationTransformTool();
	}
}
