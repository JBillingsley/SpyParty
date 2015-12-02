using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	
	public MovieTexture Video;
	public float timer = 10.0f;



	private bool playMovie = false;
	private bool timeStart = false;
	private int page = 0;

	void Start(){
		Video.Play ();
		playMovie = true;
		timeStart = true;
	}

	void Update(){
	
		if (gameObject.GetComponent<AudioSource> ().isPlaying) {
			Debug.Log("Music!");
		}

		if (timeStart == true) {
			timer -= Time.deltaTime;
			if(timer <=0){
				Video.Stop();
				playMovie = false;
			}
		}
		Debug.Log (timer);
	}


	void OnGUI(){
		if (Video != null && playMovie == true) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Video, ScaleMode.StretchToFill, false, 0.0f);
		}
	}

	public void turnPage(){
		page += 1;
		if (page == 1) {
			GameObject.Find ("Instruction1").SetActive(false);
			GameObject.Find ("Instruction2").SetActive(true);
		}
		if (page == 2) {
			GameObject.Find ("Instruction2").SetActive(false);
			GameObject.Find ("ContinueButton").SetActive(false);
		}

		Debug.Log("page");

	}

	public void StartGame(){
		Application.LoadLevel("scene7");
	}
	
	public void ExitGame(){
		Application.Quit ();
	}
}
