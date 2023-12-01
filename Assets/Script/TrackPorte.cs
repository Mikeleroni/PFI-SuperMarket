using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TrackPorte : MonoBehaviour
{
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;
    void Start()
    {
    }
    private void OnTriggerEnter()
    {
        Debug.Log("L'objet est dans le trigger !");
        animator1.SetBool("Ouvrir", true);
        animator2.SetBool("Ouvrir", true);
    }

    // appelée chaque frame pendant que l'objet est dans le trigger
    //private void OnTriggerStay()
    //{
    //    Debug.Log("L'objet reste dans le trigger !");
        
    //}

    // appelée lorsqu'un autre collider sort du trigger
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("L'objet a quitté le trigger !");
        animator1.SetBool("Ouvrir", false);
        animator2.SetBool("Ouvrir", false);
    }

}
