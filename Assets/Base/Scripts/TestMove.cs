using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class TestMove : MonoBehaviour {

	public bool marche;

	public GameObject femme;
	public GameObject spike;
	public GameObject rango;
	public GameObject armee;

	private int actualState;
	private int LastState;

	public CameraPathAnimator[] cameras;

	// Use this for initialization
	void Start () {
		LastState = 0;
	}
	
	// Update is called once per frame
	void Update () {
		actualState = DialogueLua.GetVariable("Sequence").AsInt;

		if (LastState != actualState) {	
			Debug.Log(actualState);

			//stop tout avant de changer
			StopAllCamera();

			LastState = actualState;

			switch (actualState) {
			case 1:
				cameras[1].Play();
				DialogueManager.StopConversation();
				DialogueManager.StartConversation("intro");
				break;
			case 2:
				cameras[2].Play();
				femme.SetActive(false);
				break;
			case 3://intermede
				cameras[3].Play();
				break;
			case 4://supprise
				cameras[4].Play();
				armee.SetActive(true);
				break;
			case 5://supprise / rango
				cameras[5].Play();
				break;
			case 6:
				cameras[3].Play();
				break;
			case 7:
				cameras[6].Play();
				break;
			case 8:
				cameras[7].Play();
				break;
			default:
				break;
			}
		}
	}

	void StopAllCamera(){
		for (int i = 0; i < cameras.Length; i++) {
			cameras[i].Stop();
		}
	}
}
