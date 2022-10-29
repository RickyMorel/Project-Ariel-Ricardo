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
    private static Dictionary<string, ResourceQueue> _resources = new Dictionary<string, ResourceQueue>();

    #endregion

    #region Public Properteis

    public static GWorld Instance => _instance;

    public static string FREE_EATINGCHAIR = "FreeEatingChair";
    public static string FREE_SHOPS = "FreeShops";

    public static string EATINGCHAIRS = "eatingChairs";
    public static string SHOPS = "shops";

    #endregion

    static GWorld()
    {
        _world = new WorldStates();
        _eatingChairs = new ResourceQueue("EatingChair", FREE_EATINGCHAIR, _world);
        _shops = new ResourceQueue("Shop", FREE_SHOPS, _world);

        _resources.Add(EATINGCHAIRS, _eatingChairs);
        _resources.Add(SHOPS, _shops);

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
