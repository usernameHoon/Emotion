using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    public bool isChange;

    [SerializeField]
    GameObject[] playerAbility;

    [SerializeField]
    GameObject[] fairyAbility;

    // Start is called before the first frame update
    void Start()
    {
        isChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        Change();
    }

    void Change()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isChange = !isChange;

            if(!isChange)
            {
                    //fairyAbility[0].SetActive(true);
                    //playerAbility[0].SetActive(false);
                for(int i = 0; i < fairyAbility.Length; i++)
                {
                    fairyAbility[i].SetActive(true);
                    playerAbility[i].SetActive(false);
                }
            }
            else if(isChange)
            {
                for (int i = 0; i < fairyAbility.Length; i++)
                {
                    fairyAbility[i].SetActive(false);
                    playerAbility[i].SetActive(true);
                }
            }
            Debug.Log("ÅÇ ´­¸²");
        }
    }
}
