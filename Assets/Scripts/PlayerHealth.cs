using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHits;
    private float hitsLeft;
    public float deathSpeed;

    public GameObject canvasMuerto;
    public GameObject panel;
    public GameObject infoUsuario;

    public Image image;

    public TMP_Text textoFinal;
    public TMP_Text count;
    public TMP_Text vidasTexto;

    public  Animator animatorPanel;

    public PostProcessVolume volume;
    private Vignette vignette;

    private void Start()
    {
        volume.profile.TryGetSettings(out vignette);

        vidasTexto.text = "3/3";
        animatorPanel = panel.GetComponent<Animator>();
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        hitsLeft = maxHits;
    }
    
    public bool Damage()
    {
        
        hitsLeft--;

        StartCoroutine(VignetteDaño());
        StartCoroutine(Esperar(2));
        StartCoroutine(DeshacerVignette());

        vidasTexto.text = hitsLeft + "/3";
        Debug.Log("Daño Recibido, vida actual " + hitsLeft);
        if (hitsLeft > 0) return false;
        StartCoroutine("Morirse");
        return true;

    }

    IEnumerator Morirse()
    {
        infoUsuario.SetActive(false);

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
        yield return new WaitForSeconds(15f);
    }

    IEnumerator Reintentar()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator VignetteDaño()
    {
        while (vignette.intensity.value < .70f)
        {
            vignette.intensity.value += .05f;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);
    }

    IEnumerator Esperar(float segundos)
    {
        yield return new WaitForSeconds(segundos);
    }

    IEnumerator DeshacerVignette()
    {
        while (vignette.intensity.value > 0f)
        {
            vignette.intensity.value -= .005f;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);
    }
}
