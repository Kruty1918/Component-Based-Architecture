# Унiверсальна компонентно-орiєнтована система керування групами обробникiв

### Загальний опис

Ця система реалізує уніфікований підхід до побудови компонентно-орієнтованої архітектури, де головною метою є модульне та масштабоване управління групами обробників. Вона дозволяє створювати гнучкі контролери, які керують групами компонентів, забезпечуючи зручний спосіб взаємодії між ними.

### Основна концепція

Система базується на ідеї розподілення відповідальності між трьома ключовими рівнями:

1. **Контролер (IController)** – відповідає за управління групами компонентів.
2. **Група компонентів (IComponentGroup)** – містить набір компонентів та виконує їхню обробку.
3. **Компонент (IComponentHandler)** – індивідуальна одиниця, що реалізує певну логіку обробки.

Такий підхід дозволяє будувати складні системи, де різні аспекти поведінки можна розділяти на окремі групи та компоненти, забезпечуючи їхню незалежність та можливість гнучкого розширення.

### Приклад використання: Система фізики

Уявімо, що ми реалізуємо власну фізичну систему, яка складається з різних фізичних впливів (наприклад, гравітація, зовнішні сили, тертя тощо). У такому випадку можна використати запропоновану архітектуру наступним чином:

- **Контролер фізики** керує кількома групами фізичних ефектів.
- **Групи фізичних ефектів** (наприклад, група гравітації, група зовнішніх сил) містять відповідні компоненти, які реалізують певну логіку обробки.
- **Компоненти фізичних ефектів** (наприклад, об’єкти, що піддаються дії сили тяжіння або вітру) відповідають за конкретну реалізацію фізичних законів.

Це дозволяє легко додавати або змінювати поведінку фізики, не змінюючи загальну структуру системи.

### Реалізація системи

#### **1. Контролер (IController)**

Контролер відповідає за керування групами компонентів. Його основна задача – забезпечити доступ до всіх груп, якими він управляє.

```csharp
public interface IController<B, V> where B : IComponentHandler<V>
{
    IEnumerable<IComponentGroup<B, V>> Groups { get; }
}
```

#### **2. Група компонентів (IComponentGroup)**

Група компонентів містить набір компонентів і виконує їхню обробку. Вона визначає точки входу та виходу, забезпечуючи модульність системи.

```csharp
public interface IComponentGroup<B, V> where B : IComponentHandler<V>
{
    IEnumerable<B> Components { get; }
    V Handle();
}
```

#### **3. Компонент (IComponentHandler)**

Компонент є основною одиницею, яка обробляє певні дані та реалізує конкретну поведінку.

```csharp
public interface IComponentHandler<V>
{
    V Handle();
}
```

### Конкретна реалізація

#### **Контролер фізичних ефектів**

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

#### **Група, що відповідає за гравітацію**

```csharp
public class GravityGroup : MonoBehaviour, IComponentGroup<GravityComponent, Vector2>
{
    public List<GravityComponent> components;
    public IEnumerable<GravityComponent> Components => components;

    public Vector2 Handle()
    {
        Vector2 result = Vector2.zero;
        foreach (var com in components)
        {
            result += com.Handle();
        }
        return result;
    }
}
```

#### **Компонент гравітації**

```csharp
public class GravityComponent : MonoBehaviour, IComponentHandler<Vector2>
{
    public Vector2 gravityForce = new Vector2(0, -9.81f);
    
    public Vector2 Handle()
    {
        return gravityForce;
    }
}
```

### Висновок

Ця архітектура дозволяє легко додавати нові групи та компоненти, зберігаючи гнучкість і модульність системи. Вона забезпечує чисте розділення відповідальностей та дозволяє легко налаштовувати та розширювати функціонал. Наприклад, можна додати групу тертя, яка буде враховувати силу опору середовища, або інші фізичні ефекти без зміни вже існуючого коду.

