namespace SGS29.Editor
{
    public interface IFoldout
    {
        void Draw(string key, string label, System.Action drawContent);
        void SetActive(string key, bool active);
        void Add(string key, bool defaultActive = false);
        void Remove(string key);
    }
}