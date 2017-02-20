using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node<Type> : MonoBehaviour {
	public Node<Type> creatorNode { get; set; }
	public List<Node<Type>> connectedFromNode = new List<Node<Type>> ();
	public List<Node<Type>> connectedToNode = new List<Node<Type>> ();
	public List<int> weightValues = new List<int> ();

	public int depth { get; set; }
	public int heuristic { get; set; }
	public Type value {get; set;}
	public bool isHead { get; set; }

	void Start(){
	}

	public virtual void Update () {}

	public virtual void addConnection(Node<Type> node, int weight){
		
		if (node.creatorNode == null && !node.isHead) {
			node.creatorNode = this;
		}

		connectedToNode.Add (node);
		node.connectedFromNode.Add (this);
		weightValues.Add (weight);

	}

	public virtual void removeConnection(Node<Type> node){
		weightValues.RemoveAt (connectedToNode.IndexOf(node));
		connectedToNode.Remove (node);
	}

	public virtual void destroy(){
		
		foreach (Node<Type> connected in connectedToNode) {
			
			if (connected.creatorNode == this) {
				connected.creatorNode = creatorNode;
			}

		}

		if(creatorNode != null)
			creatorNode.removeConnection (this);

		foreach (Node<Type> node in new List<Node<Type>>(connectedToNode)) {

			creatorNode.addConnection (node, weightValues[connectedToNode.IndexOf(node)]);
			removeConnection (node);

		}
			
		Destroy (gameObject);

	}

	public virtual void changeWeight(Node<Type> node, int value){

		weightValues [connectedToNode.IndexOf (node)] = value;

	}

	public int getWeight(Node<Type> node){

		if(connectedToNode.Contains(node))
			return weightValues [connectedToNode.IndexOf (node)];
		else
			return node.weightValues[node.connectedToNode.IndexOf (this)];


	}

}