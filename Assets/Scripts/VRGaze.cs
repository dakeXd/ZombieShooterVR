using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VRGaze : MonoBehaviour
{
    public Image imgGaze;
    public float totalTime = 2;
    public bool gvrStatus;
    private float gvrTimer;

    [SerializeField] private Text enemies;

    private int contadorEnemigos;

    //Puntero que sale de la pistola.
    public GvrReticlePointer pointer; 

    void Start()
    {
        contadorEnemigos = 0;
        enemies.text = contadorEnemigos.ToString();
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

        
        if (imgGaze.fillAmount >= 1)
        {

            RaycastResult result = pointer.CurrentRaycastResult;
            if (result.gameObject.CompareTag("TeleportPoint"))
            {
                result.gameObject.GetComponent<Teleport>().TeleportPlayer();
                GVROff();
            }
            else if (result.gameObject.CompareTag("Enemy"))
            {
                SimpleShoot shooter = gameObject.GetComponentInChildren<SimpleShoot>();
                shooter.StartShoot();
                bool eliminado = result.gameObject.GetComponent<EnemyHealth>().getShooted(shooter.shotDamage);
                GVROff();
                if (!eliminado)
                {
                    GVROn();
                }
                else
                {
                    contadorEnemigos++;
                    enemies.text = contadorEnemigos.ToString();
                }
                    
            }
            else if (result.gameObject.CompareTag("TextUI"))
            {
               bool terminado =  result.gameObject.GetComponent<TextFunction>().ChangeText();
               GVROff();
               if (!terminado)
               {
                   GVROn();
               }
            }
            else if (result.gameObject.CompareTag("Salir"))
            {
                Application.Quit();
                Debug.Log("Holi");
            }
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
}
