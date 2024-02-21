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

    public bool eliminated = false;

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
        if(!_isDropped)
            _handle.transform.Translate(new Vector3(_input.x, 0, _input.y) * Properties.Speed * Time.deltaTime);
    }

    #endregion
    
    #region Drop Input & Coroutine
    public void OnDrop(InputAction.CallbackContext ctx)
    {
        StartCoroutine(Drop());
    }
    
    private IEnumerator Drop()
    {
        if (!_isDropped)
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
        if (Physics.SphereCast(Position + new Vector3(0, 1f, 0), 1f, Vector3.down + new Vector3(0, -50, 0), out hit) &&
            hit.collider.CompareTag("Collectable"))
        {
            print("hit!");
            var target = hit.collider.gameObject;
            
            target.transform.SetParent(Properties.Model.transform);
            target.GetComponent<Rigidbody>().useGravity = false;
            target.GetComponent<Rigidbody>().isKinematic = true;
            Properties.heldObject = target;
        }
    }

    private void DropCollectable()
    {
        Properties.heldObject.transform.parent = null;
        Properties.heldObject.GetComponent<Rigidbody>().useGravity = true;
        Properties.heldObject.GetComponent<Rigidbody>().isKinematic = false;
        Properties.heldObject = Properties.gameObject;
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
        StartCoroutine(Properties.SpeedEffect(0, 0.5f));
        
        if (Properties.heldObject != Properties.gameObject)
            DropCollectable();

        Instantiate(particles, midPoint, Quaternion.identity);
        
        // Then i just force it away :3
        GetComponent<Rigidbody>().AddForce(Direction * -5, ForceMode.Impulse);
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
        Move();
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("Round 1");
        }
    }

    public void Eliminate()
    {
        eliminated = true;
        GetComponent<PlayerInput>().enabled = false;
        Properties.Model.SetActive(false);
    }
}
