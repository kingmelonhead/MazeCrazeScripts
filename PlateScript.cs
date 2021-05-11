using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlateScript : MonoBehaviour
{
    // Door object that it will raise once both plates of the pair are activated
    [SerializeField]
    GameObject door;
    // Plate that is paired with the plate that this script is attached to
    [SerializeField]
    GameObject pairedPlate;
    [SerializeField]
    GameObject plate;
    [SerializeField]  AudioSource confirmSound;

    private PhotonView PV;

    // Private boolean that checks to see if the plate itself is activated by the player
    private bool isActivated = false;

    private void Start() // Needed Method for Destroy() to work
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    private void activatePlate2()
    {
        isActivated = true;
        Debug.Log("Activated Plate from Other Player");
    }

    [PunRPC]
    private void deactivatePlate2()
    {
        isActivated = false;
        Debug.Log("Deactivated Plate from Other Player");
    }

    [PunRPC]
    private void liftDoor()
    {
        Debug.Log("Lifted Door");
        door.transform.position += new Vector3(0, 1.625f, 0); // Raise the door by a set amount
        Destroy(plate.GetComponent<PlateScript>()); // Once Door is raised, delete scripts off of object as to not allow multiple activations
        Destroy(pairedPlate.GetComponent<PlateScript>());
        confirmSound.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        PV.RPC("activatePlate2", RpcTarget.All);
        isActivated = true;
        
        if (isActivated && pairedPlate.GetComponent<PlateScript>().isActivated)
        {
            PV.RPC("liftDoor", RpcTarget.All);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isActivated = false; // Set as false on player leaving object
        PV.RPC("deactivatePlate2", RpcTarget.All);
    }
}
