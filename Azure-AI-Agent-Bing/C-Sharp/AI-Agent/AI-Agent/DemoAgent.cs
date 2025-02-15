using Azure;
using Azure.AI.Projects;
using Azure.Identity;

namespace AI_Agent
{
    public static class DemoAgent
    {
        static async Task Main1()
        {
            // var connectionString = Environment.GetEnvironmentVariable("connectionString");
            var connectionString = "eastus2.api.azureml.ms;5e760c1a-fd32-403d-85fa-18ce7aec5b09;ai-learning;youtube-learnings";

            AgentsClient client = new AgentsClient(connectionString, new DefaultAzureCredential());

            // Step 1: Create an agent
            Response<Agent> agentResponse = await client.CreateAgentAsync(
                model: "gpt-35-turbo",
                name: "my-assistant",
                instructions: "You are a helpful agent.",
                tools: new List<ToolDefinition> { new CodeInterpreterToolDefinition() });
            Agent agent = agentResponse.Value;

            // Intermission: agent should now be listed

            Response<PageableList<Agent>> agentListResponse = await client.GetAgentsAsync();

            //// Step 2: Create a thread
            Response<AgentThread> threadResponse = await client.CreateThreadAsync();
            AgentThread thread = threadResponse.Value;

            // Step 3: Add a message to a thread
            //var content = "I need to solve the equation `3x + 11 = 14`. Can you help me?";
            var content = "What is today's exchange rate between GBP and INR with calendar date";
            Response<ThreadMessage> messageResponse = await client.CreateMessageAsync(
                thread.Id,
                MessageRole.User,
                content);
            ThreadMessage message = messageResponse.Value;

            // Intermission: message is now correlated with thread
            // Intermission: listing messages will retrieve the message just added

            Response<PageableList<ThreadMessage>> messagesListResponse = await client.GetMessagesAsync(thread.Id);
            //Assert.That(messagesListResponse.Value.Data[0].Id == message.Id);

            // Step 4: Run the agent
            Response<ThreadRun> runResponse = await client.CreateRunAsync(
                thread.Id,
                agent.Id,
                additionalInstructions: "");
            ThreadRun run = runResponse.Value;

            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                runResponse = await client.GetRunAsync(thread.Id, runResponse.Value.Id);
            }
            while (runResponse.Value.Status == RunStatus.Queued
                   || runResponse.Value.Status == RunStatus.InProgress);

            Response<PageableList<ThreadMessage>> afterRunMessagesResponse
                = await client.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = afterRunMessagesResponse.Value.Data;

            // Note: messages iterate from newest to oldest, with the messages[0] being the most recent
            foreach (ThreadMessage threadMessage in messages)
            {
                Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
                foreach (MessageContent contentItem in threadMessage.ContentItems)
                {
                    if (contentItem is MessageTextContent textItem)
                    {
                        Console.Write(textItem.Text);
                    }
                    else if (contentItem is MessageImageFileContent imageFileItem)
                    {
                        Console.Write($"<image from ID: {imageFileItem.FileId}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}