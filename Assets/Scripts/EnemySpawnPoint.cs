using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject enemy;
    public bool isFree => enemy == null;
}