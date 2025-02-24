CREATE EXTENSION IF NOT EXISTS vector;


CREATE TABLE documents (
    id uuid NOT NULL,
    document_type text NOT NULL,
    online_url text NOT NULL,
    text text NOT NULL,
    CONSTRAINT "PK_documents" PRIMARY KEY (id)
);


CREATE TABLE keyword_vectors (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    keywords text NOT NULL,
    vector_op_txt_emb_3_lg vector(3072) NULL,
    vector_op_txt_emb_3_sm vector(1536) NULL,
    vector_ge_txt_emb_004 vector(768) NULL,
    CONSTRAINT "PK_keyword_vectors" PRIMARY KEY (id)
);


CREATE TABLE question_vectors (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    question_text text NOT NULL,
    vector_op_txt_emb_3_lg vector(3072) NULL,
    vector_op_txt_emb_3_sm vector(1536) NULL,
    vector_ge_txt_emb_004 vector(768 ) NULL
    CONSTRAINT "PK_question_vectors" PRIMARY KEY (id)
);


