using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class ScreenXMoviePlayer : MonoBehaviour
{
    private const float HelpTextDuration = 2.5f;

    private int _iterPlaylist;
    private ScreenXPlaylist _playlist;
    private ScreenXPlaylist.Item _currentPlaylistItem;
    private ScreenXStateMachine _stateMachine;
    private FixedDurationMessage _helpText;    

    public VideoPlayer VideoPlayer { private set; get; }

    public AirVRCameraRig cameraRig;
    public ScreenXScreen screen;
    public ScreenXLightRenderer lightRenderer;

    private void Awake()
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        _playlist = GetComponent<ScreenXPlaylist>();
        _stateMachine = GetComponent<ScreenXStateMachine>();
        _helpText = GetComponentInChildren<FixedDurationMessage>();
    }

    private void Start()
    {
        _stateMachine.StateChanged += StateChanged;
        VideoPlayer.prepareCompleted += VideoPlayerOnPrepareCompleted;
        VideoPlayer.started += VideoPlayerOnStarted;
    }

    private void VideoPlayerOnStarted(VideoPlayer source)
    {
        screen.content = VideoPlayer.texture;
        screen.SetState(_currentPlaylistItem.isScreenX ? ScreenXScreen.State.ScreenX : ScreenXScreen.State.Single);
        lightRenderer.EnableScreenReflection(true);
    }

    private void VideoPlayerOnPrepareCompleted(VideoPlayer source)
    {
        _stateMachine.MovieLoaded();
    }
    

    private void Update()
    {
        if (AirVRInput.GetButtonDown(cameraRig, AirVRInput.Touchpad.Button.Touch) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_stateMachine.CurrentState == ScreenXStateMachine.State.Ready)
            {
                _stateMachine.StartMovie(1.5f);
            }
            else if (_stateMachine.CurrentState == ScreenXStateMachine.State.Playing)
            {
                _helpText.Show(HelpTextDuration);
            }
        }

        else if (AirVRInput.GetButtonDown(cameraRig, AirVRInput.Touchpad.Button.BackButton) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (_stateMachine.CurrentState == ScreenXStateMachine.State.Playing)
            {
                _stateMachine.StopMovie();
            }
        }
    }

    private void StateChanged(ScreenXStateMachine statemachine, ScreenXStateMachine.State from, ScreenXStateMachine.State to)
    {
        if (from == ScreenXStateMachine.State.Ready && to == ScreenXStateMachine.State.TurningLightDown)
        {
            lightRenderer.TurnDownLights();
        }

        else if (from == ScreenXStateMachine.State.TurningLightDown && to == ScreenXStateMachine.State.Loading)
        {
            _currentPlaylistItem = _playlist.GetFirstItem(ref _iterPlaylist);
            LoadMovie(_currentPlaylistItem);
        }

        else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Loading)
        {
            _currentPlaylistItem = _playlist.GetNextItem(ref _iterPlaylist);
            LoadMovie(_currentPlaylistItem);
        }

        else if (from == ScreenXStateMachine.State.Loading && to == ScreenXStateMachine.State.Playing)
        {
            VideoPlayer.targetMaterialRenderer = screen.screenX;
            VideoPlayer.Play();
        }

        else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Paused)
        {
            VideoPlayer.Pause();
        }

        else if (from == ScreenXStateMachine.State.Paused && to == ScreenXStateMachine.State.Playing)
        {
            VideoPlayer.Play();
        }

        else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Stopping)
        {
            VideoPlayer.Stop();
            VideoPlayer.targetMaterialRenderer = null;
            _stateMachine.MovieStopped();
        }

        else if ((from == ScreenXStateMachine.State.Playing || from == ScreenXStateMachine.State.Stopping) && to == ScreenXStateMachine.State.Ready)
        {
            screen.SetState(ScreenXScreen.State.Off);

            lightRenderer.TurnUpLights();
            lightRenderer.EnableScreenReflection(false);
            _helpText.HideImmediately();
        }
    }

    private void LoadMovie(ScreenXPlaylist.Item item)
    {
        if (item.isStreamingAssets)
        {
            VideoPlayer.url = Application.streamingAssetsPath + "/" + item.url;
        }
        else
        {
            VideoPlayer.url = item.url;
        }
        VideoPlayer.Prepare();
    }
}
