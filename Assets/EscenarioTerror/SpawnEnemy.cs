using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject Enemy;
    private int xPos;
    private int zPos;
    public int zona;
    public int enemyCount;
    public int maxEnemies;

    public TMP_Text enemyDead;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        


        while (enemyCount < maxEnemies)
        {
            zona = Random.Range(1, 4);

            if (zona == 1)
            {
                xPos = Random.Range(60, 67);
                zPos = Random.Range(30, 50);
            }

            if(zona == 2)
            {
                xPos = Random.Range(51, 71);
                zPos = Random.Range(2, 60);
            }

            if (zona == 3)
            {
                xPos = Random.Range(10, 16);
                zPos = Random.Range(36, 52);
            }

            if (zona == 4)
            {
                xPos = Random.Range(30, 49);
                zPos = Random.Range(0, 12);
            }

            GameObject obj = Instantiate(Enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            obj.SetActive(true);
            
            //Dependiendo de el numero de enmigos que haya matado, la dificultad ira creciendo haciendo que aparezcan cada vez en menos tiempo
            if (System.Int32.Parse(enemyDead.text) < 6)
            {
                yield return new WaitForSeconds(5.0f);

            }else if (System.Int32.Parse(enemyDead.text) >= 6 && System.Int32.Parse(enemyDead.text) < 9)
            {
                yield return new WaitForSeconds(3.0f);
            }
            else if (System.Int32.Parse(enemyDead.text) >= 9)
            {
                yield return new WaitForSeconds(1.5f);
            }

            enemyCount += 1;
        }
    }


}
