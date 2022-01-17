using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFunction : MonoBehaviour
{

    public Text textoHis;
    protected int countHis = 0;
    public SimpleShoot s;
    public SpawnEnemy enemys;



    public bool ChangeText()
    {
        countHis++;
        switch (countHis)
        {
            case 1:
                textoHis.text = "¿Todo bien?";
                break;
            case 2:
                textoHis.text = "¿Qué haces aqui?";
                break;
            case 3:
                textoHis.text = "¿Por qué no usas tu pistola?";
                break;
            case 4:
                textoHis.text = "Venga, disparame";
                break;
            case 5:
                s.StartShoot();
                textoHis.text = "¡YA VIENEN!";
                enemys.enabled = true;
                Invoke(nameof(Desactivar), 0.5f);
                return true;
                
                break;
            default:
                
                Desactivar();
                break;
        }

        return false;

    }

    private void Desactivar()
    {
        gameObject.SetActive(false);
    }

    
}
