using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
        private Transform transf;

        private Vector3 movDir = Vector3.zero;
        private float rotSpeed = 300f;
        private float speed = 0.5f;
        private float initialVelocity = 0.0f;
        private float finalVelocity = 5f;
        private float currentVelocity = 0.0f;
        private float accelerationRate = 2f;
        private float decelerationRate = 0.8f;

        // gere chacuns des raycast de detection
        private float distForward = 0f;
        private float distLeft = 0f;
        private float distRight = 0f;
        private float distDiagLeft = 0f;
        private float distDiagRight = 0f;
        // Vecteur de distance max ( normalisation des données )
        private float maxDistance = 30f;

    public float fitness = 0f;
    private Vector3 lastPosition;
    private float distanceTraveled;

    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = gameObject.GetComponent<CharacterController>();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentVelocity += (accelerationRate * Time.deltaTime);
        }
        else
        {
            currentVelocity -= (decelerationRate * Time.deltaTime);
        }
        currentVelocity = Mathf.Clamp(currentVelocity , initialVelocity, finalVelocity);

        movDir = new Vector3(0, 0, currentVelocity);
        movDir *= speed;
        movDir = transform.TransformDirection(movDir);

        controller.Move(movDir);
        transform.Rotate(0 , Input.GetAxis("Horizontal")*rotSpeed*Time.deltaTime, 0);

        InteractRaycast();

        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        fitness += distanceTraveled / 1000;
        fitness -= 0.01f;

    }

    private void Move(Vector3 movDir)
    {
        throw new NotImplementedException();
    }

    void InteractRaycast()
    {
        transf = GetComponent<Transform>();
        Vector3 playerPosition = transform.position;

        Vector3 forwardDirection = transf.forward;
        Vector3 leftDirection = transf.right *-1;
        Vector3 rightDirection = transf.right;
        Vector3 diagLeft = transf.TransformDirection(new Vector3 (maxDistance/5, 0f, maxDistance/5));
        Vector3 diagRight = transf.TransformDirection(new Vector3(-maxDistance / 5, 0f, maxDistance / 5));

        Ray frontRay = new Ray(playerPosition, forwardDirection);
        Ray leftRay = new Ray(playerPosition, leftDirection);
        Ray rightRay = new Ray(playerPosition, rightDirection);
        Ray diagLeftRay = new Ray(playerPosition, diagLeft);
        Ray diagRightRay = new Ray(playerPosition, diagRight);

        RaycastHit hit;
        if (Physics.Raycast(frontRay, out hit , maxDistance) && hit.transform.tag == "Mur")
        {
            distForward = hit.distance;
        }
        if (Physics.Raycast(leftRay, out hit, maxDistance) && hit.transform.tag == "Mur")
        {
            distLeft = hit.distance;
        }
        if (Physics.Raycast(rightRay, out hit, maxDistance) && hit.transform.tag == "Mur")
        {
            distRight = hit.distance;
        }
        if (Physics.Raycast(diagLeftRay, out hit, maxDistance) && hit.transform.tag == "Mur")
        {
            distDiagLeft = hit.distance;
        }
        if (Physics.Raycast(diagRightRay, out hit, maxDistance) && hit.transform.tag == "Mur")
        {
            distDiagRight = hit.distance;
        }
    }
}
