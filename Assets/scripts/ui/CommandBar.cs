using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Priority_Queue;
using System.Collections.Generic;

public class CommandBar : MonoBehaviour {
	public static bool inUse{ get; set; }

	public DemoWeightedGraph weightedGraph;

	void Update () {
	
		if (GetComponent<InputField> ().isFocused) {

			inUse = true;

		} else {

			inUse = false;

		}

		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)) {
			doCommand (GetComponent<InputField> ().text);
		}

	}

	public void doCommand(string command){

		if(command.Length > 11 && command.Substring(0, 12) == "create node ")
			weightedGraph.createNode (DemoWeightedGraph.selectedNode, command.Substring (12));

		if(command.Length > 10 && command.Substring(0, 11) == "delete node")
			weightedGraph.deleteNode (DemoWeightedGraph.selectedNode);

		if(command.Length > 12 && command.Substring(0, 13) == "connect node ")
			weightedGraph.connectNode (DemoWeightedGraph.selectedNode, command.Substring(13));
		
		if(command.Length > 13 && command.Substring(0, 14) == "change weight "){
			string input = command.Substring (14);
			string input1 = input.Substring (0, input.IndexOf (" "));
			string input2 = input.Substring (input.IndexOf (" "));
			weightedGraph.changeWeight (DemoWeightedGraph.selectedNode, 
				weightedGraph.findNode(weightedGraph.head, input1), int.Parse(input2));
		}

		if(command.Length > 16 && command.Substring(0, 17) == "change heuristic "){
			
			string input = command.Substring (17);
			string input1 = input.Substring (0, input.IndexOf (" "));

			DemoWeightedGraph.selectedNode.heuristic = int.Parse (input1);

		}

		if (command.Length > 20 && command.Substring (0, 21) == "set heuristic preset ") {
			string input = command.Substring (21);

			if (input == "depth first") {
				
				foreach (Node<string> n in DemoWeightedGraph.allNodes) {
					n.heuristic = weightedGraph.maxDepth - n.depth;
				}

			} else if (input == "breath first") {

				foreach (Node<string> n in DemoWeightedGraph.allNodes) {
					n.heuristic = n.depth;
				}

			}else if (input == "uniform cost") {

				setUniformCostPreset ();

			}

		}
		 
		if(command.Length > 3 && command.Substring(0, 4) == "BFS ")
			StartCoroutine(weightedGraph.BFS (DemoWeightedGraph.selectedNode, command.Substring(4)));

		if(command.Length > 3 && command.Substring(0, 4) == "DFS ")
			StartCoroutine(weightedGraph.DFS (DemoWeightedGraph.selectedNode, command.Substring(4)));

		if(command.Length > 3 && command.Substring(0, 4) == "DWS ")
			StartCoroutine(weightedGraph.DWS (DemoWeightedGraph.selectedNode, command.Substring(4)));

		if(command.Length > 3 && command.Substring(0, 4) == "UCS ")
			StartCoroutine(weightedGraph.UCS (DemoWeightedGraph.selectedNode, command.Substring(4)));

		if(command.Length > 2 && command.Substring(0, 3) == "GS ")
			StartCoroutine(weightedGraph.GS (DemoWeightedGraph.selectedNode, command.Substring(3)));

	}

	void setUniformCostPreset(){

		// Create a queue for BFS
		Node<string> currentNode = DemoWeightedGraph.selectedNode;
		SimplePriorityQueue<Node<string>> queue = new SimplePriorityQueue<Node<string>>();
		int nodeIndex = DemoWeightedGraph.allNodes.IndexOf (currentNode);

		queue.Enqueue(currentNode, 0);

		int[] dist = new int[DemoWeightedGraph.allNodes.Count];
		bool[] visited = new bool[DemoWeightedGraph.allNodes.Count];

		for (int index = 0; index < DemoWeightedGraph.allNodes.Count; index++)
			dist [index] = SearchConstants.INFINITY;

		dist [nodeIndex] = 0;

		while(queue.Count != 0)
		{

			currentNode = queue.Dequeue();

			int v = DemoWeightedGraph.allNodes.IndexOf(currentNode);
			DemoWeightedGraph.allNodes[v].heuristic = dist [v];
			visited [v] = true;

			List<Node<string>> connected = new List<Node<string>>(currentNode.connectedToNode);
			connected.AddRange (currentNode.connectedFromNode);
			foreach(Node<string> node in connected){

				int u = DemoWeightedGraph.allNodes.IndexOf(node);
				if(!visited[u] && dist[u] > dist[v] + currentNode.getWeight(node))
				{
					dist[u] = dist[v] + currentNode.getWeight(node);

					if(queue.Contains(node)){
						queue.UpdatePriority (node, dist [u]);
					}else{
						queue.Enqueue(node, dist[u]);
					}
				}

			}
		}

	}

}
