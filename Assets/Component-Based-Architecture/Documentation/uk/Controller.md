# Контролер в компонентно-орієнтованій архітектурі

Контролер є центральним елементом у компонентно-орієнтованій архітектурі, який відповідає за управління групами компонентів. Він забезпечує зв'язок між різними групами, обробляючи їх у загальній системі. Основною його задачею є організація доступу до компонентів, їх обробка та координування взаємодії між ними.

#### Основні функції контролера:
1. **Управління групами компонентів**:
    - Контролер збирає та управляє кількома групами компонентів.
    - Він відповідає за організацію доступу до цих груп, зокрема через інтерфейс `IComponentGroup`, який містить логіку обробки даних.

2. **Організація взаємодії між групами**:
    - Контролер дозволяє організувати логіку взаємодії між різними групами компонентів.
    - Він керує обробкою даних, що надходять з кожної групи, та може модифікувати або обчислювати результат на основі цього.

3. **Модульність та гнучкість**:
    - Контролер дозволяє створювати гнучкі, масштабовані системи, де легко можна додавати нові групи компонентів або змінювати існуючі без значних змін у коді.
    - Такий підхід забезпечує високу модульність системи, оскільки кожен контролер відповідає тільки за окремі групи, а не за всю систему.

#### Приклад реалізації

У прикладі системи фізичних ефектів контролер може виглядати так:

```csharp
public class PhysicsController : MonoBehaviour, IController<IComponentHandler<Vector2>, Vector2>
{
    public GravityGroup gravityGroup;
    public WindForceGroup windForceGroup;

    public IEnumerable<IComponentGroup<IComponentHandler<Vector2>, Vector2>> Groups
    {
        get
        {
            yield return (IComponentGroup<IComponentHandler<Vector2>, Vector2>)gravityGroup;
            yield return (IComponentGroup<IComponentHandler<Vector2>, Vector2>)windForceGroup;
        }
    }
}
```

В цьому прикладі `PhysicsController` управляє двома групами компонентів: групою гравітації та групою зовнішніх сил. Контролер забезпечує доступ до цих груп через властивість `Groups`, яка повертає колекцію груп для обробки.

```
public class PlayerController : MonoBehaviour, IController<ComponentBase, Vector2>
    {
        [SerializeField] protected List<GroupBase> componentGroups;

        // Властивість, що повертає перерахування груп компонентів
        private IEnumerable<IComponentGroup<ComponentBase, Vector2>> Groups
        {
            get
            {
                foreach (var group in componentGroups)
                {
                    yield return group;
                }
            }
        }

        // Реалізація інтерфейсу IController для доступу до груп компонентів
        IEnumerable<IComponentGroup<ComponentBase, Vector2>> IController<ComponentBase, Vector2>.Groups => Groups;
    }
```
У цьому прикладі ми реалізуємо ігровий контролер, який містить список `List<GroupBase>`. Цей список може обробляти рух завдяки `Vector2`, які він повертає. Контролер реалізує інтерфейс `IController<B, V>`, що вимагає наявності нумератора для перебору елементів типу `B` та повернення значень типу `V`. Для цього ми створюємо: 

```
IEnumerable<IComponentGroup<ComponentBase, Vector2>> IController<ComponentBase, Vector2>.Groups => Groups;
```
#### Де:
1. ```IEnumerable<IComponentGroup<ComponentBase, Vector2>>``` — це визначення типу, який буде повернено,
2. ```IController<ComponentBase, Vector2>.Groups``` — уточнення для компілятора, оскільки у класі є поле `Groups`. Це дозволяє правильно серіалізувати список.

```
 public class GroupBase : IComponentGroup<ComponentBase, Vector2>
    {
        [SerializeField] protected List<ComponentBase> components;

        // Властивість, що повертає компоненти групи
        IEnumerable<ComponentBase> IComponentGroup<ComponentBase, Vector2>.Components => components;

        // Обробка компонентів і повернення результату
        public Vector2 Handle()
        {
            Vector2 result = Vector2.zero;

            // Підсумовування результатів усіх компонентів
            foreach (var component in components)
            {
                result += component.Handle();
            }

            return result;
        }
    }
```

У нас є *базовий клас*, який надає список певних компонентів, визначених раніше. Ці компоненти повинні бути одного типу, і для того, щоб вони могли серіалізуватися в інспекторі, ми використовуємо:
``` List<ComponentBase> components; ```

Оскільки цей клас реалізує інтерфейс `IComponentGroup<B, V>`, ми повинні написати:
```IEnumerable<B> IComponentGroup<B, V>.Components```

Або в нашій реалізації:
```IEnumerable<ComponentBase> IComponentGroup<ComponentBase, Vector2>.Components```

Далі в нас є метод `Handle()`, який обробляє всі компоненти `ComponentBase`, що повертають дані типу `V`, тобто `Vector2` у випадку `PlayerController`. У результаті маємо таку операцію:
```result += component.Handle();```

Це додає всі оброблені дані. Це є приклад, який можна змінити відповідно до принципу `SRP` (Принцип єдиної відповідальності). Для цього буде створено клас через інтерфейс, що додає між собою всі дані.

```
 // Реалізація базового компонента, що обробляє Vector2
    public abstract class ComponentBase : IComponentHandler<Vector2>
    {
        public abstract Vector2 Handle();
    }

```

Базовий компонент для усі дотичних наприклад можна буде від `ComponentBase` реалізувати `MovementX` який буде обробляти дані відносно `X` і відповідно вертати `Vector2`. Звісно можна реалізувати ще більш точніше аби була окрма група для певної вісі наприклад `X` де кожен компонент буде працювати і обробляти лишень цю вісь.

### Висновок
Контролер є важливою частиною компонентно-орієнтованої архітектури, оскільки він керує групами компонентів, забезпечує гнучкість і масштабованість системи та полегшує інтеграцію різних логік обробки даних.
Ми можемо в інші групи додавати інші обєкти певного типу якщо `V` однаковий. 
