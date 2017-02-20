using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoNode : Node<string> {
	public Transform weightPrefab;
	public TextMesh mesh;

	public List<TextMesh> weightObjects = new List<TextMesh> ();

	private bool collision = false;

	public override void Update(){
		
		base.Update ();

		if (!collision){
			mesh.text = value;
			mesh.color = Color.white;
		}else {
			mesh.text = heuristic.ToString();
			mesh.color = Color.green;
		}

		checkSelected ();

		Behaviour halo = (Behaviour)GetComponent("Halo");
		if (DemoWeightedGraph.selectedNode == this) {

			halo.enabled = true;

		} else {
			
			halo.enabled = false;

		}

		transform.Rotate(new Vector3(0, 0.5f, 0));

		positionConnectedNodes ();
		positionWeights ();
	}

	void checkSelected(){

		collision = false;

		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform.gameObject == gameObject){
				collision = true;
			}
		}

		if (Input.GetMouseButtonDown(0) && collision) {
			
			DemoWeightedGraph.selectedNode = this;

		}

	}

	void positionWeights(){

		for (int index = 0; index < connectedToNode.Count; index++) {

			weightObjects [index].transform.position = new Vector3 (
				(transform.position.x + connectedToNode [index].transform.position.x) / 2,
				(transform.position.y + connectedToNode [index].transform.position.y) / 2,
				1);

			weightObjects [index].text = weightValues [index].ToString();

			weightObjects [index].transform.GetComponent<LineRenderer> ().SetPosition (0, transform.position);
			weightObjects [index].transform.GetComponent<LineRenderer> ().SetPosition (1, connectedToNode[index].transform.position);

		}

	}

	void positionConnectedNodes(){

		List<Node<string>> ownedNodes = new List<Node<string>> ();

		foreach (Node<string> node in connectedToNode) {
			
			if (node.creatorNode == this) {
				ownedNodes.Add (node);
			}

		}

		float space = SearchSettings.NODE_DISTANCE_FACTOR_X / (Mathf.Pow(SearchSettings.NODE_TREE_DEGEN_FACTOR, depth));
		float depthSpace = (ownedNodes.Count * space);

		for (int index = 0; index < ownedNodes.Count; index++) {

				ownedNodes [index].gameObject.transform.position = new Vector3 (
					(index * space)
					+ (space / 2)
					+ (transform.position.x - ((depthSpace / 2))),
				transform.position.y - SearchSettings.NODE_DISTANCE_FACTOR_Y, 0);

		}

	}

	public override void addConnection(Node<string> node, int weight){
		TextMesh mesh = ((Transform)Instantiate (weightPrefab, Vector3.zero, 
			Quaternion.identity)).GetComponent<TextMesh>();
		weightObjects.Add (mesh);
		base.addConnection (node, weight);
	}

	public override void removeConnection(Node<string> node){
		TextMesh mesh = weightObjects [connectedToNode.IndexOf (node)];
		weightObjects.Remove (mesh);
		Destroy (mesh.gameObject);
		base.removeConnection (node);
	}

	public override void changeWeight(Node<string> node, int value){
		base.changeWeight (node, value);
	}
}
