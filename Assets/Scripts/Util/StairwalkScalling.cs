using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StairwalkScalling : MonoBehaviour
{
    public struct Trespasser
    {
        public GameObject go;
        public Vector3 previousPosition;
    }
    public float scaleFactor;
    bool leftToRight;

    private Dictionary<GameObject, Trespasser> trespassers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Trespasser t in trespassers.Values)
        {
            //t.go.transform.localScale =
            
        }
    }

    public void trespasserDetected(GameObject trespasser, bool walkingUp)
    {
        if(trespassers.ContainsKey(trespasser))
        {
            
        } else
        {
            //trespassers[trespasser] = 
        }
    }
}
