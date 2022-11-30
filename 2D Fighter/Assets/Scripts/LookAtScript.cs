using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target.transform);
    }
}
