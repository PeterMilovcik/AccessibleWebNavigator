using System.Net.Http.Headers;
using System.Text.Json;
using NAudio.Wave;

namespace AccessibleWebNavigator.Services;

public class SpeechToTextService : ISpeechToTextService
{
    private readonly string _apiKey;

    public SpeechToTextService(IApiKeyProvider apiKeyProvider)
    {
        if (apiKeyProvider == null)
        {
            throw new ArgumentNullException(nameof(apiKeyProvider));
        }
        _apiKey = apiKeyProvider.GetOpenAiApiKey();
    }

    public async Task<string> CaptureVoiceCommandAsync(CancellationToken cancellationToken)
    {
        string audioFilePath = Path.Combine(Path.GetTempPath(), "command.wav");
        try
        {
            await RecordAudioAsync(audioFilePath, cancellationToken);
			return await TranscribeAudioAsync(audioFilePath);
        }
        finally
        {
            RemoveTemporaryAudioFile(audioFilePath);
        }
    }

    private async Task RecordAudioAsync(string outputFilePath, CancellationToken cancellationToken)
    {
        using var waveIn = new WaveInEvent();
        using var writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);

        var tcs = new TaskCompletionSource<bool>();

        waveIn.DataAvailable += (sender, args) =>
        {
            writer.Write(args.Buffer, 0, args.BytesRecorded);
        };

        waveIn.RecordingStopped += (sender, args) =>
        {
            writer.Dispose();
            waveIn.Dispose();
            tcs.TrySetResult(true);
        };

        waveIn.WaveFormat = new WaveFormat(16000, 1); // 16 kHz, mono
        waveIn.StartRecording();

        using (cancellationToken.Register(waveIn.StopRecording))
        {
            await tcs.Task;
        }
    }

    private async Task<string> TranscribeAudioAsync(string audioFilePath)
    {
        Console.WriteLine("Transcribing audio...");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var form = new MultipartFormDataContent
        {
            { new StringContent("whisper-1"), "model" },
            { new StringContent("en"), "language" },
            { new StringContent("json"), "response_format" },
            { new StringContent("Please transcribe voice command for web navigation to assist visually impaired users."), "prompt" }
        };

        var fileBytes = File.ReadAllBytes(audioFilePath);
        var fileContent = new ByteArrayContent(fileBytes)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("audio/wav") }
        };
        form.Add(fileContent, "file", Path.GetFileName(audioFilePath));

        var response = await httpClient.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                using var json = JsonDocument.Parse(responseContent);
                if (json.RootElement.TryGetProperty("text", out var textElement))
                {
                    return textElement.GetString() ?? "No transcription text found.";
                }
                else
                {
                    Console.WriteLine("Transcription text not found in response.");
                    return "Transcription text not found in response.";
                }
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"JSON parsing error: {ex.Message}");
                return "Error parsing transcription response.";
            }
        }
        else
        {
            Console.WriteLine($"Error in transcription: {responseContent}");
            return $"Error in transcription: {responseContent}";
        }
    }

    private static void RemoveTemporaryAudioFile(string audioFilePath)
    {
        if (File.Exists(audioFilePath))
        {
            File.Delete(audioFilePath);
        }
    }
}