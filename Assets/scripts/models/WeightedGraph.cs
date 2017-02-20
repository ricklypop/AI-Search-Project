using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeightedGraph<Type> : MonoBehaviour {
	public Node<Type> head { get; set; }
	public int maxDepth {get; set;}
	public Transform nodePrefab;

	public virtual Node<Type> createNode(Node<Type> headNode, Type val){
		if (headNode != null || head == null) {
			Node<Type> node = ((Transform)Instantiate (nodePrefab, Vector3.zero, Quaternion.identity)).GetComponent<Node<Type>> ();
			node.value = val;

			DLog.Log ("Node " + val + " created.", Color.green);

			if (headNode != null) {
				headNode.addConnection (node, SearchConstants.WEIGHT_CONSTANT);
			}

			if (head == null) {
				head = node;
				node.isHead = true;

				DLog.Log ("Node " + val + " set to head.", Color.yellow);
			}

			DLog.Log("Assigned new depths: " + assignDepth (head, 0), Color.yellow);

			return node;
		}

		return null;
	}

	public virtual void deleteNode(Node<Type> headNode){
		
		if (headNode == head && headNode.connectedToNode.Count != 0) {
			head = headNode.connectedToNode [(headNode.connectedToNode.Count / 2)];
			head.isHead = true;
		}

		DLog.Log ("Node " + headNode.value + " destroyed.", Color.yellow);

		headNode.destroy ();

		DLog.Log("Assigned new depths: " + assignDepth (head, 0), Color.yellow);
	}

	public void connectNode(Node<Type> headNode, Type value){

		if(!headNode.value.Equals(value))
			headNode.addConnection(findNode(head, value), SearchConstants.WEIGHT_CONSTANT);

		DLog.Log ("Node " + headNode.value + " connected to node " + value + ".", Color.green);

		DLog.Log("Assigned new depths: " + assignDepth (head, 0), Color.yellow);
	}

	public void changeWeight(Node<Type> node1, Node<Type> node2, int value){

		if (node1.connectedToNode.Contains (node2)) {

			node1.changeWeight(node2, value);

		} else {

			node2.changeWeight(node1, value);

		}

		DLog.Log ("Weight from node " + node1.value + " to node " + node2.value + " changed to " + value + ".", Color.yellow);

	}

	public Node<Type> findNode(Node<Type> currentNode, Type value){

		if (value.Equals(currentNode.value))
			return currentNode;

		foreach (Node<Type> current in currentNode.connectedToNode) {

			if (current.creatorNode == currentNode) {
				
				Node<Type> node = findNode (current, value);

				if (node != null)
					return node;
				
			}

		}

		return null;

	}

	public virtual string assignDepth(Node<Type> currentNode, int currentDepth){
		
		if (currentNode == null)
			return "";

		if (currentNode == head)
			maxDepth = 0;

		currentNode.depth = currentDepth;

		if (currentDepth > maxDepth)
			maxDepth = currentDepth;

		string depthString = currentNode.value + ": " + currentDepth;
		foreach (Node<Type> current in currentNode.connectedToNode) {
			
			if (current.creatorNode == currentNode) {
				depthString = depthString  + ", " +  assignDepth (current, currentDepth + 1);
			}

		}

		return depthString;
	}

	public List<Node<Type>> clear(Node<Type> currentNode, bool intial){

		List<Node<Type>> list = new List<Node<Type>> ();

		foreach (Node<Type> current in currentNode.connectedToNode) {
			if(current.creatorNode == currentNode)
				list.AddRange(clear (current, false));
		}

		list.Add (currentNode);

		if (intial) {

			foreach (Node<Type> current in new List<Node<Type>> (list)) {
				current.destroy ();
			}

		}

		return list;
	}

}
