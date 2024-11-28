
CREATE EXTENSION IF NOT EXISTS vector;

GRANT ALL PRIVILEGES ON DATABASE talonrag TO talonragsvc;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO talonragsvc;
GRANT USAGE ON SCHEMA public TO talonragsvc;

CREATE TABLE IF NOT EXISTS public.article_embeddings (
    id SERIAL PRIMARY KEY,
    article_embedding VECTOR(384),
    article_content TEXT,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.users (
    id SERIAL PRIMARY KEY,
    email TEXT,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.conversations (
    id SERIAL PRIMARY KEY,
    user_id INTEGER references public.users (id),
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.messages (
    id SERIAL PRIMARY KEY,
    conversation_id INTEGER references public.conversations (id),
    author_role INTEGER,
    message_content TEXT,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);