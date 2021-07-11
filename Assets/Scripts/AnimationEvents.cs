using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
	private Animator anim;

	public void AnimEventHideSceneAnimStarted()
	{
		LevelTransition.Instance.AnimEventHideSceneAnimStarted();
	}

	public void AnimEventShowSceneAnimFinished()
	{
		LevelTransition.Instance.AnimEventShowSceneAnimFinished();
	}

	public void CleaningAnimationFinished()
	{
		base.transform.parent.SendMessage("CleaningAnimationFinished", SendMessageOptions.DontRequireReceiver);
	}

	public void StartParticles()
	{
		base.transform.GetComponentInChildren<ParticleSystem>().Play();
	}

	public void SelectFlavorBGZoomInFinished()
	{
		Camera.main.SendMessage("FillSlushContainer");
	}

	public void SelectFlavorBGZoomOutFinished()
	{
		Camera.main.SendMessage("SelectFlavorBGZoomOutFinished");
	}

	public void FlavorContainerStartFilling()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.FillFlavor);
		}
	}

	public void FlavorContainerEndFilling()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillFlavor);
		}
	}

	public void FlavorContainerOpened()
	{
	}

	public void FillSlushFlavorFinished()
	{
		Camera.main.SendMessage("FillSlushContainerFinished");
	}

	public void PitcherStarFillingWaterToMold()
	{
		DragItem component = base.transform.parent.GetComponent<DragItem>();
		component.PitcherFillMold();
	}

	public void PitcherFinishFillingWaterToMold()
	{
	}

	public void PitcherAnimEnd()
	{
		DragItem component = base.transform.parent.GetComponent<DragItem>();
		component.AnimationFinished();
	}
}
