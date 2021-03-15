using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Manager : MonoBehaviour {

	List<Vector3> newVectors;

	void Start() {
		newVectors = new List<Vector3> ();
	}

	void Update () {
	
		foreach (GameObject grass in GameObject.FindGameObjectsWithTag ("Grass")) {
			//slowly rotate the grass
			Debug.Log ("Rotating Grass");
			Vector3 rotation = grass.transform.localEulerAngles;
			rotation.z = Mathf.Sin (Time.time)*0.3f;
			grass.transform.localEulerAngles = rotation;

			//for fun, make and copy of this object and destroy it
			GameObject newObject = Instantiate(grass);
			Destroy (newObject);

			//for fun, slightly randomize the color of each piece of grass
			grass.GetComponent<MeshRenderer>().material.color =  new Color(1.0f-Random.value*0.1f,1.0f-Random.value*0.1f,1.0f,1.0f);

			//for fun, make a new vector and add it to a list
			Vector3 newVector = new Vector3 ();
			newVectors.Add (newVector);

		}


	}
}
