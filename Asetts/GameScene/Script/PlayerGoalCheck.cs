//�v���C���[�̃S�[���̔�����s���Ă���X�N���v�g
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGoalCheck : MonoBehaviour
{
    private bool goalReached = false; // �S�[���ɓ��B�������ǂ����̃t���O
    private PlayerMovement playerMovement;

    private void Start()
    {
        // PlayerMovement�R���|�[�l���g���擾
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on this GameObject. Please ensure PlayerMovement is attached.");
        }
    }

    private void Update()
    {
        if (!goalReached) // �܂��S�[���ɓ��B���Ă��Ȃ��ꍇ�ɂ̂݃`�F�b�N���s��
        {
            CheckGoalReached();
        }
    }

    private void CheckGoalReached()
    {
        if (playerMovement == null) return;

        Vector2 targetPos = playerMovement.GetTargetPos();
        int targetX = Mathf.RoundToInt(targetPos.x);
        int targetY = Mathf.RoundToInt(targetPos.y);

        if (Mathf.Approximately(transform.position.x, targetPos.x) && Mathf.Approximately(transform.position.y, targetPos.y))
        {
            if (Ground.map[targetY, targetX] == 2)
            {
                GameObject[] player1Objects = GameObject.FindGameObjectsWithTag("Player1");
                GameObject[] player2Objects = GameObject.FindGameObjectsWithTag("Player2");

                bool player1AtGoal = false;
                bool player2AtGoal = false;

                foreach (GameObject p1 in player1Objects)
                {
                    PlayerMovement p1Script = p1.GetComponent<PlayerMovement>();
                    if (p1Script == null) continue;
                    int playerX = Mathf.RoundToInt(p1Script.GetTargetPos().x);
                    int playerY = Mathf.RoundToInt(p1Script.GetTargetPos().y);

                    if (Ground.map[playerY, playerX] == 2)
                    {
                        player1AtGoal = true;
                        break;
                    }
                }

                foreach (GameObject p2 in player2Objects)
                {
                    PlayerMovement p2Script = p2.GetComponent<PlayerMovement>();
                    if (p2Script == null) continue;
                    int playerX = Mathf.RoundToInt(p2Script.GetTargetPos().x);
                    int playerY = Mathf.RoundToInt(p2Script.GetTargetPos().y);

                    if (Ground.map[playerY, playerX] == 2)
                    {
                        player2AtGoal = true;
                        break;
                    }
                }

                if (player1AtGoal && player2AtGoal)
                {
                    goalReached = true; // �����̃v���C���[���S�[���ɓ��B�����ꍇ�Ƀt���O��ݒ�
                    Debug.Log("Both players reached the goal. Loading ClearScene...");
                    SceneManager.LoadScene("ClearScene");
                }
            }
        }
    }
}
