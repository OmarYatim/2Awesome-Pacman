using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;
    [SerializeField] private AudioClip pelletSound;

    private void Start()     => GameManager.PelletResetEvent += this.OnPelletReset; 

    private void OnDestroy() => GameManager.PelletResetEvent -= this.OnPelletReset; 


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat();
        }
    }
    protected virtual void Eat() 
    {
        GameManager.Instance.PelletEaten(this);
        SoundManager.Instance.PlayClipNoReset(this.pelletSound);
    }

    private void OnPelletReset() => this.gameObject.SetActive(true);
}
