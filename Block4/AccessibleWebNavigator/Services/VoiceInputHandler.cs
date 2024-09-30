namespace AccessibleWebNavigator.Services;

public class VoiceInputHandler
{
    private readonly ISpeechToTextService _speechToTextService;

    public VoiceInputHandler(ISpeechToTextService speechToTextService)
    {
        _speechToTextService = speechToTextService ?? throw new ArgumentNullException(nameof(speechToTextService));
    }

    public async Task<string> GetVoiceCommandAsync()
    {
        using var cts = new CancellationTokenSource();

        Console.WriteLine("Press Enter to start recording your command.");
        Console.ReadLine();

        Console.WriteLine("Recording... Press Enter to stop.");
        var recordingTask = _speechToTextService.CaptureVoiceCommandAsync(cts.Token);

        // Wait for the user to press Enter to stop recording
        await Task.Run(() => Console.ReadLine());

        // Cancel the recording
        cts.Cancel();

        var commandText = await recordingTask;

        return commandText;
    }
}