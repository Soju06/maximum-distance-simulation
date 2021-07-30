using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cam : MonoBehaviour {
    public float turnSpeed = 3.0f; // 마우스 회전 속도    
    public float moveSpeed = 4.0f; // 이동 속도

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        var x = Input.GetAxis("Mouse Y") * turnSpeed;
        var y = Input.GetAxis("Mouse X") * turnSpeed;
        transform.localRotation *= Quaternion.Euler(0, y, 0);
        GetComponent<Camera>().transform.localRotation *= Quaternion.Euler(-x, 0, 0);
    }
}
