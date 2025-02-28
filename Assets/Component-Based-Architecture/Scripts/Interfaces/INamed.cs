using System;

namespace SGS29.CBA
{
    /// <summary>
    /// Інтерфейс, який забезпечує наявність імені для об'єкта.
    /// Використовується для об'єктів, які потребують ідентифікації за допомогою імені.
    /// Цей інтерфейс може бути корисним для фільтрації об'єктів за їх іменами в різних системах або колекціях.
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// Ім'я об'єкта.
        /// Це властивість надає унікальне або описове ім'я для кожного об'єкта, що реалізує цей інтерфейс.
        /// Ім'я можна використовувати для фільтрування або сортування об'єктів у колекціях.
        /// </summary>
        string Name { get; }
    }

    public interface IFilterCollection : INamed
    {
        void FilterBy(FilterData[] data);
    }

    public class FilterData
    {
        public string Name;
        public int Priority;
    }
}