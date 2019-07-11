using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is used to read all the data coming from the device. For instance,
If arduino send ->
								{"1",
								"2",
								"3",}
readQueue() will return ->
								"1", for the first call
								"2", for the second call
								"3", for the thirst call

This is the perfect script for integration that need to avoid data loose.
If you need speed and low latency take a look to wrmhlReadLatest.
*/
public class wrmhlRead : MonoBehaviour {

	wrmhl myDevice = new wrmhl(); // wrmhl is the bridge beetwen your computer and hardware.

	[Tooltip("SerialPort of your device.")]
	public string portName = "COM8";

	[Tooltip("Baudrate")]
	public int baudRate = 250000;


	[Tooltip("Timeout")]
	public int ReadTimeout = 20;

	[Tooltip("QueueLenght")]
	public int QueueLenght = 1;

    [Tooltip ("TextGivenByTheArduino")]
    public string TextToSplit = "None";

    [Tooltip("TextAfterTheSplit")]
    public string[] SplitedText;

    public ShipController sc;

    bool fireAndShieldLevier = false;
    bool hasFtled = false;
    bool shieldUp = false;
    bool blowing = false;
    bool repairingMecaCoordonnee = false; 


    int numberRepair = 0;
    //Encodeur Rotatif pour les armes
    int storedValueWeapon;
    //Encodeur Rotatif pour l'énergie
    int storedValueEnergy;

    void Start () {
		myDevice.set (portName, baudRate, ReadTimeout, QueueLenght); // This method set the communication with the following vars;
		//                              Serial Port, Baud Rates, Read Timeout and QueueLenght.
		myDevice.connect (); // This method open the Serial communication with the vars previously given.
	}

