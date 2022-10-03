using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class GodController : Singleton<GodController>
{

    public Animator PlayerAnimator;

    Camera mCam;

    public Collider2D TreeCollider;
    Collider2D myCollider;

    public PostProcessVolume Ppv;
    Vignette vignette;

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
            // Debug.Log($"W");

            newPosition.y = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log($"S");

            newPosition.y = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Debug.Log($"D");

            newPosition.x = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Debug.Log($"A");

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
        if (myCollider.IsTouching(TreeCollider))
        {
            PlayerAnimator.SetTrigger("Pray");
            DOVirtual.DelayedCall(0.5f, () =>
            {
                TreeController.Instance.Upgrade();
            });
        }
        else
        {
            PlayerAnimator.SetTrigger("Hit");
            DOVirtual.DelayedCall(0.35f, () =>
            {
                mCam.DOShakePosition(0.3f, 0.3f, 40);
                CheckHammerHit();
            });
        }

    }

    [ContextMenu("debug")]
    public void DebugVolume()
    {
        Debug.Log(Ppv.sharedProfile.settings[1]);
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
            if (CurrentCollision.gameObject.tag.Equals("Worm"))
            {
                Debug.Log($"worm hit");
                Time.timeScale = 0.3f;
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1.5f);

                if (Ppv.profile.TryGetSettings<Vignette>(out vignette))
                {
                    vignette.intensity.value = 0.5f;
                    DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.45f, 1.5f);

                }

                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1.5f);
                // TreeController.instance.CircleAnimation.SetBool("pulse", true);
            }/*
            ChronoCrystal crys;
            bool success = CurrentCollision.TryGetComponent<ChronoCrystal>(out crys);
            if (success) crys.Hit();*/
        }

        if (CurrentCollision.gameObject.tag.Equals("Tree"))
        {
            Debug.Log($"A");

            TreeController.Instance.CircleAnimation.SetBool("pulse", true);
        }
    }

    GameObject CurrentCollision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Exit")) return;
        CurrentCollision = other.gameObject;

        if (other.gameObject.tag.Equals("Tree"))
        {
            Debug.Log($"A");

            TreeController.Instance.CircleAnimation.SetBool("pulse", true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Exit"))
        {
            Debug.Log("Nothing there");
            UIController.Instance.ShowMessageLabel("There's nothing here. Except the desert. And the woooooorms.");
            return;
        }
        CurrentCollision = null;
        if (other.gameObject.tag.Equals("Tree"))
        {
            Debug.Log($"A");

            TreeController.Instance.CircleAnimation.SetBool("pulse", false);
        }
    }


}
