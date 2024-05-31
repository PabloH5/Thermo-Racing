using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

namespace KartGame.KartSystems
{
    public class RaceController : NetworkBehaviour
    {
        [SerializeField] private Transform playerPrefab;

        [SerializeField] private GameObject prefabIA;

        private static GameObject spawnPointParent;
        private NetworkList<Vector3> networkSpawnPositionList = new NetworkList<Vector3>();
        private List<Vector3> spawnPositionListSingleplayer = new List<Vector3>();
        [SerializeField] private int startAngle = 180;

        public Boolean LastQuestion { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            if (!RaceMultiplayerController.playMultiplayer)
            {
                InitializeSpawnPointsSingleplayer();
                Vector3 spawnPosition = GetRandomSpawnPoint();
                Transform playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, startAngle, 0));
                //Transform playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));

                playerTransform.tag = "Player";
                playerTransform.name = "KartPlayer";
                ArcadeKart arcadeKart = playerTransform.GetComponent<ArcadeKart>();
                Destroy(arcadeKart);

                AutoAssignEvent autoAssignEvent = playerTransform.GetComponentInChildren<AutoAssignEvent>();
                autoAssignEvent.AssignKeyboardInput();
                AutoAssignEventStop autoAssignEventStop = playerTransform.GetComponentInChildren<AutoAssignEventStop>();
                autoAssignEventStop.AssignKeyboardInput();

                //Instantiate the IA pilots
                Vector3 spawnPositionIA = GetRandomSpawnPoint();
                Transform iaTransform = Instantiate(prefabIA.transform, spawnPositionIA, Quaternion.Euler(0, startAngle, 0));
                Debug.Log(iaTransform.name);
                iaTransform.gameObject.SetActive(true);

                //! CHANGE THE BOOLEAN VARIABLE 2 START THE IA MOVEMENT

                AIController controllerAI = iaTransform.GetComponent<AIController>();
                StartCoroutine(StartIAMovement(controllerAI));
                // controllerAI.shouldMove = true;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                InitializeSpawnPoints();
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
                Debug.Log("Im in server in race");
            }
        }
        IEnumerator StartIAMovement(AIController controllerAI)
        {
            yield return new WaitForSeconds(5);
            controllerAI.shouldMove = true;
        }
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        StartCoroutine(SpawnPlayersAfterDelay(10f));
    }

    private IEnumerator SpawnPlayersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            Transform playerTransform;

            if (SceneManager.GetActiveScene().name == "Track1Core")
            {
                playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            }
            else if (SceneManager.GetActiveScene().name == "Track2")
            {
                playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                continue;
            }

            if (!IsServer) { playerTransform.GetComponent<ArcadeKart>().mySpawnPosition = spawnPosition; }

            if (!TrySpawnPlayer(playerTransform, clientId))
            {
                Debug.Log("Failed to spawn player, retrying...");
                yield return new WaitForSeconds(1f); // Wait before retrying
                TrySpawnPlayer(playerTransform, clientId);
            }
        }
    }

    private bool TrySpawnPlayer(Transform playerTransform, ulong clientId)
    {
        try
        {
            ArcadeKartSingleplayer arcadeKart = playerTransform.GetComponent<ArcadeKartSingleplayer>();
            Destroy(arcadeKart);

            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
            playerRigidbody.isKinematic = true;
            playerRigidbody.useGravity = false;

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            // Ensure the Rigidbody is properly configured after spawning
            StartCoroutine(ConfigureRigidbodyAfterSpawn(playerTransform, playerRigidbody));

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error spawning player: {e.Message}");
            if (playerTransform.GetComponent<NetworkObject>().IsSpawned)
            {
                playerTransform.GetComponent<NetworkObject>().Despawn();
                Destroy(playerTransform.gameObject);
            }
            return false;
        }
    }

    private IEnumerator ConfigureRigidbodyAfterSpawn(Transform playerTransform, Rigidbody playerRigidbody)
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure proper initialization
        playerRigidbody.isKinematic = false;
        playerRigidbody.useGravity = true;
    }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
                {
                    NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                    if (networkObject != null)
                    {
                        networkObject.Despawn();
                        Destroy(networkObject.gameObject);
                    }
                }

                Debug.Log($"Client {clientId} disconnected and resources cleaned up.");
            }
        }

        private void InitializeSpawnPoints()
        {
            if (IsServer)
            {
                spawnPointParent = GameObject.FindGameObjectWithTag("Spawnpoint");
                if (spawnPointParent != null)
                {
                    Debug.Log("I find spawnpoint");
                    foreach (Transform child in spawnPointParent.transform)
                    {
                        networkSpawnPositionList.Add(child.position);
                    }
                }
                else
                {
                    Debug.LogError("Spawnpoint parent not found.");
                }
            }
        }

        private void InitializeSpawnPointsSingleplayer()
        {
            if (RaceMultiplayerController.playMultiplayer == false)
            {
                spawnPointParent = GameObject.FindGameObjectWithTag("Spawnpoint");
                if (spawnPointParent != null)
                {
                    Debug.Log("I find spawnpoint");
                    foreach (Transform child in spawnPointParent.transform)
                    {
                        spawnPositionListSingleplayer.Add(child.position);
                    }
                }
                else
                {
                    Debug.LogError("Spawnpoint parent not found.");
                }
            }
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (RaceMultiplayerController.playMultiplayer)
            {
                if (networkSpawnPositionList.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, networkSpawnPositionList.Count);
                    Vector3 spawnPoint = networkSpawnPositionList[randomIndex];
                    networkSpawnPositionList.RemoveAt(randomIndex);
                    return spawnPoint;
                }
                else
                {
                    Debug.LogError("No spawn points available or spawnPositionList not initialized.");
                    return Vector3.zero;
                }
            }
            else
            {
                if (spawnPositionListSingleplayer.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, spawnPositionListSingleplayer.Count);
                    Vector3 spawnPoint = spawnPositionListSingleplayer[randomIndex];
                    spawnPositionListSingleplayer.RemoveAt(randomIndex);
                    return spawnPoint;
                }
                else
                {
                    Debug.LogError("No spawn points available or spawnPositionList not initialized.");
                    return Vector3.zero;
                }
            }

        }
    }
}