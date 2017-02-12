using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is useful if you want an object to start disabled, but still be found
// by any sort of getcomponent searches earlier
public class DisableOnStart : MonoBehaviour {
	void Start () {
        this.gameObject.SetActive(false);
	}
}
