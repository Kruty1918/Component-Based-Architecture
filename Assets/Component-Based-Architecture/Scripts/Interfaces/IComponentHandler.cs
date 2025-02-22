namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Загальний інтерфейс для обробників компонентів.
    /// Використовується для визначення об'єктів, які можуть обробляти певне значення.
    /// </summary>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IComponentHandler<V>
    {
        /// <summary>
        /// Назва компонента.
        /// Використовується для ідентифікації конкретного обробника.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Виконує обробку компонента та повертає результат.
        /// </summary>
        /// <returns>Результат обробки у вигляді значення типу <typeparamref name="V"/>.</returns>
        V Handle();
    }
}