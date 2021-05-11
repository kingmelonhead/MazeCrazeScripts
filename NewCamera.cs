using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private GameObject player = null;

    //commented out attributes that are never used
    public bool camInWall;
    private Camera cam;
    public float startingFOV;
    public float currentFOV;
    //private float minFOV = 1;
    //private float maxFOV = 60;
    //private float zoomRate = 3;

    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    public Transform lookAt;
    public Transform Player;

    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivity = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;

        camInWall = false;
        cam = GetComponent<Camera>();
        startingFOV = cam.fieldOfView;
        offset = transform.position - player.transform.position;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = player.transform.position + offset;

        currentFOV = cam.fieldOfView;

        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);



    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            camInWall = true;
        }

    }
}