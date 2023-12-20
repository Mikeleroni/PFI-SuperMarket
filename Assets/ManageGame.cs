using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(4);
        }
    }
}
