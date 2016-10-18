﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class LedgeGrab : MonoBehaviour
{

    private GameObject character = null;
    private bool onLedge = false;
    private GameObject ledgeCollider = null;
    private GameObject ledgeParent = null;
    private GameObject mobileJoystick = null;
    private Joystick joystickScript = null;
    private Rigidbody characterRigidbody = null;
    private ThirdPersonUserControl thirdPersonUserControl = null;
    public bool automaticClimbUp = false;

    // Use this for initialization
    void Start()
    {

        ledgeParent = gameObject.transform.parent.gameObject;
        ledgeCollider = ledgeParent.transform.GetChild(1).gameObject;
        mobileJoystick = GameObject.Find("MobileJoystick");
        joystickScript = mobileJoystick.GetComponent<Joystick>();

    }

    // Update is called once per frame
    void Update()
    {
        //if on ledge = true and player presses jump, teleport to top of ledge
        if (onLedge && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            MoveCharacterToTop();
            ResetCharacter();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            character = collider.gameObject;
            characterRigidbody = character.GetComponent<Rigidbody>();
            thirdPersonUserControl = character.GetComponent<ThirdPersonUserControl>();
        }

        if (character != null)
        {
            if (!ThirdPersonCharacter.m_IsGrounded)
            {
                GrabLedge();
            }
            else
            {
                GrabLedgeGrounded();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        ResetCharacter();
        onLedge = false;
        thirdPersonUserControl.isHanging = false;
    }


    //the aim is to have a ledge pull up animation
    //stop the character
    //stop the player input
    //play animation
    //give back input
    public void GrabLedge()
    {
        FreezeCharacter();
        LookAtLedge(ledgeCollider.transform);
        onLedge = true;
    }

    public void GrabLedgeGrounded()
    {
        LookAtLedge(ledgeCollider.transform);
        onLedge = true;
        
        if(automaticClimbUp)
        {
            MoveCharacterToTop();
        }
    }

    public void FreezeCharacter()
    {
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.useGravity = false;
        characterRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        joystickScript.SetAxis(Joystick.AxisOption.OnlyHorizontal);
        thirdPersonUserControl.isHanging = true;
    }

    public void ResetCharacter()
    {
        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        joystickScript.SetAxis(Joystick.AxisOption.Both);
        character.GetComponent<ThirdPersonUserControl>().enabled = true;
        characterRigidbody.useGravity = true;
    }

    public void LookAtLedge(Transform ledgeTransform)
    {
        character.transform.LookAt(ledgeTransform.transform);
        var rotation = character.transform.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        rotation.y = this.transform.rotation.eulerAngles.y;
        character.transform.rotation = Quaternion.Euler(rotation);
    }

    public void MoveCharacterToTop()
    {
        //stops player from making further input
        thirdPersonUserControl.enabled = false;
        ////teleports character to top of the box
        character.transform.position = Vector3.Lerp(character.transform.position, ledgeCollider.transform.position, 5);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 0.5f);
    }


}