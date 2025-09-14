using Microsoft.AspNetCore.Components;

namespace Kubrik.Web.Components;

public partial class Redirect : ComponentBase
{
    private readonly NavigationManager _navigation;

    public Redirect(NavigationManager navigation)
    {
        _navigation = navigation;
    }
    
    [Parameter]
    public required string Url { get; init; }

    protected override void OnInitialized()
    {
        _navigation.NavigateTo(Url, true);
    }
}