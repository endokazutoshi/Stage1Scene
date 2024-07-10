using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool leverOn = false; // ���o�[�̏������

    [SerializeField] private GameObject leverPrefabOn; // ON�̏�Ԃ̃��o�[��Prefab
    [SerializeField] private GameObject leverPrefabOff; // OFF�̏�Ԃ̃��o�[��Prefab

    private GameObject leverInstance; // ���݂̃��o�[�̃C���X�^���X

    // ���o�[�̃g�O������
    public void ToggleLever()
    {
        leverOn = !leverOn; // ���݂̏�Ԃ𔽓]������

        // ���o�[�̊O�ς�؂�ւ���
        if (leverOn)
        {
            Destroy(leverInstance); // ���݂̃��o�[���폜
            leverInstance = Instantiate(leverPrefabOn, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Destroy(leverInstance); // ���݂̃��o�[���폜
            leverInstance = Instantiate(leverPrefabOff, transform.position, Quaternion.identity, transform);
        }

        // ���̃Q�[���I�u�W�F�N�g�ɑ΂���e���������ŏ������邱�Ƃ��ł��܂�
    }
}
