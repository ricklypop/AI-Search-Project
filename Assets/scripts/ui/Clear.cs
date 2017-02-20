using UnityEngine;
using System.Collections;

public class Clear : MonoBehaviour {
	public DemoWeightedGraph graph;

	public void clear(){
		graph.clear (DemoWeightedGraph.selectedNode, true);
	}
	
}
