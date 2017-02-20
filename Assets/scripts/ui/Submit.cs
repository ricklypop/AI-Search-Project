using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Submit : MonoBehaviour {
	public InputField command;

	public void submit(){
		command.transform.GetComponent<CommandBar> ().doCommand (command.text);
	}

}
