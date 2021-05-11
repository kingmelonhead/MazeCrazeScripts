using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] float heightOfDeath;
    [SerializeField] GameObject player;
    private Rigidbody rb; //Enable player hitbox
    AudioSource[] sounds; //Take in player sound array

    void Awake()
    {
        //Get jump and landing sound effects in an array of sounds
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < heightOfDeath) //If the player falls below a certain Y value
        {
            
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; //Resets velocity to zero on respawn
            player.transform.position = respawnPoint.position; //Sets players new position to the position of the given respawn point
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            Debug.Log("Made Collison with hazard");
            player.transform.position = respawnPoint.position; //Sets players new position to the position of the given respawn point
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; //Resets velocity to zero on respawn
            sounds[3].Play();
        }

    }
}
