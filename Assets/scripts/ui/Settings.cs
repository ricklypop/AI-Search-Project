using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
	public Slider sliderX;
	public Slider sliderY;
	public Slider sliderDegen;
	public Slider sliderTransition;

	void Update () {

		SearchSettings.NODE_DISTANCE_FACTOR_X = sliderX.value;
		SearchSettings.NODE_DISTANCE_FACTOR_Y = sliderY.value;
		SearchSettings.NODE_TREE_DEGEN_FACTOR = sliderDegen.value;
		SearchSettings.TRANSITION_TIME = sliderTransition.value;

	}
}
