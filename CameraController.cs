using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    private Vector3 offset;
    public bool playerInSight;
    private Camera cam;
    public float startingFOV;
    public float currentFOV;
    private float minFOV = 1;
    private float maxFOV = 10;

    // Start is called before the first frame update
    void Start()
    {
        playerInSight = true;
        cam = GetComponent<Camera>();
        startingFOV = cam.fieldOfView;
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentFOV = cam.fieldOfView;

        transform.position = player.transform.position + offset;

        RaycastHit hit;
        Ray playerRay = new Ray(transform.position, Vector3.forward);

        if(Physics.Raycast(playerRay, out hit))
        {
            if(hit.collider.CompareTag("Player"))
            {
                playerInSight = true;
            }
            else
            {
                playerInSight = false;
            }
        }

        if(playerInSight == false)
        {
            currentFOV -= 1;
        }
        else
        {
            currentFOV = startingFOV;
        }

        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
        cam.fieldOfView = currentFOV;
    }
}