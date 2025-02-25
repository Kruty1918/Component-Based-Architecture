using System.Collections.Generic;
using UnityEngine;

namespace SGS29
{
    public static class CBAReaderWriter
    {
        private const string CBADATA_FILE_PATH = "Assets/Component-Based-Architecture/Resources/CBAList.json";

        // Cached data
        private static List<ControllerNode> cachedControllers = null;

        // Property to get the cached controllers
        private static List<ControllerNode> CachedControllers
        {
            get
            {
                // If the cache is null or we haven't loaded data yet, reload it
                if (cachedControllers == null)
                {
                    cachedControllers = Read();
                }
                return cachedControllers;
            }
        }

        // This method invalidates the cache so that it can be reloaded from disk
        public static void InvalidateCache()
        {
            cachedControllers = null;
        }

        public static bool ControllerContains(string controllerName)
        {
            // Manually iterate over the controllers to check if the controller exists
            foreach (var controller in CachedControllers)
            {
                if (controller.controllerName == controllerName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains(string path)
        {
            // Read the path components (controller/group/component)
            string[] pathComponents = path.Split('/');

            if (pathComponents.Length == 1)  // Controller only
            {
                return ControllerContains(pathComponents[0]);
            }
            else if (pathComponents.Length == 2)  // Controller and Group
            {
                string controllerName = pathComponents[0];
                string groupName = pathComponents[1];

                // Manually search for the controller and its group
                foreach (var controller in CachedControllers)
                {
                    if (controller.controllerName == controllerName)
                    {
                        foreach (var group in controller.groups)
                        {
                            if (group.groupName == groupName)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else if (pathComponents.Length == 3)  // Controller, Group, and Component
            {
                string controllerName = pathComponents[0];
                string groupName = pathComponents[1];
                string componentName = pathComponents[2];

                // Manually search for the controller, group, and component
                foreach (var controller in CachedControllers)
                {
                    if (controller.controllerName == controllerName)
                    {
                        foreach (var group in controller.groups)
                        {
                            if (group.groupName == groupName)
                            {
                                foreach (var component in group.components)
                                {
                                    if (component.componentName == componentName)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }

            return false;
        }

        public static List<ControllerNode> Read()
        {
            // If data is cached, return the cached version
            if (cachedControllers != null)
                return cachedControllers;

            // Otherwise, read from the file and cache it
            if (!System.IO.File.Exists(CBADATA_FILE_PATH))
                return new List<ControllerNode>();

            string jsonData = System.IO.File.ReadAllText(CBADATA_FILE_PATH);
            cachedControllers = JsonUtility.FromJson<Wrapper>(jsonData)?.controllers ?? new List<ControllerNode>();
            return cachedControllers;
        }

        public static void Write(List<ControllerNode> controllers)
        {
            string jsonData = JsonUtility.ToJson(new Wrapper { controllers = controllers }, true);
            System.IO.File.WriteAllText(CBADATA_FILE_PATH, jsonData);

            // Invalidate cache after writing to file
            InvalidateCache();
        }

        [System.Serializable]
        private class Wrapper
        {
            public List<ControllerNode> controllers;
        }
    }
}
