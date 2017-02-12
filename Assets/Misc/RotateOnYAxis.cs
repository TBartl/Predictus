using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnYAxis : MonoBehaviour {
    public float amount;
    public float speed;

    void Update() {
        float val = Mathf.Sin(Time.timeSinceLevelLoad * speed);
        this.transform.rotation = Quaternion.Euler(0, amount * val, 0);
    }
}
