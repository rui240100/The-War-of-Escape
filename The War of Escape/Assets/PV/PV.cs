using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Video;

public class PV : MonoBehaviour

{

    public float waitTime = 10.0f; // 何秒放っておいたら動画が流れるか（エディタ側で変更して）

    float elapsedTime = 0.0f;   // 経過時間

    Vector3 lastMousePosition;  // マウスの位置

    bool isPlayeng = false;     // 再生中かどうかフラグ

    VideoPlayer player;         // 動画プレイヤーコンポーネント

    public Canvas canvas;       // Unity側でCanvasをアタッチ

    public AudioSource bgmSource; // タイトル画面のBGM用 AudioSource（Unity側でアタッチ）

    private void Start()

    {

        player = GetComponent<VideoPlayer>();

        player.loopPointReached += Stop; // 動画が終わったらStopが呼ばれるようイベントを仕込む

        lastMousePosition = Input.mousePosition; // 初期マウス位置

    }

    void Update()

    {

        if (!isPlayeng)

        {

            elapsedTime += Time.deltaTime;

            if (Input.anyKeyDown || (Input.mousePosition != lastMousePosition) || PadCheck())

            {

                elapsedTime = 0.0f;

                lastMousePosition = Input.mousePosition;

            }

            if (elapsedTime > waitTime)

            {

                elapsedTime = 0.0f;

                Play();

            }

        }

        else

        {

            if (Input.anyKeyDown || PadCheck())

            {

                Stop(player);

            }

        }

    }

    /// <summary>

    /// 再生

    /// </summary>

    private void Play()

    {

        player.Play();

        isPlayeng = true;

        canvas.GetComponent<Canvas>().enabled = false; // 動画再生時はUIを消す

        if (bgmSource != null && bgmSource.isPlaying)

        {

            bgmSource.Stop(); // BGMを停止

        }

    }

    /// <summary>

    /// 停止

    /// </summary>

    /// <param name="vp">プレイヤー（動画終了時に呼ばれるため引数が必要）</param>

    private void Stop(VideoPlayer vp)

    {

        canvas.GetComponent<Canvas>().enabled = true; // 再生停止したらUIを復活

        player.Stop();

        isPlayeng = false;

        if (bgmSource != null)

        {

            bgmSource.Play(); // BGMを再開

        }

    }

    bool PadCheck()

    {

        for (int joyNum = 1; joyNum <= 8; joyNum++) // 最大8台まで想定

        {

            for (int button = 0; button <= 19; button++)

            {

                string keyName = $"Joystick{joyNum}Button{button}";

                KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName);

                if (Input.GetKeyDown(code))

                {

                    return true;

                }

            }

        }

        for (int button = 0; button <= 19; button++)

        {

            KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"JoystickButton{button}");

            if (Input.GetKeyDown(code))

            {

                return true;

            }

        }

        return false;

    }

}
