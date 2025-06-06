## RabbitMQ

 * RabbitMQ is a message-queueing software also known as a message broker or queue manager. 
   Simply said; it is software where queues are defined, to which applications connect in order to transfer a message or messages.
   A message can include any kind of information. It could, for example, 
   have information about a process or task that should start on another application (which could even be on another server), 
   or it could be just a simple text message. The queue-manager software stores the messages until a receiving application connects and takes a message off the queue.
   The receiving application then processes the message

*  What is RabbitMQ?
   RabbitMQ is open source message broker software (sometimes called message-oriented middleware) that implements the Advanced Message Queuing Protocol (AMQP). The RabbitMQ      server is written in the Erlang programming language and is built on the Open Telecom Platform framework for clustering and failover. Client libraries to interface with 
   the broker are available for all major programming languages.
   
   
   ## RabbitMQ Example
  
    * A message broker acts as a middleman for various services (e.g. a web application, as in this example). They can be used to reduce loads and delivery times       of web application servers by delegating tasks that would normally take up a lot of time or resources to a third party that has no other job.
      In this guide, we follow a scenario where a web application allows users to upload information to a website. The site will handle this information, generate       a PDF, and email it back to the user. Handling the information, generating the PDF, and sending the email will, in this example case, take several seconds.       That is one of the reasons why a message queue will be used to perform the task.
      When the user has entered user information into the web interface, the web application will create a "PDF processing" message that includes all of the             important information the user needs into a message and place it onto a queue defined in RabbitMQ.
      
      <img src="/images/workflow-rabbitmq.png?raw=true" alt="workflow-rabbitmq">
      
    ## When and why should you use RabbitMQ?
     * Message queueing allows web servers to respond to requests quickly instead of being forced to perform resource-heavy procedures on the spot that may delay       response time. Message queueing is also good when you want to distribute a message to multiple consumers or to balance loads between workers.
      The consumer takes a message off the queue and starts processing the PDF. At the same time, the producer is queueing up new messages. The consumer can be on       a totally different server than the producer or they can be located on the same server. The request can be created in one programming language and handled         in another programming language. The point is, the two applications will only communicate through the messages they are sending to each other, which means         the sender and receiver have low coupling.
      </br>
      <img src="/images/rabbitmq-beginners-updated.png" alt="rabbitmq-beginners-updated" width="900">
      </br>
      
      
      
       1. The user sends a PDF creation request to the web application.
       2. The web application (the producer) sends a message to RabbitMQ that includes data from the request such as name and email.
       3. An exchange accepts the messages from the producer and routes them to correct message queues for PDF creation.
       4. The PDF processing worker (the consumer) receives the task message and starts processing the PDF.


## EXCHANGES
  * Messages are not published directly to a queue; instead, the producer sends messages to an exchange. 
  * An exchange is responsible for routing the messages to different queues with the help of bindings and routing keys.
  * A binding is a link between a queue and an exchange.

<img src="/images/exchanges-bidings-routing-keys.png?raw=true" alt="exchanges-bidings-routing-keys">

## Standard RabbitMQ message flow
 1. The producer publishes a message to the exchange.
 2. The exchange receives the message and is now responsible for the routing of the message.
 3. Binding must be set up between the queue and the exchange. In this case, we have bindings to two different queues from the exchange. The exchange routes the message into the queues.
 4. The messages stay in the queue until they are handled by a consumer
 5. The consumer handles the message

## TYPES OF EXCHANGES
  
<img src="/images/exchanges-topic-fanout-direct.png?raw=true" alt="exchanges-topic-fanout-direct">

 * **Direct** : The message is routed to the queues whose binding key exactly matches the routing key of the message. For example, if the queue is bound to the exchange with the binding key pdfprocess, a message published to the exchange with a routing key pdfprocess is routed to that queue.
 * **Fanout** : A fanout exchange routes messages to all of the queues bound to it. 
 * **Topic** : The topic exchange does a wildcard match between the routing key and the routing pattern specified in the binding. 
 * **Headers** : Headers exchanges use the message header attributes for routing. 

