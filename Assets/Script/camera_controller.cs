using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public Transform obj;   // ตัวละครหรือวัตถุที่ให้กล้องตาม

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            obj.position.x,
            transform.position.y,
            transform.position.z
        );
    }
}
