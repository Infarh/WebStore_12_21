using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore.Infrastructure.Conventions;

public class TestConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        //Debug.WriteLine(controller.ControllerName);

        //controller.Actions.
        //controller.RouteValues.
    }
}