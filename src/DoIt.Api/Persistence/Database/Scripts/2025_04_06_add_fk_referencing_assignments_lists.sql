ALTER TABLE IF EXISTS assignments
ADD COLUMN IF NOT EXISTS assignments_list_id UUID NULL REFERENCES assignments_lists (assignments_list_id)