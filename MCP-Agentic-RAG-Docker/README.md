Start the Qdrant continer

docker run -p 6333:6333 -p 6334:6334 -v qdrant_storage:/qdrant/storage:z qdrant/qdrant

Tools
1. BrightData Account
2. MCP server

------------
# uv venv
# uv init MCP-Agentic-RAG-Docker 
# uv add -r requirements.txt
# un mcp dev server/weather.py 
# uv run mcp install server/weather.py