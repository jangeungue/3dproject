using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GMFollow : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 offset;

    public float speed = 10.0f;


    private Camera thisCamera;
    private Vector3 worldDefalutForward;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        worldDefalutForward = transform.forward;
    }
    void Update()
    {
        transform.localPosition = cameraTarget.position + offset;      

        float scroll = Input.GetAxis("Mouse ScrollWheel") * -speed;

        //�ִ� ����
        if (thisCamera.fieldOfView <= 20.0f && scroll < 0)
        {
            thisCamera.fieldOfView = 20.0f;
        }
        // �ִ� �� �ƿ�
        else if (thisCamera.fieldOfView >= 60.0f && scroll > 0)
        {
            thisCamera.fieldOfView = 60.0f;
        }
        // ���� �ƿ� �ϱ�.
        else
        {
            thisCamera.fieldOfView += scroll;
        }

        // ���� ���� ������ ���� ĳ���͸� �ٶ󺸵��� �Ѵ�.
        if (cameraTarget && thisCamera.fieldOfView <= 30.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation
                , Quaternion.LookRotation(cameraTarget.position - transform.position)
                , 0.15f);
        }
        // ���� ���� �ۿ����� ������ ī�޶� �������� �ǵ��� ����.
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation
                , Quaternion.LookRotation(worldDefalutForward)
                , 0.15f);
        }
    }
}
