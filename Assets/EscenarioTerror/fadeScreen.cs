using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeScreen : MonoBehaviour
{

    public Image image;


    public void fade()
    {

        image.CrossFadeAlpha(1.0f, 3.0f, false);
    }

}
