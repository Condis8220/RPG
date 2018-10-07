using UnityEngine;

public class PlayerIKController : MonoBehaviour
{
    public float offset;
    public bool isActiveIK = true;
    private bool isWaitCall = false;

    private Animator animator;

    private Vector3 leftFootTargetPos;
    private Vector3 rightFootTargetPos;

    private Quaternion leftFootTargetRot;
    private Quaternion rightFootTargetRot;

    private float leftFootWeight;
    private float rightFootWeight;

    private Transform leftFoot;
    private Transform rightFoot;    

    private bool isGround;

    private void Start()
    {
        animator = GetComponent<Animator>();

        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    }

    private void Update()
    {
        if (!isActiveIK)
            return;

        isGround = true;

        CheckFootPosition(leftFoot.position, ref leftFootTargetPos, ref leftFootTargetRot);
        CheckFootPosition(rightFoot.position, ref rightFootTargetPos, ref rightFootTargetRot);

        void CheckFootPosition(Vector3 _footCurrentPosition, ref Vector3 _footNextPosition, ref Quaternion _footNextRotation)
        {
            if (Physics.Raycast(_footCurrentPosition + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 1.5f))
            {
                _footNextPosition = Vector3.Lerp(_footCurrentPosition, hit.point + Vector3.up * offset, Time.deltaTime * 10f);
                _footNextRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            }
            else
                isGround = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!isActiveIK)
            return;

        if (!isGround)
            return;

        float leftWeight = animator.GetFloat("leftFoot");
        float rightWeight = animator.GetFloat("rightFoot");

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftWeight);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTargetPos);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTargetRot);

        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTargetPos);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTargetRot);
    }

    public void WaitCall()
    {
        if (isActiveIK)
            return;

        if (isWaitCall)
            return;

        isWaitCall = true;
        Invoke("ActivateIK", 1f);
    }

    private void ActivateIK()
    {
        isActiveIK = true;
        isWaitCall = false;

        Debug.Log("ActivateIK");
    }

    public void DeactivateIK()
    {
        Debug.Log("DeactivateIK");
        isActiveIK = false;
        if (isWaitCall)
        {
            CancelInvoke("ActivateIK");
            isWaitCall = false;
        }
    }
}
