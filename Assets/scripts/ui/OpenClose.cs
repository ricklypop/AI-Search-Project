using UnityEngine;
using System.Collections;

public class OpenClose : MonoBehaviour {
	private bool open { get; set; }

	public void openClose(){
		if (open) {
			
			GetComponent<CanvasGroup> ().interactable = false;
			GetComponent<CanvasGroup> ().alpha = 0;
			open = false;

		} else {

			GetComponent<CanvasGroup> ().interactable = true;
			GetComponent<CanvasGroup> ().alpha = 1;
			open = true;

		}
	}
}
