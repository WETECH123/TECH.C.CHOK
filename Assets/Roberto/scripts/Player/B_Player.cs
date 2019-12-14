using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
/*三角関係、Player, PancakeTower,KitchenHelper
 このスクリプトの目的はデ－タを集まることです。
     */
public class B_Player : MonoBehaviour,ITeam,IAttack<Transform>,Iflour
{
    public event Action<stateType> onChangeOrder;
    public event Action manaIsFull;
    public event Action takeIngredient;
    [Header("Settings")]
    public Team team = Team.chocolate;
    public string PlayerName;
    public PancakeTower Tower;
    public KitchenHelper Helper = null;
    public List<B_Player> enemyList = new List<B_Player>();
    public STATUS status;
    [SerializeField]
    WPbullet defaultBullet=null;
    public WPbullet getDefaultBullet()
    {
        return defaultBullet;
    }
    BulletSpawner bulletSpawner=null;
    public BulletSpawner BULLETSPAWNER { get { return bulletSpawner; } set { bulletSpawner = value; } }
    bool isDeath=false;
    PlayerCanvas canvas;
    Animator anim = null;
    [SerializeField]
    GameObject canvasGameObject=null;
    [Header("Orders_Mesage")]
    [SerializeField]
    GameObject message=null,icon=null;
    [SerializeField]
    SpriteRenderer messageImage=null;
    GameObject specialWeaponModel=null;
    GameObject weaponModel=null;
    bool skillIsActive=false;
    AnimatorOverrideController weaponOverride;
   
    //アクションのアイコンのため
    public GameObject getMesage()
    {
        return message;
    }
    public GameObject getIcon()
    {
        return icon;
    }
    //カンバスを渡すため
    public PlayerCanvas getCanvas()
    {
        return canvas;
    }
    ParticleSystem skillparticle;

    [SerializeField]
    Camera c=null;

    private void Start()
    {   Tower.TowerTeam = team;
        Tower.Init(this);
        Helper = Tower.returnHelper();
        Helper.INIT(this, this.team);
        status = new STATUS();
        Gun newGun = new Gun(100,20,1.5f,1,1,1);
        status.changeWeapon(newGun);
        status.getCurrentweapon.changeBullet(defaultBullet);
        status.getCurrentweapon.Attack = 3;
        Invoke("InIt", 2);
        skillparticle = GetComponentInChildren<ParticleSystem>();
        Add_SkillPoints(99);
    }

    //げ－ムマネジャーの処理が終わることを待って、敵のリストを作る
    void InIt()
    {
        
        //敵のリストを作る
        foreach (B_Player p in B_GameManager.instance.playerList)
        {
            if (p.gameObject != this.gameObject)
            {
                enemyList.Add(p);
            }

        }
       
    }

    //死イベント
   
    
    //チムを渡す
    public Team getTeam()
    {
        return team;
    }

    #region orders
   
    public void helperAttack()
    {
      onChangeOrder(stateType.attack);
    }
   
    public void helperDefenceTower()
    {
       onChangeOrder(stateType.defence);
    }
    public void helperMakeCake()
    {
       onChangeOrder(stateType.cake);
    }



    #endregion
    #region events
    public void onDeath(bool v)
    {

        isDeath = v;
        if (isDeath)
        {
            anim.SetTrigger(StaticStrings.death);
        }

    }
    //healthmanagerにイベントを渡す
    public void onDead()
    {
        //activeEvent
        HealthManager health = GetComponentInParent<HealthManager>();
        if (health)
        {
            health.onDeath += this.onDeath;

        }
    }

    #endregion

    #region skill
    public void Add_SkillPoints(float value)
    {
        if (skillIsActive) return;
        status.AddMP(value);

        if (canvas != null)
        {
            canvas.updatePlayerUI();
            if(status.getMP()>=99)
            canvas.skillImageActive(true);
        }
      if (status.getMP() >= 100)
        {
            if (manaIsFull != null)
                manaIsFull();  
        }
    }
    public void activeSkill(bool value)
    {

        StartCoroutine(skillCo(value));
       
    }

    IEnumerator skillCo(bool val)
    {
        weaponModel.SetActive(!val);
        specialWeaponModel.SetActive(val);
        skillIsActive = val;
        yield return new WaitForSeconds(1);
        if (val)
        {
            BulletSpawner sp = specialWeaponModel.GetComponent<BulletSpawner>();
            if (sp != null)
            {
                sp.INITIALIZING(this);
                yield return new WaitForSeconds(0.5f);
                sp.changeAnimator();
            }
            else
            {
                MeleeWeapon wp = specialWeaponModel.GetComponentInChildren<MeleeWeapon>();
                wp.Initialize(this);
                yield return new WaitForSeconds(0.5f);
                wp.changeAnimator();
            }
            
            skillparticle.Play();
        }
        else
        {
            bulletSpawner = weaponModel.GetComponent<BulletSpawner>();
            bulletSpawner.changeAnimator();
            status.UseManapoint(100);
            skillparticle.Stop();
           
            
        }
        if (canvas != null)
        {
            canvas.skillImageActive(false);
            canvas.changeWeaponImage(BULLETSPAWNER.weaponImage);
        }
    }
    #endregion

    #region passVALUE
    public void reciveAnimator(Animator a)
    {
        anim = a;
    }

    //
    public void initializeCanvas()
    {
        onDead();
        //active canvas
        canvas = GetComponentInChildren<PlayerCanvas>();
        if (canvas == null) return;
        canvas.Init(this);
        canvas.updatePlayerUI();
    }

    //
    public void changeMesageImage(Sprite sprite)
    {
        messageImage.sprite = sprite;
    }


    public void assignWeapons(GameObject wp, GameObject sp_wp)
    {
        weaponModel = wp;
        specialWeaponModel = sp_wp;
    }



    public void activeteCanvs()
    {
        canvasGameObject.SetActive(true);

    }

    public void overrideAnimator(AnimatorOverrideController controller)
    {
        if (anim == null) return;
        if (controller == null) return;
        weaponOverride = controller;
        anim.runtimeAnimatorController = weaponOverride;
    }

    public bool returnDeath()
    {
        return isDeath;
    }
    #endregion
    //ダイレクションを渡す、
    public void targetShoot()
    {
        Vector3 dir;
        if (Input.GetButton(StaticStrings.L2_key))
            dir = getCameraDir();

        else dir = transform.forward * 50;
       bulletSpawner.GiveDirection(dir);
    }
    public Vector3 getCameraDir()
    {
        Vector3 cameradir = c.transform.forward * status.getCurrentweapon.bulletRange;
        Vector3 dir = new Vector3(cameradir.x, cameradir.y + 10, cameradir.z);
        return dir;
    }
    //マナーポイントを増やす
    public void attack(Transform t)
    {
        IHealth health = t.GetComponent<IHealth>();
        if (health == null)
        {
            Debug.LogError("Target dont'have healthManagement");
            return;
        }
        health.takeDamage(status.getCurrentweapon.Attack);
        status.AddMP(1);
        if (canvas != null)
            canvas.updatePlayerUI();
    }
    public void onTakingIngredient()
    {
        status.Ingredients++;
        if (takeIngredient != null)
        {
            
            takeIngredient();
        }
    }

    public void setBullet(WPbullet b)
    {
        status.getCurrentweapon.changeBullet(b);
    }

    public Transform getHand()
    {
        throw new NotImplementedException();
    }

    public void activeFlour()
    {
        if (canvas != null)
            canvas.activeFlour();
    }
}


