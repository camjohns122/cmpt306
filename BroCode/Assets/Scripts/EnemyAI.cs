using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    private GameObject Baddy;
    private GameObject Player;
    public float Range;
    [SerializeField]
    public float Speed;
    //public bool moveRight;

    // Use this for initialization
    void Start()
    {
        Baddy = GameObject.FindGameObjectWithTag("Enemy");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        Range = Vector2.Distance(Baddy.transform.position, Player.transform.position);

        if (Range <= 40f)
        {
            transform.Translate(Vector2.MoveTowards(Baddy.transform.position, Player.transform.position, Range) * Speed * Time.deltaTime);
        }
    }
}
