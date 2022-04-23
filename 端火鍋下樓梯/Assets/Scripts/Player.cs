using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int hp;
    [SerializeField] GameObject hpBar;
    [SerializeField] Text scoreText;
    int score;
    float scoreTime;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;

    
    // Start is called before the first frame update
    void Start(){
        hp = 10;
        score = 0;
        scoreTime = 0;
        // anim = GetComponent<Animator>();
        // rendor = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false; // 往右走 flip = false

        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true; // 往左走 flip = true
        } 
        updateScore();
    }

    void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.tag == "Normal"){

            if(other.contacts[0].normal == new Vector2(0f, 1f)){
                // 法線為(0.0, 1.0)的狀況才設定 currentFloor
                // Debug.Log("撞到第一種階梯！");
                currentFloor = other.gameObject;
                modifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }

        else if(other.gameObject.tag == "Nails"){
           
            if(other.contacts[0].normal == new Vector2(0f, 1f)){
                // Debug.Log("撞到第二種階梯！");
                currentFloor = other.gameObject;
                modifyHp(-3);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }

        else if(other.gameObject.tag == "Ceiling"){
           
            // Debug.Log("撞到天花板！");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false; // 取消 BoxCollider 勾選
            modifyHp(-3);
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.tag == "DeathLine"){
            Debug.Log("死囉!");
           over();

        }
    }

    void modifyHp(int num){
        // 控制血量
        hp += num;
        if(hp > 10){
            hp = 10;
        }
        else if (hp < 0){
            hp = 0;
            over();
        }
        updateHpBar();
    }

    void updateHpBar(){
        // 控制血條顯示
        for(int i = 0 ; i < hpBar.transform.childCount ; i++){
            if(hp > i){
                hpBar.transform.GetChild(i).gameObject.SetActive(true);
            }

            else{
                hpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void updateScore(){
        // 控制地下層數
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f){
            score ++;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "層";
        }

    }

    void over(){
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void Replay(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
