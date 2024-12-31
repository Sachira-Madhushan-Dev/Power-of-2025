using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{
    private float speed = 5;
    bool touching = true;
    bool leftPressed = false;
    bool rightPressed = false;
    private SpriteRenderer renderer;
    private Animator animator;
    public TextMeshProUGUI scoreText;
    public GameObject[] health;
    public AudioClip walking, gift, damage, win,defeat;
    private AudioSource audio;
    public GameObject winWindow, lostWindow;
    int highscore;
    int healthCount = 0;
    int score = 0;

    public float doubleTapTime = 0.3f;

    private float lastTapTime = 0;

    private void Start()
    {
        Time.timeScale = 1;
        audio = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        winWindow.SetActive(false);
        lostWindow.SetActive(false);
        highscore = PlayerPrefs.GetInt("HighScore", 0);
    }
    private void FixedUpdate()
    {

        if (healthCount >= 5)
        {
            Time.timeScale = 0;
            if (highscore < score)
            {
                winWindow.SetActive(true);
                PlayerPrefs.SetInt("HighScore", score);
                audio.clip = win;
                audio.Play();
            }
            else
            {
                lostWindow.SetActive(true);
                audio.clip = defeat;
                audio.Play();
            }
        }
        if (leftPressed)
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
            animator.SetBool("Walking",true);
            //audio.clip = walking;
            //audio.Play();
        }else if (rightPressed)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
            animator.SetBool("Walking", true);
            //audio.clip = walking;
            //audio.Play();
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            DetectDoubleTap();
        }

        // For mouse input (for testing on PC)
        if (Input.GetMouseButtonDown(0))
        {
            DetectDoubleTap();
        }

    }
    public void MoveLeft()
    {
        leftPressed = true;
        renderer.flipX = true;
    }

    public void MoveRight()
    {
        rightPressed = true;
        renderer.flipX = false;
    }
    public void PointerUp()
    {
        leftPressed = false;
        rightPressed = false;
        animator.SetBool("Walking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gift")
        {
            
            score++;
            scoreText.text = score.ToString();
            audio.clip = gift;
            audio.Play();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            health[healthCount].SetActive(false);
            healthCount++;
            audio.clip = damage;
            audio.Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Krampus")
        {
            health[healthCount].SetActive(false);
            healthCount++;
            audio.clip = damage;
            audio.Play();
        }
    }

    void DetectDoubleTap()
    {
        // Get the current time
        float currentTime = Time.time;

        // Check if the time between taps is within the threshold
        if (currentTime - lastTapTime < doubleTapTime)
        {
            OnDoubleTap();
        }

        // Update the last tap time
        lastTapTime = currentTime;
    }

    void OnDoubleTap()
    {
        animator.SetTrigger("NormalAttack");
    }

}
