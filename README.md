Цель этого репо - показать примеры моего кода (пусть сейчас я бОльшую его часть и написал бы совершенно иначе)
Много необходимых зависимостей оставил "за бортом", и поэтому проект даже не скомпилируется.
Рабочая версия у меня есть, весит около 17Гб (в проекте есть высокополигональные модели) и содержит множество скриптов, моделей и ассетов, к которым я не причастен -> не выкладываю в открытый доступ.
При необходимости готов продемонстрировать проект очно.

Из примечательного: 
1) CustomOnClick и CustomOnClickEditor в связке с Messenger - CustomOnClick можно вешать на UI-кнопку, при этом на кнопку вешается делегат в обертке Messenger.Broadcast, что именно вещать - зависит от режима
 -режим "Авто" - в качестве сообщения используется имя объекта
 -режим "GameView" - в этом режиме становится доступным в инспекторе Enum событий, которые срабатывают на главном окне
 -режим "CityView" - в этом режиме становится доступным в инспекторе Enum событий, которые срабатывают на окне города
2) Связка SQLite + DataService + DBTypes 
  -SQLite-база данных используется для хранения данных о Юнитах, Зданиях, Технологиях, Скиллах и т.д., при этом не боится потери .meta-файлов при миграции на новые версии Юнити и её можно редактировать в отрыве от Юнити, что удобно в некоторых случаях (и можно заполнение контентом взвалить на плечи геймдизайнера)
   -DataService - набор методов, использующих C# Linq-запросы вместо "Select * from ..." для работы с БД - удобно, с учетом, что не все программисты на проекте знали SQL на должном уровне
   -DBTypes - классы/структуры, которые можно получить в процессе работы с SQL
