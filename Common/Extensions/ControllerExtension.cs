using Microsoft.AspNetCore.Mvc;

namespace Common.Extensions
{
    public static class ControllerExtension
    {
        // метод расширения IUrlHelper, отдающий nullable строку
        public static String? ControllerAction<T>(this IUrlHelper urlHelper, string name, object? arg)
            where T : ControllerBase
        {
            var controllerType = typeof(T); //получение типа контроллера
            var methodInfo = controllerType.GetMethod(name); //получение информации о методе
            if (methodInfo == null)
                return null;
            var controller = controllerType.Name.Replace("Controller", string.Empty); //убрать слово контроллер
            var action = urlHelper.Action(name, controller, arg); //создание ссылки
            return action;
        }
    }
}
