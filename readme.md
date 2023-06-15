## RabbitMQ

 * RabbitMQ is a message-queueing software also known as a message broker or queue manager. 
   Simply said; it is software where queues are defined, to which applications connect in order to transfer a message or messages.
   A message can include any kind of information. It could, for example, 
   have information about a process or task that should start on another application (which could even be on another server), 
   or it could be just a simple text message. The queue-manager software stores the messages until a receiving application connects and takes a message off the queue.
   The receiving application then processes the message
   
   
   ## RabbitMQ Example
  
    * A message broker acts as a middleman for various services (e.g. a web application, as in this example). They can be used to reduce loads and delivery times of       web application servers by delegating tasks that would normally take up a lot of time or resources to a third party that has no other job.
      In this guide, we follow a scenario where a web application allows users to upload information to a website. The site will handle this information, generate a       PDF, and email it back to the user. Handling the information, generating the PDF, and sending the email will, in this example case, take several seconds. That       is one of the reasons why a message queue will be used to perform the task.
      When the user has entered user information into the web interface, the web application will create a "PDF processing" message that includes all of the               important information the user needs into a message and place it onto a queue defined in RabbitMQ.
      
      <img src="/images/workflow-rabbitmq.png?raw=true" alt="workflow-rabbitmq" width="400" height="500">
 


