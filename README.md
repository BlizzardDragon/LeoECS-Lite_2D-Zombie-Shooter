## Документация по проекту LeoECS-Lite_2D-Zombie-Shooter

### Общая информация
Проект реализован в Unity с использованием LeoECS Lite и паттернов промышленной архитектуры. Прототип демонстрирует базовые механики платформера: управление игроком, стрельба, спавн врагов, столкновения, урон, смерть, UI-интерфейс и гейм-овер.

### Архитектура проекта
ECS (Entity Component System)
Проект построен на LeoECS Lite. Основные сущности (игрок, враги, пули, предметы) описаны с помощью компонентов, а логика реализована в системах.

### ECS Системы:
* **Init-системы:**

  * `PlayerInitSystem`, `EnemySpawnFactory`, `ItemSpawnFactory`, `BulletSpawnFactory`

* **Группа GameplayGroup:**

  * Вход: `PlayerMoveInputSystem`

  * Движение: `MoveDirectionSystem`, `TargetFollowSystem`

  * Спавн: `BulletSpawnSystem`, `EnemySpawnSystem`, `AmmoSystem`

  * Урон и смерть: `DamageSystem`, `DeathSystem`, `DespawnWithDelaySystem`, `DestroyWithDelaySystem`

  * Предметы: `ItemDropSystem`, `PlayerPickUpSystem`

  * Анимации: `AnimationSystem`, `MoveFlipAnimationSystem`

  * UI и состояние: `PlayerAmmoViewSystem`, `HealthBarSystem`, `PlayerAmmoGameOverSystem`, `PlayerHealthGameOverSystem`, `GameOverScreenSystem`

  * **Вне группы:**

  * UI события: `ClickExitEventSystem`, `ClickRestartEventSystem`

  * Аудио: `AudioSystem`

  * Очистка: `DespawnSystem`, `DestroySystem`

  * **Удаление событий:** `DelHere<>()` — используется для событийных компонентов (`ShootAnimationEvent`, `TriggerEnterEntityEvent`, и др.).

### Фабрики и DI
Использован паттерн Factory:

`ItemSpawnFactory`, `IBulletSpawnFactory`, `IEnemySpawnFactory`

Фабрики регистрируются через Inject():

```
.Inject(
  itemSpawnFactory,
  bulletSpawnFactory,
  enemySpawnFactory
)
```
Каждая реализует интерфейс (например, IItemSpawnFactory) для соблюдения инверсии зависимостей.

Сервисы и инфраструктура
SharedData — централизованное хранилище ссылок (пулы, контейнеры, префабы).

InputService — обёртка над Unity.Input, инжектится как IInputService.

TimeService — предоставляет время (Time.deltaTime) как ITimeService.

AudioPlayer — реализует интерфейс IAudioPlayer для проигрывания клипов.

UI Events — обрабатываются через EcsUguiEmitter и InjectUgui.

Пул объектов
Для всех объектов ECS (EcsMonoObject) реализован собственный пул:

MonoObjectPool<T>

Унифицированный API:

csharp
Копировать
Редактировать
pool.Spawn();
pool.Despawn(obj);
Пулы автоматически создаются для каждого типа по запросу, аналогично Easy Pool Kit.

UI и взаимодействие
Управление: A/D — движение, Mouse0 — стрельба

UI: отображение патронов, здоровья, экран смерти

Перезапуск и выход — через кнопки UI (Ugui)

Паттерны проектирования
ECS (Data-Oriented Design)

Factory Method — для создания игровых объектов

Dependency Injection — для сервисов и фабрик

Object Pooling — переиспользование объектов

Service Locator (в рамках SharedData)

Точка входа (Startup.cs)
Файл Startup.cs содержит создание мира, регистрацию всех систем, инъекцию зависимостей и обновление фреймворка.

Ключевые строки:

csharp
Копировать
Редактировать
_world = new EcsWorld();
_systems = new EcsSystems(_world, _sharedData)
  .Add(...) // системы
  .Inject(...) // сервисы и фабрики
  .Init();
Сцена и запуск
Сцена: MainScene.unity

Начало игры — автоматически при запуске сцены

Игра завершается при:

Столкновении с врагом

Исчерпании патронов