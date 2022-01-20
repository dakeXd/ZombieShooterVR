using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextFunction : MonoBehaviour
{

    public TMP_Text textoHis;
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
                textoHis.text = "¿QuÉ haces aquÍ?";
                break;
            case 3:
                textoHis.text = "¿Por quÉ no usas tu pistola?";
                break;
            case 4:
                textoHis.text = "Venga, dispÁrame";
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
