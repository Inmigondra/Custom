﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateUtilitary { Move, Weapon, Jump }
public enum WeaponUsed { One, Two }

public class ShipController : MonoBehaviour {

    


    [Header("Ship Info")]
    [Range(0,3)]
    public int lifePoint;
    [Range(0,100)]
    public int energiePoint;
    [Range(0, 150)]
    public int heatPoint;
    [Range(0, 100)]
    public int hazardPoint;
    [Range(0,100)]
    public int jumpJauge;
    [Range(0, 100)]
    public int weaponJauge;
    public StateUtilitary stateUtilitary;
    public WeaponUsed weaponUsed;

    public GameObject shield;
    public GameObject pointDestination;

   
    [Header("Parameters For Game")]
    public float speedPoint;
    [Range(0,65)]
    public float xPoint;
    [Range(0, 30)]
    public float yPoint;
    [Range(0.1f, 15)]
    public float rotationSpeed;
    [Range(0.01f, 1)]
    public float shipSpeed;
    [Range(-10, 10)]
    public int xJump;
    [Range(-10, 10)]
    public int yJump;
    public float zRotation;

    public int xRandomJump;
    public int yRandomJump;
    bool isSettingJump;

    public bool stationWeaponConnected = false;
    public bool stationUtilitaryConnected = false;
    public bool stationMotorConnected = false;
    public bool stationInfoConnected = false;
    public bool stationShieldConnected = false;

    public bool stationWeaponHazard = false;
    public bool stationutilitaryHazard = false;
    public bool stationMotorHazard = false;

    public bool playerIsBlowing = false;
    public bool playerActivatedshield = false;
    public bool playerActivatedMotor = false;
    bool blowCoroutineStarted = false;

    public KeyCode keyMove;
    public KeyCode chargeWeapon;
    public KeyCode shieldUp;
    public KeyCode fireWeapon;
    public KeyCode chargeEnergy;
    public KeyCode blowKey;
    public bool shieldActive;


    public float xTest;


    public float ConvertedPotX(int value, int max)
    {
        float converted = (value / max);
        float convertedTwo = converted * 45;
        return converted;
    }
    public float ConvertedPotY(int value, int max)
    {
        float converted = (value / max);
        float convertedTwo = converted * 26;
        yPoint = convertedTwo;
        return yPoint;
    }   
    public float ConvertedOrientationShield(int value, int max)
    {
        float converted = (value / max);
        float convertedTwo = converted * 360;
        zRotation = convertedTwo;
        return convertedTwo;
    }
    private void Awake()
    {
        shieldActive = false;
        shield.SetActive(false);
        isSettingJump = false;
        SetRandomCoordinate();
    }
    // Use this for initialization
    void Start () {
        StartCoroutine(JumpJaugeUp());
        StartCoroutine(HeatLoss());
	}
	
	// Update is called once per frame
	void Update () {
        switch (stateUtilitary)
        {
            case StateUtilitary.Move:
                SetMovePoint();
                if (Input.GetKey(keyMove))
                {
                    MoveShip();
                    if (Input.GetKeyDown(keyMove))
                    {
                        StartCoroutine(EnergyMovementDown());
                    }
                }
                break;
            case StateUtilitary.Weapon:
                if (shieldActive == false)
                {
                    if (Input.GetKeyDown(chargeWeapon))
                    {
                        ChargeWeapon();
                    }
                    if (Input.GetKeyDown(fireWeapon))
                    {
                        FireWeapon();
                    }
                    if (Input.GetKeyDown(shieldUp))
                    {
                        StartCoroutine(ShieldUp());
                    }
                }
                break;
            case StateUtilitary.Jump:
                if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    if (isSettingJump == false)
                    {
                        StartCoroutine(SetJumpCoordinate());
                    }
                }
                else
                {
                    isSettingJump = false;
                }

                if (Input.GetKeyDown(keyMove))
                {
                    DoTheJump();
                }
                break;
        }

        if (shieldActive == true)
        {
            shield.SetActive(true);
            ShieldRotation();
        }
        else
        {
            shield.SetActive(false);
        }
        
        if (hazardPoint >= 100)
        {
            HazardEvent();
        }


        
       if (Input.GetKeyDown(chargeEnergy))
        {
            ChargeEnergy();
        }

       if (Input.GetKeyDown(blowKey))
        {
            playerIsBlowing = true;
            StartCoroutine(CoolTheHeat());
        }
        else if (Input.GetKeyUp(blowKey)){
            playerIsBlowing = false;
        }

       //----------------ARDUINO--------------------------------

       if (playerIsBlowing == true && blowCoroutineStarted == false)
        {
            StartCoroutine(CoolTheHeat());
            blowCoroutineStarted = true;
        }

