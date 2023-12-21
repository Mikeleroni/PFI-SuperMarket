using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class HitByBaton : MonoBehaviour
{
    [SerializeField] Animator animatorFrapper;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    private void Update()
    {
        Frapper();
        // Position et direction du raycast
        Vector3 raycastOrigin = transform.position; // position de l'objet
        Vector3 raycastDirection = transform.forward; // direction vers l'avant de l'objet

        // Longueur du raycast
        float raycastLength = 2.0f;

        // Raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, raycastLength))
        {
            // Si le raycast frappe quelque chose
            Debug.DrawLine(raycastOrigin, hitInfo.point, Color.red);
            if (hitInfo.collider.gameObject.tag == "Voleur" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Vous avez tuer une " + hitInfo.collider.gameObject.tag);
                //hitInfo.collider.gameObject.SetActive(false);
            }
        }
        else
        {
            // Si le raycast ne frappe rien
            Vector3 endPosition = raycastOrigin + raycastDirection * raycastLength;
            Debug.DrawLine(raycastOrigin, endPosition, Color.green);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Voleur")) 
        {
          //  other.tag = "Untagged";
            Animator animator = other.gameObject.GetComponent<Animator>();
            StealerComponent stealer = other.gameObject.GetComponent<StealerComponent>();
            CompulsiveStealer compulsive = other.gameObject.GetComponent<CompulsiveStealer>();
            NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
            stealer.enabled = false;
            compulsive.enabled = false;
            agent.enabled = false;   
            animator.SetBool("Dead", true);
        }
        
    }
    void Frapper()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            animatorFrapper.SetBool("Frapper", true);

            if (!source.isPlaying)
            {
                source.PlayOneShot(clip);
            }

        }
        else
        {
            animatorFrapper.SetBool("Frapper", false);
        }

    }
}
