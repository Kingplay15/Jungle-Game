using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    Vector3 PosWorldSpace;
    Rigidbody2D PlatformRigid;
    [SerializeField] GameObject ParentObject;
    public bool AlreadyDrop { get; private set; } = false;

    void Awake()
    {
        PlatformRigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PosWorldSpace.y <= -60f)
            Destroy(ParentObject);
    }

    void LateUpdate()
    {
        PosWorldSpace = transform.TransformPoint(transform.position);
    }

    public void Drop()
    {
        PlatformRigid.isKinematic = false;
        AlreadyDrop = true;
    }
}
