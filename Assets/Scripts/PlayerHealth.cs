using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHits;
    private float hitsLeft;
    public float deathSpeed;

    public GameObject canvasMuerto;
    public GameObject panel;

    public Image image;

    public Text textoFinal;
    public TMP_Text count;

    public  Animator animatorPanel;

    private void Start()
    {
        animatorPanel = panel.GetComponent<Animator>();
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        hitsLeft = maxHits;
    }
    
    public bool Damage()
    {
        
        hitsLeft--;
        Debug.Log("Daño Recibido, vida actual " + hitsLeft);
        if (hitsLeft > 0) return false;
        StartCoroutine("Morirse");
        return true;

    }

    IEnumerator Morirse()
    {
        for (float i = transform.rotation.eulerAngles.x; i < 90; i += Time.deltaTime*deathSpeed)
        {
            Debug.Log(i);
            transform.rotation = Quaternion.Euler(new Vector3(i, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            
            yield return null;
        }

        canvasMuerto.SetActive(true);
        changeTextFinal();

        StartCoroutine(WaitToFadeOut());

        if (animatorPanel != null)
        {
            bool fadeOut = animatorPanel.GetBool("Dead");
            animatorPanel.SetBool("Dead", !fadeOut);
        }

        StartCoroutine(Reintentar());

        Debug.Log("Activar fin");
    }

    public void FadeIn()
    {
        float targetAlpha = 1.0f;
        Color curColor = image.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            Debug.Log(image.material.color.a);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, 0.5f * Time.deltaTime);
            image.color = curColor;
            
        }
    }

    public void changeTextFinal()
    {
        textoFinal.text = "Has matado: " + count.text + " zombies";
    }

    IEnumerator WaitToFadeOut()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator Reintentar()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("SampleScene");
    }
}
