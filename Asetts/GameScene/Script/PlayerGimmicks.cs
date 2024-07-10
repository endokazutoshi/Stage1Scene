using UnityEngine;
using System.Collections;

public class PlayerGimmicks : MonoBehaviour
{
    private bool isFrozen = false; // �v���C���[����������Ă��邩�ǂ����̃t���O
    private int tileNumber;

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    private void Update()
    {
        Debug.Log("Update called"); // �f�o�b�O���O

        if (isFrozen)
        {
            Debug.Log("Player is frozen"); // �f�o�b�O���O
            return;
        }

        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.y);

        // �X�y�[�X�L�[�������ꂽ����͂̃^�C���̎�ނ��`�F�b�N
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed"); // �f�o�b�O���O

            // ���͂̃M�~�b�N�̕ǂ����邩�ǂ������`�F�b�N
            if (HasNearbyGimmickWall(currentX, currentY))
            {
                int tileType = Ground.map[currentY, currentX];
                // ���݂̃^�C���̎�ނɊ�Â��ăA�N�V���������肷��
                switch (tileNumber)
                {
                    case 3:
                        Debug.Log("PerformFreezeAction called"); // �f�o�b�O���O
                        PerformFreezeAction();
                        break;
                    case 7:
                        Debug.Log("PerformLeverAction called"); // �f�o�b�O���O
                        PerformLeverAction();
                        break;
                    default:
                        Debug.Log("���m�̃^�C���ł��B");
                        break;
                }
            }
            else
            {
                Debug.Log("���͂ɃM�~�b�N�̕ǂ��Ȃ����߁A�X�y�[�X�L�[�ɔ������܂���B");
            }
        }
    }

    private void PerformFreezeAction()
    {
        FreezeAndInput freezeAndInput = FindObjectOfType<FreezeAndInput>();
        if (freezeAndInput != null)
        {
            freezeAndInput.Freeze();
            SetFrozen(true);
            StartCoroutine(WaitAndUnfreeze(freezeAndInput));
        }
    }

    private void PerformLeverAction()
    {
        Debug.Log("This tile is a lever tile (tileType == 7). Perform lever action here.");
        // ���o�[�A�N�V�����̋�̓I�ȏ������L�q����
        // �Ⴆ�΁A���o�[���g�O�����ꂽ��A���̃I�u�W�F�N�g�ɉe����^�����肷�鏈�����s��
        // ToggleLeverState() ���\�b�h���Ăяo���Ȃ�
    }

    private IEnumerator WaitAndUnfreeze(FreezeAndInput freezeAndInput)
    {
        bool passwordCorrect = false;

        while (!passwordCorrect)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESC�L�[�������ꂽ���߁A�����ʂɖ߂�܂��B");
                CloseFreezeScreen();
                SetFrozen(false);
                yield break;
            }

            if (freezeAndInput.IsPasswordCorrect())
            {
                freezeAndInput.Unfreeze();
                SetFrozen(false);
                Debug.Log("�p�X���[�h�����������߁A�A���t���[�Y����܂����B");
                passwordCorrect = true;
            }
            else
            {
                SetFrozen(true);
                yield return null;
            }
        }
    }

    public void CloseFreezeScreen()
    {
        FreezeAndInput freezeAndInput = FindObjectOfType<FreezeAndInput>();
        if (freezeAndInput != null)
        {
            freezeAndInput.CloseFreezeScreen();
        }
    }

    private bool HasNearbyGimmickWall(int x, int y)
    {
        // �㉺���E�̈ʒu���`�F�b�N���A�ǂꂩ��ł��M�~�b�N�̕ǂ������ true ��Ԃ�
        if (Ground.map[y, x - 1] == 3 ||
            Ground.map[y, x + 1] == 3 ||
            Ground.map[y - 1, x] == 3 ||
            Ground.map[y + 1, x] == 3)
        {
            tileNumber = 3; // �M�~�b�N�̕ǂ�����ꍇ�AtileNumber �ɂ��̃^�C���̒l���Z�b�g����
            return true;
        }

        tileNumber = 0; // �M�~�b�N�̕ǂ��Ȃ��ꍇ�́AtileNumber ���f�t�H���g�l�Ƀ��Z�b�g����
        return false;
    }
}
