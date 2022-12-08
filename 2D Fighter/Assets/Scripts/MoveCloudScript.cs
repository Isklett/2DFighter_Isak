using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloudScript : MonoBehaviour
{

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.05f, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > 200)
        {
            gameObject.transform.position = new Vector3(-250, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
