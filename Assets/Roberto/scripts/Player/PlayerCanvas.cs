using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [Header("Actions")]
    public GameObject actionMenu;
    [SerializeField]
    public Image arrow;
    public RectTransform arrowCenter;
    [SerializeField]
    Sprite cream=null;
    [Header("ingredients")]
    public GameObject ingredientsMenu;
    [SerializeField]
    Image ingredientImage=null;
    public Image[] powerUpsImages;
    public Color col;
    [Header("MP,HP,Skill")]
    [SerializeField]
    Image manaImage = null;
    [SerializeField]
    Slider healthSlider=null;
    [SerializeField]
    Text healthTXT=null;
    [SerializeField]
    Text towerText = null;
    [SerializeField]
    Slider TowerSlider = null;
    [Header("crossAir")]
    public GameObject crossAir = null;
    [SerializeField]
    GameObject skillImage = null;
    [SerializeField]
    Sprite[] targetSprites=null;
    [SerializeField]
    Image[] targetImages=null;
    [SerializeField]
    Image flourImage = null;
    float flourTime = 5;
    //references
    B_Player player;
    HealthManager health;
    int currentImage = 0;
    bool initialized;
    bool isdirty = false;
    [SerializeField]
    Text bombtext=null;
    float alpha = 1;
    [SerializeField]
    Image weaponImage = null;
    [SerializeField]
    Sprite defaultWeaponImageSprite = null;
    [SerializeField] Color chocolate=new Color(), vanilla = new Color(), matcha = new Color(), strawberry = new Color();
    private void Update()
    {
       crossAir.SetActive(Input.GetButton(StaticStrings.L2_key));
        updatePlayerUI();
      
        if (isdirty)
        {
            flourTime -= Time.deltaTime;
            alpha -= (0.2f*Time.deltaTime);
            if(flourImage!=null)
            flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);
            if (flourTime <= 0)
            {
                isdirty = false;
                
            }
        } 
    }
    public void Init(B_Player p)
    {
      
        player=p;
      if (manaImage != null)
            manaImage.fillAmount = p.status.getMP();

        actionMenu.SetActive(false);
        health = GetComponentInParent<HealthManager>();
        updateTowerUI();
        initialized = true;
        if (skillImage != null)
            skillImageActive(false);
    }

    public void updatePlayerUI()
    {
        if (!initialized) return;
        manaImage.fillAmount = player.status.getMP() / 100;
        healthTXT.text = health.getHealth() +" / "+ health.getMaxHealth();
        healthSlider.maxValue = health.getMaxHealth();
        healthSlider.value = health.getHealth();
        if (bombtext == null) return;
        bombtext.text = player.status.BOMB.ToString();
    }
  
   public void updateTowerUI()
    {
        TowerSlider.maxValue = player.Tower.getMaxHealth();
        TowerSlider.value = player.Tower.getHealth();
        towerText.text = player.Tower.getHealth().ToString()+
            " / "+ player.Tower.getMaxHealth();  
    }

    public void goToImage(int value)
    {
        currentImage=value;
        currentImage %= powerUpsImages.Length;
        foreach(var i in powerUpsImages)
        {
            i.color = col;
            
        }
        powerUpsImages[currentImage].color = Color.white;
    }

    public void changeArrowColor(Team tm)
    {
        Color c = new Color();
        switch (tm)
        {
            case Team.chocolate:c = chocolate; break;
            case Team.matcha: c = matcha; break;
            case Team.vanilla: c = vanilla; break;
            case Team.strawberry:c=strawberry; break;
        }

        arrow.color = c;
    }
    public void giveTargetImage(int n, int i)
    {
      
       targetImages[i].sprite = targetSprites[i];

    }
    public void changeWeaponImage(Sprite s)
    {
        if (weaponImage == null) return;
        if (s == null)
        {
            weaponImage.sprite = defaultWeaponImageSprite;
        }
        else
        {
            weaponImage.sprite = s;
        }
        
    }
    public void changeIngredientImage(Sprite s)
    {
        ingredientImage.sprite = s;
    }

    public void skillImageActive(bool v)
    {
        skillImage.SetActive(v);
    }

    public void resetIngredientImage()
    {
        if (cream == null) return;
        changeIngredientImage(cream);
    }
    public void activeFlour()
    {
        if (flourImage == null) return;
        alpha = 1;
        flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);
        flourTime = 5;
        isdirty = true;
    }
}
