using UnityEngine;																																		
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ConvesationSequencer : MonoBehaviour {

	public GameObject rango1;
	public GameObject rango2;
	public GameObject rango3;

	public int pouet = 0;

	public GameObject femme;

	public GameObject spike1;
	public GameObject spike2;
	public GameObject spike3;

	public Camera cameraOrigine;
	public Camera cameraSequence;

	
	public CameraPathAnimator[] camerasSQ9;
	public CameraPathAnimator[] camerasSQ10;

	private int actualState = 1;
	private int LastState = 1;

	private int SQ = 10;

	private bool play = false;
	
	// Use this for initialization
	void Start () {
		Play(pouet);
	}

	public void Play(int _SQ) {
		LastState = 1;
		SQ = _SQ;
		play = true;
	}
	
	// Update is called once per frame
	void Update () {
		actualState = DialogueLua.GetVariable("SQ"+SQ).AsInt;
		Debug.Log(actualState);
		if (LastState != actualState) {

			
			//stop tout avant de changer
			StopAllCamera();
			
			LastState = actualState;

			switch (SQ) {
			case 5:
				switch (actualState) {
				case 0:
					break;
				}
				break;
			case 7:
				switch (actualState) {
				case 0:
					break;
				}
				break;
			case 9:
				switch (actualState) {
				case 0:
					camerasSQ9[0].Play();
					DialogueManager.StopConversation();
					DialogueManager.StartConversation("SQ9");
					break;
				case 1:
					camerasSQ9[1].Play();
					break;
				case 2:
					camerasSQ9[2].Play();
					break;
				case 3:
					camerasSQ9[3].Play();
					break;
				case 4:
					camerasSQ9[4].Play();
					break;
				case 5:
					camerasSQ9[5].Play();
					spike1.SetActive(false);
					spike2.SetActive(true);
					rango1.SetActive(false);
					rango2.SetActive(true);
					break;
				case 11:
					camerasSQ9[6].Play();
					spike2.SetActive(false);
					spike3.SetActive(true);
					rango2.SetActive(false);
					rango3.SetActive(true);
					break;
				case 12:
					Application.LoadLevel("fin");
					break;
				}
				break;
			case 10:
				switch (actualState) {
				case 0:
					camerasSQ10[0].Play();
					DialogueManager.StopConversation();
					DialogueManager.StartConversation("SQ10");
					break;
				case 1:
					camerasSQ10[1].Play();
					break;
				case 2:
					camerasSQ10[2].Play();
					break;
				case 3:
					camerasSQ10[3].Play();
					break;
				case 4:
					camerasSQ10[4].Play();
					break;
				case 5:
					camerasSQ10[5].Play();
					break;
				case 6:
					camerasSQ10[6].Play();
					femme.SetActive(false);
					spike1.SetActive(false);
					spike2.SetActive(true);
					spike3.SetActive(true);
					break;
				case 7:
					camerasSQ10[7].Play();
					break;
				}
				break;
			}
		}
	}
	
	void StopAllCamera(){
		for (int i = 0; i < camerasSQ9.Length; i++) {
			camerasSQ9[i].Stop();
		}
		for (int i = 0; i < camerasSQ10.Length; i++) {
			camerasSQ10[i].Stop();
		}
	}
}
