using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public enum TYPES
    {
        STATIC,
        VERTICAL_MOVE
    }
    
    public TYPES type;

    EagleBaseType eagle;
    public Animator animator;
    public float moveDistance; // For Vertical_Move eagles
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case TYPES.STATIC:
                eagle = new StaticEagle();
                break;

            case TYPES.VERTICAL_MOVE:
                eagle = new VerticalMoveEagle();
                break;
        }
        eagle.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        eagle.Update(this);
    }

    public IEnumerator Die()
    {
        foreach (var collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        eagle = new StaticEagle(); // for the animation not to move up and down
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
