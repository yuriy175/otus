3. Меняем postgresql.conf на мастере

ssl = off
wal_level = replica
max_wal_senders = 4 # expected slave num

4. Подключаемся к мастеру и создаем пользователя для репликации

#docker exec -it postgres su - sa -c psql

#create role replicator with login replication password 'medtex';

6. Добавляем запись в pg_hba.conf с ip с первого шага

host    replication  replicator  172.18.0.0/16  md5

7. Перезапустим мастера

docker restart postgres

8.  Сделаем бэкап для реплик

docker exec -it postgres bash

mkdir /pgslave

pg_basebackup -h postgres -D /pgslave -U replicator -v -P --wal-method=stream

9. Копируем директорию себе

docker cp postgres:/pgslave pgslave

F5 to C:\Users\User -> ($PWD)

10. Создадим файл, чтобы реплика узнала, что она реплика

#touch pgslave/standby.signal
F5 to C:\Users\User -> ($PWD) \pgslave

11. Меняем postgresql.conf на реплике

primary_conninfo = 'host=postgres port=5432 user=replicator password=medtex application_name=slave'

12. Запускаем реплику
docker run -dit -v $PWD/pgslave/:/var/lib/postgresql/data -e POSTGRES_PASSWORD=medtex -p 15433:5432 --network=docker_default  --restart=unless-stopped --name=pgslave postgres:15.4-alpine3.18