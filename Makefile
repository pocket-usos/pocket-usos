up:
	docker-compose up -d

db-update:
	dotnet ef database update --project src/Infrastructure --startup-project src/API --context Context

# make migrations name=MigrationName
migrations:
	dotnet ef migrations add $(name) --project src/Infrastructure --startup-project src/API --context Context

migration-removal:
	dotnet ef migrations remove --project src/Infrastructure --startup-project src/API --context Context
