using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class DemoWeightedGraph : WeightedGraph<string> {
	public static Node<string> selectedNode{ get; set; }
	public static List<Node<string>> allNodes = new List<Node<string>> ();

	public IEnumerator UCS(Node<string> currentNode, string value){

		int currentStep = 0;
		string searchData = "";

		SimplePriorityQueue<Node<string>> queue = new SimplePriorityQueue<Node<string>>();
		int nodeIndex = allNodes.IndexOf (currentNode);

		queue.Enqueue(currentNode, 0);

		int[] dist = new int[allNodes.Count];
		bool[] visited = new bool[allNodes.Count];

		for (int index = 0; index < allNodes.Count; index++)
			dist [index] = SearchConstants.INFINITY;

		dist [nodeIndex] = 0;

		while(queue.Count != 0 && currentNode.value != value)
		{
			
			currentNode = queue.Dequeue();
			selectedNode = currentNode;

			if (currentNode.value == value)
				break;

			int v = allNodes.IndexOf(currentNode);
			visited [v] = true;

			currentStep += 1;
			searchData += ("[" + currentStep + ". Went to node " + currentNode.value + " with priority " + dist [v] + "] ");

			List<Node<string>> connected = new List<Node<string>>(currentNode.connectedToNode);
			connected.AddRange (currentNode.connectedFromNode);
			foreach(Node<string> node in connected){
				
				int u = allNodes.IndexOf(node);
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
				
			yield return new WaitForSeconds (SearchSettings.TRANSITION_TIME);
		
		}

		searchData += "Final distance values: | ";

		for (int index = 0; index < allNodes.Count; index++) {
			searchData += allNodes [index].value + " : " + dist [index] + " | ";
		}

		DLog.Log (searchData.Replace(SearchConstants.INFINITY.ToString(), "Inf"));

	}

	public IEnumerator BFS(Node<string> currentNode, string value){

		int currentStep = 0;
		string searchData = "";

		HashSet<Node<string>> set = new HashSet<Node<string>> ();
		Queue<Node<string>> queue = new Queue<Node<string>> ();

		set.Add (currentNode);
		queue.Enqueue (currentNode);        

		while (queue.Count != 0 && currentNode.value != value){
			
			currentNode = queue.Dequeue ();
			selectedNode = currentNode;

			currentStep += 1;
			searchData += ("[" + currentStep + ". Went to node " + currentNode.value + ".] ");

			if (currentNode.value == value)
				break;
			
			List<Node<string>> connected = new List<Node<string>>(currentNode.connectedToNode);
			connected.AddRange (currentNode.connectedFromNode);
			foreach(Node<string> node in connected){
				if (!set.Contains (node)) {
					set.Add (node);
					queue.Enqueue (node);
				}
			}

			yield return new WaitForSeconds (SearchSettings.TRANSITION_TIME);
		}

		DLog.Log (searchData);

	}

	public IEnumerator DFS(Node<string> currentNode, string value){

		int currentStep = 0;
		string searchData = "";

		SimplePriorityQueue<Node<string>> queue = new SimplePriorityQueue<Node<string>> ();
		queue.Enqueue (currentNode, maxDepth - currentNode.depth);

		while (currentNode.value != value && queue.Count != 0) {

			currentNode = queue.Dequeue ();
			selectedNode = currentNode;

			currentStep += 1;
			searchData += ("[" + currentStep + ". Went to node " + currentNode.value + " with depth " + currentNode.depth + "] ");

			List<Node<string>> connected = new List<Node<string>> (currentNode.connectedToNode);
			foreach (Node<string> node in connected) {

				if (node.creatorNode == currentNode) {

					queue.Enqueue (node, maxDepth - node.depth);
			
				}

			}

			yield return new WaitForSeconds (SearchSettings.TRANSITION_TIME);

		}
			
		searchData += " Remaining in priority queue: ";
		while (queue.Count != 0) {
			Node<string> node = queue.Dequeue ();
			searchData += node.value + " : " + node.depth + " | ";
		}

		DLog.Log (searchData);

	}

	public IEnumerator DWS(Node<string> currentNode, string value){

		int currentStep = 0;
		string searchData = "";

		Dictionary<Node<string>, int> dic = new Dictionary< Node<string>, int> ();
		SimplePriorityQueue<Node<string>> queue = new SimplePriorityQueue<Node<string>> ();
		queue.Enqueue (currentNode, maxDepth - currentNode.depth);

		while (currentNode.value != value && queue.Count != 0) {

			currentNode = queue.Dequeue ();
			selectedNode = currentNode;

			currentStep += 1;
			searchData += ("[" + currentStep + ". Went to node " + currentNode.value + " with priority " + dic[currentNode] + "] ");


			List<Node<string>> connected = new List<Node<string>> (currentNode.connectedToNode);
			foreach (Node<string> node in connected) {

				if (node.creatorNode == currentNode) {

					dic.Add (node, maxDepth - node.depth + currentNode.getWeight (node));
					queue.Enqueue (node, maxDepth - node.depth + currentNode.getWeight(node));

				}

			}

			yield return new WaitForSeconds (SearchSettings.TRANSITION_TIME);

		}

		searchData += " Remaining in priority queue: ";
		while (queue.Count != 0) {
			Node<string> node = queue.Dequeue ();
			searchData += node.value + " : " + dic[node] + " | ";
		}

		DLog.Log (searchData);

	}

	public IEnumerator GS(Node<string> currentNode, string value){

		int currentStep = 0;
		string searchData = "";

		bool[] visited = new bool [allNodes.Count];
		SimplePriorityQueue<Node<string>> queue = new SimplePriorityQueue<Node<string>>();

		queue.Enqueue (currentNode, currentNode.heuristic);

		while(queue.Count != 0 && currentNode.value != value){
			currentNode = queue.Dequeue();
			selectedNode = currentNode;

			currentStep += 1;
			searchData += ("[" + currentStep + ". Went to node " + currentNode.value + " with hueristic " + currentNode.heuristic + "] ");


			visited [allNodes.IndexOf (currentNode)] = true;

			List<Node<string>> connected = new List<Node<string>>(currentNode.connectedToNode);
			connected.AddRange (currentNode.connectedFromNode);
			foreach(Node<string> node in connected){

				if (!visited [allNodes.IndexOf (node)] && !queue.Contains(node)) {

					queue.Enqueue (node, node.heuristic);

				}
				
			}

			yield return new WaitForSeconds (SearchSettings.TRANSITION_TIME);
		}

		searchData += " Remaining in priority queue: ";
		while (queue.Count != 0) {
			Node<string> node = queue.Dequeue ();
			searchData += node.value + " : " + node.heuristic + " | ";
		}

		DLog.Log (searchData);

	}

	public override Node<string> createNode(Node<string> headNode, string val){

		Node<string> newNode = base.createNode (headNode, val);

		if (newNode != null) {
			allNodes.Add (newNode);
		}

		return newNode;
	}

	public override void deleteNode(Node<string> headNode){

		allNodes.Remove(headNode);
		base.deleteNode (headNode);

	}

}
