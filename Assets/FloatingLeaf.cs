using UnityEngine;

public class FloatingLeafElegant : MonoBehaviour
{
    [Header("Fall Settings")]
    public Transform target;          // 目标落点
    public float startHeight = 2.5f;  // 初始高度
    public float fallDuration = 3f;   // 落下总时间

    [Header("Sway Settings")]
    public float swayAmount = 0.15f;
    public float swaySpeed = 1.2f;

    [Header("Idle Hover After Landing")]
    public float hoverAmount = 0.03f;
    public float hoverSpeed = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private float timer = 0f;
    private bool hasLanded = false;
    private float randomOffset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned!");
            return;
        }

        randomOffset = Random.Range(0f, 100f);

        // 设置起始位置（目标正上方）
        endPos = target.position;
        startPos = new Vector3(endPos.x, endPos.y + startHeight, endPos.z);

        transform.position = startPos;
    }

    void Update()
    {
        if (!hasLanded)
        {
            timer += Time.deltaTime;
            float t = timer / fallDuration;

            // 使用 SmoothStep 让落地减速更自然
            float smoothT = Mathf.SmoothStep(0, 1, t);

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, smoothT);

            // 左右摆动
            float sway = Mathf.Sin(Time.time * swaySpeed + randomOffset) * swayAmount;
            currentPos.x += sway;

            transform.position = currentPos;

            if (t >= 1f)
            {
                hasLanded = true;
                transform.position = endPos;
            }
        }
        else
        {
            // 落地后轻微呼吸浮动
            float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
            transform.position = endPos + new Vector3(0, hover, 0);
        }
    }
}