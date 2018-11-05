using UnityEngine;

public class Snake : Enemy
{
    [SerializeField] Missile AttackPrefab;
    Transform AttackPos;

    protected override void Awake()
    {
        base.Awake();
        AttackPos = GetComponentInChildren<Transform>();
    }

    public void SpawningEvent()
    {
        Missile PoisonBall;
        PoisonBall = Instantiate(AttackPrefab, new Vector3(AttackPos.position.x, AttackPos.position.y, AttackPos.position.z),
                Quaternion.identity);
        PoisonBall.GetSubjectScale = transform.localScale.x;
    }
}
