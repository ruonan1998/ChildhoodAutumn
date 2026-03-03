using UnityEngine;
using UnityEngine.InputSystem; // 新输入系统

[RequireComponent(typeof(CharacterController))]
public class VRPlayerKeyboardMouse : MonoBehaviour
{
    public float speed = 2f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Transform vrCamera;
    private Vector3 velocity;

    private float yaw = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        vrCamera = Camera.main.transform; // VR头显摄像机
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // ---------- 键盘 + 鼠标 ----------
        Vector2 keyboardInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            keyboardInput.y += Keyboard.current.wKey.isPressed ? 1 : 0;
            keyboardInput.y += Keyboard.current.sKey.isPressed ? -1 : 0;
            keyboardInput.x += Keyboard.current.dKey.isPressed ? 1 : 0;
            keyboardInput.x += Keyboard.current.aKey.isPressed ? -1 : 0;
        }

        // 鼠标旋转玩家身体
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        // 移动方向跟随玩家旋转
        move += transform.forward * keyboardInput.y + transform.right * keyboardInput.x;

        // ---------- VR手柄移动 ----------
        Vector2 inputAxis = Vector2.zero;
        UnityEngine.XR.InputDevice leftDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out inputAxis);
        Vector3 camForward = Vector3.ProjectOnPlane(vrCamera.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(vrCamera.right, Vector3.up).normalized;
        move += camForward * inputAxis.y + camRight * inputAxis.x;

        // ---------- 移动 ----------
        controller.Move(move * speed * Time.deltaTime);

        // ---------- 重力 ----------
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}