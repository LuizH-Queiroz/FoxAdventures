using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    float moveDirection;
    float moveTime;
    float timer;

    public SpriteRenderer sprite;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        SetMovementVariables();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

        if (Time.time - timer >= moveTime)
        {
            SetMovementVariables();
        }
    }

    void SetMovementVariables()
    {
        moveDirection = Random.Range(-1, 2) <= 0 ? -1 : 1;
        if (moveDirection == 1)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        moveTime = Random.Range(3, 6);
        timer = Time.time;
    }

    public IEnumerator Die()
    {
        foreach (var collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        moveSpeed = 0;
        gameObject.GetComponent<Animator>().SetTrigger("Death");

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
