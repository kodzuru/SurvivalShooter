using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f; //скорость игрока

    Vector3 movement; //передвижение игрока
    Animator anim; //ссылка на анимацию
    Rigidbody playerRigidbody; // ссылка на ТТ игрока
    int floorMask; // меш коллайдер пола
    float camRayLength = 100f; //длина луча камеры

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor"); //получаем маску(меш коллайдер) пола
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //ввод с клавиатуры
        float h = Input.GetAxisRaw("Horizontal");//вывод значений по горизонтальной оси [-1;1] 
        float v = Input.GetAxisRaw("Vertical");
        //Debug.Log(h +  "   " + v);
        //вызываем функции движения, поворот персонажа по курсору мыши, анимации
        Move(h, v);
        Turning();
        Animating(h, v);

    }

    void Move(float h, float v)
    {
        movement.Set(h, 0.0f, v);//устанавливаем координаты полученые от клавиатуры
        movement = movement.normalized*speed*Time.deltaTime; //нормализация координат и скорость
        playerRigidbody.MovePosition(transform.position + movement); //передвижение ТТ игрока
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Возвращает луч, идущий от камеры через точку на экране.
        RaycastHit floorHit;

        //bool @return True если луч пересекся хотя-бы с одним коллайдером,в ином случае - false.
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0.0f || v != 0.0f; //проверка перемещается ли персонаж по осям
        //Debug.Log(walking);

        anim.SetBool("IsWalking", walking); //устанавливает bool для параметра из анимации 
    }
}
