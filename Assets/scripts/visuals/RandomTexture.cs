using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTexture : MonoBehaviour {
	public List<Material> textures = new List<Material>();

	void Start () {
		GetComponent<MeshRenderer>().material = textures[ Random.Range(0, textures.Count)];
	}
}
