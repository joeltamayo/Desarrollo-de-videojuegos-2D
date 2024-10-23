using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float Speed;
    public float JumpForce;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;

    private int coinsenBD;
    private int coinsenGame;

    FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializamos la base de aatos
        db = FirebaseFirestore.DefaultInstance;
        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Obtehemos las monedas de la base de datos
        getCoins("players", "player1");
        // Reiniciamos las monedas actuales
        coinsenGame = 0;

        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


        Animator.SetBool("Running", Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);

        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;


        if ((Input.GetKeyDown(KeyCode.W) && Grounded) || (Input.GetKeyDown(KeyCode.UpArrow) && Grounded))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        OnCoinCollected(1);
    }

    private void Shoot()
    {

        Vector3 direction;

        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }

    private void getCoins(string collection, string documentId)
    {
        DocumentReference docRef = db.Collection(collection).Document(documentId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    if (snapshot.TryGetValue("coins", out int coinCount))
                    {
                        coinsenBD = coinCount;
                        Debug.Log($"Número de monedas en la base de datos: {coinCount}");
                    }
                    else
                    {
                        Debug.Log("Campo 'coins' no encontrado en el documento.");
                    }
                }
                else
                {
                    Debug.Log($"Documento '{documentId}' no existe en la colección '{collection}'.");
                }
            }
            else
            {
                Debug.LogError("Error obteniendo el documento: " + task.Exception);
            }
        });
    }

    public void UpdatePlayerCoins(string collection, string documentId, int newCoins)
    {
        DocumentReference docRef = db.Collection(collection).Document(documentId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "coins", newCoins }
        };

        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("Número de monedas actualizado en la base de datos: " + newCoins);
            }
            else
            {
                Debug.LogError("Error actualizando el número de monedas: " + task.Exception);
            }
        });
    }

    // Método para añadir monedas cuando el jugador las recoge
    public void OnCoinCollected(int coinsCollected)
    {
        coinsenGame += coinsCollected;
        Debug.Log($"Monedas recogidas: {coinsCollected}, Total en juego: {coinsenGame}");
    }

    void OnApplicationQuit()
    {
        // Subir monedas cuando se cierra el juego
        UpdatePlayerCoins("players", "player1", coinsenBD + coinsenGame);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Subir monedas cuando se cambia de escena
        UpdatePlayerCoins("players", "player1", coinsenBD + coinsenGame);
    }
}

