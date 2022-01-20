using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ReintentarMenu : MonoBehaviour
{
    public Image imgGaze;
    public UnityEvent GVRClick;
    public float totalTime = 2;
    public bool gvrStatus;
    private float gvrTimer;

    // Start is called before the first frame update
    void Start()
    {
        imgGaze.fillAmount = 0;
        gvrStatus = false;
        gvrTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer / totalTime;
        }

        if (gvrTimer > totalTime)
        {
            GVRClick.Invoke();
        }
    }
    public void GVROn()
    {
        gvrStatus = true;
    }

    public void GVROff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
    }

    public void Reintentar()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
