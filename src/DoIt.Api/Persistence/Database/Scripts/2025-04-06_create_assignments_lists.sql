CREATE TABLE IF NOT EXISTS assignments_lists (
    assignments_list_id UUID NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL
);