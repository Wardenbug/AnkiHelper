namespace AnkiHelper.Core.Abstractions;

public interface IAnkiHelper
{
    public Task<bool> HandleMessageAsync(
        string message, 
        AnkiHelperConfiguration config, 
        CancellationToken cancellationToken = default);
}