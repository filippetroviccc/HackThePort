using CompleteProject;
using UnityEngine;

public class ShooterManager : EnemyManager
{
    private EnemySpawnPoint[] spawnPoints;

    public void Start()
    {
        spawnPoints = new EnemySpawnPoint[spawnPointsObj.Length];
        for (var index = 0; index < spawnPointsObj.Length; index++)
        {
            var obj = spawnPointsObj[index];
            spawnPoints[index] = obj.GetComponent<EnemySpawnPoint>();
        }

        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        bool leftFree = false;
        bool rightFree = false;
        if (spawnPoints[0].isFree && spawnPoints[1].isFree) leftFree = true;
        if (spawnPoints[2].isFree && spawnPoints[3].isFree) rightFree = true;

        int spawnPointIndex;

        if (leftFree && rightFree)
        {
            spawnPointIndex = Random.Range(0, 2) == 0 ? Random.Range(0, 2) : Random.Range(2, 4);
        }
        else if (leftFree) spawnPointIndex = Random.Range(0, 2);
        else if (rightFree) spawnPointIndex = Random.Range(2, 4);
        else return;

        if (numOfEnemies < maxNumOfEnemies)
        {
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            var enemyObj = Instantiate(enemyPrefab, spawnPointsObj[spawnPointIndex].gameObject.transform.position,
                spawnPointsObj[spawnPointIndex].gameObject.transform.rotation);

            spawnPoints[spawnPointIndex].enemy = enemyObj;
        }
    }
}