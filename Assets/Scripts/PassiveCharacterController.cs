﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCharacterController : MonoBehaviour
{
    public bool useAI = false;
    public bool slowMotion = false;

    Rigidbody rb;

    [Header("Speed")]
    public float characterSpeedNormal = 5f;
    public float characterSpeedSlowed = 2f;
    float currentCharacterSpeed;

    [Header("Jump")]
    public float jumpForce = 5.5f;
    public float doubleJumpForce = 8f;

    [Header("Shrink")]
    public float normalScale = 1f;
    public float shrinkScale = 0.8f;
    public float doubleShrinkScale = 0.6f;

    [Header("Dash")]
    public float dashForce = 5.5f;
    public float doubleDashForce = 8f;

    [Header("Smash")]
    private int smashCounter = 0;
    private GameObject[] smashObs;
    private MeshRenderer meshRenderer;
    public Material[] smashMaterials; //[0] normal, [1] single dash, [2] double dash

    [Header("DeathTimer")]
    public float deathTimer = 3;
    private float timeDeath = 0;
    private BreakScriptRef breakscript;
    private Vector3 old_position;
    private float timeCount_UpdatePosition = 1;
    private float timeLeft_UpdatePosition = 0;

    private BreakScript collider_breakscript = null;

    void Start()
    {
        gameObject.transform.Rotate(new Vector3(0, 1, 0), 180);
        breakscript = GetComponent<BreakScriptRef>();
        rb = GetComponent<Rigidbody>();
        currentCharacterSpeed = characterSpeedNormal;
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        old_position = transform.position + new Vector3(-999, 0, 0);
        timeLeft_UpdatePosition = timeCount_UpdatePosition;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ResetCharacter()
    {
        GameManager.instance.Death();

        old_position = transform.position + new Vector3(-999, 0, 0);

        GameManager.instance.inputManager.ResetPlayerInputs();
        GameManager.instance.inputManager.DisablePlayerInput();

        if (GameManager.instance.levelManager != null)
        {
            GameManager.instance.UpdatePlayerStartingPosition(GameManager.instance.levelManager.placeNewStartingArea());
        }

        transform.position = GameManager.instance.characterStartingPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ResetSize();
        ResetSmash();
        ResetDash();

        if (slowMotion)
            GameManager.instance.pcharacter.SlowDown(false);
        ResetSmashObstacles();

        timeDeath = 0;
        if (breakscript != null)
            breakscript.Revive();
    }

    private void Update()
    {
        if (breakscript != null && !breakscript.isAlive())
        {
            timeDeath += Time.deltaTime;
            if (timeDeath > deathTimer) ResetCharacter();
            return;
        }

        if (this.transform.position.y < -6)
        {
            ResetCharacter();
            return;
        }

        if (useAI) return;

        InputManager.SingleInputType type1 = InputManager.SingleInputType.None;
        InputManager.SingleInputType type2 = InputManager.SingleInputType.None;

        if (Input.GetKey(KeyCode.W)) type1 = InputManager.SingleInputType.Jump;
        if (Input.GetKey(KeyCode.S)) type1 = InputManager.SingleInputType.Shrink;
        if (Input.GetKey(KeyCode.D)) type1 = InputManager.SingleInputType.Dash;
        if (Input.GetKey(KeyCode.A)) type1 = InputManager.SingleInputType.Smash;

        if (Input.GetKey(KeyCode.UpArrow)) type2 = InputManager.SingleInputType.Jump;
        if (Input.GetKey(KeyCode.DownArrow)) type2 = InputManager.SingleInputType.Shrink;
        if (Input.GetKey(KeyCode.RightArrow)) type2 = InputManager.SingleInputType.Dash;
        if (Input.GetKey(KeyCode.LeftArrow)) type2 = InputManager.SingleInputType.Smash;

        InputManager mngr = GameManager.instance.inputManager;
        if (type1 != InputManager.SingleInputType.None) mngr.UpdatePlayerInput(type1, 0);
        if (type2 != InputManager.SingleInputType.None) mngr.UpdatePlayerInput(type2, 1);
    }

    public void SetInputType(int playerNumber, InputManager.SingleInputType type)
    {
        if (type != InputManager.SingleInputType.None)
        {
            if (GameManager.instance.inputManager.UpdatePlayerInput(type, playerNumber))
            {
                GameManager.instance.UpdateButtonPressedUI(playerNumber, type);
            }
        }
    }

    void FixedUpdate()
    {
        if (breakscript != null)
        {
            if (!breakscript.isAlive()) return;

            //timeLeft_UpdatePosition -= Time.deltaTime;
            if (timeLeft_UpdatePosition <= 0)
            {
                timeLeft_UpdatePosition = timeCount_UpdatePosition;
                if (transform.position.x <= (old_position.x)) breakscript.PlayDeath();
                old_position = transform.position;
            }
            else timeLeft_UpdatePosition--;
        }

        //Move character
        rb.velocity = new Vector3(currentCharacterSpeed, rb.velocity.y, rb.velocity.z);
    }

    public void TriggerEnter(InputManager.SingleInputType type1, InputManager.SingleInputType type2)
    {
        //Enable Player Input
        GameManager.instance.inputManager.EnablePlayerInput();
        GameManager.instance.ShowPressNowText();
        if (slowMotion) GameManager.instance.pcharacter.SlowDown(true);

        if (useAI)
        {
            InputManager mngr = GameManager.instance.inputManager;
            mngr.UpdatePlayerInput(type1, 0);
            mngr.UpdatePlayerInput(type2, 1);
        }
    }

    public void TriggerExit(InputManager.SingleInputType Type1, InputManager.SingleInputType Type2)
    {
        //Disable Player Input
        GameManager.instance.inputManager.DisablePlayerInput();

        if (GameManager.instance.inputManager.getCurrentInput() == InputManager.CombinedInputType.None)
            GameManager.instance.ShowNoInputText();

        if (slowMotion) GameManager.instance.pcharacter.SlowDown(false);
        if (collider_breakscript != null)
        {
            collider_breakscript.cleanUp();
            collider_breakscript = null;
        }
    }

    public void SlowDown(bool slowed)
    {
        currentCharacterSpeed = slowed ? characterSpeedSlowed : characterSpeedNormal;
    }

    public void Jump(bool doubleJump)
    {
        var jumpFactor = doubleJump ? doubleJumpForce : jumpForce;

        rb.velocity = new Vector3(0, jumpFactor, 0);
    }

    public void Shrink(bool doubleShrink)
    {
        var scale = doubleShrink ? doubleShrinkScale : shrinkScale;
        transform.localScale = Vector3.one * scale;
    }

    public void Smash(bool doubleSmash)
    {
        smashCounter++;
        if (doubleSmash)
        {
            smashCounter++;
            //matHelper = 1;
            meshRenderer.material = smashMaterials[2];
        }
        else
        {
            //matHelper = 0;
            meshRenderer.material = smashMaterials[1];
        }
    }

    public void Dash(bool doubleDash)
    {
        GameManager.instance.pcharacter.GetComponent<TrailRenderer>().enabled = true;
        var dashFactor = doubleDash ? doubleDashForce : dashForce;
        var dashHeight = doubleDash ? 3f : 1.5f;
        currentCharacterSpeed += dashFactor;
        rb.velocity = new Vector3(0, dashHeight, 0);
    }

    public void ResetSize()
    {
        transform.localScale = Vector3.one * normalScale;
    }

    public void ResetSmash()
    {
        smashCounter = 0;

        meshRenderer.material = smashMaterials[0];
    }

    public void ResetDash()
    {
        GameManager.instance.pcharacter.GetComponent<TrailRenderer>().enabled = false;
        currentCharacterSpeed = characterSpeedNormal;
        rb.velocity = Vector3.zero;
    }

    public void ResetSmashObstacles()
    {
        if (smashObs == null)
        {
            smashObs = GameObject.FindGameObjectsWithTag("Smashable");
        }
        foreach (GameObject toBeRespawned in smashObs)
        {
            if (toBeRespawned.GetComponent<BoxCollider>() != null) toBeRespawned.GetComponent<BoxCollider>().enabled = true;
            toBeRespawned.GetComponent<MeshRenderer>().enabled = true;
            GameObject remains = GameObject.Find(toBeRespawned.name + " Remains");
            if (remains != null) remains.GetComponent<BreakScript>().cleanUp();
        }
        smashObs = null;
    }

    private IEnumerator SmashAction(Collider collider, GameObject remains)
    {
        if (GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        collider_breakscript = remains.GetComponent<BreakScript>();
        collider_breakscript.transform.position = collider.transform.position;
        collider_breakscript.transform.localScale = collider.transform.localScale;
        collider_breakscript.transform.rotation = collider.transform.rotation;
        collider_breakscript.Explode(true, collider.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.tag == "Smashable" && smashCounter > 0)
        {
            collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            GameObject remains = GameObject.Find(collider.name + " Remains");
            if (remains != null) StartCoroutine(SmashAction(collider, remains));
            smashCounter--;
        }
        else
        {
            //string materialname = collision.collider.GetComponent<MeshRenderer>().materials[0].name;
            //if (!materialname.Contains("Ground"))
            //{
            //    //Vector3 direction = (collision.transform.position - transform.position).normalized;
            //    if (rb.velocity.x <= currentCharacterSpeed / 4)
            //    {
            //       if (breakscript != null) breakscript.PlayDeath();
            //    }
            //}
        }
    }
}
