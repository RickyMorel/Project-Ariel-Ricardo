using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceQueue
{
    public Queue<GameObject> Queue = new Queue<GameObject>();
    public string Tag;
    public string ModState;

    public ResourceQueue(string tag, string modState, WorldStates worldStates)
    {
        Tag = tag;
        ModState = modState;

        if(tag != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject resource in resources)
                Queue.Enqueue(resource);
        }

        if(modState != "")
        {
            worldStates.ModifyState(modState, Queue.Count);
        }
    }

    public void AddResource(GameObject resource)
    {
        Queue.Enqueue(resource);
    }

    public GameObject RemoveResource()
    {
        if(Queue.Count == 0) { return null; }

        return Queue.Dequeue();
    }
}

public sealed class GWorld
{
    #region Private Variables

    private static readonly GWorld _instance = new GWorld();
    private static WorldStates _world;
    private static ResourceQueue _eatingChairs;
    private static ResourceQueue _shops;
    private static ResourceQueue _hideLocations;
    private static ResourceQueue _restLocations;
    private static ResourceQueue _shipAttackPoints;
    private static ResourceQueue _healPoints;
    private static Dictionary<string, ResourceQueue> _resources = new Dictionary<string, ResourceQueue>();

    #endregion

    #region Public Properteis

    public static GWorld Instance => _instance;

    public static string FREE_EATINGCHAIR = "FreeEatingChair";
    public static string FREE_SHOPS = "FreeShops";
    public static string FREE_HIDE_LOCATIONS = "FreeHideLocation";
    public static string FREE_REST_LOCATIONS = "FreeRestLocation";
    public static string FREE_SHIP_ATTACK_POINTS = "FreeShipAttackPoint";
    public static string FREE_HEAL_POINTS = "FreeHealPoint";

    public static string EATINGCHAIRS = "eatingChairs";
    public static string SHOPS = "shops";
    public static string HIDE_LOCATIONS = "hideLocations";
    public static string REST_LOCATIONS = "restLocations";
    public static string SHIP_ATTACK_POINTS = "shipAttackPoints";
    public static string HEAL_POINTS = "healPoints";

    #endregion

    static GWorld()
    {
        _world = new WorldStates();
        _eatingChairs = new ResourceQueue("EatingChair", FREE_EATINGCHAIR, _world);
        _shops = new ResourceQueue("Shop", FREE_SHOPS, _world);
        _hideLocations = new ResourceQueue("HideLocation", FREE_HIDE_LOCATIONS, _world);
        _restLocations = new ResourceQueue("RestLocation", FREE_REST_LOCATIONS, _world);
        _shipAttackPoints = new ResourceQueue("ShipAttackPoint", FREE_SHIP_ATTACK_POINTS, _world);
        _healPoints = new ResourceQueue("HealPoint", FREE_HEAL_POINTS, _world);

        _resources.Add(EATINGCHAIRS, _eatingChairs);
        _resources.Add(SHOPS, _shops);
        _resources.Add(HIDE_LOCATIONS, _hideLocations);
        _resources.Add(REST_LOCATIONS, _restLocations);
        _resources.Add(SHIP_ATTACK_POINTS, _shipAttackPoints);
        _resources.Add(HEAL_POINTS, _healPoints);

        //Leave this here for future testing
        //Time.timeScale = 5;
    }

    public ResourceQueue GetQueue(string type)
    {
        return _resources[type];
    }

    private GWorld()
    {

    }

    public WorldStates GetWorld()
    {
        return _world;
    }
}
