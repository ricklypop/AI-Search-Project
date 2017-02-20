using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour {
	public DemoWeightedGraph tree;
	public InputField input;

	public void doButton(){
		tree.createNode (DemoWeightedGraph.selectedNode, input.text);
	}
}
