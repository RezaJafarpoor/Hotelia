namespace Hotelia.Shared.Common;

public interface IEndpoint
{
    void RegisterEndpoint(IEndpointRouteBuilder app);
}