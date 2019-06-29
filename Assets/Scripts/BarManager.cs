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

    public Image jumpBar;
    [Range(0, 1)]
    public float jumpFill;
    public ShipController sC;

    public Text xToJumpText;
    public int xToJump;

    public Text yToJumpText;
    public int yToJump;

    public Text xSetToJumpText;
    public int xSetToJump;

    public Text ySetToJumpText;
    public int ySetToJump;

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
    float JumpFillSet()
    {
        float converted = (float)sC.jumpJauge / 100f;
        return converted;
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        healthFill = HealthFillSet();
        energyFill = EnergyFillSet();
        heatFill = HeatFillSet();
        weaponFill = WeaponFillSet();
        jumpFill = JumpFillSet();


        healthBar.fillAmount = healthFill;
        energyBar.fillAmount = energyFill;
        heatBar.fillAmount = heatFill;
        weaponBar.fillAmount = weaponFill;
        jumpBar.fillAmount = jumpFill;

        xToJump = sC.xRandomJump;
        SetText(xToJumpText, xToJump);

        yToJump = sC.yRandomJump;
        SetText(yToJumpText, yToJump);

        xSetToJump = sC.xJump;
        SetText(xSetToJumpText, xSetToJump);

        ySetToJump = sC.yJump;
        SetText(ySetToJumpText, ySetToJump);


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
