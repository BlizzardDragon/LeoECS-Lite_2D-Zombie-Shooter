# Документация по проекту LeoECS Lite - 2D Zombie Shooter

## Общая информация

Проект реализован в Unity с использованием LeoECS Lite и паттернов промышленной архитектуры.  
Прототип демонстрирует базовые механики платформера: управление игроком, стрельба, спавн врагов, столкновения, урон, смерть, UI-интерфейс и гейм-овер.

## Паттерны проектирования

- **ECS (Data-Oriented Design)** — основа архитектуры
- **Dependency Injection** — внедрение зависимостей через `.Inject()`
- **Factory Method** — создание игровых объектов
- **Object Pooling** — переиспользование объектов
- **Service Locator** — частично используется через `SharedData`
- **YAGNI-подход** — реализация только необходимых механик (например, анимации переключаются без событийной системы)

## Архитектура проекта

### ECS (Entity Component System)

Проект построен на LeoECS Lite. Основные сущности (игрок, враги, пули, предметы) описаны с помощью компонентов, а логика реализована в системах.

### ECS Системы

#### Init-системы:
- `PlayerInitSystem`
- `EnemySpawnFactory`
- `ItemSpawnFactory`
- `BulletSpawnFactory`

#### Группа GameplayGroup:
- **Ввод:** `PlayerMoveInputSystem`
- **Движение:** `MoveDirectionSystem`, `TargetFollowSystem`
- **Стрельба:** `ShootSystem`
- **Спавн:** `BulletSpawnSystem`, `EnemySpawnSystem`
- **Урон и смерть:** `DamageSystem`, `DeathSystem`, `DespawnWithDelaySystem`, `DestroyWithDelaySystem`
- **Предметы:** `ItemDropSystem`, `PlayerPickUpSystem`
- **Анимации:** `PlayerAnimationSystem`, `MoveFlipAnimationSystem`
- **UI и состояние:** `PlayerAmmoViewSystem`, `HealthBarSystem`, `PlayerAmmoGameOverSystem`, `PlayerHealthGameOverSystem`, `GameOverScreenSystem`

#### Вне группы:
- **UI события:** `ClickExitEventSystem`, `ClickRestartEventSystem`
- **Аудио:** `AudioSystem`
- **Очистка:** `DespawnSystem`, `DestroySystem`
- **Удаление событий:** `DelHere<>()` — используется для событийных компонентов (`ShootAnimationEvent`, `TriggerEnterEntityEvent` и др.)

## Точка входа (Startup.cs)

Файл `Startup.cs` содержит:
- создание мира и систем
- регистрацию зависимостей
- запуск фреймворка

Пример:
```csharp
_world = new EcsWorld();
_systems = new EcsSystems(_world, _sharedData)
  .Add(...)       // системы
  .Inject(...)    // сервисы и фабрики
  .Init();
```

## Сервисы и инфраструктура

- `SharedData` — централизованное хранилище ссылок (например, конфиги, ссылки на важные префабы и каналы данных).
- `InputService` — обёртка над `Unity.Input`, инжектируется как `IInputService`.
- `TimeService` — предоставляет время как `ITimeService`.
- `AudioPlayer` — реализует `IAudioPlayer` для проигрывания аудио.
- UI события обрабатываются через `EcsUguiEmitter` и `.InjectUgui()`.

## Фабрики и DI

Использован паттерн **Factory**:
- `ItemSpawnFactory`
- `BulletSpawnFactory`
- `EnemySpawnFactory`

Каждая фабрика инжектируется по интерфейсу — соблюдён принцип **инверсии зависимостей (D)**.

Фабрики внедряются в системы через `.Inject()`:

```csharp
.Inject(
  itemSpawnFactory,
  bulletSpawnFactory,
  enemySpawnFactory
)
```

## Предметы (Items)

Система предметов построена на `ScriptableObject`-конфигурациях (`ItemConfig`), что обеспечивает удобную масштабируемость. Добавление новых предметов возможно без изменения кода — достаточно создать новый конфиг в редакторе.

Каждому предмету соответствует префаб ECS-объекта и ID, автоматически вычисляемый по имени (в `OnValidate()`).

Система спроектирована с упором на расширяемость и следование принципу открытости к расширению (**Open/Closed Principle**), без избыточного предусматривания логики — в духе **YAGNI**.

## Пул объектов

Для управления объектами используется ассет **Easy Pool Kit – Easiest Pool Manager for objects**.  
Пули, враги и предметы создаются через фабрики, которые используют пул вместо обычного `Instantiate()` и `Destroy()`.

## UI и взаимодействие

- Управление: `A/D` — движение, `Mouse0` — стрельба
- UI: отображение патронов, здоровья, экран смерти
- Перезапуск и выход — через кнопки UI (Ugui)

## Сцена и запуск

- Сцена: `MainScene.unity`
- Игра начинается автоматически при запуске сцены
- Игра завершается при:
  - столкновении с врагом
  - исчерпании патронов

## Затраченное время

Реализация выполнена в течение **12 часов**\
Рефакторинг **1 час 30 минут**\
Документация **1 час**

Фактическое время зафиксировано на видео:\
[Реализация](https://youtu.be/84MfcBmBtok)\
[Рефакторинг](https://youtu.be/uMtI9CPJkfI)

## Скриншоты
<img src="Assets\_project\2D\Screenshots\Screenshot-1.jpg" width="300">
<img src="Assets\_project\2D\Screenshots\Screenshot-2.jpg" width="300">
