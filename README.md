# File-Audit-Viewer
File Audit Viewer solves the problem of viewing the output of File Server Audit. Uses Microsoft SQL or MySQL to read output of File Server Audit and then displays it in a pivot table like interface.

### MSSQL Table Name: FileAudit

| Column Name           | Data Type     |
| --------------------- | ------------- |
| ID                    | int           |
| FolderPath            | nvarchar(MAX) |
| AccountSAMAccountName | nvarchar(MAX) |
| GroupSAMAccountName   | nvarchar(MAX) |
| ManagedBy             | nvarchar(MAX) |
| Inheritance           | nvarchar(MAX) |
| IsInherited           | nvarchar(MAX) |
| Rights                | nvarchar(MAX) |
| Owner                 | nvarchar(MAX) |
| Computer              | nvarchar(MAX) |
| RunDate               | bigint        |

### MySQL Table Name: FileAudit

| Column Name           |Data Type |
| --------------------- | -------- |
| ID                    | int      |
| FolderPath            | LONGTEXT |
| AccountSAMAccountName | LONGTEXT |
| GroupSAMAccountName   | LONGTEXT |
| ManagedBy             | LONGTEXT |
| Inheritance           | LONGTEXT |
| IsInherited           | LONGTEXT |
| Rights                | LONGTEXT |
| Owner                 | LONGTEXT |
| Computer              | LONGTEXT |
| RunDate               | bigint   |