## RABBITMQ AND SERVER CONCEPTS 
* **Producer** : Application that sends the messages.
* **Consumer**: Application that receives the messages.
* **Queue**: Buffer that stores messages.
* **Message**: Information that is sent from the producer to a consumer through RabbitMQ.
* **Connection**: A TCP connection between your application and the RabbitMQ broker.
* **Channel**: A virtual connection inside a connection. When publishing or consuming messages from a queue - it's all done over a channel.
* **Exchange**: Receives messages from producers and pushes them to queues depending on rules defined by the exchange type. To receive messages, a queue needs to be bound to at least one exchange.
* **Binding**: A binding is a link between a queue and an exchange.
* **Routing key**: A key that the exchange looks at to decide how to route the message to queues. Think of the routing key like an address for the message.
* **AMQP**: Advanced Message Queuing Protocol is the protocol used by RabbitMQ for messaging.
* **Users**: It is possible to connect to RabbitMQ with a given username and password. Every user can be assigned permissions such as rights to read, write and configure privileges within the instance. Users can also be assigned permissions for specific virtual hosts.
* **Vhost, virtual host**: Provides a way to segregate applications using the same RabbitMQ instance. Different users can have different permissions to different vhost *and queues and exchanges can be created, so they only exist in one vhost.


## Exchange to exchange bindings in RabbitMQ
  * It is possible to add bindings between exchanges in RabbitMQ. The exchange to exchange binding is useful when messages need to be saved for queues that are       automatically deleted, or when load balancing topics within a single broker. Additionally, when sending the same messages to different exchange types,             exchange to exchange binding is the best way forward.

<img src="/images/exchange-to-exchange-binding.png?raw=true" alt="exchange-to-exchange-binding">

 
 ## Collecting Unroutable Messages in a RabbitMQ Alternate Exchange
 
 * No matter how careful you are, mistakes can happen. For example, a client may accidentally or maliciously route messages using non-existent routing keys. To     avoid complications from lost information, collecting unroutable messages in a RabbitMQ alternate exchange is an easy, safe backup.

 ## Avoid Data Loss with a RabbitMQ Alternate Exchange 
  * RabbitMQ also lets you define an alternate exchange to apply logic to unroutable messages. Set an alternate exchange using policies or within arguments when      declaring an exchange.

<img src="/images/rabbitmq-alternate-exchange.png?raw=true" alt="rabbitmq-alternate-exchange">

## How to Optimize the RabbitMQ Prefetch Count
 * Knowing how to tune your broker correctly brings the system up to speed without having to set up a larger cluster or doing a lot of updates in your client code. Understanding how to optimize the RabbitMQ prefetch count maximizes the speed of the system.

# What is the RabbitMQ prefetch count?
 * The short answer: The RabbitMQ prefetch value is used to specify how many messages are being sent at the same time.
* Messages in RabbitMQ are pushed from the broker to the consumers. The RabbitMQ default prefetch setting gives clients an unlimited buffer, meaning that          RabbitMQ, by default, sends as many messages as it can to any consumer that appears ready to accept them. It is, therefore, possible to have more than one message "in flight" on a channel at any given moment.

<img src="/images/prefetch-in-flight.png?raw=true" alt="prefetch-in-flight">

* Messages are cached by the RabbitMQ client library (in the consumer) until processed. All pre-fetched messages are invisible to other consumers and are listed as unacked messages in the RabbitMQ management interface.
An unlimited buffer of messages sent from the broker to the consumer could lead to a window of many unacknowledged messages. Prefetching in RabbitMQ simply allows you to set a limit of the number of unacked (not handled) messages.
There are two prefetch options available, channel prefetch count and consumer prefetch count.




 
 
  


