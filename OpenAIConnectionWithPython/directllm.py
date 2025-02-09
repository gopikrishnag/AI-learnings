from promptflow import tool
from promptflow.connections import AzureOpenAIConnection
from openai import AzureOpenAI
 
@tool
def my_python_tool(input1: str, conn: AzureOpenAIConnection) -> str:
    client = AzureOpenAI(api_key=conn.api_key, azure_endpoint=conn.api_base, api_version=conn.api_version)
    completion=client.chat.completions.create(
        messages=[{"role": "user", "content":input1}],
        model="gpt-4o"
    )
    return completion.choices[0].message.content
     