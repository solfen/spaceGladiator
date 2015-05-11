
using UnityEngine;
using System.Collections;

/// <summary>
/// A FPS counter.
/// It calculates frames/second over each updateInterval,
/// so the display does not keep changing wildly.
/// </summary>
public class FpsCounter : MonoBehaviour {
/*
{
	public float updateInterval = 0.5f;	// Update every...
	float lastInterval;					// Last interval end time
	int frames = 0;						// Frames over current interval
	public static float fps;			// Current FPS

	void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	void Update()
	{
		frames++;
		float timeNow = Time.realtimeSinceStartup;

		if (timeNow > lastInterval + updateInterval)
		{
			fps = frames / (timeNow - lastInterval);
			frames = 0;
			lastInterval = timeNow;

			if (guiText)
				guiText.text = string.Format(textPattern, fps);
		}
	}


	#region GUI

	public bool showGUI = true;
	public string textPattern = "FPS : {0:0.00}";
	public Rect guiRect = new Rect(5, 5, 100, 30);
	public TextAnchor rectAlignment = TextAnchor.MiddleCenter;
	public TextAnchor contentAlignment = TextAnchor.MiddleCenter;

	void OnGUI()
	{
		if (showGUI)
		{
			Rect rect = new Rect(guiRect);

			// HACK: need to do all alignments!
			switch (rectAlignment)
			{
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.LowerLeft:
					rect.y = Screen.height - rect.height - rect.y;
					break;
				case TextAnchor.LowerRight:
                    rect.y = Screen.height - rect.height - rect.y;
                    rect.x = Screen.width - rect.width - rect.x;
					break;
				case TextAnchor.MiddleCenter:
					break;
				case TextAnchor.MiddleLeft:
					break;
				case TextAnchor.MiddleRight:
					break;
				case TextAnchor.UpperCenter:
					break;
				case TextAnchor.UpperLeft:
					break;
				case TextAnchor.UpperRight:
					rect.x = Screen.width - rect.width - rect.x;
					break;
			}

			GUILayout.BeginArea(rect, (GUIStyle)"box");
			{
				GUIStyle style = new GUIStyle("label");
				style.alignment = contentAlignment;
				style.stretchHeight = true;

				GUILayout.Label(string.Format(textPattern, fps), style);
			}
			GUILayout.EndArea();
		}
	}

	#endregion
*/
}
