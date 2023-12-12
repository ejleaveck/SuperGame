using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumerates the playable characters in the game to be selected by the player.
/// </summary>
    public enum PlayableCharacter
    {
        SuperDude,
        SuperGirl
    }

/// <summary>
/// Manages the spawning of the player characters in the game.
/// </summary>
public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject superDudePrefab;
    [SerializeField] private GameObject superGirlPrefab;
    [SerializeField] private Transform spawnPoint;

    

    public PlayableCharacter playerChoice { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Handles the spawning of the selected player character.
    /// </summary>
    void SpawnPlayer()
    {
        if (spawnPoint != null && superDudePrefab != null && superGirlPrefab != null)
        {
            GameObject instantiatedPlayer = null;

            switch (playerChoice)
            {
                case PlayableCharacter.SuperDude:
                    instantiatedPlayer = Instantiate(superDudePrefab, spawnPoint.position, spawnPoint.rotation);
                                        break;
                case PlayableCharacter.SuperGirl:
                   instantiatedPlayer =  Instantiate(superGirlPrefab, spawnPoint.position, spawnPoint.rotation);
                                        break;
                default:
                    Debug.LogWarning($"{nameof(Spawner)} > {nameof(SpawnPlayer)}: Unhandled player choice.");
                    break;
            }

            if (instantiatedPlayer != null)
            {
                Camera.main.GetComponent<CameraFollow>().SetPlayer(instantiatedPlayer.transform);
            }
        }
        else
        {
            Debug.LogError($"{nameof(Spawner)} > {nameof(SpawnPlayer)}: One or more required components are not assigned.");

        }

        
    }
}
