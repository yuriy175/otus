1. Run script from master.txt

2. Restart master

docker restart postgres

3.  Make backup for replicas

docker exec -it postgres bash

mkdir /pgslave

pg_basebackup -h postgres -D /pgslave -U replicator -v -P --wal-method=stream

4. Copy dir to host

docker cp postgres:/pgslave pgslave

5. Copy standby.signal, to make replica

#F5 to ($PWD)\pgslave

6. Change postgresql.conf on replica

primary_conninfo = 'host=postgres port=5432 user=replicator password=medtex application_name=slave'

7. Start replica
docker run -dit -v $PWD/pgslave/:/var/lib/postgresql/data -e POSTGRES_PASSWORD=medtex -p 15433:5432 --network=docker_default  --restart=unless-stopped --name=pgslave postgres:15.4-alpine3.18

8 Check on master
#select application_name, sync_state from pg_stat_replication;

---

9 Start 2nd replica
docker exec -it postgres bash

mkdir /pgslave2

pg_basebackup -h postgres -D /pgslave2 -U replicator -v -P --wal-method=stream

docker cp postgres:/pgslave2 pgslave2

#Copy standby.signal, to make replica

primary_conninfo = 'host=postgres port=5432 user=replicator password=medtex application_name=slave2'

docker run -dit -v $PWD/pgslave2/:/var/lib/postgresql/data -e POSTGRES_PASSWORD=medtex -p 25433:5432 --network=docker_default  --restart=unless-stopped --name=pgslave2 postgres:15.4-alpine3.18