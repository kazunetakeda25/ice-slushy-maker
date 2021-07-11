using UnityEngine;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private Text FPS_Text;

	private void Start()
	{
		timeleft = updateInterval;
		FPS_Text = base.transform.GetComponent<Text>();
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float num = accum / (float)frames;
			string text = $"{num:F2} FPS";
			FPS_Text.text = text;
			if (num < 25f)
			{
				FPS_Text.color = Color.yellow;
			}
			else if (num < 15f)
			{
				FPS_Text.color = Color.red;
			}
			else
			{
				FPS_Text.color = Color.green;
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
