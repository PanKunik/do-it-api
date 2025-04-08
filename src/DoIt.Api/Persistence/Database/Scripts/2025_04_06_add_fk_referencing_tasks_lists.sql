ALTER TABLE IF EXISTS tasks
ADD COLUMN IF NOT EXISTS task_list_id UUID NULL REFERENCES task_lists (task_list_id)