using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour
{

    [Header("Atributes")]
    public float health;
    public float speed;
    public float jumpforce;
    public float attackrange;
    public float recoveryTime;
    bool isJumping;
    bool isAttacking;
    bool isDead;
    float recoveryCount;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator animacao;
    public Transform firepoint;
    public LayerMask enemylayer;
    public Image life;
    public GameControl gc;

    [Header("Audio")]
    public AudioSource audiosource;
    public AudioClip audioclip;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(isDead == false)
        {
            Jump();
            OnAttack();
        }
    }

    void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if(isJumping == false)
            {
                animacao.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                isJumping = true;
            }
            
        }
    }

    void OnAttack()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            animacao.SetInteger("Transition", 3);
            audiosource.PlayOneShot(audioclip);

            Collider2D hit = Physics2D.OverlapCircle(firepoint.position, attackrange, enemylayer);

            if(hit != null)
            {
                hit.GetComponent<Flying_Eye>().OnHit();
            }

            StartCoroutine(OnAttacking());
        }
    }

    IEnumerator OnAttacking()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(firepoint.position, attackrange);
    }

    public void OnHit(float damage)
    {

        recoveryCount += Time.deltaTime;

        if(recoveryCount >= recoveryTime && isDead == false)
        {
            animacao.SetTrigger("Hit");
            health -= damage;
            life.fillAmount = health/100;
            GameOver();
            recoveryCount = 0f;
        }
    }

    void GameOver()
    {
        if(health <= 0)
        {
            animacao.SetTrigger("Death");
            isDead = true;
            gc.showGameOver();
        }
    }

    // FixedUpdate é chamado na física do jogo
    void FixedUpdate()
    {
        if(isDead == false)
        {
            OnMove();
        }
    }

    void OnMove()
    {
        //float direcao = Input.GetAxis("Horizontal");
        float direcao = CrossPlatformInputManager.GetAxis("Horizontal");
        rig.velocity = new Vector2(direcao * speed, rig.velocity.y);

        if (direcao > 0 && isJumping == false && isAttacking == false)
        {
            transform.eulerAngles = new Vector2(0, 0);
            animacao.SetInteger("Transition", 1);
        }

        if (direcao < 0 && isJumping == false && isAttacking == false)
        {
            transform.eulerAngles = new Vector2(0, 180); // rotacionando a sprite do player em 180 graus para a direção esquerda
            animacao.SetInteger("Transition", 1);
        }

        if (direcao == 0 && isJumping == false && isAttacking == false)
        {
            animacao.SetInteger("Transition", 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //verificando se o player está no chão
        if(collision.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }
}
