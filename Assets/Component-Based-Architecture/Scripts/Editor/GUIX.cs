using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public class EditorButton
    {
        public string IconName { get; set; }
        public float Size { get; set; }

        // Властивості для різних кольорових станів кнопки
        public Color NormalColor { get; set; }
        public Color HoverColor { get; set; }
        public Color PressedColor { get; set; }

        // Події
        public event Action OnClick;
        public event Action OnSelect;
        public event Action OnDraw;
        public event Action OnPointerDown;
        public event Action OnPointerUp;

        public EditorButton(string iconName, float size)
        {
            IconName = iconName;
            Size = size;
            // За замовчуванням: нормальний – білий, при наведенні – трохи світліший, при натисканні – темніший
            NormalColor = Color.white;
            HoverColor = new Color(0.9f, 0.9f, 0.9f, 1f);
            PressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        }

        /// <summary>
        /// Малює кнопку з іконкою та обробляє події:
        /// - OnPointerDown – коли користувач натискає мишкою на кнопку
        /// - OnPointerUp – коли користувач відпускає мишку над кнопкою
        /// - OnClick – коли користувач клацає по кнопці
        /// - OnSelect – коли кнопка отримує фокус
        /// Також іконка змінює свій колір відповідно до стану (нормальний, hover, pressed).
        /// </summary>
        public void Draw()
        {
            OnDraw?.Invoke();

            // Унікальне ім'я для контролу, що дозволяє відслідковувати фокус
            string controlName = "EditorButton_" + IconName;
            Rect rect = GUILayoutUtility.GetRect(Size, Size, GUILayout.Width(Size), GUILayout.Height(Size));
            GUI.SetNextControlName(controlName);

            Event evt = Event.current;

            // Обробка подій миші для OnPointerDown та OnPointerUp
            if (evt.type == EventType.MouseDown && rect.Contains(evt.mousePosition))
            {
                OnPointerDown?.Invoke();
            }
            if (evt.type == EventType.MouseUp && rect.Contains(evt.mousePosition))
            {
                OnPointerUp?.Invoke();
            }

            // Збереження поточного кольору та визначення нового кольору залежно від стану
            Color originalColor = GUI.color;
            Color currentColor = NormalColor;
            if (rect.Contains(evt.mousePosition))
            {
                // Якщо миша знаходиться над кнопкою – використовується HoverColor або PressedColor
                if (evt.type == EventType.MouseDown || evt.type == EventType.MouseDrag)
                {
                    currentColor = PressedColor;
                }
                else
                {
                    currentColor = HoverColor;
                }
            }
            GUI.color = currentColor;

            // Налаштовуємо стиль кнопки з прозорим бекграундом
            GUIStyle iconButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = UIStyles.MakeTex((int)Size, (int)Size, Color.clear) },
                padding = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter
            };

            GUIContent iconContent = EditorGUIUtility.IconContent(IconName);

            // Малюємо кнопку
            if (GUI.Button(rect, iconContent, iconButtonStyle))
            {
                OnClick?.Invoke();
                GUI.FocusControl(controlName);
            }

            // Відновлюємо оригінальний GUI.color
            GUI.color = originalColor;

            // Якщо цей контрол знаходиться у фокусі, викликаємо подію OnSelect
            if (GUI.GetNameOfFocusedControl() == controlName)
            {
                OnSelect?.Invoke();
            }
        }
    }

    public static class GUIX
    {
        public static void DrawLine(float treeWidth, float width = 0, float height = 0.5f)
        {
            float lineWidth = width == 0 ? treeWidth : width;
            Rect rect = GUILayoutUtility.GetRect(lineWidth, height);
            EditorGUI.DrawRect(rect, Color.Lerp(UIStyles.BOX_COLOR, Color.white, 0.12f));
        }

        /// <summary>
        /// Створює кнопку з іконкою, підписує на подію OnClick задану дію,
        /// малює кнопку та повертає екземпляр EditorButton для подальшої взаємодії.
        /// </summary>
        /// <param name="iconName">Назва іконки (наприклад, "Toolbar Minus")</param>
        /// <param name="size">Розмір кнопки</param>
        /// <param name="action">Дія, яка викликається при натисканні</param>
        public static EditorButton DrawButtonWithIcon(string iconName, float size, Action action)
        {
            EditorButton button = new EditorButton(iconName, size);
            button.OnClick += action;
            button.Draw();
            return button;
        }

        public static string DrawNameField(string label, string value, float labelWidth = 50, float textFieldWidth = 200, float extraIndent = 0)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (extraIndent > 0)
                    GUILayout.Space(extraIndent);
                GUILayout.Label(label, GUILayout.Width(labelWidth));
                return EditorGUILayout.TextField(value, GUILayout.Width(textFieldWidth));
            }
        }

        /// <summary>
        /// Універсальний метод для відображення кнопок "Додати" та "Видалити" для будь-якого списку.
        /// </summary>
        /// <typeparam name="T">Тип елементів списку.</typeparam>
        /// <param name="list">Список, з яким будемо працювати.</param>
        /// <param name="createNewItem">Делегат, який створює новий екземпляр T.</param>
        /// <param name="onRemove">Необов'язковий делегат для додаткової логіки видалення елемента (якщо не задано — видаляється останній елемент).</param>
        /// <param name="buttonSize">Розмір кнопок (ширина і висота).</param>
        /// <param name="spacing">Відступ перед кнопками.</param>
        public static void DrawListButtons<T>(
            List<T> list,
            Func<T> createNewItem,
            Action<T> onRemove = null,
            int buttonSize = 25,
            float spacing = 30f)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(spacing);

            // Кнопка видалення
            DrawButtonWithIcon("Toolbar Minus", buttonSize, () =>
            {
                if (list.Count > 0)
                {
                    T item = list[list.Count - 1];
                    if (onRemove != null)
                        onRemove(item);
                    else
                        list.RemoveAt(list.Count - 1);
                }
            });

            // Кнопка додавання
            DrawButtonWithIcon("Toolbar Plus", buttonSize, () =>
            {
                list.Add(createNewItem());
            });

            EditorGUILayout.EndHorizontal();
        }
    }
}
