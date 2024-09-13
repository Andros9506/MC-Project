using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Chest_Default : MonoBehaviour, IOpenChest
{

    public GameObject coinPrefab;  // Prefab della moneta
    public Transform coinSpawnPoint;  // Il punto da cui spawnano le monete
    public int numCoins = 10;  // Numero di monete da spawnare
    public GameObject specialCoinPrefab;  // Prefab della moneta speciale
    public Transform playerTransform;     // Riferimento al player
    public Vector3 specialCoinOffset = new Vector3(0, 0.7f, 0);  // Offset per la posizione della moneta speciale
    public float delay;
    private Player_Precius player;
    private Animator animator;
    private bool isOpen = false;
    private GameObject playerFind;

    // Start is called before the first frame update

    void Start()
    {
        playerFind = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        playerTransform = playerFind.transform;
        player = playerFind.GetComponent<Player_Precius>();
    }


    public void OpenChest()
    {
        if (isOpen) return;  // Evita di aprire la chest pi� volte
        isOpen = true;
        // Genera monete
        for (int i = 0; i < numCoins; i++)
        {
            SpawnCoin();
        }
        // Spawna la moneta speciale sopra al player
        player.PlaySpecialAnimation();
        StartCoroutine(SpawnSpecialCoinWithDelay(delay));

        animator.SetBool("isOpen", true);
    }

    void SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
        // Impulso verso l'alto e casuale in direzione orizzontale
        float forceX = Random.Range(-2f, 2f);
        float forceY = Random.Range(2f, 5f);
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }


    void CollectSpecialCoin(GameObject specialCoin)
    {
        // Aggiorna il punteggio o aggiungi altri comportamenti speciali
        player.AddCoin(specialCoin.GetComponent<Coin_Gold_Chest>().GetCoinValue());  // Punteggio speciale
    }

    IEnumerator SpawnSpecialCoinWithDelay(float delay)
    {
        // Aspetta per il ritardo specificato
        yield return new WaitForSeconds(delay);

        // Instanzia la moneta speciale sopra al player con un offset
        Vector3 spawnPosition = playerTransform.position + specialCoinOffset;
        GameObject specialCoin = Instantiate(specialCoinPrefab, spawnPosition, Quaternion.identity);

        // Imposta il playerTransform e l'offset per la moneta speciale
        Follow_Player specialCoinScript = specialCoin.GetComponent<Follow_Player>();
        //Coin_Gold specialCoinScript = specialCoin.GetComponent<Coin_Gold>();
        if (specialCoinScript != null)
        {
            specialCoinScript.Initialize(playerTransform);
        }

        // Raccogli la moneta immediatamente e avvia l'animazione del player
        CollectSpecialCoin(specialCoin);
    }
}
