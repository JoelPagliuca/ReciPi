.PHONY: help build test clean clobber install develop start-db

NAME := recipi.dll

DB_CONTAINER_NAME := recipi_db
DB_CONTAINER_ID := $(shell docker ps -af "name=recipi_db" --format '{{.ID}}')

help: ## Print this message and exit
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

build: $(NAME) ## Builds a DLL

$(NAME): $(wildcard *.cs) VERSION.txt
	@echo "+ $@"
	@echo '¯\_(ツ)_/¯' # TODO

install: ## Install dotnet dependencies
	@echo "+ $@"
	@dotnet restore

init-env: ## Create secrets
	@echo "+ $@"
	./Deployment/init-env.sh
	@echo "Authentication:Google:ClientSecret \nAuthentication:Google:ClientId" # TODO

start-db: ## Runs a postgresql db in a container
	@echo "+ $@"
ifneq ($(DB_CONTAINER_ID),)
	@echo "resuming container $(DB_CONTAINER_ID)"
	@docker start $(DB_CONTAINER_NAME)
else
	@echo "creating new postgres container"
	@docker run -p '127.0.0.1:5432:5432' --name $(DB_CONTAINER_NAME) --env-file Secrets/db.env -d postgres
endif
	@echo "local pg container $(DB_CONTAINER_NAME) running on 127.0.0.1:5432"

develop: init-env start-db install ## Runs local dev environment
	@echo "+ $@"
	@echo "**************************************"
	@echo "* local environment ready to go -- run"
	@echo "* 	dotnet run"
	@echo "* or"
	@echo "* 	dotnet watch run"
	@echo "* to start the app"
	@echo "**************************************"

test: ## Runs tests
	@echo "+ $@"
	@echo '¯\_(ツ)_/¯' # TODO

clean: ## Cleanup any build binaries or packages
	@echo "+ $@"
ifneq ($(DB_CONTAINER_ID),)
	@docker stop $(DB_CONTAINER_NAME)
endif

clobber: clean ## Destroy everything
	@echo "+ $@"
	rm -f Secrets/*.env
	dotnet user-secrets remove "DB:ConnectionString"
ifneq ($(DB_CONTAINER_ID),)
	@docker rm $(DB_CONTAINER_NAME)
endif