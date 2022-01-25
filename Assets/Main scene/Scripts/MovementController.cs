using UnityEngine;

namespace GAME.CONTROL
{
    public class MovementController : MonoBehaviour
    {

        [SerializeField] private float _speed = 5f;
        private PlayerControlInput _inputSystem;
       
        private void Awake()
        {
            _inputSystem = new PlayerControlInput();
            _inputSystem.Player.Enable();
            
        }

        private void Update()
        {
            Vector2 dir = _inputSystem.Player.Move.ReadValue<Vector2>();

            Move(new Vector3(dir.x, 0, dir.y));
        }


        private void Move(Vector3 direction)
        {
            transform.Translate(direction * Time.deltaTime * _speed, Space.World);

            if (direction != Vector3.zero)
            {
                transform.forward = direction;

                transform.Rotate(new Vector3(0, -90, 0));
            }
        }
    }
}
