using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class StationSoundCaller : MonoBehaviour
{

    private static GameObject soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = gameObject;
    }


    public enum StationActions
    {
        StationButtonPressed,
        StationManivelleUsed,
        StationPowerUp,
        StationPowerDown,
        StationBroken,
        StationElectronicBroken,
        StationEnemyUpdate,
        StationObjectiveUpdate
    }


    public static void StationSound(StationActions action)
    {
        EmitterGameEvent eventToTrigger = EmitterGameEvent.None;

        switch (action)
        {
            case StationActions.StationButtonPressed:
                eventToTrigger = EmitterGameEvent.StationBoutton;
                break;

            case StationActions.StationManivelleUsed:
                eventToTrigger = EmitterGameEvent.StationManivelle;
                break;

            case StationActions.StationPowerUp:
                eventToTrigger = EmitterGameEvent.StationPowerup;
                break;

            case StationActions.StationPowerDown:
                eventToTrigger = EmitterGameEvent.StationPowerdown;
                break;

            case StationActions.StationBroken:
                eventToTrigger = EmitterGameEvent.AlarmeVaisseau;
                break;

            case StationActions.StationElectronicBroken:
                eventToTrigger = EmitterGameEvent.AlarmeVaisseau;
                break;

            case StationActions.StationEnemyUpdate:
                eventToTrigger = EmitterGameEvent.AlarmeEnnemi;
                break;

            case StationActions.StationObjectiveUpdate:
                eventToTrigger = EmitterGameEvent.AlarmeInfo;
                break;

        }

        foreach (var emitter in soundManager.GetComponents<StudioEventEmitter>())
        {
                emitter.HandleGameEvent(eventToTrigger);
        }
    }

}
