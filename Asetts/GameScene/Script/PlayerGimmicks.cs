using UnityEngine;
using System.Collections;

public class PlayerGimmicks : MonoBehaviour
{
    private bool isFrozen = false; // プレイヤーが凍結されているかどうかのフラグ
    private int tileNumber;

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    private void Update()
    {
        Debug.Log("Update called"); // デバッグログ

        if (isFrozen)
        {
            Debug.Log("Player is frozen"); // デバッグログ
            return;
        }

        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.y);

        // スペースキーが押されたら周囲のタイルの種類をチェック
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed"); // デバッグログ

            // 周囲のギミックの壁があるかどうかをチェック
            if (HasNearbyGimmickWall(currentX, currentY))
            {
                int tileType = Ground.map[currentY, currentX];
                // 現在のタイルの種類に基づいてアクションを決定する
                switch (tileNumber)
                {
                    case 3:
                        Debug.Log("PerformFreezeAction called"); // デバッグログ
                        PerformFreezeAction();
                        break;
                    case 7:
                        Debug.Log("PerformLeverAction called"); // デバッグログ
                        PerformLeverAction();
                        break;
                    default:
                        Debug.Log("未知のタイルです。");
                        break;
                }
            }
            else
            {
                Debug.Log("周囲にギミックの壁がないため、スペースキーに反応しません。");
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
        // レバーアクションの具体的な処理を記述する
        // 例えば、レバーがトグルされたり、他のオブジェクトに影響を与えたりする処理を行う
        // ToggleLeverState() メソッドを呼び出すなど
    }

    private IEnumerator WaitAndUnfreeze(FreezeAndInput freezeAndInput)
    {
        bool passwordCorrect = false;

        while (!passwordCorrect)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESCキーが押されたため、操作画面に戻ります。");
                CloseFreezeScreen();
                SetFrozen(false);
                yield break;
            }

            if (freezeAndInput.IsPasswordCorrect())
            {
                freezeAndInput.Unfreeze();
                SetFrozen(false);
                Debug.Log("パスワードが正しいため、アンフリーズされました。");
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
        // 上下左右の位置をチェックし、どれか一つでもギミックの壁があれば true を返す
        if (Ground.map[y, x - 1] == 3 ||
            Ground.map[y, x + 1] == 3 ||
            Ground.map[y - 1, x] == 3 ||
            Ground.map[y + 1, x] == 3)
        {
            tileNumber = 3; // ギミックの壁がある場合、tileNumber にそのタイルの値をセットする
            return true;
        }

        tileNumber = 0; // ギミックの壁がない場合は、tileNumber をデフォルト値にリセットする
        return false;
    }
}
