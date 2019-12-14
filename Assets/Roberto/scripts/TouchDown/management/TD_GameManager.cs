using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace td
{
    public class TD_GameManager : MonoBehaviour
    {
        [SerializeField]
        GameObject world = null, gameEndCutScene = null;
        [SerializeField]
        Sprite[] numberSprites = null;
        [SerializeField]
        Image Number = null;
        int maxPoint = 3;
        List<GameObject> resettableObjects = new List<GameObject>();
        [SerializeField]
        GameObject Ymascotte=null,Bmascotte=null;
        [SerializeField]
        GameObject _camera=null, followCamera=null;
        public Td_Player playerA=new Td_Player();
        public Td_Player playerB =new Td_Player();
        string dance = "";
        Td_Player possessor;
        string winner = "";
        
        Vector3 playerSpawnPos = Vector3.zero;
        #region returnValues
        
        public Td_Player returnPossessor()
        {
            return possessor;
        }
        public void GivePossessor(Team p)
        {
            if (p == playerA.t)
            {
                possessor = playerA;
            }
            else
            {
                possessor = playerB;
            }
            
        }
      
#endregion  
        [SerializeField]
        Text playerAtext=null;
        [SerializeField]
        Text playerBtext = null;
        bool canMakePoint = true;
        [SerializeField]
        Transform[] camPositions =null;

        #region resetting
        public IEnumerator resetStatus()
        {
            foreach (var item in resettableObjects)
            {
                if (!item.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                }
                TD_Character c = item.GetComponent<TD_Character>();
                if (c != null)
                {
                    c.reset();
                }
               
            }
            yield return null;
            
        }
        public void setResetableObjects(GameObject r)
        {
            resettableObjects.Add(r);  
        }

    
#endregion

        #region init
        private void Start()
        {
            TD_TeamDirector[] director = FindObjectsOfType<TD_TeamDirector>();
            foreach (var item in director)
            {
                item.Init(this);
            }
            playerAtext.text = playerA.point.ToString();
            playerBtext.text = playerB.point.ToString();
            TD_InputHandler[] inputhandler = FindObjectsOfType<TD_InputHandler>();
            foreach (var v in inputhandler)
            {
                v.InitInGame();
            }
            StartCoroutine(countDownCo());

        }
        #endregion
        IEnumerator pointShow(Team t)
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("RefereeWhistle");
            foreach(var v  in resettableObjects)
            {
                TD_Character c = v.GetComponent<TD_Character>();
               if(c!=null)
                 c.block();
            }
            yield return new WaitForSeconds(1);
            if (Soundmanager.instance != null)
                Soundmanager.instance.musicPause();
            followCamera.SetActive(false);
            _camera.SetActive(true);
            yield return new WaitForSeconds(1);
            if (t == playerA.t)
            {
                danceCheck(Ymascotte);
                _camera.transform.position = camPositions[0].position;
               
            }
            else
            {
                danceCheck(Bmascotte);
                _camera.transform.position = camPositions[1].position;
               
            }
            
            yield return new WaitForSeconds(5);
            goTonextGame();
            yield return resetStatus();
            yield return new WaitForSeconds(0.5f);
            followCamera.SetActive(true);
            _camera.SetActive(false);
            canMakePoint = true;
            if (Soundmanager.instance != null)
                Soundmanager.instance.musicResume();
        }
        #region game
        public void addPointToPlayer(Team tm)
        {
            if (!canMakePoint) return;
            canMakePoint = false;
            int value = 0;
            if (tm == playerA.t)
            {
                playerA.point++;
                value = playerA.point;
            }
            else
            {
                playerB.point++;
                value = playerB.point;
            }
            if (value >= maxPoint)
            {
                if (playerA.point > playerB.point)
                {
                    winner = playerA.t.ToString();
                }
                else
                {
                    winner = playerB.t.ToString();
                }
                Win(tm);
            }
            else
            {
                StartCoroutine(pointShow(tm));
            }
            playerAtext.text = playerA.point.ToString();
            playerBtext.text = playerB.point.ToString();
        }
        IEnumerator gameEndCo()
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("RefereeWhistle");
            yield return new WaitForSeconds(1);
            world.SetActive(false);
            yield return new WaitForSeconds(1);
            Soundmanager.instance.PlaySeByName("WinSound");
            gameEndCutScene.SetActive(true);
            CutSene c = gameEndCutScene.GetComponent<CutSene>();
            c.reciveMessage("Team " + winner + " Wins");
 
        }
        void goTonextGame()
        {
            TD_PancakeBall.instance.resetStatus();
        }
        private void Win(Team tm)
        {
            StartCoroutine(gameEndCo());
        }

        #endregion
        void danceCheck(GameObject obj)
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("Stadium");
            int rnd = UnityEngine.Random.Range(1, 5);
            switch (rnd)
            {
                case 1: dance = "DANCE";
                    break;
                case 2: dance = "SALSA1";
                    break;
                case 3: dance = "SALSA2";
                    break;
                case 4: dance = StaticStrings.samba;
                    break;
            }
            obj.GetComponent<Animator>().SetTrigger(dance);
        }

        IEnumerator countDownCo()
        {
            if(Soundmanager.instance!=null)
            Soundmanager.instance.PlaySeByName(StaticStrings.CountDownSoundEffect);
            Number.enabled = true;
            Number.sprite = numberSprites[0];
            yield return new WaitForSeconds(1);
            Number.sprite = numberSprites[1];
            yield return new WaitForSeconds(1);
            Number.sprite = numberSprites[0];
            yield return new WaitForSeconds(1);
            Number.enabled = false;
           
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlayBgmByName("TouchdownModeBGM");

        }
    }
    public class Td_Player
    {
        
         public int point;
         public Team t;
         public Transform door;
    }
}
