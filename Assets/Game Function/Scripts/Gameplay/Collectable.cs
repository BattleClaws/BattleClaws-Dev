using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum SpecialCollectableType
{
    Bomb, 
    Ice,
    ShuffleZones,
    Speed
}

public enum CollectableType
{
    Toy,
    Sweets,
    Tech,
    Money
}

public class Collectable : MonoBehaviour
{

    public static int Points { get; private set; }
    public Color Color { get; private set; }

    private GameObject currentMesh;

    public GameObject Mesh
    {
        get
        {
            return currentMesh;
        }
        set
        {
            if(currentMesh)
                Destroy(currentMesh);
            var newMesh = Instantiate(value, transform);
            newMesh.transform.localPosition = Vector3.zero;
            currentMesh = newMesh;
        }
    }
    public PlayerController Holder { get; set; }

    [Header("Group Models")] [SerializeField]
    private List<GameObject> moneyModels;
    [SerializeField]private List<GameObject> toyModels;
    [SerializeField]private List<GameObject> sweetsModels;
    [SerializeField]private List<GameObject> techModels;
    [SerializeField] private List<GameObject> specialModels;
    
    [Header("Special Settings")]
    [Tooltip("Percentage of special collectables: 0-100")]
    [SerializeField]
    private int specialPercent = 5;

    [Tooltip("Is this instance a power-up/round shaker?")]
    [SerializeField]private bool isSpecial;
    [SerializeField] private SpecialCollectableType specialType;
    
    [Space] [Header("Lockdown/Bomb Settings")]
    [SerializeField] private float diffuseTime;
    [SerializeField] private float blastRadius = 500;
    [SerializeField] private float lockdownTime;
    [SerializeField] private float stunTime = 3;

    private void Start()
    {
        currentMesh = transform.GetChild(0).gameObject;
        CollectableSetup();
    }

    private void CollectableSetup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity) && hit.transform.CompareTag("DropZone"))
        {
            GameUtils.instance.InitCollectables();
            Destroy(gameObject);
            return;
        }

        if (!RoundManager.draw)
        {
            Mesh = RandomCollectable();
        }
        else
        {
            Mesh.GetComponent<Renderer>().material.color = Color.yellow;
        }
        
        if (UnityEngine.Random.Range(0, 100) < specialPercent && !RoundManager.draw)
        {
            // Assigning the random power up model and also setting the local SpecialType from the models name
            var chosenModel = specialModels[UnityEngine.Random.Range(0, specialModels.Count )];
            String newSpecialType = chosenModel.name.Split("_")[1];
            
            // This is taking the second half of the prefab name and parsing it as an enum.
            // This ONLY works if the special collectable prefab's name is formatted as such:
            //     if the type value in the enum is: SpecialCollectableType.Bomb
            //     then, the prefabs name MUST follow the format "XXXXX_Bomb"
            // From there it should work fine
            specialType = (SpecialCollectableType)Enum.Parse(typeof(SpecialCollectableType), newSpecialType);
            Mesh = chosenModel;
            isSpecial = true;
        }
    }

    private GameObject RandomCollectable()
    {
        var random = UnityEngine.Random.Range(0, 4);
        List<GameObject> itemPool;

        switch (random)
        {
            case (int) CollectableType.Money:
                itemPool = moneyModels;
                break;
            case (int) CollectableType.Tech:
                itemPool = techModels;
                break;
            case (int) CollectableType.Sweets:
                itemPool = sweetsModels;
                break;
            default:
                itemPool = toyModels;
                break;
        }

        var chosenModel = itemPool[UnityEngine.Random.Range(0, itemPool.Count )];
        return chosenModel;
    }

    public static void SetValue(int value)
    {
        Points = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            if (!Holder || Holder == null)
            {
                GameUtils.instance.InitCollectables();
                Destroy(gameObject);
                return;
            }
            if (isSpecial || RoundManager.draw || other.GetComponent<Renderer>().material.color == Color)
            {
                if (isSpecial)
                {
                    SpecialAction(Holder, other.gameObject);
                }
                else
                {
                    var value = Holder.Properties.AddPoints(Points);
                    GameUtils.ScoreNotication(value, transform);
                }
                Destroy(gameObject);
            }   
        }
    }
    
    public void SpecialAction(PlayerController user, GameObject zone)
    {
        GameUtils.EffectNotification(specialType.ToString(), user.Properties.PlayerNum);
        switch (specialType)
        {
            //case spec:
            //    user.Properties.ApplyMultiplier(2, 10);
            //    break;
            case SpecialCollectableType.Speed:
                user.Properties.ApplySpeed(8, 10);
                break;
            case SpecialCollectableType.ShuffleZones:
                GameUtils.InitDropZones(false);
                break;
            case SpecialCollectableType.Ice:
                FindObjectsOfType<PlayerController>().Where(p => p!=user).ToList()
                    .ForEach(p=> p.Properties.ApplySpeed(1, 2));
                break;
            case SpecialCollectableType.Bomb:
                var newExplosion = Resources.Load<GameObject>("Prefabs/Explosion");
                var explosionInstance = Instantiate(newExplosion, transform.position, Quaternion.identity);
                explosionInstance.GetComponent<ParticleSystem>().Play();
                GameUtils.instance.LockZone(zone);
                Collider[] collidingPlayers = new Collider[10];
                Physics.OverlapSphereNonAlloc(user.Position, blastRadius, collidingPlayers, 6);
                foreach (var collidingPlayer in collidingPlayers)
                {
                    if (collidingPlayer == null)
                        return;
                    print(collidingPlayer.name);
                    var playerData = collidingPlayer.GetComponentInParent<PlayerController>();
                    playerData.Properties.ApplySpeed(1, stunTime);
                }
                break;
        }
    }
}