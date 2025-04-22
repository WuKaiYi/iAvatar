using DG.Tweening;
using ReadyPlayerMe.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Invertex.Unity.Audio
{
    /// <summary>
    /// Component that allows playback of an audio file from a server or locally that begins playing before the file is completely downloaded.
    /// </summary>
    public class StreamingAudioPlayer : MonoBehaviour
    {
        public AudioSource audioSrc;

        public string streamAudio;
        public AudioType audioStreamType = AudioType.UNKNOWN;
        [Min(64)] public int bytesToDownloadBeforePlaying = 100000;
        private void OnEnable()
        {
            //PlayAudioAsync(streamAudio);
        }
        private void Start()
        {
            //Example usage that can be called from other scripts

        }

        /// <summary>
        /// Play audio with the currently assigned values.
        /// </summary>
        public void PlayAudioAsync()
        {
            PlayAudioAsync(streamAudio);
        }

        /// <summary>
        /// Play audio from given URL, using component's current values.
        /// </summary>
        /// <param name="streamURL">The source URL of the audio you want to play</param>
        public void PlayAudioAsync(string streamURL)
        {
            PlayAudioAsync(streamURL, true, audioStreamType, (ulong)bytesToDownloadBeforePlaying);
        }
        public void PlayAudioAsync(string streamURL, bool autoPlay)
        {
            PlayAudioAsync(streamURL, autoPlay, audioStreamType, (ulong)bytesToDownloadBeforePlaying);
        }

        /// <summary>
        /// Play audio from given URL, using custom AudioType and byte buffering.
        /// </summary>
        /// <param name="streamURL">The source URL of the audio you want to play</param>
        /// <param name="audioType"></param>
        /// <param name="bytesBeforePlayback">How many bytes to download first before starting playback. Useful to buffer longer for slow downloads.</param>

        public void PlayAudioAsync(string streamURL, bool autoPlay, AudioType audioType, ulong bytesBeforePlayback = 10000)
        {
            StartCoroutine(PlayAudioAsyncCoroutine(streamURL, autoPlay, audioType, bytesBeforePlayback));
        }

        private IEnumerator PlayAudioAsyncCoroutine(string streamURL, bool autoPlay, AudioType audioType, ulong bytesBeforePlayback = 10000)
        {

            if (string.IsNullOrWhiteSpace(streamURL)) { Debug.LogWarning("No audio stream URL provided. Playback skipped."); yield break; }

            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(streamURL, audioType))
            {
                DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

                //Allow AudioClip to be played before it's finished downloading
                dlHandler.streamAudio = true;
                //Begin downloading the clip
                var download = uwr.SendWebRequest();
                yield return null;

                //Wait for enough bytes to download
                while (uwr.downloadedBytes < bytesBeforePlayback)
                {
                    if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogWarning($"Error downloading audio stream from:{streamURL} : {uwr.error}");
                        yield break;
                    }
                    yield return null;
                }

                AudioClip audioClip = dlHandler.audioClip;
                //It will be null if it wasn't able to format the audio
                if (audioClip == null)
                {
                    Debug.Log("Couldn't process audio stream.");
                    yield break;
                }

                //Assign the streaming clip and play it now that we're ready.
                audioSrc.clip = audioClip;
                if (autoPlay)
                {
                    audioSrc.Play();
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }
                    currentCoroutine = StartCoroutine(PlayVideoWithAudioLength(audioSrc.clip.length));

                }


                Debug.Log("Playing audio stream!");
                //Continue downloading the rest of the audio stream.
                yield return download;

                Debug.Log("Finished downloading audio stream!");
            }
        }
        Coroutine currentCoroutine;

        IEnumerator PlayVideoWithAudioLength(float audioLength)
        {
          if (this.GetComponent<VideoControl>() != null)
            {
                this.GetComponent<VideoControl>().SetPlayBackPosition(0);
                this.GetComponent<VideoControl>().PlayVideo();
                // 播放视频的代码
                yield return new WaitForSeconds(audioLength);
                // 停止视频的代码
                this.GetComponent<VideoControl>().SetPlayBackPosition(0);
            }
          else
            {
                this.transform.parent.GetComponent<VoiceHandler>().AudioClip = audioSrc.clip;
                this.transform.parent.GetComponent<VoiceHandler>().PlayCurrentAudioClip();
                yield return 0;
            }
            
        }





    }



}