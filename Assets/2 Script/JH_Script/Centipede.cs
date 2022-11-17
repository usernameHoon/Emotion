using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    bool isPlay;
    bool isClear;
    bool isHit;
    bool isChase;

    float randDis;
    float dis;
    float nowHit;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;

    [SerializeField]
    PlayerRenewal player;

    [SerializeField]
    BoxCollider2D attackBox;

    [SerializeField]
    float maxHit;
    [SerializeField]
    float x;
    [SerializeField]
    float y;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isClear)
        {
            //passObject로 변경해서 더이상 플레이어와 작용하지 않게 함
            attackBox.gameObject.layer = 10;
            gameObject.layer = 10;
            return;
        }

        dis = Vector2.Distance(transform.position, player.transform.position);
        if (dis < 40)
        {
            //플레이

            rigid.velocity = new Vector2(randDis * 3, rigid.velocity.y);

            //앞에 벽이 있다면 반대로 이동
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right, (spriteRenderer.flipX ? 4 : -20), LayerMask.GetMask("Platform"));
            if (hit && !isChase)
            {
                randDis = spriteRenderer.flipX ? -1 : 1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
                attackBox.offset = spriteRenderer.flipX ? new Vector2(4f, 0) : new Vector2(-4f, 0);
            }

            if (!isPlay && !isHit && !isChase)
                Think();
        }
        else
        {
            CancelInvoke();
        }
        PlayerChase();
    }
    void Think()
    {
        isPlay = true;

        randDis = Random.Range(-1, 2);
        //randDis = 0; //테스트용

        if (randDis >= 1)
        {
            spriteRenderer.flipX = true;
            attackBox.offset = new Vector2(4f, 0);
        }
        else if (randDis <= -1)
        {
            spriteRenderer.flipX = false;
            attackBox.offset = new Vector2(-4f, 0);
        }
        float playTime = Random.Range(2f, 4f);

        if (randDis == 0)
        {
            playTime = 1;
        }
        else
        {
            anim.SetTrigger("run");
        }

        Invoke("againThink", playTime);
    }
    void againThink()
    {
        isPlay = false;
    }
    void PlayerChase()
    {
        //앞에 플레이어가 있다면 플레이어를 향해 달림
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 1.5f, Vector3.right, (spriteRenderer.flipX ? 18 : -18), LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector3.right * (spriteRenderer.flipX ? 18 : -18), Color.white);

        if (hit && !isHit)
        {
            isChase = true;
            switch (nowHit)
            {
                case 0:
                    randDis = (spriteRenderer.flipX ? 2.5f : -2.5f);
                    break;
                case 1:
                    randDis = (spriteRenderer.flipX ? 1.8f : -1.8f);
                    break;
                case 2:
                    randDis = (spriteRenderer.flipX ? 0.8f : -0.8f);
                    break;
                default:
                    randDis = (spriteRenderer.flipX ? 0f : 0f);
                    break;
            }
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Centipede_Move"))
                anim.SetTrigger("run");
        }
        else if (!hit)
        {
            isChase = false;
        }
    }
    void HitNextReady(float delayTime)
    {
        isHit = true;
        isPlay = false;
        CancelInvoke();
        nowHit++;
        randDis = 0;
        StartCoroutine(HitNextThinkOrChase(delayTime));
    }

    IEnumerator HitNextThinkOrChase(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isHit = false;

        if (!isClear && !isChase)
            //추격중이 아니였다면, 생각을 하고 || 추격중이였다면 하던 추격 계속 해라
            Think();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ThrowObject" && !isClear)
        {
            //퍼즐 해방 조건
            if (!isHit)
            {
                ObjectManager.Instance.ReturnObject(collision.gameObject, "angryBall");
                switch (nowHit)
                {
                    case 0:
                        HitNextReady(0.8f);
                        anim.SetTrigger("stand");
                        break;
                    case 1:
                        HitNextReady(1.0f);
                        anim.SetTrigger("stand");
                        break;
                    case 2:
                        HitNextReady(1.0f);
                        anim.SetTrigger("stand");
                        break;
                    case 3:
                        HitNextReady(1.0f);
                        anim.SetTrigger("die");
                        // this.gameObject.SetActive(false);
                        break;
                    default:
                        Debug.Log("지네 Hit 관련 부분 에러");
                        break;

                }
                if (maxHit <= nowHit)
                {
                    // 퍼즐 클리어
                    isClear = true;
                    Debug.Log("Clear");
                }
                Debug.Log(nowHit);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigid = collision.gameObject.GetComponent<Rigidbody2D>();

            player.Rigid.AddForce(new Vector2(randDis * 10, 10), ForceMode2D.Impulse);
            Invoke("StartPositionWarp", 2.0f);
        }

    }

    void StartPositionWarp()
    {
        gameObject.transform.position = new Vector2(x, y);
    }
}

//public class Centipede : MonoBehaviour
//{
//    public Transform[] patrolPoints;
//    public float moveSpeed;
//    public int patrolDestination;

//    Animator anim;

//    void Awake()
//    {
//        anim = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        if(patrolDestination == 0)
//        {
//            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
//            if(Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
//            {
//                anim.SetTrigger("run");
//                transform.localScale = new Vector3(-1, 1, 1);
//                patrolDestination = 1;
//            }
//        }

//        if (patrolDestination == 1)
//        {
//            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
//            if (Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
//            {
//                anim.SetTrigger("run");
//                transform.localScale = new Vector3(1, 1, 1);
//                patrolDestination = 0;
//            }
//        }
//    }
//}