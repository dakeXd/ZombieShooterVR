using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoMuerte : MonoBehaviour
{

    public Text textoFinal;
    public Text count;

    public void changeTextFinal()
    {
        textoFinal.text = "Has matado: " + count.text;
    }

}
