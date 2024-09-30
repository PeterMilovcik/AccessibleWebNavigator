namespace AccessibleWebNavigator.Services;

public interface ISpeechToTextService
{
    Task<string> CaptureVoiceCommandAsync(CancellationToken cancellationToken);
}