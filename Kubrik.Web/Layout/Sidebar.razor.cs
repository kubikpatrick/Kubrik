using Kubrik.Models.Circles;
using Kubrik.Models.Devices;

using Microsoft.AspNetCore.Components;

namespace Kubrik.Web.Layout;

public partial class Sidebar : ComponentBase
{
    [Parameter]
    public List<Circle> Circles { get; set; }
    
    [Parameter]
    public List<Device> Devices { get; set; }
}