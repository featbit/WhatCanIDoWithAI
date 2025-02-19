# WhatCanIDoWithAI
A platform to discover emerging AI technologies and inspire developers and founders to build impactful solutions and achieve billionaire success, drawing from FeatBit's best practices in feature management.

## GCP

### Connect to Memorystore

Based on [post in stackoverflow](https://stackoverflow.com/questions/50281492/accessing-gcp-memorystore-from-local-machines), create a VM instance in the same VPC network as your Redis instance

```bash
gcloud compute instances create redis-forwarder --machine-type=f1-micro  // doesn't exist, manually create a e1 instance
```

Actually, my compliance project doesn't work with command above, I have to manually create a VM instance in the same VPC network as my Redis instance. The only difference with the command above is that I have to specify the compute resource to e1-micro, define the zone and the network (default one).

Then by using the command below (generated in [gcloud doc](https://cloud.google.com/memorystore/docs/redis/connect-redis-instance#connecting_from_a_local_machine_with_port_forwarding)), I can connect to the Redis instance from my local machine.

```bash
gcloud compute ssh redis-forwarder --zone=us-east1-d -- -N -L 6378:10.15.114.117:6378
```

The command above should be run in git bash, not in PowerShell. Then a PuTTY window will pop up.

Then you can open a redis gui to connect to the Redis instance:

- host: localhost
- port: 6378
- password: auth string in memorystore for redis instance
- no username
- name: featbit
- no other selected cases

### Connect to MongoDB

featbit
OW1aJ8KY4OXcRaSM

mongodb+srv://featbit:OW1aJ8KY4OXcRaSM@cluster0.wiirw.mongodb.net/
mongodb+srv://featbit:OW1aJ8KY4OXcRaSM@cluster0.wiirw.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0

mongodb://atlas-sql-67a39e6a1bded35b308f60ba-wiirw.a.query.mongodb.net/sample_mflix?ssl=true&authSource=admin