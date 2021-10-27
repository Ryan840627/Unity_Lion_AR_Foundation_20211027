using UnityEngine;
using UnityEngine.XR.ARFoundation;  //�ޥ� Foundation API
using UnityEngine.XR.ARSubsystems;  //�ޥ� ARSubsystems API
using System.Collections.Generic;   //�ޥ� ���X API�A�]�t�M�� List

namespace Ryan.ARFoundation
{
    /// <summary>
    /// �����a�O�I���޲z��
    /// �I���a�O��B�z���ʦ欰
    /// �ͦ�����P������m
    /// </summary>
    public class DetechGroundManager : MonoBehaviour
    {
        [Header("�I����n�ͦ�������")]
        public GameObject goSpawn;
        [Header("AR �g�u�޲z��"),Tooltip("���޲z���n��b Origin ����W")]
        public ARRaycastManager arRaycastManager;
        [Header("�ͦ�����n���V����v������")]
        public Transform traCamera;
        [Header("���󭱬���v���t��"),Range(0,100)]
        public float speedLookAt;

        private Transform traSpawn;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();  //�M����� = �s�W ���� �M�檫��

        /// <summary>
        /// �ƹ�����PĲ��
        /// </summary>
        public bool inputMouseLeft { get=> Input.GetKeyDown(KeyCode.Mouse0); }


        private void Update()
        {
            ClickAndDetechGround();
            SpawnLookAtCamera();
        }
        /// <summary>
        /// �I���P�˴��a�O
        /// 1.�����O�_�����w����
        /// 2.�N�I���y�Ь���
        /// 3.�g�u����
        /// 4.����
        /// </summary>
        private void ClickAndDetechGround()
        {
            if (inputMouseLeft)                                                                         //�p�G���U���w�y��
            {
                Vector2 positionMouse = Input.mousePosition;                                            //���o �I���y��

                // Ray ray = Camera.main.ScreenPointToRay(positionMouse);                               //�N �I���y�� �ର �g�u

                if (arRaycastManager.Raycast(positionMouse,hits,TrackableType.PlaneWithinPolygon))      //�p�G �g�u ������w���a�O����
                {
                    Vector3 positionHit = hits[0].pose.position;                                        //���o�I�쪺�y��

                    if (traSpawn == null)
                    {
                        traSpawn = Instantiate(goSpawn, positionHit, Quaternion.identity).transform;    //�N����ͦ��b�I�쪺�y�ФW
                        traSpawn.localScale = Vector3.one * 0.5f;                                       //�վ�ؤo
                    }
                    else
                    {
                        traSpawn.position = positionHit;                                                //�_�h ���ͦ��L������ �N��s�y��
                        
                    }
                }
            }
        }

        private void SpawnLookAtCamera()
        {
            if (traSpawn == null) return;
            Quaternion angle = Quaternion.LookRotation(traCamera.position - traSpawn.position);          //���o�V�q
            traSpawn.rotation = Quaternion.Lerp(traSpawn.rotation, angle, Time.deltaTime * speedLookAt); //���״���
            Vector3 angleOrigial = traSpawn.localEulerAngles;                                            //���o����
            angleOrigial.x = 0;                                                                          //�ᵲX
            angleOrigial.z = 0;                                                                          //�ᵲZ
            traSpawn.localEulerAngles = angleOrigial;                                                    //��s����
        }
    }

}
