using System.Collections.Generic;

namespace SGS29.Editor
{
    public class NamesValidator
    {
        private List<string> names;

        public NamesValidator(List<string> names)
        {
            // Use the provided list to track names (could also clone if needed)
            this.names = names;
        }

        // Checks if the given name already exists in the list
        public bool Contains(string name)
        {
            return names.Contains(name);
        }

        // Creates a new unique name using the provided key (e.g., "Controller" or "Component")
        public string CreateName(string key)
        {
            int counter = 1;
            string newName;
            do
            {
                newName = $"New {key} {counter}";
                counter++;
            } while (Contains(newName));

            // Once a unique name is found, add it to the list
            names.Add(newName);
            return newName;
        }
    }

}

