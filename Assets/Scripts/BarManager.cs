using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour {

    public Image healthBar;
    [Range(0, 1)]
    public float healthFill;

    public Image energyBar;
    [Range(0, 1)]
    public float energyFill;

    public Image heatBar;
    [Range(0, 1)]
    public float heatFill;

    public Image weaponBar;
    [Range(0, 1)]
    public float weaponFill;

    /* public Image jumpBar;
    [Range(0, 1)]
    public float jumpFill;*/

    public ShipController sC;

    public Text xToJumpText;
    public int xToJump;

    public Text yToJumpText;
    public int yToJump;

    public Text xSetToJumpText;
    public int xSetToJump;

    public Text ySetToJumpText;
    public int ySetToJump;

    public GameObject motorOnObject, motorOffObject, motorFailObject;
    public GameObject coordOnObject, coordOffObject, coordFailObject;
    public GameObject weaponOnObject, weaponOffObject, weaponFailObject;
    public GameObject shieldOnObject, shieldOffObject, shieldFailObject;



    float HealthFillSet()
    {
        float converted = (float)sC.lifePoint / 3f;
        return converted;
    }
    float EnergyFillSet()
    {
        float converted = (float)sC.energiePoint / 100f;
        return converted;
    }
    float HeatFillSet()
    {
        float converted = (float)sC.heatPoint / 100f;
        return converted;
    }
    float WeaponFillSet()
    {
        float converted = (float)sC.weaponJauge / 100f;
        return converted;
    }
    /*float JumpFillSet()
    {
        float converted = (float)sC.jumpJauge / 100f;
        return converted;
    }*/
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        healthFill = HealthFillSet();
        energyFill = EnergyFillSet();
        heatFill = HeatFillSet();
        weaponFill = WeaponFillSet();
        //jumpFill = JumpFillSet();


        healthBar.fillAmount = healthFill;
        energyBar.fillAmount = energyFill;
        heatBar.fillAmount = heatFill;
        weaponBar.fillAmount = weaponFill;
        //jumpBar.fillAmount = jumpFill;

        xToJump = sC.xRandomJump;
        SetText(xToJumpText, xToJump);

        yToJump = sC.yRandomJump;
        SetText(yToJumpText, yToJump);

        xSetToJump = sC.xJump;
        SetText(xSetToJumpText, xSetToJump);

        ySetToJump = sC.yJump;
        SetText(ySetToJumpText, ySetToJump);

        //Utilitary
        if (sC.stationUtilitaryConnected == true && sC.stationutilitaryHazard == false){
            coordOnObject.SetActive(true);
            coordOffObject.SetActive(false);
            coordFailObject.SetActive(false);

        }else if (sC.stationUtilitaryConnected == false && sC.stationutilitaryHazard == false){
            coordOnObject.SetActive(false);
            coordOffObject.SetActive(true);
            coordFailObject.SetActive(false);

        }else if (sC.stationutilitaryHazard == true){
            coordOnObject.SetActive(false);
            coordOffObject.SetActive(false);
            coordFailObject.SetActive(true);
        }
        //Weapon & shield
        if (sC.stationWeaponConnected == true && sC.stationShieldConnected == false && sC.stationWeaponHazard == false){
            weaponOnObject.SetActive(true);
            weaponOffObject.SetActive(false);
            weaponFailObject.SetActive(false);

            shieldOnObject.SetActive(false);
            shieldOffObject.SetActive(true);
            shieldFailObject.SetActive(false);
        }else if (sC.stationWeaponConnected == false && sC.stationShieldConnected == true && sC.stationWeaponHazard == false){
            weaponOnObject.SetActive(false);
            weaponOffObject.SetActive(true);
            weaponFailObject.SetActive(false);


            shieldOnObject.SetActive(false);
            shieldOffObject.SetActive(true);
            shieldFailObject.SetActive(false);

        }else if (sC.stationWeaponHazard == true){
            weaponOnObject.SetActive(false);
            weaponOffObject.SetActive(false);
            weaponFailObject.SetActive(true);


            shieldOnObject.SetActive(false);
            shieldOffObject.SetActive(false);
            shieldFailObject.SetActive(true);
        }
        if (sC.stationWeaponConnected == false && sC.stationWeaponHazard == false){
            weaponOnObject.SetActive(false);
            weaponOffObject.SetActive(true);
            weaponFailObject.SetActive(false);

        }
        if (sC.stationShieldConnected == true && sC.stationWeaponConnected == true && sC.stationWeaponHazard == false){
            weaponOnObject.SetActive(false);
            weaponOffObject.SetActive(true);
            weaponFailObject.SetActive(false);


            shieldOnObject.SetActive(true);
            shieldOffObject.SetActive(false);
            shieldFailObject.SetActive(false);

        }

        //Motor
        if (sC.stationMotorConnected == true && sC.stationMotorHazard == false){
            motorOnObject.SetActive(true);
            motorOffObject.SetActive(false);
            motorFailObject.SetActive(false);

        }else if (sC.stationMotorConnected == false && sC.stationMotorHazard == false){
            motorOnObject.SetActive(false);
            motorOffObject.SetActive(true);
            motorFailObject.SetActive(false);
        }else if (sC.stationMotorHazard == true){
            motorOnObject.SetActive(false);
            motorOffObject.SetActive(false);
            motorFailObject.SetActive(true);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="toConvert"></param>
    /// <param name="converted"></param> le float entre [0,1] sorti à la fin
    /// <param name="max"></param> determine le max de la valeur à convertir
    void SetPercentage(int toConvert, float converted, int max)
    {
        converted = (float)toConvert / (float)max;
        Debug.Log(converted);
    }

    void SetText(Text textToChange, int valueToDisplay)
    {
        textToChange.text = valueToDisplay.ToString();
    }
}
