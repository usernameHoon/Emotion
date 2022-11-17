using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryObject : MonoBehaviour
{
    [SerializeField]
    float moveX;
    [SerializeField]
    float moveY;

    [SerializeField]
    GameObject angryObject;

    SpriteRenderer angryObjectSprite;
    Animator anim;


    // Start is called before the first frame update
    void Awake()
    {
        angryObjectSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rock_Move()
    {
        StartCoroutine(RockMove());
    }

    public void Rock_Destroy()
    {
        StartCoroutine(RockDestroy());
    }

    public void Stalactite_Destroy()
    {
        StartCoroutine(StalactiteDestroy());
    }

    IEnumerator RockMove()
    {
        yield return new WaitForSeconds(0.5f);
        angryObject.transform.position = new Vector2(moveX, moveY);
    }

    IEnumerator RockDestroy()
    {
        // yield return new WaitForSeconds(0.5f);
        // angryObject.SetActive(false);

        float progress = 0;
        anim.SetTrigger("destroy");
        while (progress < 1)
        {
            progress += 0.1f;
            angryObjectSprite.color = Color.Lerp(Color.red, new Color(1, 0, 0, 0), progress);
            if (progress > 0.5f)
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator StalactiteDestroy()
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += 0.2f;
            angryObjectSprite.color = Color.Lerp(Color.red, new Color(1, 0, 0, 0), progress);
            if (progress > 0.5f)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3.0f);
        angryObjectSprite.color = Color.Lerp(Color.red, new Color(1, 0, 0, 1), Time.deltaTime);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