	// Update is called once per frame
	void Update () {
        TextToSplit = myDevice.readQueue();
        //print(myDevice.readQueue());
        SplitedText = TextToSplit.Split(" "[0]);
        for (int i =0; i< SplitedText.Length; i++)
        {
            
            switch (SplitedText[i])
            {                
                //---------------------------------------------------------------------------------------
                //Branchement : tout se fait avec les câbles
                case "StationWeaponOn:":
                    sc.stationWeaponConnected = true;
                    break;
                case "StationUtilitaryOn:":
                    sc.stationUtilitaryConnected = true;
                    break;
                case "StationMotorOn:":
                    sc.stationMotorConnected = true;
                    break;
                case "StationInfoOn:":
                    sc.stationInfoConnected = true;
                    break;
                case "StationEnergyOff:":
                    sc.stationWeaponConnected = false;
                    break;
                case "StationUtilitaryOff:":
                    sc.stationUtilitaryConnected = false;
                    break;
                case "StationMotorOff:":
                    sc.stationMotorConnected = false;
                    break;
                case "StationInfoOff:":
                    sc.stationInfoConnected = false;
                    break;
                //Branchement end
                //-------------------------------------------------------------------------------------
                //Station Coordonnée
                case "ValPotState:":
                    int valuePot = int.Parse(SplitedText[i + 1]);
                    if (valuePot < 341)
                    {
                        sc.stateUtilitary = StateUtilitary.Move;
                    }
                    else if (valuePot > 341 && valuePot < 682)
                    {
                        sc.stateUtilitary = StateUtilitary.Weapon;

                    }
                    else
                    {
                        sc.stateUtilitary = StateUtilitary.Jump;
                    }
                    //print(SplitedText[i+1]);
                    break;
                case "ValPotX:":
                    float ValuePotX = float.Parse(SplitedText[i + 1]);
                    //sc.xPoint = sc.ConvertedPotX(ValuePotX, 1024) * 65;
                    sc.xPoint = (ValuePotX/1024) * 45; 
                    //sc.xPoint = i+1
                    break;
                case "ValPotY:":
                    float ValuePotY = float.Parse(SplitedText[i + 1]);
                    //sc.ConvertedPotY(ValuePotY, 1024);
                    sc.yPoint = (ValuePotY/1024) * 26;
                    break;

                    case "RepairMecaCoordonneeOn:" :
                        if (repairingMecaCoordonnee == false){
                            repairingMecaCoordonnee = true;
                            numberRepair += 1;
                        }
                        if (numberRepair >= 3){
                            sc.RepairHazard(1);
                            numberRepair = 0;
                        }
                    break;

                    case "RepairMecaCoordonneeOff:" :
                        repairingMecaCoordonnee = false;

                    break;

                    case "RepairElecCoordonneOn:" :

                    break;
                    case "RepairElecCoordonneOff:" :

                    break;
                //Station Coordonnée end
                //----------------------------------------------------------------------------------
                //Station Combat
                //Codeur Rotatif!!!
                case "ChargeWeapon:":
                    int actualValue = int.Parse(SplitedText[i + 1]);
                    if (actualValue != storedValueWeapon)
                    {
                        Debug.Log("charge weapon");
                        storedValueWeapon = actualValue;
                        sc.ChargeWeapon();
                    }
                    else
                    {
                        storedValueWeapon = actualValue;
                        Debug.Log("not charging weapon");
                    }
                    break;
                
                
                // bouton
                case "FireWeaponOn":
                    if (fireAndShieldLevier == false)
                    {
                        if (shieldUp == false)
                        {
                            sc.FireWeapon();
                            fireAndShieldLevier = true;
                        }
                        else
                        {
                            fireAndShieldLevier = true;
                            shieldUp = true;
                            sc.playerActivatedshield = shieldUp;
                        }
                        
                    }
                    break;
                case "FireWeaponOff":
                    if (fireAndShieldLevier == true)
                    {
                        fireAndShieldLevier = false;
                        shieldUp = false;
                        sc.playerActivatedshield = shieldUp;
                    }
                    break;
                //Bouton
                case "ChooseWeaponOne":
                    sc.weaponUsed = WeaponUsed.One;
                    break;
                //Bouton
                case "ChooseWeaponTwo":
                    sc.weaponUsed = WeaponUsed.Two;
                    break;
                //Bouton
                case "ShieldConnected":
                    if (shieldUp == false){
                        sc.shieldActive = true;
                        shieldUp = true;
                    }
                    break;
                case "ShieldDisconnected":
                    shieldUp = false;
                    break;
                //Potentiometre
                case "ShieldOrientation:":
                    float ValOrShield = float.Parse(SplitedText[i + 1]);
                    //sc.ConvertedOrientationShield(ValOrShield, 1024);
                    sc.zRotation = (ValOrShield/1024) * 360;
                    break;

                case "RepairBattleOn" :
                break;
                
                case "RepairBattleOff" :
                break;
                //Station Combat end
                //---------------------------------------------------------------------------------
                //Station Moteur
                //Codeur Rotatif
                case "ChargeEnergy:":
                    int actualValueEnergy = int.Parse(SplitedText[i + 1]);
                    if (actualValueEnergy != storedValueEnergy)
                    {
                        Debug.Log("charge");
                        sc.ChargeEnergy();
                        storedValueEnergy = actualValueEnergy;
                    }
                    else
                    {
                        storedValueEnergy = actualValueEnergy;
                        Debug.Log("not charging");

                    }
                    break;

                //bouton
                case "FtlOn:":
                    hasFtled = true;
                    break;
                case "FtlOff:":
                    hasFtled = false;
                    break;

                //Bouton
                case "MotorOn:":
                    if (sc.stationMotorConnected == true)
                    {
                        sc.MoveShip();
                    }
                    sc.MoveShip();

                    break;
                case "MotorOff:":
                    sc.playerActivatedMotor = false;
                    break;

                //Micro
                case "BlowOn:": //not blowjob dude
                    if (blowing == false)
                    {
                        blowing = true;
                    }
                    break;
                case "BlowOff:":
                    if (blowing == true)
                    {
                        blowing = false;
                    }
                    break;

                case "RepairMotorOn:" :
                break;
                
                case "RepairMotorOff:" :
                break;
                //Station Moteur End

            }

            sc.playerIsBlowing = blowing;
            //Tes Fonctions tes conditions
            //print(SplitedText[i]);
        }
		//print (myDevice.readQueue () ); // myDevice.read() return the data coming from the device using thread.
	}

	void OnApplicationQuit() { // close the Thread and Serial Port
		myDevice.close();
	}
}
