using System.Net.Http.Headers;
using System.Net.Http.Json;
using NAudio.Wave;

namespace AccessibleWebNavigator.Services;

public class TextToSpeechService : ITextToSpeechService
{
    private readonly string _apiKey;

    public TextToSpeechService(IApiKeyProvider apiKeyProvider)
    {
        _apiKey = apiKeyProvider.GetOpenAiApiKey();
    }

    public async Task SpeakAsync(string text)
    {
        Console.WriteLine($"Synthesizing text to speech...");
        var audioData = await SynthesizeSpeechAsync(text);

        if (audioData != null && audioData.Length > 0)
        {
            Console.WriteLine("Playing audio...");
            await PlayAudioAsync(audioData);
            Console.WriteLine("Audio playback complete.");
        }
        else
        {
            Console.Error.WriteLine("Failed to synthesize speech.");
        }
    }

    private async Task<byte[]> SynthesizeSpeechAsync(string text)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var requestContent = new
            {
                model = "tts-1",
                input = text,
                voice = "onyx" // You can try different voices here
            };
            // for more voice options, visit: https://platform.openai.com/docs/guides/text-to-speech/quickstart

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/audio/speech", requestContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error in TTS API call: {error}");
                return Array.Empty<byte>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while synthesizing speech: {ex.Message}");
            return Array.Empty<byte>();
        }
    }

    private async Task PlayAudioAsync(byte[] audioData)
    {
        using var ms = new MemoryStream(audioData);
        using var rdr = new Mp3FileReader(ms);
        using var waveOut = new WaveOutEvent();
        waveOut.Init(rdr);
        waveOut.Play();
        while (waveOut.PlaybackState == PlaybackState.Playing)
        {
            await Task.Delay(500);
        }
    }
}