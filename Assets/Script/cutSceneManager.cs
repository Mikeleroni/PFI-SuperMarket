using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class cutSceneManager : MonoBehaviour
{
    // SOURCES: https://forum.unity.com/threads/free-basic-camera-fade-in-script.509423/ 
    // https://www.reddit.com/r/UnityHelp/comments/y82yc6/how_to_move_a_cinemachine_camera_with_script_fov/
    public bool isDead;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] VideoPlayer video;
    CinemachineTransposer transposer;
    CinemachineComposer composer;
    
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;

    float elapsedTime = 0;
    float zoomSpeedFinish = 1f;

    private void Awake()
    {
        isDead = false;
        video.enabled = false;
        transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        composer = cam.GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 90, zoomSpeedFinish);
        isDead = true;
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, new Vector3(-5, 3, 2), 0.003f);
        composer.m_TrackedObjectOffset = Vector3.Lerp(composer.m_TrackedObjectOffset, new Vector3(1200, 0, 25), 0.003f);
        StartCoroutine(PlayVideo());
        StartCoroutine(ChangeScene());
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(3f);
        video.enabled = true;
        Reset();
    }
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(2);
    }

    void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;
    }

    void OnGUI()
    {
        if (isDead)
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
