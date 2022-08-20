using UnityEngine;

public class VerticalMoveEagle : EagleBaseType
{
    float moveDistance;
    float moveSpeed;
    float initial_Y_value;
    int direction;

    public override void Enter(Eagle eagle)
    {
        moveDistance = eagle.moveDistance;
        moveSpeed = eagle.moveSpeed;
        initial_Y_value = eagle.transform.position.y;
        direction = 1;
    }

    public override void Update(Eagle eagle)
    {
        eagle.transform.Translate(0, moveSpeed * Time.deltaTime * direction, 0);
        
        if (eagle.transform.position.y >= initial_Y_value + moveDistance)
        {
            eagle.transform.position = new Vector3(eagle.transform.position.x, initial_Y_value + moveDistance, eagle.transform.position.z);
            direction = -1;
        }
        else if (eagle.transform.position.y <= initial_Y_value)
        {
            eagle.transform.position = new Vector3(eagle.transform.position.x, initial_Y_value, eagle.transform.position.z);
            direction = 1;
        }
    }
}
