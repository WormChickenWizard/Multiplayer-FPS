using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkTransform : NetworkBehaviour {


    public GameObject pivot;
    [SyncVar]
    public Vector3 location;
    [SyncVar]
    public Quaternion rotation;
    [SyncVar]
    public Quaternion pivotRotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
            CmdSendLocationRotation(transform.position, transform.localRotation, pivot.transform.localRotation);
        else
        {
            transform.position = Vector3.Lerp(transform.position, location, .1f);
            transform.rotation = Quaternion.Lerp(transform.localRotation, rotation, .1f);
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, pivotRotation, .1f);
        }
	}


    [Command]
    public void CmdSendLocationRotation(Vector3 loc, Quaternion rot, Quaternion pivotRot)
    {
        location = loc;
        rotation = rot;
        pivotRotation = pivotRot;
    }
}
