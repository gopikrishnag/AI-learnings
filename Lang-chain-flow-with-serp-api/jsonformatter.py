import json
from promptflow import tool

@tool
def format_serp_results(api_response: dict) -> dict:
    # Convert the API response to a JSON string
    # json_string = json.dumps(api_response, indent=4)  # Pretty-print for readability
    # print(f"View traces at: {json_string}")

    # Extract organic results from the API response
    results = api_response.get("organic_results", []) 

    # Format the results into a readable string
    formatted_text = "\n".join([
        f"Title: {r.get('title', 'N/A')}\nSnippet: {r.get('snippet', 'N/A')}\nURL: {r.get('link', 'N/A')}\n"
        for r in results
    ])

    # Return the formatted results as a dictionary
    return {"formatted_results": formatted_text}