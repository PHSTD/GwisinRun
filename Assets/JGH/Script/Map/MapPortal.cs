using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPortal : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private Transform destinationPosition;
    [SerializeField] private MapPortal destinationPortal;

    // 포탈 사용 추적 (전역)
    private static GameObject lastPlayer = null;
    private static MapPortal lastExitedPortal = null;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 이전에 이 포탈에서 나왔으면 무시 (아직 나가지 않았음)
        if (other.gameObject == lastPlayer && lastExitedPortal == this)
            return;

        TeleportPlayer(other);
    }

    private void TeleportPlayer(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;

            // 위치 약간 위로
            other.transform.position = destinationPosition.position + Vector3.up * 0.5f;

            // 회전은 Y축만 유지 (눕는 문제 방지)
            Quaternion targetRotation = Quaternion.Euler(0, destinationPosition.rotation.eulerAngles.y, 0);
            other.transform.rotation = targetRotation;

            controller.enabled = true;
        }
        else
        {
            other.transform.position = destinationPosition.position + Vector3.up * 0.5f;
            Quaternion targetRotation = Quaternion.Euler(0, destinationPosition.rotation.eulerAngles.y, 0);
            other.transform.rotation = targetRotation;
        }
        
        lastPlayer = other.gameObject;
        lastExitedPortal = destinationPortal;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == lastPlayer)
        {
            // 나가면 기록 삭제
            lastPlayer = null;
            lastExitedPortal = null;
        }
    }
}
