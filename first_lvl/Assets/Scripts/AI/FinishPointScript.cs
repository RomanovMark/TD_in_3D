using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.instance.CurrentEnemiesList.Remove(other.gameObject);
            Destroy(other.gameObject);
            GameManager.instance.Lives--;

            if (GameManager.instance.Lives <= 0)
                GameManager.instance.IsGameOver = true;
        }
    }
}
