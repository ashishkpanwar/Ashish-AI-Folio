## Overview

An AI-powered knowledge assistant built using .NET, Azure OpenAI, and Azure AI Search, implementing a Retrieval-Augmented Generation (RAG) architecture with production-grade guardrails to prevent hallucinations and control cost.



## Key Features

* Clean Architecture (Application / Infrastructure separation)
* Azure OpenAI chat \& embeddings
* Vector search using Azure AI Search (HNSW + cosine)
* Similarity-based relevance filtering
* Graded confidence responses (high vs low confidence)
* Fail-fast behavior for unrelated queries
* Integration tests using xUnit

## Tech Stack

* .NET
* Azure OpenAI
* Azure AI Search
* React

## Architecture

User Question

&nbsp;    ↓

Embedding (Azure OpenAI)

&nbsp;    ↓

Vector Search (Azure AI Search)

&nbsp;    ↓

Similarity Threshold + Guardrails

&nbsp;    ↓

System Prompt (Grounded Context)

&nbsp;    ↓

LLM Response (Azure OpenAI)



## Hallucination Control



* Minimum similarity threshold (0.70)
* Early exit when no relevant context exists
* Confidence-aware prompting
* Explicit “I don’t know” fallback



## Testing Strategy



#### Integration tests for:

* Embedding generation
* Vector search
* Chat completion
* Full RAG pipeline
* No temporary endpoints
* No SDK mocking



## Cost Awareness



* Azure AI Search on Basic tier
* Top-K context limiting
* LLM not called when context is insufficient



## Future Project

* Citations per answer
* Query intent classification via ML
* Automated index creation
* Alternative vector stores (pgvector / Qdrant)
* Log-analysis RAG (Project 2.0)
