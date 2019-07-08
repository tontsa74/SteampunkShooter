
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Vector3 thrusterForce = Vector3.zero;
    private Vector3 jumpForce = Vector3.zero;
    private float distToGround;

    private Rigidbody rb;

    [SerializeField]
    AudioClip[] footStepSounds;
    [SerializeField]
    AudioClip jumpSound;
    [SerializeField]
    AudioClip landSound;
    [SerializeField]
    AudioClip thrusterSound;
    public GameObject audioPrefab;

    private GameObject thrusterSoundPlayer;
    private AudioScript tsp;

    private bool step = true;
    private bool running = false;
    private bool hasJumped = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<BoxCollider>().bounds.extents.y;
        thrusterSoundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        tsp = thrusterSoundPlayer.GetComponent<AudioScript>();
    }

    public void Move(Vector3 _velocity, bool _running, float _runMultiplier)
    {
        if(_running && IsGrounded())
        {
            _velocity *= _runMultiplier;
        }
        velocity = _velocity;

        running = _running;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    public void ApplyJump(Vector3 _jumpForce)
    {
        jumpForce = _jumpForce;

    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if(!IsGrounded())
        {
            hasJumped = true;
        }

        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

            if(step && IsGrounded())
            {
                if(!running)
                {
                    PlayWalkSteps();
                } else
                {
                    PlayRunSteps();
                }

            }

        }

        if (thrusterForce != Vector3.zero && !IsGrounded())
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        //    rb.AddForce(jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);

            if(!tsp.playing)
            {
                tsp.PlaySound(thrusterSound, true, 3f);
            }
        } else
        {
            if(tsp.playing)
            {
                tsp.PauseSound();
            }
        }

        if (jumpForce != Vector3.zero && IsGrounded())
        {
            rb.AddForce(jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);

            PlayJumpSound();
        }

        if(IsGrounded() && hasJumped)
        {
            hasJumped = false;
            PlayLandSound();
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }

    void PlayWalkSteps()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(footStepSounds[Random.Range(0, footStepSounds.Length)], false, 0.2f);
        StartCoroutine(WaitForFootSteps(0.5f));
    }

    void PlayRunSteps()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(footStepSounds[Random.Range(0, footStepSounds.Length)], false, 2f);
        StartCoroutine(WaitForFootSteps(0.3f));
    }

    void PlayJumpSound()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(jumpSound, false, 3f);
    }

    void PlayLandSound()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(landSound, false, 3f);
    }

    private IEnumerator WaitForFootSteps(float stepsLength)
    {
        step = false;
        yield return new WaitForSeconds(stepsLength);
        step = true;
    }


}
