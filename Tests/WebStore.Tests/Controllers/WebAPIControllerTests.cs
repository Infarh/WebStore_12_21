using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers;

[TestClass]
public class WebAPIControllerTests
{
    [TestMethod]
    public void Index_returns_with_DataValues()
    {
        var data = Enumerable.Range(1, 10)
           .Select(i => $"Value - {i}")
           .ToArray();

        var values_service_mock = new Mock<IValuesService>(); // stab | mock
        values_service_mock
           .Setup(c => c.GetValues())
           .Returns(data);

        var controller = new WebAPIController(values_service_mock.Object);

        var result = controller.Index();

        var view_result = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

        var i = 0;
        foreach (var actual_value in model)
        {
            var expected_value = data[i++];
            Assert.Equal(expected_value, actual_value);
        }

        values_service_mock.Verify(s => s.GetValues()); // проверка, что метод был реально вызван
        values_service_mock.VerifyNoOtherCalls();       // проверка, что других методов интерфейса вызвано не было
    }
}