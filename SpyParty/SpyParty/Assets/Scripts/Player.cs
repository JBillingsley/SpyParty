using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour {
    public GameObject startSquare;
    public GameObject currentSquare;
    public float YOffsetFromBoard;
    public GameObject examineIcon;
    public GameObject hideIcon;
    public delegate void PlayerTurn();
    public static event PlayerTurn turn;

    private GameObject currentIcon;

    private static Player _instance;
    public static Player instance {
        get {
            if(_instance == null) {
                _instance = GameObject.Find("Player").GetComponent<Player>();
                Debug.Log("we have found an instance " + _instance);
                DontDestroyOnLoad(_instance.gameObject);
            } 
            return _instance;
        }
    }

    void Awake() {
        Debug.Log("player is awake");
        if(_instance == null) {
            _instance = this;
            Debug.Log("instance is null instance is now " + _instance);
            DontDestroyOnLoad(_instance.gameObject);
        } else if(this != _instance) {
            Debug.Log("destorying an instance");
            Destroy(this.gameObject);
        }
    }
   // private bool selected = false;
    private float animationTime = 0.6f;

	// Use this for initialization
	void Start () {
        updateLocation();
        updateCurrentSquare();
	}

    public void moving(GameObject newSquare) {
        currentSquare.GetComponent<ClickObject>().clearSquares();
        currentSquare = newSquare;
        updateLocation();
       // updateCurrentSquare();
       // turn();
    }

    void updateLocation() {
       // Debug.Log(string.Format("The player position is {0}", transform.position));
       // Debug.Log(string.Format("The currentSquare position is {0}", currentSquare.transform.position));
        iTween.MoveTo(gameObject, iTween.Hash("position",new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z),
                                              "time", animationTime,
                                              "easetype",iTween.EaseType.easeInOutQuad,
                                              "oncomplete", "updateCurrentSquare"));
    }

    void updateCurrentSquare() {
       // Debug.Log(string.Format("The players updated position is {0}", transform.position));
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
        updateIcon();
        finished();
    }

    // this is just a container for the turn function, it will call if the NPC has been initialized so the NPC can have a turn
    public void finished() {
        if(turn != null) {
            turn();
            currentSquare.GetComponent<ClickObject>().holdingPlayer();
        }
    }

    public void caught() {
        currentSquare.GetComponent<ClickObject>().clearSquares();
        currentSquare = startSquare;
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z),
                                              "time", animationTime,
                                              "easetype", iTween.EaseType.easeInOutQuad));
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
    }

    public void updateIcon() {
        if(currentSquare.GetComponent<ClickObject>().hiddenObjectNotification != null) {
            currentIcon = examineIcon;
            currentIcon.SetActive(true);
        } else if(currentSquare.GetComponent<ClickObject>().hideable) {
            currentIcon = hideIcon;
            currentIcon.SetActive(true);
        } else if(currentIcon != null) {
            currentIcon.SetActive(false);
            currentIcon = null;
        }
    }
}
