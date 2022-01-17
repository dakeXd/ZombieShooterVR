using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHits;
    private float hitsLeft;
    public float deathSpeed;

    public GameObject canvasMuerto;

    public Image image;

    public Text textoFinal;
    public Text count;

    private void Start()
    {
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

        //FadeIn();
        canvasMuerto.SetActive(true);
        changeTextFinal();

        //yield return null;

        Debug.Log("Activar fin");
        //SceneManager.LoadScene(0);
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

}
