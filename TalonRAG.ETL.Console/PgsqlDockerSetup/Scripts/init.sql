
CREATE EXTENSION IF NOT EXISTS vector;

GRANT ALL PRIVILEGES ON DATABASE talonrag TO talonragsvc;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO talonragsvc;
GRANT USAGE ON SCHEMA public TO talonragsvc;

CREATE TABLE IF NOT EXISTS public.article_embeddings (
    id SERIAL PRIMARY KEY,
    article_embedding VECTOR(384),
    article_content TEXT
);