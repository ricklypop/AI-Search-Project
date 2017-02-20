using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	

	void Update () {

		if (!CommandBar.inUse) {
			Vector3 pos = Camera.main.transform.position;
			if (Input.GetKey (KeyCode.W)) {

				Camera.main.transform.position = new Vector3 (pos.x, pos.y + SearchConstants.CAMERA_SPEED, pos.z); 

			}

			if (Input.GetKey (KeyCode.A)) {

				Camera.main.transform.position = new Vector3 (pos.x - SearchConstants.CAMERA_SPEED, pos.y, pos.z); 

			}

			if (Input.GetKey (KeyCode.S)) {

				Camera.main.transform.position = new Vector3 (pos.x, pos.y - SearchConstants.CAMERA_SPEED, pos.z); 

			}

			if (Input.GetKey (KeyCode.D)) {

				Camera.main.transform.position = new Vector3 (pos.x + SearchConstants.CAMERA_SPEED, pos.y, pos.z); 

			}
		}

	}
}
