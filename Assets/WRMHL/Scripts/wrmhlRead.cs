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
                //Branchement
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
                    int ValuePotX = int.Parse(SplitedText[i + 1]);
                    sc.ConvertedPotX(ValuePotX, 1024);
                    break;
                case "ValPotY:":
                    int ValuePotY = int.Parse(SplitedText[i + 1]);
                    sc.ConvertedPotY(ValuePotY, 1024);
                    break;
                //Station Coordonnée en
                //----------------------------------------------------------------------------------
                //Station Combat
                //Codeur Rotatif!!!
                case "ChargeWeapon:":
                    int actualValue = int.Parse(SplitedText[i + 1]);
                    if (actualValue != storedValueWeapon)
                    {
                        Debug.Log("charge weapon");
                        storedValueWeapon = actualValue;
                    }
                    else
                    {
                        storedValueWeapon = actualValue;
                        Debug.Log("not charging weapon");

                    }
                    break;
                case "FireWeaponOn:":
                    break;
                case "FireWeaponOff:":
                    break;
                case "ChooseWeaponOne:":
                    break;
                case "ChooseWeaponTwo:":
                    break;
                case "ShieldConnected:":
                    break;
                case "ShieldUnConnected:":
                    break;
                case "ShieldOrientation:":
                    int ValOrShield = int.Parse(SplitedText[i + 1]);
                    sc.ConvertedOrientationShield(ValOrShield, 1024);
                    break;
                //Station Combat end
                //---------------------------------------------------------------------------------
                //Station Moteur
                case "ChargeEnergy:":
                    int actualValueEnergy = int.Parse(SplitedText[i + 1]);
                    if (actualValueEnergy != storedValueEnergy)
                    {
                        Debug.Log("charge");
                        storedValueEnergy = actualValueEnergy;
                    }
                    else
                    {
                        storedValueEnergy = actualValueEnergy;
                        Debug.Log("not charging");

                    }
                    break;
                case "Ftl:":
                    break;
                case "FtlOff:":
                    break;
                case "MotorOn:":
                    break;
                case "MotorOff:":
                    break;
                case "BlowOn:": //not blowjob dude
                    break;
                case "BlowOff:":
                    break;
                //Station Moteur End

            }
            //Tes Fonctions tes conditions
            //print(SplitedText[i]);
        }
		//print (myDevice.readQueue () ); // myDevice.read() return the data coming from the device using thread.
	}

	void OnApplicationQuit() { // close the Thread and Serial Port
		myDevice.close();
	}
}
