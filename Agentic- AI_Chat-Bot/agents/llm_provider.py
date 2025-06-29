# agenticai_chatbot/agents/llm_provider.py

from langchain_groq import ChatGroq
from langchain_openai import ChatOpenAI
from app.config import GROQ_API_KEY, OPENAI_API_KEY

def get_llm(provider: str, model) -> str:
    """
    Returns the API key for the given provider.
    Raises ValueError if the key does not exist.
    Raises ValueError for unknown providers.
    """
     
    provider = provider.lower()
    if provider == "groq":
        key = GROQ_API_KEY
        if not key:
            raise ValueError("GROQ_API_KEY not found in environment variables.")
        return ChatGroq(model=model, api_key=key)
    elif provider == "openai":
        key = OPENAI_API_KEY
        if not key:
            raise ValueError("OPEN_API_KEY not found in environment variables.")
        return ChatOpenAI(model=model, api_key=key)
    else:
        raise ValueError(f"Unknown provider: {provider}")

