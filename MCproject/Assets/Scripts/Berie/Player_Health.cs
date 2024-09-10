using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Health : MonoBehaviour
{
    public int health;
    public float deadAnimation = 1f;
    public Animator animator;
    public string hitAnimationName = "Hit"; // Nome del trigger dell'animazione di attacco
    public Cinemachine.CinemachineVirtualCamera cinemachineCamera;
    public bool isImmune = false; // Stato dell'immunita'
    public float immuneDuration = 1f; // Durata dell'immunit�
    public Text healText;
    private void Start()
    {
        health = 100;
        healText.text = "Health:" + health;
    }

    private void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            animator.SetTrigger(hitAnimationName); // Trigger animazione colpo subito
            health -= damage;
            UpdateHealthUI();
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(ActivateImmunity());
        }
        
    }

    private void Die()
    {
        GameObject placeholder = new GameObject("Placeholder");
        placeholder.transform.position = transform.position;
        animator.SetTrigger("Death");
        // Imposta la camera per seguire l'oggetto vuoto
        cinemachineCamera.Follow = placeholder.transform;
        // Avvia la coroutine per disabilitare il GameObject dopo l'animazione di morte
        StartCoroutine(DisableAfterDeathAnimation());
    }

    IEnumerator DisableAfterDeathAnimation()
    {
        // Aspetta la durata dell'animazione di morte
        yield return new WaitForSeconds(deadAnimation);

        // Disabilita il GameObject dopo l'animazione
        gameObject.SetActive(false);
    }

    IEnumerator ActivateImmunity()
    {
        isImmune = true;

        // Aspetta fino al termine dell'animazione
        yield return new WaitForSeconds(immuneDuration);

        // Disattiva l'immunit�
        isImmune = false;
    }

    public void Heal(int amount)
    {
        health += amount;
        UpdateHealthUI();
        
    }
    void UpdateHealthUI()
    {
        healText.text = "Health: " + health;
    }
}
