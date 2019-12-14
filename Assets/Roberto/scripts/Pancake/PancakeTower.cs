using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PancakeTower : MonoBehaviour,ITeam,IHealth,IBurn
{
    [SerializeField]
    Transform flag = null;
    [SerializeField]
    Slider healthbar = null;
    [SerializeField]
    float spawnOffset=5;
    [SerializeField]
    Text pointText=null;
    #region values
    float health=25;
    float maxHealh = 25;
    BoxCollider b_collider;
    Vector3 flagPos = new Vector3();
    Vector3 flagStartPos = new Vector3();
    public float getMaxHealth()
    {
        return maxHealh;
    }
    public float getHealth()
    {
        return health;
    }
    float height;
    public float Height { get { return height; } set { height = value; } }
    KitchenHelper helper = null;
    public KitchenHelper HelperPrefab;
    Team towerteam;
    public Team TowerTeam { get { return towerteam; } set { towerteam = value; } }

    public bool isBurning { get ; set ; }
    public float fireLifeTime { get; set ; }
    public float damageforSecond { get ; set ; }
    public float damageDelay { get ; set; }
    public float damageDelayCounter { get ; set ; }

    public GameObject piece;
    public GameObject basePiece,Burnedpancake;
    bool invulnerability;
    float invulnerabilityTime = 3;
    float invulnerabilityCounter = 3;
    List<GameObject> pieces = new List<GameObject>();
    B_Player player;
    #endregion

 
    private void Update()
    {
        pointText.text = height.ToString();
       
        if (invulnerability)
        {
            invulnerabilityCounter -= Time.deltaTime;
            if (invulnerabilityCounter <= 0)
            {
                invulnerability = false;
                invulnerabilityCounter = invulnerabilityTime;
            }
        }
        if (healthbar == null) return;
        healthbar.value = health;
        if (isBurning)
        {
            getFireDamage();
        }
    }
    //最初はヘルパーをスポーンする
    public void Init(B_Player p)
    {
        player = p;
        if (helper == null)
        {
         Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnOffset);
         KitchenHelper newHelper= Instantiate(HelperPrefab, spawnPoint, transform.rotation)as KitchenHelper;
         helper = newHelper;
         helper.towerScript = this;
         Transform gameScene = GameObject.FindGameObjectWithTag(StaticStrings.gameScene).transform;
         helper.transform.SetParent(gameScene); 
        }
        updateTowerUI();
        b_collider = GetComponent<BoxCollider>();
        if (flag != null)
        {
            flagPos = flag.transform.localPosition;
            flagStartPos = flagPos;
        }
        if (healthbar == null) return;
        healthbar.maxValue = maxHealh;
        Height = 0;
        fireLifeTime = 8;
        damageforSecond = 1;
        damageDelayCounter = 1;
        damageDelay = 1;
    }

    public Vector3 lastBoxHeight()
    {
      Vector3 pos= new Vector3(0, basePiece.transform.localPosition.y + height, 0);
        return pos;
    }
   
    //プレやに渡すため
    public KitchenHelper returnHelper() { return helper; }

    public Team getTeam()
    {
        return towerteam;
    }

    
    //大きくなる関数
   public void Healing(float cm)
  {
        health += cm;
        Build(cm);
        updateTowerUI();
  }
    //減る関数
    public void takeDamage(float dmg)
    {
        if (invulnerability&&height==0) return;
        health -= dmg;
        if (health <= 0)
        {
            DestroyTower();
        }
        updateTowerUI();
    }
    public void DestroyTower()
    {
        invulnerability = true;
        if(Soundmanager.instance!=null)
        Soundmanager.instance.PlaySeByName(StaticStrings.towerBreak);
        var a = Enumerable.Range(0, pieces.Count).Reverse();
        foreach (var p in a)
        {
            if (pieces.Count > 0)
            {
                GameObject newPankake = Instantiate(Burnedpancake, pieces[p].transform.position, Quaternion.identity);
                Destroy(pieces[p].gameObject);
                pieces.Remove(pieces[p]);
                height--;
                b_collider.size = new Vector3(b_collider.size.x, b_collider.size.y - 1, b_collider.size.z);
                height = height <= 0 ? 0 : height;
            }

        }
        
        health = maxHealh;
        if (flag == null) return;
        flagPos = flagStartPos;
        flag.transform.localPosition = flagPos;
    }

    public void powerUp(float value)
    {
        maxHealh += value;
        health += value;
        updateTowerUI();
        if (healthbar == null) return;
        healthbar.maxValue = maxHealh;
    }
    public void Build(float cm)
    {

        height += cm;
        GameObject newPiece = Instantiate(piece, transform.position, transform.rotation);
        newPiece.transform.SetParent(basePiece.transform);
        newPiece.transform.localPosition = lastBoxHeight();
        newPiece.name = "Piece " + height;
        pieces.Add(newPiece);
        b_collider.size = new Vector3(b_collider.size.x, b_collider.size.y + 1, b_collider.size.z);
        updateFlag(cm);
    }
    
    public void updateTowerUI()
    {
        if (player.getCanvas() != null)
            player.getCanvas().updateTowerUI();
    }

    void updateFlag(float valuer)
    {
        if (flag == null) return;
        flagPos = new Vector3(flagPos.x, flagPos.y + valuer, flagPos.z);
        flag.transform.localPosition = flagPos;
    }

    public void burn()
    {
        if(!isBurning)
        isBurning = true;
    }

    public void getFireDamage()
    {
        fireLifeTime -= Time.deltaTime;
        damageDelayCounter -= Time.deltaTime;
        if(damageDelayCounter<=0)
        {
            damageDelayCounter = damageDelay;
            takeDamage(damageforSecond);
        }
        if (fireLifeTime <= 0)
        {
            isBurning = false;
            fireLifeTime = 8;
            damageDelayCounter = damageDelay;
        }
    }

    public Transform getHand()
    {
        throw new System.NotImplementedException();
    }
}
