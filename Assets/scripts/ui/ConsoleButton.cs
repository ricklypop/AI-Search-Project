using UnityEngine;
using System.Collections;
using Tiinoo.DeviceConsole;

public class ConsoleButton : MonoBehaviour {

	public void pressed(){
		UIWindowMgr.Instance.PopUpWindow(UIWindow.Id.Console, false);
	}

}
