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
    vector_op_txt_emb_3_lg vector(3072),
    vector_op_txt_emb_3_sm vector(1536),
    vector_ge_txt_emb_004 vector(768),
    CONSTRAINT "PK_keyword_vectors" PRIMARY KEY (id)
);


CREATE TABLE question_vectors (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    question_text text NOT NULL,
    vector_op_txt_emb_3_lg vector(3072),
    vector_op_txt_emb_3_sm vector(1536),
    vector_ge_txt_emb_004 vector(768),
    CONSTRAINT "PK_question_vectors" PRIMARY KEY (id)
);


CREATE TABLE report_codes (
    id uuid NOT NULL,
    report_id uuid NOT NULL,
    code jsonb NOT NULL,
    CONSTRAINT "PK_report_codes" PRIMARY KEY (id)
);


CREATE TABLE reports (
    id uuid NOT NULL,
    created_at timestamp with time zone,
    specification jsonb NOT NULL,
    CONSTRAINT "PK_reports" PRIMARY KEY (id)
);


