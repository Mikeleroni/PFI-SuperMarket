using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGame : MonoBehaviour
{
    [SerializeField] cutSceneManager cutScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        PlayerPrefs.SetInt("CurrentEscaped", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("CurrentEscaped") == 3)
        {
            cutScene.isDead = true;
        }
    }
}
