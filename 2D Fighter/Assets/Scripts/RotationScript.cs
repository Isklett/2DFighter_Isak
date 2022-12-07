using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//ANVÄNDS INTE I SPELET ATM
public class RotationScript : MonoBehaviour
{
    private float Zangle;
    private bool lookingRight;
    void FixedUpdate()
    {
        if (!GetComponentInParent<CharacterScript>().isKeyboard)
        {
            var gamepad = Gamepad.current;

            if (gamepad.leftStick.ReadValue().x > 0)
            {
                lookingRight = true;
            }
            else if (gamepad.leftStick.ReadValue().x < 0)
            {
                lookingRight = false;
            }

            //Kollar hur mycket höger spak är roterad och roterar karaktären efter det
            if (gamepad.rightStick.ReadValue().magnitude > 0.02)
            {
                Vector2 temp = gamepad.rightStick.ReadValue();

                //If
                if (GetComponentInParent<CharacterScript>().rightStickTurn)
                {
                    if (lookingRight)
                    {
                        Zangle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        Zangle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg * -1;
                    }

                    if (Zangle <= 20 && Zangle >= -100)
                    {
                        Zangle = 20;
                    }
                    else if (Zangle < -100)
                    {
                        Zangle = 180;
                    }
                }
                else
                {
                    if (temp.y > 0.8f)
                    {
                        Zangle = -90;
                    }
                    
                }
            }
            else
            {
                if (lookingRight)
                {
                    Zangle = 0;
                }
                else
                {
                    Zangle = 0;
                }
            }


            if (GetComponentInParent<CharacterScript>().rightStickTurn)
            {
                transform.localRotation = Quaternion.Euler(Zangle - 90, 0, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(Zangle, 0, 0);
            }
        }
    }
}
