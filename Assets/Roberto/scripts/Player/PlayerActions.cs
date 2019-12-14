using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SA;

public class PlayerActions
{
    //powerUpValue
    int value = 0;
    B_Player player = null;
    int Currentstate = 0;
    GameObject actionMenu;
    public List<Transform> targets = new List<Transform>();
    public delegate void powerUpAction(float value);
    public event Action<bool> onSkillActive;
    powerUpAction pwActions;

    bool skillActive = false;
    public Dictionary<string, Sprite> messageImages = new Dictionary<string, Sprite>();
    Animator anim;
    Inputhandler hand;
    float fireRate = 0.2f;
    float fireCounter = 0.2f;
    Vector3 dir;
    float skillTime = 15;
    Sprite retrunSprite(string n)
    {
        if (messageImages.ContainsKey(n))
        {
            return messageImages[n];
        }
        return null;
    }
    #region constructor
    public PlayerActions(B_Player _p, Inputhandler handle,Animator _anim)
    {
        player = _p;
        actionMenu = player.getCanvas().actionMenu;
        foreach (var p in player.enemyList)
        {
            targets.Add(p.transform);
            targets.Add(p.Tower.transform);
            targets.Add(p.Helper.transform);
        }
        
        hand = handle;
        anim = _anim;

        int count = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].GetComponent<B_Player>())
            {
                count = 0;
            }
            else if (targets[i].GetComponent<PancakeTower>())
            {
                count = 1;
            }
            else
            {
                count = 2;
            }

            player.getCanvas().giveTargetImage(i, count);
            //attackCounter = player.status.getCurrentweapon.attackdelay;
        }
        Sprite[] sprites = Resources.LoadAll<Sprite>("UI_Images/Icons/message");
        foreach(var item in sprites)
        {
            messageImages.Add(item.name, item);
        }
        onSkillActive += player.activeSkill;
    }

    #endregion

    #region inputs

    bool goUp = false;
    float x_input;
    //directions inputs
      bool upInput()
    {
        x_input = Input.GetAxis(StaticStrings.Up);
        x_input= x_input > 0 ? 1 : 0;
        if (x_input == 0) { goUp = false; }
        return x_input > 0;
    }

    bool downInput()
    {
        return Input.GetAxis(StaticStrings.Up) < 0;
    }
    bool RightInput()
    {
        return Input.GetAxis(StaticStrings.Right) > 0;
    }
    bool LeftInput()
    {
        return Input.GetAxis(StaticStrings.Right) < 0;
    }

    //backButtons inputs
   
    bool L1input()
    {
        return Input.GetButton(StaticStrings.L1_key);
    }
    bool R2Input()
    {
        
        return Input.GetButton(StaticStrings.R2_key);
    }
    bool R1Input()
    {
        return Input.GetButton(StaticStrings.R1_key);
    }
   
    //symbolsButtons
    bool TriangleInput()
    {
        return Input.GetButtonDown(StaticStrings.Triangle_key);
    }
    bool X_input()
    {
        
        return Input.GetButtonDown(StaticStrings.X_key);
       
    }
    bool CircleInput()
    {
        return Input.GetButtonDown(StaticStrings.Circle_key);
    }
    bool SquareInput()
    {
        return Input.GetButtonDown(StaticStrings.Square_key);
    }
    #endregion


    //UPDATE
    public void InputUpdating()
    {
        if (R2Input())
        {
            fire();
        }
        if (R1Input())
        {
            powerUpsUpdating();
        }
        else
        {
            if (player.getCanvas().ingredientsMenu.activeInHierarchy)
            {
                player.getCanvas().ingredientsMenu.SetActive(false);
            }
        }
       if (L1input())
        {
            ActionsInputs();
            if (CircleInput())
            {
                player.helperAttack();
            }
        }
        if (TriangleInput())
        {
          UseSkill();
        }
        if (SquareInput())
        {
            tossBomb();
        }
        actionMenu.SetActive(L1input());
        player.getMesage().SetActive(L1input());
        player.getIcon().SetActive(!L1input());
        if (skillActive)
        {
            SkillUpdating(Time.deltaTime);
        }
    }

    private void tossBomb()
    {
        if (player.status.BOMB < 1) { return; }
        player.status.BOMB--;
        anim.SetTrigger(StaticStrings.bomb);
    }


    #region PowerUps
    void PowerUp_Helper(float value)
    {
        player.Helper.powerUp(value);
    }
     void PowerUp_Tower(float value)
    {
        player.Tower.powerUp(value);
    }
     void PowerUp_Add_SkillPoints(float value)
    {
        player.Add_SkillPoints(value);
    }

     void powerUpsUpdating()
    {
        if (player.status.Ingredients<1) return;
        if (!player.getCanvas().ingredientsMenu.activeInHierarchy)
        player.getCanvas().ingredientsMenu.SetActive(true);

        if (upInput())
        {
            player.getCanvas().goToImage(0);
            value = 50;
            pwActions = PowerUp_Add_SkillPoints;
        
        }
         if (LeftInput())
        {
            player.getCanvas().goToImage(1);
            value = 5;
            pwActions = PowerUp_Helper;
        }
         if (RightInput())
        {
            player.getCanvas().goToImage(2);
            value = 5;
            pwActions = PowerUp_Tower;
        }
        if (CircleInput())
        {
           if (pwActions != null)
            {
                pwActions(value);
            }
               
            else
            {
                Debug.LogError("pwaAction is null");
            }
            player.status.Ingredients--;
        }

    }
    #endregion

    
    
    #region input_Actions
    public void ActionsInputs()
    {
        
        if (upInput())
           {
            if (!goUp)
            {
                goUp = true;
                Currentstate++;
                Currentstate %= targets.Count;
                hand.reciveTrget(targets[Currentstate]);
                player.getCanvas().arrowCenter.rotation = Quaternion.Euler(0, 0, Currentstate * 360 / 9);
                player.getCanvas().changeArrowColor(targets[Currentstate].GetComponent<ITeam>().getTeam());
            }
            player.changeMesageImage(retrunSprite("Attack_Icon"));


            }

            if (RightInput())
            {
                player.helperDefenceTower();
            player.changeMesageImage(retrunSprite("Defense_Icon"));
        }
            if (LeftInput())
            {
                player.helperMakeCake();
            player.changeMesageImage(retrunSprite("MakePancake"));
        }

            
    }
    #endregion

    #region FightIng
     void Target_and_Shoot()
    {
        player.targetShoot();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting)&&!anim.IsInTransition(0))
            anim.SetTrigger(StaticStrings.shooting);
    }

 
    #endregion

    #region skill
    public void UseSkill()
    {
        if (player.status.getMP() < 100||skillActive) return;
        fireRate = 0.75f;
        fireCounter = 0.75f;
        skillActive = true;
        if(onSkillActive!=null)
        onSkillActive(true);
    }

    public void SkillUpdating(float delta)
    {
        skillTime -= delta;
        if (skillTime <= 0)
        {
            skillTime = 15;
            fireRate = 0.2f;
            fireCounter = 0.2f;
            if(onSkillActive!=null)
            onSkillActive(false);
        }
    }
    #endregion

    void fire()
    {
        fireRate -= Time.deltaTime;
        if (fireRate <= 0)
        {
            fireRate = fireCounter;
            Target_and_Shoot();
        }
       
    }
}

