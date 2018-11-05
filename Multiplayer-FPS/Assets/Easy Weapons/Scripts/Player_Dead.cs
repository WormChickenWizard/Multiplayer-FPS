using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dead : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float xRot = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 170;
        float yRot = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 170;

        transform.localRotation = Quaternion.Euler(Mathf.Clamp(transform.localEulerAngles.x + yRot, 271, 359.9f), transform.localEulerAngles.y, -90);

        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + xRot, -90);

        if (Input.GetKey(KeyCode.Space))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
