# Project 
## Create compose file
```
docker-compose -f docker-compose.yml up -d
```
## Create  Topic
```
kafka-topics.sh --create --zookeeper zookeeper:2181 \
    --replication-factor 1 --partitions 1 --topic transcriptionOut
```
## Purge topic
```
kafka-topics.sh --zookeeper zookeeper:2181 --delete --topic ConsumeInput

kafka-topics.sh --create --zookeeper zookeeper:2181 \
    --replication-factor 1 --partitions 1 --topic transcriptionOut

kafka-topics.sh --zookeeper zookeeper:2181 --delete --topic ProduceInput

kafka-topics.sh --create --zookeeper zookeeper:2181 \
    --replication-factor 1 --partitions 1 --topic ProduceInput
```
# Show Offset Topic 
```
kafka-consumer-groups.sh --describe --group transcription-group --bootstrap-server localhost:9092
kafka-consumer-groups.sh --describe --group csharp-consumer --bootstrap-server localhost:9092

kafka-consumer-groups.sh --describe --group my-consumers-group --bootstrap-server localhost:9092
```