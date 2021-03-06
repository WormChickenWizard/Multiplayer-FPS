﻿/// <summary>
/// Health.cs
/// Author: MutantGopher
/// This is a sample health script.  If you use a different script for health,
/// make sure that it is called "Health".  If it is not, you may need to edit code
/// referencing the Health component from other scripts
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Health : NetworkBehaviour
{
	public bool canDie = true;					// Whether or not this health can die
	
	public float startingHealth = 100.0f;		// The amount of health to start with
	public float maxHealth = 100.0f;			// The maximum amount of health
    [SyncVar]
	public float currentHealth;				    // The current ammount of health

	public bool replaceWhenDead = false;		// Whether or not a dead replacement should be instantiated.  (Useful for breaking/shattering/exploding effects)
	public GameObject deadReplacement;			// The prefab to instantiate when this GameObject dies
	public bool makeExplosion = false;			// Whether or not an explosion prefab should be instantiated
	public GameObject explosion;				// The explosion prefab to be instantiated

	public bool isPlayer = false;				// Whether or not this health is the player
	public GameObject deathCam;					// The camera to activate when the player dies

    [SyncVar]
	public bool dead = false;					// Used to make sure the Die() function isn't called twice
    public GameObject regularPlayer;
    NetworkStartPosition[] spawnPoints;
    GameObject tempDeadReplacement = null;
    FirstPersonCharacter fpc;
    CapsuleCollider charCollider;
    MeshRenderer charRenderer;
    public GameObject visor;
    public MouseRotator mainRotator;
    public MouseRotator pivotRotator;

	// Use this for initialization
	void Start()
	{
		// Initialize the currentHealth variable to the value specified by the user in startingHealth
		currentHealth = startingHealth;
	}

    private void Awake()
    {
        fpc = GetComponent<FirstPersonCharacter>();
        charCollider = GetComponent<CapsuleCollider>();
        charRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // If the health runs out, then Die.
        if (currentHealth <= 0 && !dead && canDie && isLocalPlayer)
            Die();

        // Make sure that the health never exceeds the maximum health
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if(tempDeadReplacement == null && dead && isLocalPlayer)
        {
            CmdRespawn();
        }
    }


    public void ChangeHealth(float amount)
	{
		// Change the health by the amount specified in the amount variable
		currentHealth += amount;
	}

	public void Die()
	{
        if (dead) return;
        //"kills" the player globally
        CmdDie();
        if (isPlayer && deadReplacement != null && tempDeadReplacement == null)
        {
            tempDeadReplacement = Instantiate(deadReplacement,regularPlayer.transform);
            tempDeadReplacement.transform.parent = null;
        }
	}

    [Command]
    public void CmdDie()
    {
        if (dead) return;
		dead = true;
        Debug.Log("Disabling killed reg_player");
        ActivePlayer(false);
    }

    [Command]
    public void CmdRespawn()
    {
        if (!dead) return;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        System.Random rand = new System.Random();
        int i = rand.Next(spawnPoints.Length);

        ActivePlayer(true);

        dead = false;
        currentHealth = startingHealth;

        if (isLocalPlayer)
        {
            regularPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        RpcVelocity();

        Debug.Log(spawnPoints[i].transform.rotation.eulerAngles);


        RpcMoveNetworkPlayer(spawnPoints[i].transform.position, spawnPoints[i].transform.localRotation.eulerAngles.y);
    }

    public void ActivePlayer(bool isActive)
    {
        if(isLocalPlayer)
            fpc.enabled = isActive;
        charCollider.enabled = isActive; // character collision
        //character visability
        if(!isLocalPlayer)
        {
            charRenderer.enabled = isActive;
            visor.SetActive(isActive);
        }
        GetComponent<WeaponSystem>().ShowCurrentWeapon(isActive);
        RpcActivePlayer(isActive);
    }

    [ClientRpc]
    public void RpcActivePlayer(bool isActive)
    {
        if(isLocalPlayer)
            fpc.enabled = isActive;
        charCollider.enabled = isActive; // character collision
        //character visability
        if (!isLocalPlayer)
        {
            charRenderer.enabled = isActive;
            visor.SetActive(isActive);
        }
        GetComponent<WeaponSystem>().ShowCurrentWeapon(isActive);
    }

    [ClientRpc]
    public void RpcVelocity()
    {
        if (isLocalPlayer)
            regularPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    [ClientRpc]
    public void RpcMoveNetworkPlayer(Vector3 v3, float rotation)
    {
        if(isLocalPlayer)
        {
            regularPlayer.transform.position = v3;
            StartCoroutine(mainRotator.SetPosition(Quaternion.Euler(0, rotation, 0)));
            StartCoroutine(pivotRotator.SetPosition(Quaternion.identity));
        }

    }
}
