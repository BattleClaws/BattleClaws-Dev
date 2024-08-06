using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    public GameObject _handle;
    private bool _isDropped;
    private bool _knockback;
    public bool _roundActive = true;
    public bool isWinningPlayer;
    private Vector3 parentPosition;
    private Vector3 moveDelta;


    public List<Coroutine> activeRoutines;


    // Allows for other scripts to access the Player class without calling it again
    public Player Properties { get; set; }
    
    // For Readying Up
    public bool IsReady { get; private set; } = false;

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
        if (!_isDropped && _input != Vector2.zero && _roundActive && !Properties.eliminated && !GameUtils.isMenuOpen)
        {
            Vector3 movement = new Vector3(_input.x, 0, _input.y) * Properties.Speed * Time.fixedDeltaTime;
            Vector3 newPosition = _handle.GetComponent<Rigidbody>().position + movement + moveDelta;
            
            RaycastHit hit;
            if (Physics.Raycast(_handle.GetComponent<Rigidbody>().position, movement.normalized, out hit, movement.magnitude))
            {
                if(hit.collider.CompareTag("Constraint"))
                    newPosition = hit.point - movement.normalized * 0.05f;
            }
            
            
            
            _handle.GetComponent<Rigidbody>().MovePosition(newPosition);
        }
    }

    public void hold()
    {
        var newPosition = Position - _handle.GetComponent<Rigidbody>().velocity.normalized * 0.1f;
        _handle.GetComponent<Rigidbody>().MovePosition(newPosition);
    }

    public void OnReset(InputAction.CallbackContext ctx)
    {
        ResetPosition();
    }

    public void OnMenu(InputAction.CallbackContext ctx)
    {
        GameUtils.menuManager.SetVisibility(true);
    }
    
    public void OnBack(InputAction.CallbackContext ctx)
    {
        GameUtils.menuManager.OnBackPressed();
    }

    public void ResetPosition()
    {
        Position = GameUtils.RequestSpawnLocation(Properties.PlayerNum).position;
    }

    #endregion
    
    #region Drop Input & Coroutine
    public void OnDrop(InputAction.CallbackContext ctx)
    {
        StartCoroutine("Drop");
    }

    public void StopCoroutines()
    {
        StopCoroutine("Drop");
        _isDropped = false;
    }





    private IEnumerator Drop()
    {
        if (!_isDropped && _roundActive && !GameUtils.isMenuOpen)
        {
            _isDropped = true;
            if (Properties.heldObject != Properties.gameObject)
            {
                Properties.Animator.SetTrigger("Open");
                GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Claw/ClawLetGo");
                try
                {
                    // Have a try here for the instance that the collectable has been deleted while the player was
                    // holding it. Without the try/catch it will error out and freeze the claw. With this, even if the
                    // drop fails, the claw will not have any issues.
                    DropCollectable();
                }
                catch (Exception e)
                {
                    //empty
                }
                
                Properties.heldObject = Properties.gameObject;
                
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
                GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Claw/ClawDrop");
                for (float i = 0; i < 1.1f; i += 0.07f)
                {
                    Position = Vector3.Lerp(startPosition, goalPosition, i);
                    //print("changing position of player " + Properties.PlayerNum);
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
        //print("check");
        RaycastHit hit;

        int collectableMask = 1 << 7;
        
        if (Physics.SphereCast(Position + new Vector3(0, 1f, 0), 0.15f, Vector3.down + new Vector3(0, -70, 0),out hit, Mathf.Infinity,collectableMask) &&
            hit.collider.CompareTag("Collectable"))
        {
            //print("hit!");
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
        //var midPoint = Vector3.Lerp(otherPosition, Position, 0.5f);

        var particles = Resources.Load<GameObject>("Prefabs/Sparks");
        StartCoroutine(GameUtils.instance.CamShake(0.1f));
        
        // Here, im getting the normalised position between the current claw and the collider, getting a direction to move away from
        Vector3 Direction = Vector3.Normalize(otherPosition - Position);
        StartCoroutine(Properties.SpeedEffect(0, 0.2f, false));
        
        GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Claw/ClawHit1");
        GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Claw/ClawHit2");
        
        if (Properties.heldObject != Properties.gameObject)
            DropCollectable();

        Instantiate(particles, Position, Quaternion.identity);
        // Then i just force it away :3
        
        //print("Strike Direction" + Direction);
        _isDropped = true;
        StartCoroutine(ApplyImpulseOverTime((Direction * -1), 0.5f));
        //GetComponent<Rigidbody>().AddForce(Direction * -5, ForceMode.Impulse);
    }
    
    IEnumerator ApplyImpulseOverTime(Vector3 impulse, float duration)
    {
        float elapsedTime = 0.0f;
        Vector3 velocityChange = impulse / 1;

        while (elapsedTime < duration)
        {
            // Calculate the position change for this frame
            Vector3 positionChange = velocityChange;
            var newPosition = _handle.GetComponent<Rigidbody>().position + positionChange;
            
            RaycastHit hit;
            if (Physics.Raycast(_handle.GetComponent<Rigidbody>().position, impulse.normalized, out hit, velocityChange.magnitude))
            {
                if(hit.collider.CompareTag("Constraint"))
                    newPosition = hit.point - velocityChange.normalized * 0.05f;
            }

            // Update the kinematic Rigidbody's position
            _handle.GetComponent<Rigidbody>().MovePosition(newPosition);

            velocityChange /= 1.35f;
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(0.2f);

        _isDropped = false;
    }

    #endregion

    #region Utils

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StartCoroutine(Drop());
        if (Properties.heldObject != Properties.gameObject)
        {
            DropCollectable();
        }
        _isDropped = false;
        Properties.heldObject = Properties.gameObject;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
    
    // Set some values
    private void Awake()
    {
        if(GetComponentInChildren<Projector>(true).enabled == false)
            Invisible(false);
        Properties = GetComponent<Player>();
        _handle = transform.Find("AnchorPoint").gameObject;
        DontDestroyOnLoad(gameObject);
        Properties.Model.transform.position = _handle.transform.position - new Vector3(0, 1, 0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void SetReady(bool ready) // used to mark the player as ready and activate the light
    {
        if (!IsReady)
        {
            ReadyUp.UpdateReadiedPlayersCount();
            IsReady = ready;
            ReadyUp.Instance.ShowPlayerReadyStatus(Properties.PlayerNum, ready);
        }
        
    }
    
   
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("Round 1");
        }
        
        Debug.DrawLine(Position + new Vector3(0, 10f, 0), Position + Vector3.down + new Vector3(0, -70, 0), Color.red);
    }

    void checkDelta()
    {
        moveDelta = transform.position - parentPosition;
        parentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        checkDelta();
        Move();
        
    }

    public void Eliminate()
    {
        Properties.RoundReset();
        //print("eliminate");
        Properties.eliminated = true;
        Invisible(true);
    }

    public void Invisible(bool active)
    {
        GetComponentInChildren<Projector>(true).enabled = !active;
        Properties.Model.transform.localScale = (active) ? Vector3.zero : new Vector3(1, 1, 1);
        Position = (active) ? new Vector3(0, -20, 0) : GameUtils.RequestSpawnLocation(Properties.PlayerNum).position;
        GetComponentInChildren<Canvas>(true).gameObject.SetActive(!active);
        
    }
}
