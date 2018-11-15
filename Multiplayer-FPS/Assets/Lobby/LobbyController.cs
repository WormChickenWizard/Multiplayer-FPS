using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

    public Text infoText;
    public Button playHostButton;
    public Button dedicatedButton;
    public Button joinButton;

	// Use this for initialization
	void Start () {
#if UNITY_WEBGL
        infoText.text = "Put the address for the server in the box above\nThen hit join!";
        playHostButton.interactable = false;
        dedicatedButton.interactable = false;
#endif

#if UNITY_STANDALONE
        infoText.text = "Your Local IP Address is: \nGive this to your friends when you're hosting a dedicated server!\n\n\nHit the dedicated server button to start the server!\nor hit the level editor button to start the level editor! (coming soon)";
        playHostButton.interactable = false;
        joinButton.interactable = false;
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
