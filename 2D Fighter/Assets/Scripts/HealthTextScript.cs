using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTextScript : MonoBehaviour
{
    public GameObject player1;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = System.Math.Round(player1.GetComponent<CharacterScript>().health, 1).ToString() + "%";
    }
}
