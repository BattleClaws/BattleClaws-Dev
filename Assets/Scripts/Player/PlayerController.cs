using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private GameObject _handle;
    private bool _isDropped;
    private bool _knockback;
    public bool _roundActive = true;
    public bool isWinningPlayer; 
    

    
    // Allows for other scripts to access the Player class without calling it again
    public Player Properties { get; set; }

    // Gets the position of the "Handle", which is where the claw actually is
    public Vector3 Position
    {
        get => _handle.transform.position; 
        set => _handle.transform.position = value; 
    }

    #region Move Input & Response

    // New input system, for more info see hierarchy + input folder in Assets
    void OnMove(InputAction.CallbackContext ctx) => _input = ctx.ReadValue<Vector2>();

    private void Move()
    {
        if (!_isDropped && _input != Vector2.zero && _roundActive)
        {
            Vector3 movement = new Vector3(_input.x, 0, _input.y) * Properties.Speed * Time.fixedDeltaTime;
            Vector3 newPosition = _handle.GetComponent<Rigidbody>().position + movement;
            
            RaycastHit hit;
            if (Physics.Raycast(_handle.GetComponent<Rigidbody>().position, movement.normalized, out hit, movement.magnitude))
            {
                if(hit.collider.CompareTag("Constraint"))
                    newPosition = hit.point - movement.normalized * 0.05f;
            }
            
            _handle.GetComponent<Rigidbody>().MovePosition(newPosition);
        }
    }

    #endregion
    
    #region Drop Input & Coroutine
    public void OnDrop(InputAction.CallbackContext ctx)
    {
        StartCoroutine(Drop());
    }

     

    private IEnumerator Drop()
    {
        if (!_isDropped && _roundActive)
        {
            _isDropped = true;
            if (Properties.heldObject != Properties.gameObject)
            {
                Properties.Animator.SetTrigger("Open");
                DropCollectable();
                yield return new WaitForSeconds(0.5f);
                _isDropped = false;
                Properties.Animator.SetTrigger("Idle");
            }
            else
            {
                var startPosition = Position;
                var goalPosition = Position - new Vector3(0, 1.8f, 0);
                //print(startPosition + " | " + goalPosition);

                Properties.Animator.SetTrigger("Open");
                for (float i = 0; i < 1.1f; i += 0.07f)
                {
                    Position = Vector3.Lerp(startPosition, goalPosition, i);
                    yield return new WaitForSeconds(0.02f);
                }


                CheckForCollectable();
                Properties.Animator.SetTrigger("Close");


                yield return new WaitForSeconds(0.6f);

                for (float i = 1; i >= -0.1f; i -= 0.14f)
                {
                    Position = Vector3.Lerp(startPosition, goalPosition, i);
                    yield return new WaitForSeconds(0.02f);
                }

                Position = startPosition;

                _isDropped = false;
            }
        }
    }

    private void CheckForCollectable()
    {
        print("check");
        RaycastHit hit;

        int collectableMask = 1 << 7;
        
        if (Physics.SphereCast(Position + new Vector3(0, 1f, 0), 0.15f, Vector3.down + new Vector3(0, -70, 0),out hit, Mathf.Infinity,collectableMask) &&
            hit.collider.CompareTag("Collectable"))
        {
            print("hit!");
            var target = hit.collider.gameObject;
            
            target.transform.SetParent(Properties.Model.transform);
            target.GetComponent<Rigidbody>().useGravity = false;
            target.GetComponent<Rigidbody>().isKinematic = true;
            target.GetComponent<Collectable>().Holder = this;
            StartCoroutine(GameUtils.LerpToLocalPosition(target, Vector3.zero - new Vector3(0, 0.1f, 0), 0.02f));
            Properties.heldObject = target;


            if (RoundManager.draw)
            {
                isWinningPlayer = true;
            }
            
        }
    }

    private void DropCollectable()
    {
        if (Properties.heldObject != null && Properties.heldObject != Properties.gameObject)
        {
            Properties.heldObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(Properties.heldObject, SceneManager.GetActiveScene());
            Properties.heldObject.GetComponent<Rigidbody>().useGravity = true;
            Properties.heldObject.GetComponent<Rigidbody>().isKinematic = false;
            Properties.heldObject = Properties.gameObject;
        }

        if (RoundManager.draw)
        {
            isWinningPlayer = false;
        }
    }


    #endregion

    #region Player Collision
    
    // This actually handles the knockback
    public void KnockBack(Transform other, bool isPlayer)
    {
        var otherPosition = (isPlayer) ? other.GetComponent<PlayerController>().Position : other.transform.position;
        var midPoint = Vector3.Lerp(otherPosition, Position, 0.5f);

        var particles = Resources.Load<GameObject>("Prefabs/Sparks");
        
        // Here, im getting the normalised position between the current claw and the collider, getting a direction to move away from
        Vector3 Direction = Vector3.Normalize(otherPosition - Position);
        StartCoroutine(Properties.SpeedEffect(0, 0.2f, false));
        
        if (Properties.heldObject != Properties.gameObject)
            DropCollectable();

        Instantiate(particles, midPoint, Quaternion.identity);
        
        // Then i just force it away :3
        GetComponent<Rigidbody>().AddForce(Direction * -5, ForceMode.Impulse);
    }

    #endregion

    #region Utils

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Drop());
    }



    #endregion
    
    // Set some values
    private void Awake()
    {
        Properties = GetComponent<Player>();
        _handle = transform.Find("AnchorPoint").gameObject;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("Round 1");
        }
        
        Debug.DrawLine(Position + new Vector3(0, 10f, 0), Position + Vector3.down + new Vector3(0, -70, 0), Color.red);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Eliminate()
    {
        print("eliminate");
        Properties.eliminated = true;
        GetComponent<PlayerInput>().enabled = false;
        GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
        Properties.Model.SetActive(false);
    }
}
