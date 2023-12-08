using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum PlayableCharacter
    {
        SuperDude,
        SuperGirl
    }

public class Spawner : MonoBehaviour
{
    public GameObject superDudePrefab;
    public GameObject superGirlPrefab;
    public Transform spawnPoint;


    public PlayableCharacter playerChoice; 


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
