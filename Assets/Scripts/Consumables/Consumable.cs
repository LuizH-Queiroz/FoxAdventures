using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public int healAmount;
    public Animator animator;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerStateManager>().Heal(healAmount);
            StartCoroutine(OnConsume());
        }
    }

    IEnumerator OnConsume()
    {
        animator.SetTrigger("OnConsume");
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
