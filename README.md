# FoxAdventures

_This is my final project for [CS50's Introduction to Game Development](https://cs50.harvard.edu/games/2018/)._

FoxAdventures is a 2D, platform game build in [Unity](https://unity.com/) Game Engine, which has one playable scene/level called Play and three other simple scenes/screens: Start, GameOver and Victory.

On the Play scene the player wins if he manages to go through the map and reach the house at the end without being killed by the enemies, and loses otherwise.

The player mechanics is mainly walking or jumping. Jumping is important to win the game, as the player can only kill enemies by falling on top of them.

<br>


## Detailing the Game

### **Game Flow**

The game is divided in basically three main sections (in order of appearance):
- Start: A simple Scene presenting the game
- Play: The playable level, in which the player can either win or lose
- End: There are two Scenes that may be presented:
    - Victory: if the player won in the Play Scene
    - GameOver: if the player lost in the Play Scene


### **Scenes**

Note: the game is a 2D platformer

- Start Scene

    A simple screen with a SunnyLand (main asset) background, a music and texts to welmcome the player and inform how to start playing the game.

- Play Scene

    The main part of the game. The scenario, collidable objects and triggers were created using tilemaps, so there are multiple tilemaps in the scene.

    The player must cross the map to reach the house at the end. To do this, he'll have to dodge or kill the enemies that will appear on the way.

    The player has 50 life points and the damage caused by a enemy is 10, but there are collectables in the map where the player can pick up and heal. A life bar is shown on the top left corner for the player to know approximately how much life he has.

    To kill a enemy the player must fall on top of them, either by falling from a higher place or after jumping.

    The player will only take damage when colliding with an enemy but not falling.

    When the player reaches the house the Play Scene ends and the game goes to the Victory Scene. If the player runs out of life, the game goes to the GameOver Scene.

- Victory Scene

    A simple screen that plays a victory sound effect and congrats the player for completing the game.

- GameOver Scene

    A simple screen that plays a defeat sound effect and tells the player to try again by pressing Enter.

<br>


### **Player**

The player has 5 States: IDLE, WALK, JUMP, FALL and ON_HIT.

The main file for controlling the player is called PlayerStateManager.cs, in which the State calls are made (like Enter and Update) and where we can find the methods that may be used by any State the player is in, like these:

```cs
public void ChangeState(PlayerBaseState state)
{
    currentState = state;
    currentState.EnterState(this);
}

public void SetSpriteDirection(float moveDirection)
{
    if (moveDirection == -1)
    {
        sprite.flipX = true;
    }
    else if (moveDirection == 1)
    {
        sprite.flipX = false;
    }
}

public void TakeDamage(int amount)
{
    currentHealth -= amount;

    if (currentHealth > 0)
    {
        ChangeState(OnHitState);
    }
    else
    {
        SceneManager.LoadScene("GameOver");
    }
}

public void Heal(int amount)
{
    currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
}
```

<br>

Follows detailed explanations of how every State works (if the Enter method doesn't have any important features it will not be covered):

- IDLE
    - Enter: the animation is changed by setting the *Animator* 'State' variable value:
        ```cs
        public override void EnterState(PlayerStateManager player)
        {
            player.animator.SetInteger("State", (int) PlayerStateManager.STATES.IDLE);
        }
        ```
        This is made in every single State in the whole game. In every State's Enter method (even in the enemies States) a *Animator* value is set to a new value (so it won't be said again in order not to be repetitive).

    - Update: it is checked if the player wants to move the character by walking or jumping.

<br>

- WALK
    - Update: the character moves horizontally, changing the Sprite direction if needed, and may change the State to IDLE (if the player stops moving), FALL (if the *Rigidbody*'s "y" velocity is minor than zero) or JUMP (if the player hits Space Key).

<br>

- JUMP
    - Enter: the *Rigidbody*'s "y" velocity is set
        ```cs
        rigidbody.velocity = new Vector2(0, jumpForce);
        ```
    
    - Update: the player may move to left or right. The State is changed to FALL when the *Rigidbody*'s velocity gets minor then zero.

<br>

- FALL
    - Update: the player may move to left or right when falling. The State is changed when the character reaches the ground, going to IDLE if the player is not moving or WALK, otherwise.

<br>

- ON_HIT
    - Enter: the character is thrown to the opposite side in wich he's loking at
        ```cs
        player.rigidbody.velocity = new Vector2(6 * (player.sprite.flipX ? 1 : -1), 12);
        ```
    
    - Update: change to IDLE State after 0.8 seconds
        ```cs
        if (Time.time - startTime >= 0.8)
        {
            player.ChangeState(player.IdleState);
        }
        ```
<br>

A very important method on PlayerStateManager.cs is the OnCollisionEnter2D. It is responsible for making damage to the enemies or taking damage from them.

If in FALL state it will do the damage (and automatically jump again), or take it, otherwise.

```cs
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag.Equals("Enemy"))
    {
        if (currentState.GetType() == typeof(PlayerFallState))
        {
            ChangeState(JumpState);

            if (collision.gameObject.GetComponent<Eagle>() != null)
            {
                StartCoroutine(collision.gameObject.GetComponent<Eagle>().Die());
            }
            else if (collision.gameObject.GetComponent<FrogStateManager>() != null)
            {
                StartCoroutine(collision.gameObject.GetComponent<FrogStateManager>().Die());
            }
            else
            {
                StartCoroutine(collision.gameObject.GetComponent<Opossum>().Die());
            }
        }
        else
        {
            TakeDamage(10);
        }
    }
}
```

<br>


## Health Bar

The Health Bar has a single file which controls the life's rectangle width through a *Slider* depending on the player's amount of life.
```cs
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerStateManager player;


    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = player.maxHealth;
        slider.value = player.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.currentHealth;
    }
}
```

<br>


## Enemies

There are three enemies in the game: Eagle, Grog and Opossum.

A description of each of them follows.

<br>

### **Eagle**

An Eagle may be of two types: STATIC or VERTICAL_MOVE.

A STATIC Eagle is the simpler enemy in the game, as it just stays in the exact same place all the time, so it should be easy to kill them.

A VERTICAL_MOVE Eagle is a little different. The "x" position of this type of Eagle is always the same, but the "y" coordinate keeps continually changing as it flies up and down.

The Eagle type may be selected in its inspector, since the variable that determines its type is public and *enum*.

<br>

### **Frog**

Frogs are also designed with State Machines. A Frog has three States: IDLE, JUMP and FALL.

- IDLE
    - Enter: it is defined a time in seconds, in the range [2, 7], that the Frog will continue IDLE. The side on which the Frog must look is also randomly defined.

    - Update: after the time defined on the Enter method, the Frog will go to the Jump State.

<br>

- JUMP
    - Enter: the Frog's *Rigidbody* velocity is changed on both "x" and "y" axis.
        ```cs
        frog.rigidbody.velocity = new Vector2(frog.jumpForceLateral * (frog.sprite.flipX? 1 : -1), frog.jumpForceUp);
        ```

    - Update: when the Frog starts falling (*Rigidbody*'s "y" velocity is minor than zero) the State changes to FALL.

<br>

- FALL
    - Update: when the Frog reaches the ground a timer starts to be increased and the State changes back to IDLE when the timer gets equal to or higher than two.
        ```cs
        public override void UpdateState(FrogStateManager frog)
        {
            if (frog.rigidbody.velocity.y == 0)
            {
                frog.animator.SetInteger("State", 0); // although we're not in IdleState, we want
                                                    // the Idle animation to be played
                timer += Time.deltaTime;

                if (timer >= 2)
                {
                    frog.ChangeState(frog.IdleState);
                }
            }
        }
        ```

<br>

### **Opossum**

Opossums are enemies that are walking all the time. They may walk to right or left.

First it is defined in which direcion it'll go and how much time it'll walk. After that determined time passes, this cycle restart.

```cs
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
```

<br>

The enemies death (**Die()** method) consists basically in deactivating its *Colliders* and *Rigidbody* and playing the death animation.

The enemies **Die()** method are quite similar because the differences are just to make sure their position will not change while playing the death animation.

Follows the Frog's **Die()** method:

```cs
public IEnumerator Die()
{
    foreach (var collider in gameObject.GetComponents<Collider2D>())
    {
        collider.enabled = false;
    }
    rigidbody.Sleep();
    ChangeState(IdleState); // for the death animation not to move
    animator.SetTrigger("Death");

    yield return new WaitForSeconds(1);

    Destroy(gameObject);
}
```

<br>


## Consumables

Consumables are simply game objects meant to heal the player, having no movement at all and no conditions to worry about except checking if collided with the player, so its code is pretty simple:

```cs
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
```

<br>


## Credits

- Development:
    - [Luiz Henrique Queiroz de Albuquerque Silva](https://github.com/LuizH-Queiroz)
    
    <br>

- Art:
    - [Ansimuz](https://assetstore.unity.com/packages/2d/characters/sunny-land-103349) - [LICENSE](https://unity.com/legal/as-terms)
    - [PaperHatLizard](https://paperhatlizard.itch.io/cryos-mini-gui) - [LICENSE](https://creativecommons.org/licenses/by/4.0/)
    - [Tiny Worlds](https://assetstore.unity.com/packages/2d/fonts/free-pixel-font-thaleah-140059) - [LICENSE](https://unity.com/legal/as-terms)

    <br>

- Music:
    - [rezoner](https://opengameart.org/content/happy-arcade-tune) - [LICENSE](https://creativecommons.org/licenses/by/3.0/)
    - [Not Jam](https://not-jam.itch.io/not-jam-music-pack) - [LICENSE](https://creativecommons.org/licenses/by/4.0/)

    <br>

- Sound Effects:
    - [Pixabay](https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14800) - [LICENSE](https://pixabay.com/pt/service/license/)
    - [den_yes](https://opengameart.org/content/game-over-soundold-school) - [LICENSE](https://creativecommons.org/publicdomain/zero/1.0/)

    <br>

- Important Notes
    - **I'm not the author/creator of any asset (art, music or sound effect) used in this project**
    - No changes were made in any asset used in this project
    - You can check any asset by clicking its author's name/identifier. Please note that each asset's license is provided along with the author's name/identifier
    - You are free to use, share, modify and build upon this project as long as you give the credits to all of the authors listed in this section (_Credits_), respecting each license's

    <br>

    - The sound effect from [Pixabay](https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14800) is named "piglevelwin2.mp3"