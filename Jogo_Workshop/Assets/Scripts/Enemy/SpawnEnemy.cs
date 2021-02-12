using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public float minTime;
    public float maxTime;
    float spawnTime;

    public GameObject enemyPrefab;

    float timeCount;


    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;

        if(timeCount >= spawnTime)
        {
            //criar inimigos aqui
            Vector3 enemypos = new Vector3(transform.position.x, transform.position.y, 0f);
            Instantiate(enemyPrefab, enemypos, transform.rotation);
            spawnTime = Random.Range(minTime, maxTime);
            timeCount = 0f;
        }
    }
}
