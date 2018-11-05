using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    Rigidbody2D InstanceRigid;

    public int InstanceNumOrValue; //Num for normal item and value for gold
    public List<int[]> InstanceSpecials = new List<int[]>();
    [SerializeField] bool DisappearAfterTime;

    float LifeTime;
    float BornTime;
    BoxCollider2D InstanceBox;
    CircleCollider2D InstanceCircle;

    Animator InstanceAnim;
    int DestroyedHarsh = Animator.StringToHash("Destroyed");
    int DisappearHarsh = Animator.StringToHash("Disappear");

    void Awake()
    {
        InstanceRigid = GetComponent<Rigidbody2D>();
        InstanceCircle = GetComponent<CircleCollider2D>();
        InstanceBox = GetComponent<BoxCollider2D>();
        InstanceAnim = GetComponent<Animator>();
        BornTime = Time.time;

        //Item bounces on creation
        if (DisappearAfterTime)
        {
            float RandomDirection = Random.Range(-2f, 2f);
            InstanceRigid.velocity = new Vector2(RandomDirection, 2f);
        }
    }

    void Update()
    {
        LifeTime = Time.time - BornTime;

        if (DisappearAfterTime)
        {
            if (LifeTime > 7f)
                InstanceAnim.SetTrigger(DisappearHarsh);

            if (LifeTime > 10f)
                DestroyInstance();
        }
    }

    public void RandomizeSpecials()
    {
        //Initializing random special bonuses for the item
        int ChanceOfQuantity = Random.Range(1, 101);
        int Quantity;
        if (ChanceOfQuantity <= 10) //10% of having 3 bonuses
            Quantity = 3;
        else if (ChanceOfQuantity > 10 && ChanceOfQuantity <= 30) //20% of having 2 bonuses
            Quantity = 2;
        else if (ChanceOfQuantity > 30 && ChanceOfQuantity <= 60) //30% of having 1 bonus
            Quantity = 1;
        else Quantity = 0; //40% of having no bonus

        int[] RandType = new int[] { Random.Range(0, 9), Random.Range(0, 9), Random.Range(0, 9) };

        //All bonuses must be different
        for (int i = 0; i < Quantity; i++)
            for (int j = i + 1; j < Quantity; j++)
                if (RandType[i] == RandType[j])
                    while (RandType[i] == RandType[j])
                        RandType[j] = Random.Range(0, 8);

        int[] RandMainValue = new int[] { Random.Range(1, 4), Random.Range(1, 4), Random.Range(1, 4) };
        int[] RandSubValue = new int[] { Random.Range(2, 11), Random.Range(0, 11), Random.Range(0, 11) };
        for (int i = 0; i < Quantity; i++)
        {
            if (RandType[i] < 3)
            {
                if (RandMainValue[i] == 0)
                    continue;
                else InstanceSpecials.Add(new int[] { RandType[i], RandMainValue[i] });
            }
            else
            {
                if (RandSubValue[i] == 0)
                    continue;
                else InstanceSpecials.Add(new int[] { RandType[i], RandSubValue[i] });
            }
        }
    }

    public void InitializeUniqueSpecials()
    {
        Jewelry tempJewelry;
        if (tag == "Amulet") //Unique amulets
            ItemDatabase.Instance.GetAmulets.TryGetValue(InstanceNumOrValue, out tempJewelry);
        else ItemDatabase.Instance.GetRings.TryGetValue(InstanceNumOrValue, out tempJewelry); //Unique rings
        for (int i = 0; i < tempJewelry.FixedSpecials.Count; i++)
        {
            if (tempJewelry.FixedSpecials[i] <= 8)
                InstanceSpecials.Add(new int[] { tempJewelry.FixedSpecials[i], Random.Range(1, 10) });
            else InstanceSpecials.Add(new int[] { tempJewelry.FixedSpecials[i], 0 });
        }
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack") 
            Physics2D.IgnoreCollision(InstanceCircle, OtherObject.gameObject.GetComponent<BoxCollider2D>());
    }

    public void DestroyInstance()
    {
        InstanceBox.enabled = false;
        InstanceAnim.SetTrigger(DestroyedHarsh);
    }

    void DisappearEvent()
    {
        Destroy(gameObject);
    }
}
