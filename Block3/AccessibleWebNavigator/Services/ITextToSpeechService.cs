namespace AccessibleWebNavigator.Services;

public interface ITextToSpeechService
{
    Task SpeakAsync(string text);
}