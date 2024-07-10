//�}�b�v�̏����ׂ����ݒ�ł���}�b�v�X�N���v�g
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject[] _goalPrefabs;
    [SerializeField] private GameObject _PIN;
    [SerializeField] private GameObject _deadPrefab;
    [SerializeField] private GameObject[] _switchPrefabs;
    [SerializeField] private GameObject _leverPrefabOn;
    [SerializeField] private GameObject _leverPrefabOff;
    [SerializeField] private float _goalSceneTime;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int _spawnPositionX; // Inspector����ݒ�\�ȏo���ʒuX
    [SerializeField] private int _spawnPositionY; // Inspector����ݒ�\�ȏo���ʒuY
    [SerializeField] private int _tileTypeToSpawn; // Inspector����ݒ�\�ȏo������^�C���̃^�C�v

    private GameObject leverInstance;

    private bool leverOn = false;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject passWorldPrefab;
    [SerializeField] private GameObject deadPrefab;
    [SerializeField] private GameObject switchPrefab;
    [SerializeField] private GameObject moveRandomPrefab;

    const int _nLengthStart = 1;
    const int _nWidthStart = 1;
    const int _nLengthEnd = 22;
    const int _nWidthEnd = 9;
    const int _nCenter = 11;

    [SerializeField] private int _length;
    [SerializeField] private int _width;

    // 2D�}�b�v�f�[�^:
    //0=�ǁA1=���A2=�S�[���A3=�Í��A4=���S�A5=�X�C�b�`�A6=�ړ��L�[�����Ⴎ����}�X�A7=���o�[
    public static int[,] map = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0},
        {0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 3, 0, 1, 0, 0},
        {0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0},
        {0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0},
        {0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0},
        {0, 3, 0, 0, 0, 1, 1, 1, 4, 1, 7, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    private void Start()
    {
        // �X�e�[�W�̈ړ��}�X�A�ǃ}�X�̔z�u�̏���
        for (int length = 0; length < _length; length++)
        {
            for (int width = 0; width < _width; width++)
            {
                int tileType;
                if (length >= _nLengthStart && length < _nLengthEnd && width >= _nWidthStart && width < _nWidthEnd) // ���̍��W��0����22�͈̔͂̏ꍇ
                {
                    tileType = map[width, length]; // ������Map�̏����擾���邽�߂�tileType��map�̏�������
                }
                else
                {
                    tileType = 0; // �O�g�̏ꍇ�͕ǂƂ���
                }

                if (tileType == 0) // map�̏���0�������ꍇ�͕ǂ�z�u����
                {
                    Instantiate(_wallPrefab, new Vector3(length, width, 0), Quaternion.identity, transform); // �ǂ̕\��
                }

                if (tileType == 1) // map�̏���1�������ꍇ�͐i�߂�
                {
                    Instantiate(_tilePrefab, new Vector3(length, width, 0), Quaternion.identity, transform); // �ړ��ł���}�X(�^�C��)�̕\��
                }

                if (tileType == 2) // map�̏���2�������ꍇ�̓S�[������(�����Ƃ��ė�����Player���S�[�����Ȃ��Ƃ����Ȃ�)
                {
                    Vector3 goalPosition = new Vector3(length, width, 0);
                    StartCoroutine(SwitchGoalPrefab(goalPosition));
                }

                if (tileType == 3) // map�̏���3�������ꍇ�̓M�~�b�N��W�J����
                {
                    PlacePassword(new Vector3(length, width, 0));
                }

                if (tileType == 4)
                {
                    Vector3 deadPosition = new Vector3(length, width, 0);
                    Instantiate(_deadPrefab, new Vector3(length, width, 0), Quaternion.identity, transform); // �ړ��ł���}�X(�^�C��)�̕\��
                }

                if (tileType == 5)
                {
                    Switch(new Vector3(length, width, 0));
                }

                //���o�[�̒ǉ�
                if (tileType == 7)
                {
                    leverInstance = Instantiate(_leverPrefabOff, new Vector3(length, width, 0), Quaternion.identity, transform);
                    // ������ leverInstance ��K�؂ɏ������邽�߂̒ǉ��̐ݒ���s���\��������܂�
                }
                if(tileType == 8)
                {
                    leverInstance = Instantiate(_leverPrefabOn, new Vector3(length, width, 0), Quaternion.identity, transform);
                }
            }
        }
    }


    IEnumerator SwitchGoalPrefab(Vector3 position)
    {
        GameObject goalInstance = null;
        int goalIndex = 0;

        while (true)
        {
            // ���݂�Prefab���폜
            if (goalInstance != null)
            {
                Destroy(goalInstance);
            }

            // ����Prefab���C���X�^���X��
            goalInstance = Instantiate(_goalPrefabs[goalIndex], position, Quaternion.identity, transform);

            // �C���f�b�N�X���X�V
            goalIndex = (goalIndex + 1) % _goalPrefabs.Length;

            // 1�b�҂�
            yield return new WaitForSeconds(_goalSceneTime);
        }
    }

    private void PlacePassword(Vector3 position)
    {
        Instantiate(_PIN, position, Quaternion.identity, transform);
        Debug.Log("�M�~�b�N�}�X��z�u: " + position);

        // �Ïؔԍ��̎��͂ŃX�y�[�X�L�[�������ꂽ��Ïؔԍ��p�l����\��
        if (IsSpaceKeyPressedNearPosition(position))
        {
            // Inspector����ݒ肳�ꂽ�ʒu�ƃ^�C���^�C�v�Ɋ�Â��čX�V����
            UpdateTileType(_spawnPositionX, _spawnPositionY, _tileTypeToSpawn);
        }
    }

    public void UpdateTileType(int x, int y, int newTileType)
    {
        // x��y���L���Ȕ͈͓��ɂ��邱�Ƃ��m�F
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            map[y, x] = newTileType; // �}�b�v��V�����^�C���^�C�v�ōX�V����

            // �^�C���^�C�v�Ɋ�Â��đΉ�����v���n�u���C���X�^���X������
            switch (newTileType)
            {
                case 0: // ��
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 1: // ��
                    Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 2: // �S�[��
                    Instantiate(goalPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 3: //�Í�
                    Instantiate(passWorldPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 4: //���S
                    Instantiate(deadPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 5: //�X�C�b�`
                    Instantiate(switchPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                case 6: //�ړ��L�[�����Ⴎ����
                    Instantiate(moveRandomPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    break;
                    // �K�v�ɉ����đ��̃^�C���^�C�v�ɑ΂��鏈����ǉ�����
            }
        }
        else
        {
            Debug.LogError("�^�C���^�C�v�̍X�V�ɖ����Ȉʒu: (" + x + ", " + y + ")");
        }
    }




    // �w�肳�ꂽ�ʒu�̎��͂ŃX�y�[�X�L�[�������ꂽ���ǂ������`�F�b�N���郁�\�b�h
    private bool IsSpaceKeyPressedNearPosition(Vector3 position)
    {
        // �X�y�[�X�L�[��������Ă��邩�A�w�肳�ꂽ�ʒu�̎��͂Ƀv���C���[�����݂��邩���`�F�b�N
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1f); // �w�肳�ꂽ�ʒu�̔��a1�̉~�̈���̃R���C�_�[���擾
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player")) // �v���C���[�^�O�����I�u�W�F�N�g�����݂���ꍇ
                {
                    return true; // �X�y�[�X�L�[�������ꂽ���v���C���[�����͂ɑ��݂���ꍇ��true��Ԃ�
                }
            }
        }
        return false;
    }

    private void Switch(Vector3 position)
    {
        if (_switchPrefabs.Length > 0) // _switchPrefabs����łȂ����Ƃ��m�F
        {
            Instantiate(_switchPrefabs[0], position, Quaternion.identity, transform); // �Ƃ肠�����ŏ���Prefab��z�u
            Debug.Log("�X�C�b�`��z�u: " + position);
        }
        else
        {
            Debug.LogError("�X�C�b�`��Prefab���Z�b�g����Ă��܂���B_switchPrefabs��Prefab���Z�b�g���Ă��������B");
        }
    }

    public int GetTileTypeAtPosition(Vector3 position)
    {
        // position �𐮐��ɕϊ����A�Ή�����^�C���^�C�v��Ԃ�
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        return map[y, x];
    }

    public int Width
    {
        get { return map.GetLength(1); }
    }

    public int Length
    {
        get { return map.GetLength(0); }
    }

    // ���o�[�̃N���b�N���o�ƃg�O�������̗�
    void OnMouseDown()
    {
        ToggleLeverState();
    }

    void ToggleLeverState()
    {
        leverOn = !leverOn; // ���݂̏�Ԃ𔽓]������

        // ���o�[�̊O�ς�؂�ւ���
        if (leverOn)
        {
            Destroy(leverInstance); // ���݂̃��o�[���폜
            leverInstance = Instantiate(_leverPrefabOn, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Destroy(leverInstance); // ���݂̃��o�[���폜
            leverInstance = Instantiate(_leverPrefabOff, transform.position, Quaternion.identity, transform);
        }

        // ���̃Q�[���I�u�W�F�N�g�ɑ΂���e���������ŏ������邱�Ƃ��ł��܂�
    }
}
