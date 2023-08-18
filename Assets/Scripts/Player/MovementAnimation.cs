using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PlayerMovement player;

    private Animator animator;

    [SerializeField]
    private bool idleStarted = false;
    [SerializeField]
    private bool startIdle = false;
    [SerializeField]
    private bool isMoving;

    public bool isSitting;
    

    public bool usePhone;

    public Rigidbody2D rb;

    public Vector2 lastMovement;


    void Awake()
    {
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        usePhone = false;
        isSitting = false;
    }

    // Update is called once per frame
    void Update()
    {
    }



   
}
