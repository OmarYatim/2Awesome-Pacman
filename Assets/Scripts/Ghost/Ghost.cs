using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement;
    public GhostHome home;
    public GhostScatter scatter;
    public GhostChase chase;
    public GhostFrightened frightened;
    public GhostBehavior initialBehavior;
    public Transform target;
    public ParticleSystem Particels;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private AudioClip ghostEatenSound;
    public int points = 200;


    private void Awake()
    {
        GameManager.ObjectsEnabledEvent  += this.ResetState;
        GameManager.PacmanEatenEvent     += this.OnPacmanEaten;
        GameManager.ShowGhostsEvent      += this.OnShowGhost;
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.ObjectsEnabledEvent -= this.ResetState;
        GameManager.PacmanEatenEvent    -= this.OnPacmanEaten;
        GameManager.ShowGhostsEvent     -= this.OnShowGhost;
    }

    private void Start()     => this.ResetState();

    public void ResetState()
    {
        this.movement.ResetState();
        this.bodyAnimator.enabled = true;

        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        if (this.home != this.initialBehavior) {
            this.home.Disable();
        }

        if (this.initialBehavior != null) {
            this.initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = this.transform.position.z;
        this.transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled) {
                GameManager.Instance.GhostEaten(this);
                SoundManager.Instance.PlayClip(this.ghostEatenSound);
            } else {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

    private void OnPacmanEaten()
    {
        this.gameObject.SetActive(false);
        this.movement.enabled = false;
        this.movement.ResetInitialPosition();
    }

    private void OnShowGhost()
    {
        this.gameObject.SetActive(true);
        this.movement.enabled = false;
    }
}
