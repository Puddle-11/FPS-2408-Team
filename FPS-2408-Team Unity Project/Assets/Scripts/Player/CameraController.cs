using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    [Space]
    [Header("Camera variables")]
    [Space]
    [SerializeField] private int sens;
    [SerializeField] private float miny, maxy;
    [SerializeField] private bool invert;
    [SerializeField] private Transform cameraAnchor;


    [Space]
    [Header("Camera Offset variables")]
    [Space]
    [SerializeField] private float offsetResetSpeed;
    [SerializeField] private float offsetResetDampening;
    private float rotX;
    [Header("Camera Shake variables")]
    [Space]
    [SerializeField] AnimationCurve camShakeIntensity;
    [SerializeField]  private float camShakeScalar;
    [SerializeField] private float camShakeDurration;
    public bool resettingOffset;
    public Vector2 offset;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Found two Camera Controllers in scene\nDestroyed at " + gameObject.name);
            Destroy(this);
        }
    }
    public void StartCamShake()
    {
        StartCoroutine(StartCamShakeEnum(camShakeDurration, camShakeScalar, camShakeIntensity));
    }

    public void StartCamShake(float _durr, float _scalar)
    {
        StartCoroutine(StartCamShakeEnum(_durr, _scalar, camShakeIntensity));
    }

    private IEnumerator StartCamShakeEnum(float _durr,  float _scalar, AnimationCurve _intensity)
    {
        if (_durr == 0) _durr = camShakeDurration;
        if (_scalar == 0) _scalar = camShakeScalar;
        Vector3 originPos = transform.localPosition;
        float timeElapsed = 0.0f;

        while (timeElapsed < _durr)
        {
        
                float evaluatedIntensity = _intensity.Evaluate(timeElapsed / camShakeDurration);
                float x = Random.Range(-1f, 1f) * _scalar * evaluatedIntensity;
                float y = Random.Range(-1f, 1f) * _scalar * evaluatedIntensity;
                transform.localPosition = new Vector3(x, y, originPos.z);
                timeElapsed += Time.deltaTime;
                yield return null;
           
            

        }
        transform.localPosition = originPos;

    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState  = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance == null || (UIManager.instance != null && !UIManager.instance.GetStatePaused()))
        {
            UpdateCamPos();
        }
    }
    private void FixedUpdate()
    {
       if(resettingOffset) ResetOffsetPos();
    }

    public void UpdateCamPos()
    {

        float yaw = Input.GetAxis("Mouse X") * sens;
        float pitch = Input.GetAxis("Mouse Y") * sens;
        rotX = invert ? rotX + pitch: rotX - pitch;
        rotX = Mathf.Clamp(rotX, miny, maxy);

        cameraAnchor.localRotation = Quaternion.Euler(rotX + offset.y, 0, 0);
        GameManager.instance.playerRef.transform.Rotate(Vector3.up * yaw);
    }
    public void UpdateOffsetPos(Vector2 _offset)
    {
        SetOffsetPos(offset + _offset);
    }
    public void SetOffsetPos(Vector2 _offset)
    {
        offset = _offset;
    }
    public void ResetOffset(bool _val)
    {
        resettingOffset = _val;
    }
    private void ResetOffsetPos()
    {
        offset = Vector2.MoveTowards(offset, Vector2.zero, offsetResetSpeed + Mathf.Pow(Vector2.Distance(offset, Vector2.zero), 2) * offsetResetDampening);
        if (Mathf.Abs(offset.y) < 0.01) offset.y = 0;
        if (Mathf.Abs(offset.x) < 0.01) offset.x = 0;
    }
}
