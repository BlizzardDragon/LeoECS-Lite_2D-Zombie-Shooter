# Документация по проекту 2D-Платформера на LeoECS Lite

## Общая информация

Проект реализован в Unity с использованием LeoECS Lite и паттернов промышленной архитектуры.  
Прототип демонстрирует базовые механики платформера: управление игроком, стрельба, спавн врагов, столкновения, урон, смерть, UI-интерфейс и гейм-овер.

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
- **Вход:** `PlayerMoveInputSystem`
- **Движение:** `MoveDirectionSystem`, `TargetFollowSystem`
- **Спавн:** `BulletSpawnSystem`, `EnemySpawnSystem`, `AmmoSystem`
- **Урон и смерть:** `DamageSystem`, `DeathSystem`, `DespawnWithDelaySystem`, `DestroyWithDelaySystem`
- **Предметы:** `ItemDropSystem`, `PlayerPickUpSystem`
- **Анимации:** `AnimationSystem`, `MoveFlipAnimationSystem`
- **UI и состояние:** `PlayerAmmoViewSystem`, `HealthBarSystem`, `PlayerAmmoGameOverSystem`, `PlayerHealthGameOverSystem`, `GameOverScreenSystem`

#### Вне группы:
- **UI события:** `ClickExitEventSystem`, `ClickRestartEventSystem`
- **Аудио:** `AudioSystem`
- **Очистка:** `DespawnSystem`, `DestroySystem`
- **Удаление событий:** `DelHere<>()` — используется для событийных компонентов (`ShootAnimationEvent`, `TriggerEnterEntityEvent` и др.)

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

## Сервисы и инфраструктура

- `SharedData` — централизованное хранилище ссылок (например, конфиги, ссылки на важные префабы и каналы данных).
- `InputService` — обёртка над `Unity.Input`, инжектируется как `IInputService`.
- `TimeService` — предоставляет время как `ITimeService`.
- `AudioPlayer` — реализует `IAudioPlayer` для проигрывания аудио.
- UI события обрабатываются через `EcsUguiEmitter` и `.InjectUgui()`.

## Пул объектов

Для управления объектами используется ассет **Easy Pool Kit – Easiest Pool Manager for objects**.  
Пули, враги и предметы создаются через фабрики, которые используют пул вместо обычного `Instantiate()` и `Destroy()`.

## UI и взаимодействие

- Управление: `A/D` — движение, `Mouse0` — стрельба
- UI: отображение патронов, здоровья, экран смерти
- Перезапуск и выход — через кнопки UI (Ugui)

## Паттерны проектирования

- **ECS (Data-Oriented Design)** — основа архитектуры
- **Factory Method** — создание игровых объектов
- **Dependency Injection** — внедрение зависимостей через `.Inject()`
- **Object Pooling** — переиспользование объектов
- **Service Locator** — частично используется через `SharedData`
- **YAGNI-подход** — реализация только необходимых механик (например, анимации переключаются без событийной системы)

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

## Сцена и запуск

- Сцена: `MainScene.unity`
- Игра начинается автоматически при запуске сцены
- Игра завершается при:
  - столкновении с врагом
  - исчерпании патронов

## Затраченное время

Реализация выполнена в течение **X часов**  
(фактическое время зафиксировано на видео, прилагается отдельно).
