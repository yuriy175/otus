В Powershell перейти в папку \otus\docker и выполнить docker-compose -f ./full-compose.yml up
В броузере набрать http://localhost:8001/swagger/index.html для c# сервиса
В броузере набрать http://localhost:8002/swagger/index.html для Go сервиса

Загрузка тесмовых данных реализована только для c# и из докера недоступна
Отчет по оптимизации запроса находится в otus\results\indexes\indexes_report.pdf

Отчет по репликациям находится в otus\results\replica\replica_report.pdf

Реализован функционал шардирования диалогов в citus для C# и golang в отдельных микросервисах. Отчет по шардированию находится в otus\results\shards\shards_report.pdf

Реализован функционал обновления ленты новостей в реальном времени с передачей сообщений через RabbitMQ и вебсокеты для C# и golang в отдельных микросервисах. Фронтенд не рализован, по этому для взаимодействия по вебсокетам использован frontend\wsclient.html

Реализован функционал кеширования постов друзей в redis при помощи хранимых процедур (lua-функции) для C# и golang.
