# Project Variables
PROJECT_NAME ?= BlogWebAPI
ORG_NAME ?= BlogWebAPI
REPO_NAME ?= BlogWebAPI

.PHONY: migrations db

migrations:
	cd ./BlogWebAPI.Data && dotnet ef --startup-project ../BlogWebAPI.Web/ migrations add $(mname) && cd ..

db:
	cd ./BlogWebAPI.Data && dotnet ef --startup-project ../BlogWebAPI.Web/ database update && cd ..

