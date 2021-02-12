using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Eye : MonoBehaviour
{
    [Header("Atributes")]
    public float speed;
    float initialSpeed;
    public float close;
    public bool isRight;
    public float health;
    public float damage;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator animacao;

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        float playerpos = transform.position.x - player.position.x;

        if(playerpos > 0)
        {
            isRight = false;
        }
        else
        {
            isRight = true;
        }

        if(distance <= close)
        {
            speed = 0f;
            player.GetComponent<Player>().OnHit(damage);
        }
        else
        {
            speed = initialSpeed;
        }
    }

    void FixedUpdate()
    {
        if (isRight)
        {
            rig.velocity = new Vector2(speed, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (!isRight)
        {
            rig.velocity = new Vector2(-speed, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void OnHit()
    {
        //StartCoroutine(Hurt());
        health--;
        if (health <= 0)
        {
            speed = 0f;
            animacao.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
        else
        {
           
            animacao.SetTrigger("Hit");
            
        }
    }

    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(10f);
    }
}