       if (playerActivatedshield == true && shieldActive == false)
        {
            StartCoroutine(ShieldUp());
        }

	}
    //a setup pour du potentiomètre (0;1024)
    //X [-20;32]
    //Y [-18;20]
    public void SetMovePoint()
    {
        /* if (Input.GetButton("Horizontal"))
         {
             xPoint += Input.GetAxis("Horizontal") * speedPoint;
             //xPoint = (int)xPoint;
             xPoint = Mathf.Clamp(xPoint, 0, 65);
         }
         if (Input.GetButton("Vertical"))
         {
             yPoint += Input.GetAxis("Vertical") * speedPoint;
             //yPoint = (int)yPoint;
             yPoint = Mathf.Clamp(yPoint, 0, 30);
         }*/
        pointDestination.transform.position = new Vector2(xPoint, yPoint);
    }
    public void MoveShip()
    {

        if (Vector3.Distance(transform.position, pointDestination.transform.position) > 0.01f)
        {
            Vector2 direction = pointDestination.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, pointDestination.transform.position, shipSpeed);
            //energiePoint -= 3;
        }
        if (playerActivatedMotor == false)
        {
            playerActivatedMotor = true;
            StartCoroutine(EnergyMovementDown());
            
        }
    }
    public void HeatToHazard ()
    {

    }
    public void HazardEvent()
    {
        hazardPoint = 0;
        Debug.Log("HAZARD MAGGLE!");
    }
    public void ChargeWeapon()
    {
        weaponJauge += 5;
    }
    public void FireWeapon()
    {
        if (weaponJauge >= 100)
        {
            weaponJauge = 0;
            heatPoint += 20;
        }
        else
        {
            weaponJauge = 0;
            heatPoint += 20;
            hazardPoint += 10;
        }
    }
    public void ShieldRotation()
    {
        shield.transform.rotation = Quaternion.Euler(0,0, zRotation);
    }
    public void Repair()
    {

    }
    public void ChargeEnergy()
    {
        energiePoint += 5;
    }
    public void SetRandomCoordinate()
    {
        xRandomJump = Random.Range(-10, 10);
        yRandomJump = Random.Range(-10, 10);
    }
    public void DoTheJump()
    {
        if (xJump == xRandomJump && yJump == yRandomJump)
        {
            if (xRandomJump == xJump && yRandomJump == yJump)
            {
                if (jumpJauge == 100)
                {
                    Debug.Log("JumpCorrect");
                    SetRandomCoordinate();
                }
                else
                {
                    hazardPoint += 15;
                    SetRandomCoordinate();
                }
            }
        }else
        {
            hazardPoint += 20;
        }
    }
    public void RepairHazard (int toRepair)
    {
        if (toRepair == 0){
            bool stationWeaponHazard = false;
        }
        if (toRepair == 1){
            bool stationutilitaryHazard = false;
        }
        if (toRepair == 2){
         bool stationMotorHazard = false;
            
        }
        
    }
    private IEnumerator HeatLoss()
    {
        while (true)
        {
            if (heatPoint <= 0)
            {
                heatPoint = 0;
            }
            else
            {
                heatPoint -= 2;
            }

            if (heatPoint >= 100)
            {
                lifePoint -= 1;
                heatPoint -= 20;
            }
            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator ShieldUp()
    {
        shieldActive = true;
        energiePoint -= 40;
        shieldActive = true;
        yield return new WaitForSeconds(5f);
        shieldActive = false;
        yield return null;
    }
    //a adapter pour un bouton
    private IEnumerator EnergyMovementDown ()
    {

        while (playerActivatedMotor == true)
        {
            energiePoint -= 2;
            yield return new WaitForSeconds(1);
            if (playerActivatedMotor == false)
            {
                yield break;
            }
        }
        
    }
    private IEnumerator JumpJaugeUp()
    {
        while (true)
        {
            jumpJauge += 1;
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator SetJumpCoordinate()
    {
        while (isSettingJump == false)
        {
            isSettingJump = true;
            yield return null;
        }
        while (isSettingJump == true)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                xJump += 1 * (int)Input.GetAxis("Horizontal");
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                yJump += 1 * (int)Input.GetAxis("Vertical");
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
    private IEnumerator CoolTheHeat()
    {
        while (playerIsBlowing == true)
        {
            heatPoint -= 10;
            yield return new WaitForSeconds(1.5f);
        }
        if (playerIsBlowing == false)
        {
            blowCoroutineStarted = false;
            yield break;
        }
    }

    public void TestCircle()
    {
        Vector2 pointTest = new Vector2(Mathf.Cos(xTest * Mathf.Deg2Rad) , Mathf.Sin(xTest * Mathf.Deg2Rad));
        Debug.Log(pointTest);
    }
}
