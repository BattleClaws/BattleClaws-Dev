using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public enum SpecialCollectableType
{
    Lockdown, 
    Ice
}

public class Collectable : MonoBehaviour
{
    public static int Points { get; private set; }
    public Color Color { get; private set; }
    public GameObject Mesh { get; private set; }
    public PlayerController Holder { get; set; }
    
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
        Mesh = transform.GetChild(0).gameObject;
        CollectableSetup();
    }

    private void CollectableSetup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity) && hit.transform.CompareTag("DropZone"))
        {
            GameUtils.InitCollectables();
            Destroy(gameObject);
            return;
        }

        if (UnityEngine.Random.Range(0, 100) < specialPercent && !RoundManager.draw)
        {
            isSpecial = true;
            Mesh.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Collectableglint");
        }

        if (!RoundManager.draw)
        {
            Color = GameUtils.RequestColor();
            Mesh.GetComponent<Renderer>().material.color = Color;
        }
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
                GameUtils.InitCollectables();
                Destroy(gameObject);
                return;
            }
            if (other.GetComponent<Renderer>().material.color == Color)
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
            /*case "DoublePoints":
                user.Properties.ApplyMultiplier(2, 10);
                break;
            case "SpeedBoost":
                user.Properties.ApplySpeed(8, 10);
                break;
            case "ShuffleZones":
                InitDropZones(false);
                break;
            case "FreezeMetal":
                FindObjectsOfType<PlayerController>().Where(p => p!=user).ToList()
                    .ForEach(p=> p.Properties.ApplySpeed(1, 2));
                break;*/
            case SpecialCollectableType.Lockdown:
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
            default:
                break;
        }
    }
}