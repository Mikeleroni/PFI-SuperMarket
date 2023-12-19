using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage : MonoBehaviour
{
    [SerializeField] float vitesse = 50;
    [SerializeField] float forceJump = 5;
    [SerializeField] float senssibiliterCamera = 1;
    [SerializeField] float gravity = 1;
    [SerializeField] Animator animatorFrapper;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    Vector3 rotationCamera = Vector3.zero;
    Vector3 jump = Vector3.zero;
    CharacterController cc;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
        RotationCamera();
        
    }
    
    void RotationCamera()
    {
        rotationCamera += new Vector3(-Input.GetAxis("Mouse Y") * senssibiliterCamera * Time.deltaTime,
                                    Input.GetAxis("Mouse X") * senssibiliterCamera * Time.deltaTime, 0);

        rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70, 70);

        cam.transform.rotation = Quaternion.Euler(rotationCamera);
    }
    void Deplacement()
    {
        Vector3 direction = cam.transform.forward * Input.GetAxis("Vertical") + cam.transform.right * Input.GetAxis("Horizontal");

        if (direction.magnitude > 0)
        {
            direction = new Vector3(direction.x, 0, direction.z);
            //direction.y = 0;

            direction.Normalize();
            //direction = direction.normalized;

            direction = direction * (-Input.GetAxis("Sprint") * 0.5f + 1);


        }
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = Vector3.up * forceJump;
            }
            jump.y = Mathf.Max(-1, jump.y);
        }
        else
        {
            jump -= Vector3.up * Time.deltaTime * gravity;
        }

        cc.Move((direction * vitesse + jump) * Time.deltaTime);
    }
}
