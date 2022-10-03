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
    Bloom bloom;

    public void ResetState()
    {
        myCollider = GetComponent<BoxCollider2D>();
        mCam = Camera.main;
        transform.position = new Vector3(5, 5, 0);
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

        HandleKeyboard();

    }

    //movement
    Vector3 newPosition = new Vector3();
    public float MoveSpeed = 1f;

    void HandleKeyboard()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (GameConroller.Instance.RedCount < 5)
            {
                UIController.Instance.GodSupportLabel.text = "<color=red>Not enough matter</color>";
                DOVirtual.DelayedCall(1f, () => UIController.Instance.GodSupportLabel.text = "");
                return;
            }
            PlayerAnimator.SetTrigger("Pray");
            for (int i = 0; i < 5; i++)
            {
                GameConroller.Instance.RemoveRedCrystal();
            }
            GameConroller.Instance.SpawnMeteor(Random.Range(5, 9), 49, 3);
        }
    }

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
            UIController.Instance.GodSupportLabel.transform.localScale = flippedScale * 0.1f;
            PlayerAnimator.SetBool("moveMirror", true);
        }
        else if (newPosition.x > 0)
        {
            transform.localScale = normalScale;
            UIController.Instance.GodSupportLabel.transform.localScale = normalScale * 0.1f;
            PlayerAnimator.SetBool("moveMirror", false);

        }

        transform.position = Vector3.Lerp(transform.position, transform.position + newPosition, MoveSpeed * Time.deltaTime);
        newPosition = new Vector3();

    }

    void SwingHammer()
    {
        if (myCollider.IsTouching(TreeCollider))
        {
            if (GameConroller.Instance.Seconds < 10)
            {
                return;
            }

            PlayerAnimator.SetTrigger("Pray");
            DOVirtual.DelayedCall(1f, () =>
            {
                TreeController.Instance.Upgrade();
                for (int i = 0; i < 10; i++)
                {
                    GameConroller.Instance.RemoveSeconds();
                }
            });
        }
        else if (CurrentCollision != null && CurrentCollision.gameObject.tag.Equals("Rift"))
        {
            if (GameConroller.Instance.RedCount < 10) return;

            PlayerAnimator.SetTrigger("Pray");
            DOVirtual.DelayedCall(2f, () =>
            {
                CurrentCollision.GetComponentInChildren<ParticleSystem>().Play();
                CurrentCollision.GetComponentInChildren<BoxCollider2D>().enabled = false;

                for (int i = 0; i < 10; i++)
                {
                    GameConroller.Instance.RemoveRedCrystal();
                }
                GameConroller.Instance.PlaceForge(CurrentCollision.transform.position);
            });
        }
        else if (CurrentCollision != null && CurrentCollision.gameObject.tag.Equals("Forge"))
        {
            if (GameConroller.Instance.BlueCount < 1) return;

            PlayerAnimator.SetTrigger("Give");
            CurrentCollision.gameObject.GetComponent<Forge>().DwarfAnimator.SetBool("hasBlue", true);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                CurrentCollision.gameObject.GetComponent<Forge>().AddFuel();
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
        if (CurrentCollision != null)
        {
            if (CurrentCollision.gameObject.tag.Equals("Worm"))
            {
                WormSegment segment = CurrentCollision.gameObject.GetComponent<WormSegment>();
                Debug.Log($"worm hit");
                if (segment.HeadReference.isDead)
                {
                    GameConroller.Instance.SpawnRedCrystal((Vector3)UnityEngine.Random.insideUnitCircle + segment.transform.position);
                    Time.timeScale = 0.8f;
                    DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1f);

                    if (Ppv.profile.TryGetSettings<Vignette>(out vignette))
                    {
                        vignette.intensity.value = 0.47f;
                        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.416f, 1f);
                    }
                    Destroy(CurrentCollision.gameObject);
                }
                else
                {
                    Time.timeScale = 0.3f;
                    segment.HeadReference.isDead = true;
                    DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1.5f);

                    if (Ppv.profile.TryGetSettings<Vignette>(out vignette))
                    {
                        vignette.intensity.value = 0.5f;
                        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.416f, 1.5f);
                    }
                }
            }
            if (CurrentCollision.gameObject.tag.Equals("Red"))
            {
                CurrentCollision.gameObject.GetComponent<ChronoCrystal>().Hit();
            }
            if (CurrentCollision.gameObject.tag.Equals("Blue"))
            {
                CurrentCollision.gameObject.GetComponent<ChronoCrystal>().Hit();

            }

        }
    }

    GameObject CurrentCollision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Exit")) return;
        CurrentCollision = other.gameObject;


        if (other.tag.Equals("Tree"))
        {
            if (GameConroller.Instance.Seconds < 10) UIController.Instance.GodSupportLabel.text = "<color=red>Not enough seconds</color>";
            else
                UIController.Instance.GodSupportLabel.text = "SPACE to perform magic";
            return;
        }
        if (other.tag.Equals("Rift"))
        {
            if (GameConroller.Instance.RedCount < 10) UIController.Instance.GodSupportLabel.text = "<color=red>Not enough matter</color>";
            else
                UIController.Instance.GodSupportLabel.text = "SPACE to create a forge";
            return;
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
        if (other.tag.Equals("Tree"))
        {
            UIController.Instance.GodSupportLabel.text = "";
            return;
        }
        if (other.tag.Equals("Rift"))
        {
            UIController.Instance.GodSupportLabel.text = "";
            return;
        }

        CurrentCollision = null;
    }


}
