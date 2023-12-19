using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutSceneManager : MonoBehaviour
{
    //SOURCE: https://forum.unity.com/threads/free-basic-camera-fade-in-script.509423/ 
    public bool isArrested;
    [SerializeField] CinemachineVirtualCamera cam; 
    CinemachineTransposer transposer;
    
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;

    float elapsedTime = 0;
    float zoomSpeedFinish = 1f;

    private void Awake()
    {
        isArrested = false;
        transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        if(isArrested)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 90, zoomSpeedFinish);

            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, new Vector3(-5, 5, 5), 0.01f);
            StartCoroutine(returnToNormal());
        }
    }

    IEnumerator returnToNormal()
    {
        yield return new WaitForSeconds(2f);
        Reset();
    }

    void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;

        transposer.m_FollowOffset = new Vector3(0,0,-10);
        cam.m_Lens.FieldOfView = 60;
    }

    void OnGUI()
    {
        if (isArrested)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= 5f)
            {
                if (_done) return;
                if (_texture == null) _texture = new Texture2D(1, 1);

                _texture.SetPixel(0, 0, new Color(0, 0, 0, _alpha));
                _texture.Apply();

                _time += Time.deltaTime;
                _alpha = FadeCurve.Evaluate(_time);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

                if (_alpha <= 0) _done = true;
            }
        }
        
    }
}
