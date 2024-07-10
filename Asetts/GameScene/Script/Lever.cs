using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool leverOn = false; // レバーの初期状態

    [SerializeField] private GameObject leverPrefabOn; // ONの状態のレバーのPrefab
    [SerializeField] private GameObject leverPrefabOff; // OFFの状態のレバーのPrefab

    private GameObject leverInstance; // 現在のレバーのインスタンス

    // レバーのトグル処理
    public void ToggleLever()
    {
        leverOn = !leverOn; // 現在の状態を反転させる

        // レバーの外観を切り替える
        if (leverOn)
        {
            Destroy(leverInstance); // 現在のレバーを削除
            leverInstance = Instantiate(leverPrefabOn, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Destroy(leverInstance); // 現在のレバーを削除
            leverInstance = Instantiate(leverPrefabOff, transform.position, Quaternion.identity, transform);
        }

        // 他のゲームオブジェクトに対する影響をここで処理することもできます
    }
}
