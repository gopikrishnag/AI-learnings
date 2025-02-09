from promptflow import tool
from promptflow.connections import AzureOpenAIConnection
from langchain_openai import AzureChatOpenAI
 
@tool
def my_python_tool(input1: str  , conn: AzureOpenAIConnection) -> str:
    flow = AzureChatOpenAI(model="gpt-4o", azure_endpoint=conn.api_base, api_key=conn.api_key, api_version=conn.api_version)
    
    messages = [
        {"role": "system", "content": "You are a helpful AI assistant."},
        {"role": "user", "content": f"Given the following search results, generate a summary:\n\n{input1}\n\nSummary:"}
    ]
    result = flow.invoke(messages)
    return result.content