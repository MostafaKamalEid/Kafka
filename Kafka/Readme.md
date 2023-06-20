# Project 
## Create compose file
```
docker-compose -f docker-compose.yml up -d
```
## Create  Topic
```
kafka-topics.sh --create --zookeeper zookeeper:2181 \
    --replication-factor 1 --partitions 1 --topic test
```
## Purge topic
```
kafka-topics.sh --zookeeper zookeeper:2181 --delete --topic test

kafka-topics.sh --create --zookeeper zookeeper:2181 \
    --replication-factor 1 --partitions 1 --topic test
```
# Show Offset Topic 
```
kafka-consumer-groups.sh --describe --group csharp-consumer --bootstrap-server localhost:9092
```