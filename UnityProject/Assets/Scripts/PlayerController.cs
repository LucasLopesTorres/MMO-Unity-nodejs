using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Reponsavel pelo movimento/Fala do player
    public bool IslocalPlayer {get; set;} //Verifica se é o player local

    [SerializeField] float horizontalSpeed;
    [SerializeField] float verticalSpeed;

    [SerializeField]Text speachText; //texto de fala
    [SerializeField]GameObject speachBox;
    [SerializeField] GameObject myCamera;
    // Start is called before the first frame update
    void Start()
    {
        speachBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IslocalPlayer)
        {
            //Movimenta o personagem
            Move();
        }
    }

    void Move()
    {
        var horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed;
        var vertical = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;

        if (horizontal != 0 || vertical != 0)
        {
            transform.Translate(0, 0, vertical);
            transform.Rotate(0, horizontal, 0);
            
            //Avisa ao connection manager a nova posição
            ConnectionManager.instance.EmitPosic_Rot(transform.position, transform.rotation); 
        }
    }

    public void Speach(string speak){  
        speachText.text = speak;
        speachBox.SetActive(true);

        CancelInvoke();

        Invoke("ClosedSpeach", 3f);
    }

    void ClosedSpeach(){
        speachText.text = "";
        speachBox.SetActive(false);
    }

    public void MyCamera(){
        myCamera.SetActive(true);
    }

}
