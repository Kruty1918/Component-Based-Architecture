namespace GlowspireGames.ComponentBasedArchitecture
{
    /// <summary>
    /// Інтерфейс обробника компонентів.
    /// </summary>
    /// <typeparam name="V">Тип значення, що обробляється.</typeparam>
    public interface IComponentHandler<V>
    {
        /// <summary>
        /// Метод обробки компонента.
        /// </summary>
        /// <returns>Результат обробки.</returns>
        V Handle();
    }
}
