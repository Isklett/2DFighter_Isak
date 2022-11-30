using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationScript : MonoBehaviour
{
    private float Zangle;
    private bool lookingRight;
    //private Quaternion parentRot;

    //private void Start()
    //{
    //    parentRot = GetComponentInParent<Transform>().transform.localRotation;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GetComponentInParent<CharacterScript>().isKeyboard)
        {
            var gamepad = Gamepad.current;
            
            if (gamepad.rightStick.ReadValue().magnitude > 0.02)
            {
                Vector2 temp = gamepad.rightStick.ReadValue();

                if (gamepad.leftStick.ReadValue().x > 0)
                {
                    lookingRight = true;
                }
                else if(gamepad.leftStick.ReadValue().x < 0)
                {
                    lookingRight = false;
                }

                //Clamp
                //if (lookingRight)
                //{
                //    Zangle = Mathf.Clamp(Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg, 20, 180);
                //    print(Zangle);
                //}
                //else
                //{
                //    Zangle = Mathf.Clamp(Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg * -1, 20, 180);
                //    print(Zangle);
                //}

                //Abs
                //if (lookingRight)
                //{
                //    Zangle = Mathf.Abs(Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg);
                //    print(Zangle);
                //}
                //else
                //{
                //    Zangle = Mathf.Abs(Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg * -1);
                //    print(Zangle);
                //}

                //If
                if (lookingRight)
                {
                    Zangle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;
                    //print(Zangle);
                }
                else
                {
                    Zangle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg * -1;
                    //print(Zangle);
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



            transform.localRotation = Quaternion.Euler(Zangle - 90, 0, 0);
        }
        else
        {

        }

    }
}
