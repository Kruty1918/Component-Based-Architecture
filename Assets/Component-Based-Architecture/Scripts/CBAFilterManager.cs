using System.Collections.Generic;
using System.Linq;


namespace SGS29.CBA
{
    public static class CBAFilterManager
    {
        /// <summary>
        /// Отримання всіх шляхів айтемів для заданого контролера.
        /// </summary>
        public static List<string> GetControllerItemsPath(string controllerName)
        {
            var controllers = CBAReaderWriter.Read();
            var controller = controllers.FirstOrDefault(c => c.controllerName == controllerName);

            if (controller == null) return null;

            List<string> paths = new List<string>();

            foreach (var group in controller.groups)
            {
                foreach (var component in group.components)
                {
                    paths.Add($"{controllerName}/{group.groupName}/{component.componentName}");
                }
            }

            return paths;
        }

        /// <summary>
        /// Отримання всіх шляхів айтемів для всіх груп контролера.
        /// </summary>
        public static List<string> GetGroupsItemsPath(string controllerName)
        {
            var controllers = CBAReaderWriter.Read();
            var controller = controllers.FirstOrDefault(c => c.controllerName == controllerName);

            if (controller == null) return null;

            List<string> paths = new List<string>();

            foreach (var group in controller.groups)
            {
                paths.Add($"{controllerName}/{group.groupName}");
            }

            return paths;
        }

        /// <summary>
        /// Фільтрація контролерів за ієрархією.
        /// </summary>
        public static bool CanCombine(string controllerName, string groupName = null, string componentName = null)
        {
            var controllers = CBAReaderWriter.Read();
            var controller = controllers.FirstOrDefault(c => c.controllerName == controllerName);

            if (controller == null)
                return false; // Якщо контролера немає, не можна об'єднати

            if (string.IsNullOrEmpty(groupName))
                return true; // Якщо шукаємо тільки контролер, то він існує

            var group = controller.groups.FirstOrDefault(g => g.groupName == groupName);
            if (group == null)
                return false; // Якщо групи немає, об'єднання неможливе

            if (string.IsNullOrEmpty(componentName))
                return true; // Якщо компонент не вказано, достатньо контролера і групи

            var component = group.components.FirstOrDefault(c => c.componentName == componentName);
            return component != null; // Перевіряємо, чи існує компонент
        }

        /// <summary>
        /// Отримання списку всіх контролерів.
        /// </summary>
        public static List<string> GetControllerNames()
        {
            var controllers = CBAReaderWriter.Read();
            return controllers.Select(c => c.controllerName).ToList();
        }

        /// <summary>
        /// Отримання списку всіх груп у вказаному контролері.
        /// </summary>
        public static List<string> GetGroupNames(string controllerName)
        {
            var controller = CBAReaderWriter.Read().FirstOrDefault(c => c.controllerName == controllerName);
            return controller?.groups.Select(g => g.groupName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// Отримання списку всіх компонентів у вказаній групі контролера.
        /// </summary>
        public static List<string> GetComponentNames(string controllerName, string groupName)
        {
            var controller = CBAReaderWriter.Read().FirstOrDefault(c => c.controllerName == controllerName);
            var group = controller?.groups.FirstOrDefault(g => g.groupName == groupName);
            return group?.components.Select(c => c.componentName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// Отримання всіх груп і їхніх компонентів у форматі "Група/Компонент".
        /// </summary>
        public static List<string> GetAllGroupsAndComponents(string controllerName)
        {
            var controller = CBAReaderWriter.Read().FirstOrDefault(c => c.controllerName == controllerName);
            if (controller == null) return new List<string>();

            List<string> result = new List<string>();
            foreach (var group in controller.groups)
            {
                foreach (var component in group.components)
                {
                    result.Add($"{group.groupName}/{component.componentName}");
                }
            }
            return result;
        }
    }
}