using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    public AudioClip soundEffect;
    public AudioClip soundEffectReload;
    public float V = 1.0f;

    [Header("Ammo")] 
    public int maxAmmo;
    private int actualAmmo;
    public float reloadTime;
    public float shotDamage;
    public TMP_Text display;
    public bool reloading;
    public Image barraCarga;
    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
   


    void Start()
    {
        reloading = false;
        actualAmmo = maxAmmo;
        UpdateText();
        
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }
    
    public void UpdateText()
    {
        display.text = actualAmmo + "/" + maxAmmo;
    }

    public void StartShoot()
    {
        if (actualAmmo > 0)
        {
            actualAmmo--;
            UpdateText();
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            Reload();
        }
    }

    public void Reload()
    {
       
        if (!reloading && maxAmmo>actualAmmo)
        {
            AudioSource.PlayClipAtPoint(soundEffectReload, barrelLocation.position, V);
            reloading = true;
            StartCoroutine(nameof(BarraRecarga));
            Invoke(nameof(ActualReload), reloadTime);
        }
            
    }

    private void ActualReload()
    {
        reloading = false;
        actualAmmo = maxAmmo;
        UpdateText();
    }

    IEnumerator BarraRecarga()
    {
        float actualTime = 0;   
        while (actualTime<=reloadTime)
        {
            actualTime += Time.deltaTime;
            barraCarga.fillAmount = actualTime / reloadTime;
            //Debug.Log(actualTime + " " + reloadTime / actualTime);
            //Debug.Log(barraCarga.fillAmount);
            yield return null;
        }
        barraCarga.fillAmount = 0;
    }
    //This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        //Play Audio
        AudioSource.PlayClipAtPoint(soundEffect, barrelLocation.position, V);

        // Create a bullet and add force on it in direction of the barrel
        //Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
