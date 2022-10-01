using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GodController : Singleton<GodController>
{

    public Animator PlayerAnimator;

    Camera mCam;

    public Collider2D TreeCollider;
    Collider2D myCollider;

    public void ResetState()
    {
        myCollider = GetComponent<BoxCollider2D>();
        mCam = Camera.main;
    }

    void Start()
    {
        ResetState();
    }

    private void Update()
    {
        HandleMovement();
        if (Input.GetKeyUp(KeyCode.Space))
            SwingHammer();

    }

    //movement
    Vector3 newPosition = new Vector3();
    public float MoveSpeed = 1f;



    Vector3 normalScale = new Vector3(1, 1, 1);
    Vector3 flippedScale = new Vector3(-1, 1, 1);
    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            newPosition.y = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition.y = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition.x = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition.x = -1f;
        }

        PlayerAnimator.SetFloat("moceSpeed", Mathf.Abs(newPosition.magnitude));
        // Debug.Log($"{PlayerAnimator.GetFloat("moceSpeed")} {Mathf.Abs(newPosition.magnitude)} ");
        if (newPosition.x < 0)
        {
            transform.localScale = flippedScale;
            PlayerAnimator.SetBool("moveMirror", true);
        }
        else if (newPosition.x > 0)
        {
            transform.localScale = normalScale;

            PlayerAnimator.SetBool("moveMirror", false);

        }


        transform.position = Vector3.Lerp(transform.position, transform.position + newPosition, MoveSpeed * Time.deltaTime);
        newPosition = new Vector3();

    }

    void SwingHammer()
    {
        PlayerAnimator.SetTrigger("Hit");
        DOVirtual.DelayedCall(0.35f, () =>
        {
            mCam.DOShakePosition(0.3f, 0.3f, 40);
            CheckHammerHit();
        });

    }


    void CheckHammerHit()
    {
        // Debug.Log("Tree Hit Check");

        if (myCollider.IsTouching(TreeCollider))
        {
            TreeController.Instance.Upgrade();
        }

        if (CurrentCollision != null)
        {
            ChronoCrystal crys;
            bool success = CurrentCollision.TryGetComponent<ChronoCrystal>(out crys);
            if (success) crys.Hit();
        }
    }

    GameObject CurrentCollision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentCollision = other.gameObject;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentCollision = null;
    }


}
