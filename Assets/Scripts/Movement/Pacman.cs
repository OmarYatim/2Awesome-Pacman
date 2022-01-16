using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public Movement movement;

    [SerializeField] private Collider2D PacmanCollider;
    [SerializeField] private Animator pacmanAnimator;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        GameManager.PacmanEatenEvent += this.DeathSequence;
        GameManager.ObjectsEnabledEvent += this.ResetState;
    }

    private void OnDestroy()
    {
        GameManager.PacmanEatenEvent -= this.DeathSequence;
        GameManager.ObjectsEnabledEvent -= this.ResetState;
    }

    private void Update()
    {
        // Set the new direction based on the current input
        if (InputManager.Instance.GetUpMovement) {
            this.movement.SetDirection(Vector2.up);
        }
        else if (InputManager.Instance.GetDownMovement) {
            this.movement.SetDirection(Vector2.down);
        }
        else if (InputManager.Instance.GetLeftMovement) {
            this.movement.SetDirection(Vector2.left);
        }
        else if (InputManager.Instance.GetRightMovement) {
            this.movement.SetDirection(Vector2.right);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        this.pacmanAnimator.Play("Base Layer.PacmanMunch");
        this.enabled = true;
        this.PacmanCollider.enabled = true;
        this.pacmanAnimator.enabled = true;
        this.movement.ResetState();
        this.gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        this.enabled = false;
        this.PacmanCollider.enabled = false;
        this.movement.enabled = false;
        this.pacmanAnimator.Play("Base Layer.PacmanDeath");
        SoundManager.Instance.PlayClip(this.deathSound);
    }
}
