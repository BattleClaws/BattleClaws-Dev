using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private GameObject _handle;
    private bool _isDropped;

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
    public void OnMove(InputAction.CallbackContext ctx) => _input = ctx.ReadValue<Vector2>();
    
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
            var startPosition = Position;
            var goalPosition = Position - new Vector3(0, 0.4f, 0);
            //print(startPosition + " | " + goalPosition);

            for (float i = 0; i < 1.1f; i += 0.1f)
            {
                Position = Vector3.Slerp(startPosition, goalPosition, i);
                yield return new WaitForSeconds(0.02f);
            }

            if (Properties.heldObject == Properties.gameObject)
            {
                CheckForCollectable();
            }
            else
            {
                DropCollectable();
            }
            
            yield return new WaitForSeconds(0.6f);

            for (float i = 1; i >= -0.1f; i -= 0.2f)
            {
                Position = Vector3.Slerp(startPosition, goalPosition, i);
                yield return new WaitForSeconds(0.02f);
            }

            Position = startPosition;

            _isDropped = false;
        }
    }

    private void CheckForCollectable()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Position + new Vector3(0, 50, 0), 1.5f, Vector3.down * 1000f, out hit) &&
            hit.collider.CompareTag("Collectable"))
        {
            var target = hit.collider.gameObject;
            
            target.transform.SetParent(Properties.Model.transform);
            target.GetComponent<Rigidbody>().useGravity = false;
            Properties.heldObject = target;
        }
    }

    private void DropCollectable()
    {
        Properties.heldObject.transform.parent = null;
        Properties.heldObject.GetComponent<Rigidbody>().useGravity = true;
        Properties.heldObject = null;
    }


    #endregion
    
    // Set some values
    private void Awake()
    {
        Properties = GetComponent<Player>();
        _handle = transform.Find("AnchorPoint").gameObject;
    }

    private void Update()
    {
        Move();
    }

    
}
