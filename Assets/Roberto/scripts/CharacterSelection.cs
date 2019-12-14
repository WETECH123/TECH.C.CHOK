using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;
using SA;
public class CharacterSelection : MonoBehaviour
{
   public B_Player[] player;
   public GameObject[] buttons;
   public GameObject[] characters;
   public GameObject[] cpuCharacters;
   PancakeSpawner spawner;
    GameObject gamescene =null;
    Vector3 offsetPosition = new Vector3(0, 0, 0);
    
    private void Start()
    {
       gamescene = GameObject.FindGameObjectWithTag(StaticStrings.gameScene);
    }
    public void selectCaracter(int num)
    {
       for (int i = 0; i < buttons.Length; i++)
        {
            if (i== num)
            {
                GameObject newChar = Instantiate(characters[num], player[i].transform.position, player[i].transform.rotation);
                player[i].gameObject.transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                player[i].activeteCanvs();
                newChar.transform.SetParent(gamescene.transform);
                Inputhandler hand = newChar.GetComponent<Inputhandler>();
                hand.InitInGame();
            }
            else
            {
                GameObject newChar = Instantiate(cpuCharacters[i], player[i].transform.position, player[i].transform.rotation);
                player[i].transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                newChar.transform.SetParent(gamescene.transform);
                Camerahandler hand = player[i].GetComponentInChildren<Camerahandler>();
                Destroy(hand.gameObject);
                CPUBrain brain = newChar.GetComponent<CPUBrain>();
                brain.INIT(player[i]);
            }
            buttons[i].SetActive(false);

        }

        B_GameManager.instance.StartcountDown(true);
    }

    public void activeGame()
    {
        spawner = FindObjectOfType<PancakeSpawner>();
        
        Clock clock = FindObjectOfType<Clock>();
        clock.GameEnd = false;
        Camera.main.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Soundmanager sm = FindObjectOfType<Soundmanager>();
        if (sm != null)
        {
            sm.StopBgm();
            sm.PlayBgmByName(StaticStrings.BattleSceneBGM);
        }
        spawner.startGame();
    }
}

